using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test interstitial
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910"; // Test interstitial
#else
    private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitial;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("InterstitialAdController: Unsupported platform.");
            return;
        }

        Debug.Log("InterstitialAdController: Requesting interstitial...");

        AdRequest request = new AdRequest();

        InterstitialAd.Load(_adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"InterstitialAdController: Failed to load. Reason: {error.GetMessage()}");
                return;
            }

            Debug.Log("InterstitialAdController: Interstitial loaded.");
            _interstitial = ad;

            RegisterEvents(_interstitial);
        });
    }

    public void ShowInterstitial()
    {
        if (_interstitial != null && _interstitial.CanShowAd())
        {
            Debug.Log("InterstitialAdController: Showing interstitial.");
            _interstitial.Show();
        }
        else
        {
            Debug.Log("InterstitialAdController: Not ready, loading a new one.");
            LoadInterstitial();
        }
    }

    private void RegisterEvents(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("InterstitialAdController: Interstitial closed. Loading another.");
            LoadInterstitial();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"InterstitialAdController: Failed to show. Reason: {error.GetMessage()}");
        };
    }

    private void OnDestroy()
    {
        if (_interstitial != null)
        {
            _interstitial.Destroy();
        }
    }
}

