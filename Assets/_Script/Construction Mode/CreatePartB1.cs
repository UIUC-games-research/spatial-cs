using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartB1 : MonoBehaviour
{

    private GameObject[] instantiated;
    public GameObject[] parts;
    private bool[] partCreated;
    public Button[] partButtons;
    private Vector3 createLoc;
    private Vector3 offscreenCreateLoc;
    public GameObject eventSystem;
    private SelectPart selectionManager;
    public int NUM_PARTS;
    private GameObject startObject;

    public GameObject rotateYButton;
    public GameObject rotateXButton;
    public GameObject rotateZButton;
    public RotationGizmo rotateGizmo;

    private const float MOVEMENT_SPEED = 100;
    private float step;
    private const float WAIT_TIME = 0.01f;

    // Use this for initialization
    void Awake()
    {
        //number of parts to fuse
        partCreated = new bool[NUM_PARTS];
        instantiated = new GameObject[NUM_PARTS];
        for (int i = 0; i < NUM_PARTS; i++)
        {
            partCreated[i] = false;
        }
        for (int i = 0; i < NUM_PARTS; i++)
        {
            instantiated[i] = null;
        }
        createLoc = new Vector3(-40, 25, 100);
        offscreenCreateLoc = new Vector3(-40, -60, 100);
        selectionManager = eventSystem.GetComponent<SelectPart>();
        startObject = GameObject.Find("bb1Start");
        GameObject bb1B1p1A1 = startObject.transform.Find("bb1_b1p1_a1").gameObject;
        GameObject bb1B1p2A1 = startObject.transform.Find("bb1_b1p2_a1").gameObject;
        GameObject bb1B1p2A2 = startObject.transform.Find("bb1_b1p2_a2").gameObject;
        GameObject bb1B1p3A1 = startObject.transform.Find("bb1_b1p3_a1").gameObject;
        //to avoid errors when selectedObject starts as startObject
        bb1B1p1A1.GetComponent<FuseBehavior>().isFused = true;
        bb1B1p2A1.GetComponent<FuseBehavior>().isFused = true;
        bb1B1p2A2.GetComponent<FuseBehavior>().isFused = true;
        bb1B1p3A1.GetComponent<FuseBehavior>().isFused = true;

        rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();

    }

    // y+ = up, y- = down
    // z+ = back, z- = front
    // x+ = right, x- = left
    // (looking at boot from the front)

    //returns list of objects body can fuse to
    public FuseAttributes b1p1Fuses()
    {
        GameObject bb1 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb1Pos = bb1.transform.position;
        Vector3 fuseLocation = new Vector3(bb1Pos.x, bb1Pos.y, bb1Pos.z - 22.5f);
        fuseLocations.Add("bb1_b1p1_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fuseRotations.Add("bb1_b1p1_a1", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 0, 0);
        Quaternion acceptableRotation2 = Quaternion.Euler(0, 90, 270);
        Quaternion acceptableRotation3 = Quaternion.Euler(90, 180, 0);
        Quaternion acceptableRotation4 = Quaternion.Euler(0, 270, 90);
        Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, acceptableRotation3, acceptableRotation4};
        fusePositions.Add("bb1_b1p1_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b1p2Fuses()
    {
        GameObject bb1 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


        Vector3 bb1Pos = bb1.transform.position;
        Vector3 fuseLocation = new Vector3(bb1Pos.x + 7.6f, bb1Pos.y, bb1Pos.z - 4.3f);
        fuseLocations.Add("bb1_b1p2_a1", fuseLocation);
        fuseLocations.Add("bb1_b1p2_a2", fuseLocation);
        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fuseRotations.Add("bb1_b1p2_a1", fuseRotation);
        fuseRotations.Add("bb1_b1p2_a2", fuseRotation);

        Quaternion acceptableRotation1 = Quaternion.Euler(270, 0, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1 };
        fusePositions = new Dictionary<string, Quaternion[]>();
        fusePositions.Add("bb1_b1p2_a1", acceptableRotations);
        fusePositions.Add("bb1_b1p2_a2", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    public FuseAttributes b1p3Fuses()
    {
        GameObject bb1 = startObject;
        Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
        Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
        Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

        Vector3 bb1Pos = bb1.transform.position;
        Vector3 fuseLocation = new Vector3(bb1Pos.x - 4.5f, bb1Pos.y, bb1Pos.z + 10.15f);
        fuseLocations.Add("bb1_b1p3_a1", fuseLocation);

        Quaternion fuseRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fuseRotations.Add("bb1_b1p3_a1", fuseRotation);
        Quaternion acceptableRotation1 = Quaternion.Euler(270, 0, 0);
        Quaternion[] acceptableRotations = { acceptableRotation1};

        fusePositions.Add("bb1_b1p3_a1", acceptableRotations);

        FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

        return newAttributes;

    }

    //when a new part is created, clear partsCreated
    public void clearPartsCreated()
    {
        for (int i = 0; i < partCreated.Length; i++)
        {
            partCreated[i] = false;
        }
        for (int i = 0; i < instantiated.Length; i++)
        {
            if (instantiated[i] != null && !instantiated[i].GetComponent<IsFused>().isFused)
            {
                Destroy(instantiated[i]);
                partButtons[i].interactable = true;
            }
        }
    }

    public void enableManipulationButtons(GameObject toRotate)
    {
        rotateYButton.transform.GetComponent<Button>().interactable = true;
        rotateXButton.transform.GetComponent<Button>().interactable = true;
        rotateZButton.transform.GetComponent<Button>().interactable = true;

        rotateYButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
        rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
        rotateZButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
    }

    // Makes the newly created part zip up from a lower point as it's created, making it seem like it was pulled up from the ground
    IEnumerator moveToStartingPosition(GameObject part)
    {
        // while the part hasn't reached its destination and while it hasn't been destroyed by choosing another part
        while (part != null && !part.transform.position.Equals(createLoc))
        {
            step = MOVEMENT_SPEED * Time.deltaTime;
            part.transform.position = Vector3.MoveTowards(part.transform.position, createLoc, step);

            yield return new WaitForSeconds(WAIT_TIME);
        }
        Debug.Log("finished moving " + part.name + "!");
    }


    public void createB1p1()
    {
        if (!partCreated[0])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated		
            Quaternion fuseToRotation = Quaternion.Euler(90, 90, 0);
            GameObject newB1p1 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[0], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB1p1)); // this creates the zooming up from the ground effect

            Transform b1p1_bb1_a1 = newB1p1.transform.Find("b1p1_bb1_a1");

            FuseAttributes fuseAtts = b1p1Fuses();

            b1p1_bb1_a1.gameObject.AddComponent<FuseBehavior>();
            b1p1_bb1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b1p1_bb1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B1p1"));

            b1p1_bb1_a1.gameObject.AddComponent<FaceSelector>();
            b1p1_bb1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b1p1_bb1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());


            instantiated[0] = newB1p1;
            partCreated[0] = true;
            partButtons[0].interactable = false;

            selectionManager.newPartCreated("b1p1Prefab(Clone)");

            enableManipulationButtons(newB1p1);


        }
    }

    public void createB1p2()
    {
        if (!partCreated[1])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0, 180, 0);
            GameObject newB1p2 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[1], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB1p2)); // this creates the zooming up from the ground effect

            Transform b1p2_bb1_a1 = newB1p2.transform.Find("b1p2_bb1_a1");
            Transform b1p2_bb1_a2 = newB1p2.transform.Find("b1p2_bb1_a2");

            FuseAttributes fuseAtts = b1p2Fuses();

            b1p2_bb1_a1.gameObject.AddComponent<FuseBehavior>();
            b1p2_bb1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b1p2_bb1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B1p2"));

            b1p2_bb1_a1.gameObject.AddComponent<FaceSelector>();
            b1p2_bb1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b1p2_bb1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            b1p2_bb1_a2.gameObject.AddComponent<FuseBehavior>();
            b1p2_bb1_a2.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b1p2_bb1_a2.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B1p2"));

            b1p2_bb1_a2.gameObject.AddComponent<FaceSelector>();
            b1p2_bb1_a2.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b1p2_bb1_a2.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());


            instantiated[1] = newB1p2;
            partCreated[1] = true;
            partButtons[1].interactable = false;

            selectionManager.newPartCreated("b1p2Prefab(Clone)");

            enableManipulationButtons(newB1p2);


        }
    }

    public void createB1p3()
    {
        if (!partCreated[2])
        {
            clearPartsCreated();
            Vector3 pos = offscreenCreateLoc; // this is where the object will appear when it's instantiated
            Quaternion fuseToRotation = Quaternion.Euler(0,90,270);
            GameObject newB1p3 = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));
            StartCoroutine(moveToStartingPosition(newB1p3)); // this creates the zooming up from the ground effect

            Transform b1p3_bb1_a1 = newB1p3.transform.Find("b1p3_bb1_a1");

            FuseAttributes fuseAtts = b1p3Fuses();

            b1p3_bb1_a1.gameObject.AddComponent<FuseBehavior>();
            b1p3_bb1_a1.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
            b1p3_bb1_a1.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find("B1p3"));

            b1p3_bb1_a1.gameObject.AddComponent<FaceSelector>();
            b1p3_bb1_a1.gameObject.GetComponent<FaceSelector>().setSelectPartScript(GameObject.Find("EventSystem").GetComponent<SelectPart>());
            b1p3_bb1_a1.gameObject.GetComponent<FaceSelector>().setFuseButton(GameObject.Find("FuseButton").GetComponent<Button>());

            instantiated[2] = newB1p3;
            partCreated[2] = true;
            partButtons[2].interactable = false;

            selectionManager.newPartCreated("b1p3Prefab(Clone)");

            enableManipulationButtons(newB1p3);


        }
    }

    //checks to see if an object has been fused already
    public bool alreadyFused(string part)
    {
        GameObject partInstance = GameObject.Find(part);
        if (partInstance != null && !partInstance.GetComponent<FuseBehavior>().fused())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
