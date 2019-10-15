using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JsonParser;

namespace _3DSpaceGame {
    public class Prefab {

        private readonly Transform transform;
        private readonly Dictionary<Type, object[]> components = new Dictionary<Type, object[]>();
        private readonly List<Prefab> children = new List<Prefab>();

        public Prefab(Transform t) {
            transform = t;
        }

        public static Prefab Load(JObject json) {

            // Transform:
            Transform t;
            if (json.ContainsKey("transform")) {
                t = Transform.FromJson(json["transform"] as JObject);
            } else t = new Transform();

            var res = new Prefab(t);

            // Components
            if (json.ContainsKey("components")) {
                foreach (var item in json["components"] as JArray) {
                    var o = item as JObject;
                    var type = Type.GetType(o["type"] as JString);
                    if (o.ContainsKey("args")) {
                        var jargs = o["args"] as JArray;
                        var args = new List<object>();
                        foreach (var arg in jargs) {
                            var resarg = arg.ToObject();
                            if (arg.IsString) {
                                var str = (string)(arg as JString);
                                if (str.StartsWith("@")) {
                                    // preprocessing directive:
                                    str = str.Substring(1);
                                    resarg = PreProcessingDirective(str);
                                }
                            }
                            args.Add(resarg);
                        }
                        res.AddComp(type, args.ToArray());
                    } else res.AddComp(type);
                }
            }


            // Children:
            if (json.ContainsKey("children")) {
                foreach (var childjson in json["children"] as JArray) {
                    res.AddChild(Load(childjson as JObject));
                }
            }

            return res;
        }

        private static object PreProcessingDirective(string str) {
            if (str.StartsWith("mesh(")) {
                str = str.Substring(5, str.Length - 6);
                return Assets.OBJs[str].GenMesh();
            } else if (str.StartsWith("material(")) {
                str = str.Substring(9, str.Length - 10);
                return Material.Chrome;
            }

            return null;
        }

        public Prefab AddChild(Prefab child) {
            children.Add(child);
            return this;
        }

        public Prefab AddChildren(params Prefab[] _children) {
            children.AddRange(_children);
            return this;
        }

        public Prefab AddComp(Type type, params object[] args) {
            components.Add(type, args);
            return this;
        }

        public Prefab AddComp<T>(params object[] args) where T : Component {
            components.Add(typeof(T), args);
            return this;
        }

        public GameObject NewInstance() {
            var g = new GameObject();
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            g.transform.scale = transform.scale;

            foreach (var item in components) {
                g.AddComp(item.Key.GetConstructor(item.Value.Select(x => x.GetType()).ToArray()).Invoke(item.Value) as Component);
            }

            foreach (var item in children) {
                var c = item.NewInstance();
                g.AddChild(c);
            }

            g.Start();
            return g;
        }

    }
}
