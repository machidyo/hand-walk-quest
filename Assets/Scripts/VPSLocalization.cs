using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using UnityEngine;

public class VPSLocalization : MonoBehaviour
{
    [SerializeField] private ARLocationManager arLocationManager;

    void Start()
    {
        arLocationManager.locationTrackingStateChanged += OnLocationTrackingStateChanged;
    }

    private void OnLocationTrackingStateChanged(ARLocationTrackedEventArgs args)
    {
        if (args.Tracking)
        {
            args.ARLocation.gameObject.SetActive(true);
        }
        else
        {
            // PrivateなLocationはTrackingがfalseで、ARLocationTrackingStateReason.Limitedになります
            if (args.TrackingStateReason == ARLocationTrackingStateReason.Limited)
            {
                args.ARLocation.gameObject.SetActive(true);
            }
            else
            {
                args.ARLocation.gameObject.SetActive(false);
            }
        }
    }
}