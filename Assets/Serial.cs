using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Crosstales.FB;

public class Serial : MonoBehaviour {
    // Use this for initialization
    void Start() {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public static void saveFile() {
        string path = FileBrowser.SaveFile("Save File", "", "save", "json");
        if (path == null) return;

        World w = new World();
        w.saveGameState();
        string json = JsonUtility.ToJson(w);
        File.WriteAllText(path, json);
    }

    public static void loadFile() {
        string path = FileBrowser.OpenSingleFile("Open File", "", "json");
        if (!File.Exists(path)) {
            Debug.Log("no such file");
            return;
        }
        string json = File.ReadAllText(path);
        World w = JsonUtility.FromJson<World>(json);

        script.the.clearStage();
        foreach (var wn in w.nodes) {
            var obj = Node.Create();
            obj.transform.position = wn.position;
            if (script.the.cursor == null)
                script.the.setCursor(obj.gameObject);
        }
        foreach (var wr in w.rods) {
            var obj = Rod.Create(wr.size);
            obj.transform.position = wr.position;
            obj.transform.rotation = wr.rotation;
        }
    }
}

[Serializable]
public class World {
    public List<WorldNode> nodes;
    public List<WorldRod> rods;
    public World() {
        nodes = new List<WorldNode>();
        rods = new List<WorldRod>();
    }
    public void saveGameState() {
        var fnodes = GameObject.FindObjectsOfType<Node>();
        Debug.Log("saving " + fnodes.Length + " nodes");
        foreach (Node node in fnodes) {
            WorldNode wn = new WorldNode();
            wn.position = node.gameObject.transform.position;
            nodes.Add(wn);
        }

        var frods = GameObject.FindObjectsOfType<Rod>();
        Debug.Log("saving " + frods.Length + " rods");
        foreach (Rod rod in frods) {
            WorldRod wr = new WorldRod();
            wr.position = rod.gameObject.transform.position;
            wr.rotation = rod.gameObject.transform.rotation;
            wr.size = rod.size;
            rods.Add(wr);
        }
    }
}

[Serializable]
public class WorldNode {
    public Vector3 position;
}

[Serializable]
public class WorldRod {
    public Vector3 position;
    public Quaternion rotation;
    public int size;
}
