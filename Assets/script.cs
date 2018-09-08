using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//// todo
// clone and shift current layer
// traversing the graph is duplicating nodes sometimes
// auto-assign connector types while building
//  - construct() is calling autoAssign() but it isn't detecting rods that were just created
// manual override of connector type and orientation (cycle through possible connectors/orientations only?)
// 3D connectors
// rod mesh
// window loses focus on file load
// orient orange/ender connectors correctly when rod through center (depends on rod's orientation)


public class script : MonoBehaviour {
    public static script the;
    // state
    public GameObject cursor, cursorBall;
    public int rodSize = 2;
    public Stack<UndoFrame> undoStack = new Stack<UndoFrame>();
    bool deleteHandled = false;
    bool autoDiag = true;

    int symmetry = 0;
    string[] symmetryName = { "No symmetry", "Mirror symmetry", "4-way rotational symmetry" };
    int symmetryCount = 3;
    const int symNone = 0, symMirror = 1, sym4Rotate = 2;
    
    // piece data
    public static float[] rodUnits = { 17.25f, 33f, 55f, 86f, 130f, 192f }; // mm
    public static Color[] rodColors = { Color.green, Color.white, Color.blue, Color.yellow, Color.red, Color.gray };
    string[] rodNames = { "1 green", "2 white", "3 blue", "4 yellow", "5 red", "6 gray" };
    const int maxRodSize = 5;

    // constants
    public const int nodeLayer = 9;
    public const int rodLayer = 10;
    public const int nodeMask = 1 << nodeLayer;
    public const int rodMask = 1 << rodLayer;

    // camera
    float camZoom = 1000f;
    Quaternion camMouseLookStart;
    bool targetCursor = true;
    Quaternion camAngles = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    bool camFocus = false;

	void Start () {
        the = this;
        clearStage();
        loadNew();
        cursorBall.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
    }

    public void clearStage() {
        undoStack.Clear();
        foreach (var obj in FindObjectsOfType<Node>())
            Object.Destroy(obj.gameObject);
        foreach (var obj in FindObjectsOfType<Rod>())
            Object.Destroy(obj.gameObject);
        cursor = null;
    }

    // starting point
    public void loadNew() {
        var root = Node.Create();
        root.transform.position = new Vector3(0f, 0f, 0f);
        setCursor(root);
    }

    public void setCursor(GameObject obj) {
        cursor = obj;
        cursorBall.transform.position = cursor.transform.position;
        targetCursor = true;
    }

    void construct(Vector3 direction) {
        direction.Normalize();

        // direction is relative to camera's look vector
        direction = camAngles * direction;

        int size = rodSize;
        float max = Mathf.Max(Mathf.Abs(direction.x), Mathf.Max(Mathf.Abs(direction.y), Mathf.Abs(direction.z)));
        if (autoDiag && max < 0.9 && size < maxRodSize)
            size++; // diagonal

        Vector3 origin = cursor.transform.position + direction * 10f;
        Vector3 target = origin + direction * rodUnits[size];
        Vector3 rodC = (origin + target) / 2;

        UndoFrame undo = new UndoFrame(cursor, true);

        // existing rod?
        Collider[] hits = Physics.OverlapSphere(rodC, 5f, rodMask);
        if (hits.Length == 0) {
            var rod = Rod.Create(size);
            rod.transform.position = rodC;
            rod.transform.up = target - origin;
            undo.objs.Add(rod);

            if (symmetry == symMirror) {
                // do we need to check for duplicate rods here?
                rod = Rod.Create(size);
                rod.transform.position = Vector3.Reflect(rodC, Vector3.right);
                rod.transform.up = Vector3.Reflect(target - origin, Vector3.right);
                undo.objs.Add(rod);

            } else if (symmetry == sym4Rotate) {
                for (float theta = 90f; theta < 360; theta += 90f) {
                    rod = Rod.Create(size);
                    rod.transform.position = Quaternion.AngleAxis(theta, Vector3.forward) * rodC;
                    rod.transform.up = Quaternion.AngleAxis(theta, Vector3.forward) * (target - origin);
                    undo.objs.Add(rod);
                }
            }


        } else {
            if (Input.GetKey(KeyCode.Delete)) {
                undo = new UndoFrame(cursor, false);
                undo.objs.Add(hits[0].gameObject);
                undoStack.Push(undo);
                hits[0].gameObject.SetActive(false);

                deleteHandled = true;
                return;
            }
        }

        if (Input.GetKey(KeyCode.Delete)) {
            // not trying to create something
            return;
        }

        cursor.GetComponent<Node>().autoAssign(); // update old cursor for new connectors

        // existing node?
        hits = Physics.OverlapSphere(target, 5f, nodeMask);
        if (hits.Length == 0) {
            var node = Node.Create();
            Vector3 pos = target + direction * 10f;
            node.transform.position = pos;
            if (!Input.GetKey(KeyCode.LeftShift))
                setCursor(node);

            undo.objs.Add(node);
            if (symmetry == symMirror) {
                Vector3 symPos = Vector3.Reflect(pos, Vector3.right);
                if (symPos != pos) {
                    node = Node.Create(symPos);
                    if (node) undo.objs.Add(node);
                }
            } else if (symmetry == sym4Rotate) {
                for (float theta = 90f; theta < 360; theta += 90f) {
                    Vector3 symPos = Quaternion.AngleAxis(theta, Vector3.forward) * pos;
                    if (symPos != pos) {
                        node = Node.Create(symPos);
                        if (node) undo.objs.Add(node);
                    }
                }
            }
        } else {
            if (!Input.GetKey(KeyCode.LeftShift))
                setCursor(hits[0].gameObject);
        }

        cursor.GetComponent<Node>().autoAssign(); // update new cursor for new connectors

        if (undo.objs.Count > 0)
            undoStack.Push(undo);
    }

