using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSpaceGame.UI {
    public class ParentElement : Element {

        public readonly List<Element> children = new List<Element>();


        public T AddChild<T>() where T : Element, new() {
            T elm = new T();
            children.Add(elm);
            return elm;
        }

    }
}
