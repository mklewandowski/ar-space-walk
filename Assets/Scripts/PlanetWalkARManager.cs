using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class PlanetWalkARManager : MonoBehaviour
{
    PlanetWalkLocationManager locationManager;

    [SerializeField]
    private Camera arCamera = null;
    [SerializeField]
    private ARAnchorManager anchorManager = null;
    [SerializeField]
    private TextMeshProUGUI DebugText;
    [SerializeField]
    GameObject[] CelestialObjectsToPlace;

    [SerializeField]
    GameObject StartButton;
    [SerializeField]
    GameObject BeginPlacementButton;
    [SerializeField]
    GameObject PlacementButton;

    [SerializeField]
    ARPlaneManager arPlaneManager = null;
    bool waitingForPlanes = false;

    private float distanceFromCamera = .5f;

    private List<GameObject> placedObjects = new List<GameObject>();
    private int walkThresholdIndex = 0;
    private int celestialObjectIndex = 0;
    string[] celestialObjectsNames = {"the Sun", "the Terrestrial Planets", "the Asteroid Belt", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"};

    void Start()
    {
        Application.targetFrameRate = 60;
        locationManager = this.GetComponent<PlanetWalkLocationManager>();
        arPlaneManager.planesChanged += PlanesChanged;
        arPlaneManager.enabled = false;
    }

    // START WALK
    public void StartWalk()
    {
        StartButton.SetActive(false);
        DebugText.text = "Move camera to detect placement planes.";
        waitingForPlanes = true;
        arPlaneManager.enabled = true;
    }

    // PLANE DETECTION
    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (waitingForPlanes)
        {
            Activate();
        }
    }

    private void Activate()
    {
        DebugText.text = "Press Place Object to place " + celestialObjectsNames[walkThresholdIndex] + ".";
        waitingForPlanes = false;
        arPlaneManager.enabled = false;
        PlacementButton.SetActive(true);
    }

    // PLACE CELESTIAL OBJECT
    public void AddCelestialObject()
    {
        if (walkThresholdIndex == 1) // add terrestrials
        {
            Vector3 pos0 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera));
            AddCelestialObjectAtPos(pos0, 1);
            Vector3 pos1 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + .901f));
            pos1.y = pos0.y;
            AddCelestialObjectAtPos(pos1, 2);
            Vector3 pos2 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 1.646f));
            pos2.y = pos0.y;
            AddCelestialObjectAtPos(pos2, 3);
            Vector3 pos3 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 3.052f));
            pos3.y = pos0.y;
            AddCelestialObjectAtPos(pos3, 4);
            celestialObjectIndex = 5;
        }
        else
        {
            Vector3 centerPosition = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera));
            AddCelestialObjectAtPos(centerPosition, celestialObjectIndex);
            celestialObjectIndex++;
        }
        if (walkThresholdIndex == 0) // place the sun
        {
            locationManager.SetStartLocation();
        }
        DebugText.text = "Explore  " + celestialObjectsNames[walkThresholdIndex] + " or keep walking.";
        PlacementButton.SetActive(false);
    }

    void AddCelestialObjectAtPos(Vector3 position, int celestialObjectIndex)
    {
        GameObject go = GameObject.Instantiate(CelestialObjectsToPlace[celestialObjectIndex], position, Quaternion.identity);
        float scale = 1f;
        if (celestialObjectIndex == 0)
            scale = 1000f;
        go.transform.localScale = new Vector3(.00025f * scale, .00025f * scale, .00025f * scale);
        if (go.GetComponent<ARAnchor>() == null)
        {
            go.AddComponent<ARAnchor>();
        }
        placedObjects.Add(go);
    }

    // PASSED A NEW CELESTIAL OBJECT
    public void NewCelestialObjectThresholdPassed(int pIndex)
    {
        walkThresholdIndex = pIndex;
        BeginPlacementButton.SetActive(true);
        DebugText.text = "Press Begin Placement to add " + celestialObjectsNames[walkThresholdIndex] + ".";
    }

    // BEGIN PLACEMENT OF NEW OBJECT
    public void BeginPlacement()
    {
        BeginPlacementButton.SetActive(false);
        DebugText.text = "Move camera to detect placement planes.";
        waitingForPlanes = true;
        arPlaneManager.enabled = true;
    }
}
