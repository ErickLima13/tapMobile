using UnityEngine;

public class RewardTester : MonoBehaviour
{
    public RewardedAdController rewardedAdController;

    public int coins;

    public void OnClickShowRewarded()
    {
        rewardedAdController.ShowRewardedAd();
    }

    private void GiveCoins()
    {
        coins += 50;
        Debug.Log($"RewardTester: Player received 50 coins. Total coins = {coins}");
    }
}
