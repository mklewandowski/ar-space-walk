using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using TMPro;

public class PlanetWalkLocationManager : MonoBehaviour
{
    PlanetWalkARManager arManager;

    [SerializeField]
    TextMeshProUGUI LocationText;
    [SerializeField]
    TextMeshProUGUI DistanceText;
    [SerializeField]
    TextMeshProUGUI StatusText;

    GameObject dialog = null;
    bool locationIsReady = false;
    bool locationGrantedAndroid = false;

    double startLat = 0;
    double startLong = 0;
    double currLat = 0;
    double currLong = 0;
    double currAcc = 0;

    double[] thresholds = {3, 8.5, 13.976, 25.63, 51.553, 80.822, 106.203};
    int currentThreshold = 0;
    bool startLocationMarked = false;
    float maxAcceptableAccuracy = 5;

    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
        else
        {
            locationGrantedAndroid = true;
            locationIsReady = NativeGPSPlugin.StartLocation();
        }

#elif PLATFORM_IOS
        locationIsReady = NativeGPSPlugin.StartLocation();
#endif
        arManager = this.GetComponent<PlanetWalkARManager>();
    }

    public void Reset()
    {
        currentThreshold = 0;
        startLocationMarked = false;
        startLat = 0;
        startLong = 0;
    }

    private void Update()
    {
        if (locationIsReady)
        {
            // retrieves the device's current location
            double lat = NativeGPSPlugin.GetLatitude();
            double lon = NativeGPSPlugin.GetLongitude();
            double acc = NativeGPSPlugin.GetAccuracy();
            string loc = "LOC: " + lat + ", " + lon;
            Debug.Log(loc);
            if (acc < maxAcceptableAccuracy)
            {
                currLat = lat;
                currLong = lon;
                currAcc = acc;
                LocationText.text = loc;
                if (startLat != 0 && startLong != 0)
                    ComputeDistance();
            }

            if (startLocationMarked && currentThreshold < thresholds.Length)
            {
                double dist = DistanceBetweenPointsInMeters(startLat, startLong, currLat, currLong);
                if (dist > thresholds[currentThreshold])
                {
                    currentThreshold++;
                    StatusText.text = "Passed threshold " + currentThreshold;
                    arManager.NewCelestialObjectThresholdPassed(currentThreshold);
                }
            }
        }
    }

    void OnGUI ()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // The user denied permission to use the fineLocation.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if (dialog != null)
        {
            if (!locationGrantedAndroid)
            {
                locationGrantedAndroid = true;
                locationIsReady = NativeGPSPlugin.StartLocation();
            }

            Destroy(dialog);
        }
        #endif
    }

    public void SetStartLocation()
    {
        startLat = currLat;
        startLong = currLong;
        currentThreshold = 0;
        startLocationMarked = true;
    }

    public void ComputeDistance()
    {
        double dist = DistanceBetweenPointsInMeters(startLat, startLong, currLat, currLong);
        DistanceText.text = "D: " + dist + "m";
    }

    public static double DistanceBetweenPointsInMeters(double lat1, double lon1, double lat2, double lon2)
    {
        double rlat1 = System.Math.PI * lat1 / 180;
        double rlat2 = System.Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Mathf.PI * theta / 180;
        double dist =
            System.Math.Sin(rlat1) * System.Math.Sin(rlat2) + System.Math.Cos(rlat1) *
            System.Math.Cos(rlat2) * System.Math.Cos(rtheta);
        dist = System.Math.Acos(dist);
        dist = dist * 180 / System.Math.PI;

        // 60 is the number of minutes in a degree
        // 1.1515 is the number of statute miles in a nautical mile
        // One nautical mile is the length of one minute of latitude at the equator
        // this gives us the distance in miles
        double distInMiles = dist * 60 * 1.1515;
        // 1.609344 is the number of kilometres in a mile
        // 1000 is the number of metres in a kilometre
        double distInMeters = distInMiles * 1.609344 * 1000;
        return distInMeters;
    }
}