    Vector3 mouseStart;
    void Update() {
        /////////////////
        // animate camera
        if (targetCursor) {
            Vector3 offset = new Vector3(0f, 0f, -camZoom);
            offset = camAngles * offset;
            Vector3 camPos = cursor.transform.position + offset;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPos, 5f * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, camAngles, 5f * Time.deltaTime);
        }

        ////////////////////
        // move camera
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // mouse pan
        if (Input.GetMouseButtonDown(0)) {
            targetCursor = false;
            mouseStart = Input.mousePosition;
        } else if (Input.GetMouseButton(0)) {
            Vector3 delta = Input.mousePosition - mouseStart;
            delta.Scale(new Vector3(-0.8f, -0.8f, 0.0f));

            Camera.main.transform.Translate(delta);
            mouseStart = Input.mousePosition;
        }

        // mouse tilt
        if (Input.GetMouseButtonDown(1)) {
            targetCursor = false;
            mouseStart = Input.mousePosition;
            camMouseLookStart = Camera.main.transform.rotation;
        } else if (Input.GetMouseButton(1)) {
            Vector3 delta = Input.mousePosition - mouseStart;
            delta.Scale(new Vector3(0.6f, -0.6f, 0)); // invert Y axis

            Quaternion x = Quaternion.AngleAxis(delta.x, Vector3.up);
            Quaternion y = Quaternion.AngleAxis(delta.y, Vector3.right);
            Camera.main.transform.rotation = camMouseLookStart * x * y;
        }
        
        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
            targetCursor = true; // snap back to cursor
        }

        camZoom += -500 * Input.GetAxis("Mouse ScrollWheel");

        if (ctrl) { // camera modifier
            // zoom
            if (Input.GetKey(KeyCode.KeypadPlus))
                camZoom -= 15;
            if (Input.GetKey(KeyCode.KeypadMinus))
                camZoom += 15;

            // barrel roll
            if (Input.GetKeyDown(KeyCode.Home))
                camAngles = camAngles * Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f));
            if (Input.GetKeyDown(KeyCode.PageUp))
                camAngles = camAngles * Quaternion.AngleAxis(90f, new Vector3(0f, 0f, -1f));

            // rotate
            if (Input.GetKeyDown(KeyCode.UpArrow))
                camAngles = camAngles * Quaternion.AngleAxis(90f, new Vector3(1f, 0f, 0f));
            if (Input.GetKeyDown(KeyCode.DownArrow))
                camAngles = camAngles * Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f));
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                camAngles = camAngles * Quaternion.AngleAxis(90f, new Vector3(0f, 1f, 0f));
            if (Input.GetKeyDown(KeyCode.RightArrow))
                camAngles = camAngles * Quaternion.AngleAxis(-90f, new Vector3(0f, 1f, 0f));

        }

        // save and load
        if (Input.GetKeyDown(KeyCode.S))
            Serial.saveFile();
        if (Input.GetKeyDown(KeyCode.L))
            Serial.loadFile();
        if (Input.GetKeyDown(KeyCode.N)) {
            clearStage();
            loadNew();
        }

        
        ////////////////////
        // construction
        if (!ctrl) {
            // udlr
            if (Input.GetKeyDown(KeyCode.RightArrow))
                construct(new Vector3(1, 0, 0));
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                construct(new Vector3(-1, 0, 0));
            if (Input.GetKeyDown(KeyCode.UpArrow))
                construct(new Vector3(0, 1, 0));
            if (Input.GetKeyDown(KeyCode.DownArrow))
                construct(new Vector3(0, -1, 0));

            // diagonal
            if (Input.GetKeyDown(KeyCode.Home))
                construct(new Vector3(-1, 1, 0));
            if (Input.GetKeyDown(KeyCode.End))
                construct(new Vector3(-1, -1, 0));
            if (Input.GetKeyDown(KeyCode.PageUp))
                construct(new Vector3(1, 1, 0));
            if (Input.GetKeyDown(KeyCode.PageDown))
                construct(new Vector3(1, -1, 0));

            // in/out
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
                construct(new Vector3(0, 0, 1));
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
                construct(new Vector3(0, 0, -1));

            if (Input.GetKeyDown(KeyCode.Delete)) {
                deleteHandled = false;
            }

            if (Input.GetKeyUp(KeyCode.Delete)) {
                if (!deleteHandled) {
                    GameObject newCursor = Node.nearestNotAt(cursor.transform.position);
                    if (newCursor == null) {
                        Debug.Log("No other nodes to set cursor to, cannot delete");
                    } else {
                        UndoFrame undo = new UndoFrame(cursor, false);
                        undo.objs.Add(cursor);
                        undoStack.Push(undo);

                        cursor.SetActive(false);
                        setCursor(newCursor);
                    }
                }
            }
        }

        // undo
        if (Input.GetKeyDown(KeyCode.Backspace) || (ctrl && Input.GetKeyDown(KeyCode.Z))) {
            if (undoStack.Count > 0) {
                UndoFrame undo = undoStack.Pop();
                setCursor(undo.cursor);
                undo.Revert();
            }
        }

        // rod size
        if (Input.GetKeyDown(KeyCode.Alpha1))
            rodSize = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            rodSize = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            rodSize = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            rodSize = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            rodSize = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            rodSize = 5;

        // toggles
        if (Input.GetKeyDown(KeyCode.D))
            autoDiag = !autoDiag;
        if (Input.GetKeyDown(KeyCode.M))
            symmetry = (symmetry + 1) % symmetryCount;

        if (Input.GetKeyDown(KeyCode.C)) {
            cursor.GetComponent<Node>().cycleShape();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            cursor.transform.Rotate(new Vector3(0, 0, 45));
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            var objs = FindObjectsOfType<Node>();
            foreach (var node in objs) {
                node.autoAssign();
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            camFocus = !camFocus;
            Camera.main.nearClipPlane = camFocus ? camZoom - 50 : 5;
            Camera.main.farClipPlane = camFocus ? camZoom + 50 : 10000;
        }
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), rodNames[rodSize]);
        GUI.Label(new Rect(10, 30, 200, 20), "D: " + (autoDiag ? "Automatic diagonal" : "No automatic diagonal"));
        GUI.Label(new Rect(10, 50, 200, 20), "M: " + symmetryName[symmetry]);
        GUI.Label(new Rect(10, 70, 200, 20), "look axis " + Camera.main.transform.eulerAngles.ToString());
    }
}


public class UndoFrame {
    public bool create;
    public List<GameObject> objs;
    public GameObject cursor;
    public UndoFrame(GameObject cursor, bool create) {
        this.cursor = cursor;
        this.create = create;
        objs = new List<GameObject>();
    }
    public void Revert() {
        if (create) {
            foreach (var obj in objs) Object.Destroy(obj);
        } else {
            foreach (var obj in objs) obj.SetActive(true);
        }
    }
}
