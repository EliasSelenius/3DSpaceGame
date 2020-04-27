

function printobj(obj) {
    for (let m in obj) {
        print(m);
    }
}

function spawn(name, pos) {
    let g = sg.Assets.Prefabs[name].NewInstance();
    g.EnterScene(sg.Program.scene);
    g.transform.position = pos;

    return g;
}