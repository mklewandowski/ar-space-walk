using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera = null;
    [SerializeField]
    private ARAnchorManager anchorManager = null;
    [SerializeField]
    private TextMeshProUGUI DebugText;
    [SerializeField]
    GameObject[] PlanetsToPlace;

    [SerializeField]
    ARPlaneManager arPlaneManager = null;

    private bool initialized = false;

    private float distanceFromCamera = .3f;

    private List<GameObject> placedPlanets = new List<GameObject>();
    private int planetIndex = 0;
    string[] planetNames = {"the Sun", "Mercury", "Venus", "Earth", "Mars", "the Asteroid Belt", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"};
    string[] groupedPlanetNames = {"the Sun", "the Terrestrial Planets", "the Asteroid Belt", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"};

    [SerializeField]
    bool UsePlanetGroups = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        arPlaneManager.planesChanged += PlanesChanged;
    }

    public void AddCelestialObjects()
    {
        if (planetIndex == 1 && UsePlanetGroups)
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
            planetIndex = 5;
        }
        else
        {
            AddSingleCelestialObject();
        }
    }

    public void AddSingleCelestialObject()
    {
        Vector3 centerPosition = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera));
        GameObject go = GameObject.Instantiate(PlanetsToPlace[planetIndex], centerPosition, Quaternion.identity);
        float scale = 1f;
        if (planetIndex == 0)
            scale = 1000f;
        go.transform.localScale = new Vector3(.00025f * scale, .00025f * scale, .00025f * scale);
        if (go.GetComponent<ARAnchor>() == null)
        {
            go.AddComponent<ARAnchor>();
        }
        placedPlanets.Add(go);
        planetIndex++;

        DebugText.text = "Explore " + (UsePlanetGroups ? groupedPlanetNames[planetIndex - 1] : planetNames[planetIndex - 1]) + " or keep walking.";
    }

    public void AddAllCelestialObjects()
    {
        Vector3 pos0 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera));
        AddCelestialObjectAtPos(pos0, 0);
        Vector3 pos1 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 1.04f));
        pos1.y = pos0.y;
        AddCelestialObjectAtPos(pos1, 1);
        Vector3 pos2 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 1.941f));
        pos2.y = pos0.y;
        AddCelestialObjectAtPos(pos2, 2);
        Vector3 pos3 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 2.686f));
        pos3.y = pos0.y;
        AddCelestialObjectAtPos(pos3, 3);
        Vector3 pos4 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 4.092f));
        pos4.y = pos0.y;
        AddCelestialObjectAtPos(pos4, 4);
        Vector3 pos5 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 8.5f));
        pos5.y = pos0.y;
        AddCelestialObjectAtPos(pos5, 5);
        Vector3 pos6 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 13.976f));
        pos6.y = pos0.y;
        AddCelestialObjectAtPos(pos6, 6);
        Vector3 pos7 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 25.63f));
        pos7.y = pos0.y;
        AddCelestialObjectAtPos(pos7, 7);
        Vector3 pos8 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 51.553f));
        pos8.y = pos0.y;
        AddCelestialObjectAtPos(pos8, 8);
        Vector3 pos9 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 80.822f));
        pos9.y = pos0.y;
        AddCelestialObjectAtPos(pos9, 9);
        Vector3 pos10 = arCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera + 106.203f));
        pos10.y = pos0.y;
        AddCelestialObjectAtPos(pos10, 10);

        DebugText.text = "Explore the solar system.";
    }

    void AddCelestialObjectAtPos(Vector3 position, int planetIndex)
    {
        GameObject go = GameObject.Instantiate(PlanetsToPlace[planetIndex], position, Quaternion.identity);
        float scale = 1f;
        if (planetIndex == 0)
            scale = 1000f;
        go.transform.localScale = new Vector3(.00025f * scale, .00025f * scale, .00025f * scale);
        // Add an ARAnchor component if it doesn't have one already.
        if (go.GetComponent<ARAnchor>() == null)
        {
            go.AddComponent<ARAnchor>();
        }
        placedPlanets.Add(go);
    }

    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!initialized)
        {
            Activate();
        }
    }

    private void Activate()
    {
        DebugText.text = "Press Place Planet to place  " + (UsePlanetGroups ? groupedPlanetNames[planetIndex - 1] : planetNames[planetIndex - 1]) + ".";
        initialized = true;
        arPlaneManager.enabled = false;
    }

    public void NewPlanetThresholdPassed(int pIndex)
    {
        planetIndex = pIndex;
        NewPlanetDetected();
    }

    public void NewPlanetDetected()
    {
        DebugText.text = "Press Begin Placement to add " + (UsePlanetGroups ? groupedPlanetNames[planetIndex - 1] : planetNames[planetIndex - 1]) + ".";
    }

    public void StartWalk()
    {
        DebugText.text = "Press Begin Placement to detect planes and add the Sun.";
    }

    public void BeginAddingPlanet()
    {
        DebugText.text = "Move camera to detect placement planes.";
        ClearPreviousPlanesAndPanets();
        arPlaneManager.enabled = true;
    }

    public void Reset(bool isThreshold)
    {
        ClearPreviousPlanesAndPanets();
        arPlaneManager.enabled = false;
        planetIndex = 0;
        if (isThreshold)
            DebugText.text = "Press Start to begin walk and place the Sun.";
        else
            DebugText.text = "Press Simulate Detection to simulate planet detection on space walk.";
    }

    private void ClearPreviousPlanesAndPanets()
    {
        initialized = false;
        for (int x = 0; x < placedPlanets.Count; x++)
        {
            Destroy(placedPlanets[x]);
        }
        placedPlanets.Clear();
    }

}
