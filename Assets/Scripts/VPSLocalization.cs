using System.Collections.Generic;
using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using UnityEngine;
using R3;

public class VPSLocalization : MonoBehaviour
{
    [SerializeField] private ARLocationManager arLocationManager;

    [SerializeField] private GameObject debugARLocationGameObject;

    public ReactiveProperty<bool> IsTracked { get; } = new();

    void Start()
    {
        IsTracked.Value = false;
        arLocationManager.locationTrackingStateChanged += OnLocationTrackingStateChanged;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsTracked.Value = true;
            debugARLocationGameObject.SetActive(true);
        }
    }

    private void OnLocationTrackingStateChanged(ARLocationTrackedEventArgs args)
    {
        if (args.Tracking)
        {
            args.ARLocation.gameObject.SetActive(true);
            IsTracked.Value = true;
        }
        else
        {
            // PrivateなLocationはTrackingがfalseで、ARLocationTrackingStateReason.Limitedになります
            if (args.TrackingStateReason == ARLocationTrackingStateReason.Limited)
            {
                args.ARLocation.gameObject.SetActive(true);
                IsTracked.Value = true;
            }
            else
            {
                args.ARLocation.gameObject.SetActive(false);
                IsTracked.Value = false;
            }
        }
    }
}