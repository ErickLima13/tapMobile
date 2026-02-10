using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111"; // Test banner
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716"; // Test banner
#else
    private string _adUnitId = "unused";
#endif

    private BannerView _bannerView;
    private ScreenOrientation _currentOrientation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _currentOrientation = Screen.orientation;
        Debug.Log($"BannerAdController: Initial orientation = {_currentOrientation}");

        CreateAndLoadBanner();
    }

    private void Update()
    {
        if (Screen.orientation != _currentOrientation)
        {
            Debug.Log($"BannerAdController: Orientation changed to {Screen.orientation}");
            _currentOrientation = Screen.orientation;
            RecreateBannerForNewOrientation();
        }
    }

    private void RecreateBannerForNewOrientation()
    {
        if (_bannerView != null)
        {
            Debug.Log("BannerAdController: Destroying old banner.");
            _bannerView.Destroy();
            _bannerView = null;
        }

        CreateAndLoadBanner();
    }

    private void CreateAndLoadBanner()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("BannerAdController: Unsupported platform.");
            return;
        }

        Debug.Log($"BannerAdController: Creating banner for orientation: {_currentOrientation}");

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("BannerAdController: Banner loaded.");
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError($"BannerAdController: Failed to load banner. Reason: {error.GetMessage()}");
        };

        AdRequest request = new AdRequest();
        Debug.Log("BannerAdController: Loading banner...");
        _bannerView.LoadAd(request);
    }

    private void OnDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }
}

