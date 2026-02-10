using UnityEngine;
using GoogleMobileAds.Api;

public class AdsInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Keep this object alive across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("AdsInitializer: Initializing Mobile Ads...");

        MobileAds.Initialize(initStatus =>
        {
            if (initStatus == null)
            {
                Debug.LogError("AdsInitializer: Initialization failed.");
                return;
            }

            Debug.Log("AdsInitializer: Mobile Ads initialized.");
        });
    }
}
