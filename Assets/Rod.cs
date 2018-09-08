using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour {
    public static GameObject Create(int size) {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Rod rod = obj.AddComponent<Rod>();
        rod.setSize(size);
        return obj;
    }

    public int size = 2;

	// Use this for initialization
	void Start () {
        this.gameObject.layer = script.rodLayer;
    }

    public void setSize(int size) {
        this.size = size;
        this.gameObject.transform.localScale = new Vector3(7f, script.rodUnits[size] / 2f, 7f);
        this.gameObject.GetComponent<Renderer>().material.color = script.rodColors[size];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
