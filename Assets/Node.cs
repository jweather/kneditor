using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public int shape = 0;
    string[] shapes = { "snowflake", "yellow", "green", "red", "gray", "orange", "ender", "blue-blue", "blue-purple", "purple-purple" };
    const int shapeMax = 9;
    const int shapeSnowflake = 0, shapeYellow = 1, shapeGreen = 2, shapeRed = 3, shapeGray = 4, shapeOrange = 5, shapeEnder = 6;
    Color[] shapeColor = { Color.white, Color.yellow, new Color(0, 0.5f, 0), Color.red, Color.gray, new Color(1, 0.65f, 0), Color.gray, Color.blue, Color.blue, Color.blue };

    public void cycleShape() {
        setShape((shape + 1) % shapeMax);
    }

    public void setShape(int shape) {
        this.shape = shape;
        Mesh m = Resources.Load<Mesh>(shapes[shape]);
        GetComponentInChildren<MeshFilter>().mesh = m;
        GetComponentInChildren<MeshRenderer>().material.color = shapeColor[shape];
    }

    string strlist(List<int> list) {
        string res = "";
        foreach (int i in list) {
            if (res != "") res += ", ";
            res += i;
        }
        return res;
    }

    public void autoAssign() {
        // only in one plane for now (XY)
        bool[] rod = new bool[8];
        List<int> rods = new List<int>();
        int count = 0;

        Collider centerHit = null;
        var hits = Physics.OverlapSphere(transform.position, 5f, script.rodMask);
        if (hits.Length > 0) {
            // found a rod through the center hole
            centerHit = hits[0];
        }

        for (int i=0; i<8; i++) {
            Vector3 offset = new Vector3(0, 20, 0);
            offset = Quaternion.AngleAxis(i*45f, new Vector3(0, 0, 1)) * offset;
            Vector3 test = this.transform.position + offset;
            hits = Physics.OverlapSphere(test, 5f, script.rodMask);

            int hitc = 0;
            foreach (var hit in hits) {
                if (hit == centerHit) {
                    // don't count it as a connection
                    continue;
                }
                hitc++;
            }

            if (hitc > 0) {
                rod[i] = true;
                rods.Add(i);
                count++;
            }
        }

        if (count == 0) return; // nothing to be done -- should there be an "invisible" node?

        // find smallest span
        int start = rods[0];
        int span = rods[rods.Count-1] - rods[0];
        for (int i = 1; i<rods.Count; i++) {
            int s = ((8 + rods[i - 1]) - rods[i]);
            if (s < span) {
                start = rods[i];
                span = s;
            }
        }

        if (span == 0) {
            setShape(shapeEnder);
            var rot = Quaternion.identity;
            if (centerHit != null) {
                // hack hack hack
                rot = Quaternion.AngleAxis(90f, new Vector3(1, 0, 0));
                }
            transform.rotation = rot * Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
            return;
        }
        if (span == 1) {
            setShape(shapeGray);
            transform.rotation = Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
            return;
        }
        if (span == 2) {
            setShape(shapeRed);
            transform.rotation = Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
            return;
        }
        if (span == 3) {
            setShape(shapeGreen);
            transform.rotation = Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
            return;
        }
        if (span == 4) {
            if (count == 2) {
                setShape(shapeOrange);
                var rot = Quaternion.identity;
                if (centerHit != null) {
                    // hack hack hack
                    rot = Quaternion.AngleAxis(90f, new Vector3(0, 1, 0));
                }
                transform.rotation = rot * Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
                return;
            }
            setShape(shapeYellow);
            transform.rotation = Quaternion.AngleAxis(start * 45f, new Vector3(0, 0, 1));
            return;
        }
        setShape(shapeSnowflake);

    }

    public static GameObject Create() {
        GameObject obj = GameObject.Instantiate(Resources.Load("snowflake") as GameObject);
        obj.AddComponent<SphereCollider>();
        obj.GetComponent<SphereCollider>().radius = 5f;
        obj.AddComponent<Node>();
        return obj;
    }

    public static GameObject Create(Vector3 pos) {
        // check for overlap first
        var hits = Physics.OverlapSphere(pos, 5f, script.nodeMask);
        if (hits.Length > 0) {
            Debug.Log("overlap rejected");
            return null;
        }
        var obj = Create();
        obj.transform.position = pos;
        return obj;
    }

    public static GameObject nearestNotAt(Vector3 pos) {
        Collider best = null;
        float bestD = 1000f;
        var hits = Physics.OverlapSphere(pos, 1000f, script.nodeMask);
        foreach (var hit in hits) {
            float d = Vector3.Distance(hit.transform.position, pos);
            if (d == 0) continue; // not that one

            if (best == null || d < bestD) {
                best = hit;
                bestD = d;
            }
        }
        if (best == null) return null;
        return best.gameObject;
    }

	// Use this for initialization
	void Start () {
        this.gameObject.layer = script.nodeLayer;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
