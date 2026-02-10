using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Test rewarded
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313"; // Test rewarded
#else
    private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    public event Action OnRewardEvent;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("RewardedAdController: Unsupported platform.");
            return;
        }

        Debug.Log("RewardedAdController: Requesting rewarded ad...");

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"RewardedAdController: Failed to load. Reason: {error.GetMessage()}");
                return;
            }

            Debug.Log("RewardedAdController: Rewarded ad loaded.");
            _rewardedAd = ad;

            RegisterEvents(_rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("RewardedAdController: Showing rewarded ad.");
            _rewardedAd.Show(reward =>
            {
                Debug.Log($"RewardedAdController: User earned reward: {reward.Amount} {reward.Type}");
                OnRewardEvent?.Invoke();
            });
        }
        else
        {
            Debug.Log("RewardedAdController: Not ready, loading new one.");
            LoadRewardedAd();
        }
    }

    private void RegisterEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("RewardedAdController: Ad closed. Loading next one.");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"RewardedAdController: Failed to show. Reason: {error.GetMessage()}");
        };
    }

    private void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }
    }
}
