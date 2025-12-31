using System.Collections.Generic;
using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using UnityEngine;

public class VPSLocalization : MonoBehaviour
{
    [SerializeField] private ARLocationManager arLocationManager;
    [SerializeField] private List<GameObject> coins;

    void Start()
    {
        HideCoins();
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
                ShowCoins();
            }
            else
            {
                args.ARLocation.gameObject.SetActive(false);
            }
        }
    }

    private void ShowCoins()
    {
        foreach (var coin in coins)
        {
            coin.SetActive(true);
        }
    }

    private void HideCoins()
    {
        foreach (var coin in coins)
        {
            coin.SetActive(false);
        }
    }
}