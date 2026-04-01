using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int MaxLife;
    public int CurrentLife;

    public int Score;

    public event Action<PointType, int> OnUpdateHud;

    public event Action OnPlayerDamage;
    public event Action OnGameOver;

    public void IncreaseScore(PointType value)
    {
        Score++;
        OnUpdateHud?.Invoke(value, Score);
    }

    public void DamageEvent(PointType value)
    {
        TakeDamage();
        OnUpdateHud?.Invoke(value, CurrentLife);
    }

    public void TakeDamage()
    {
        if (CurrentLife <= MaxLife)
        {
            CurrentLife--;

            OnPlayerDamage?.Invoke();

            if (CurrentLife <= 0)
            {
                CurrentLife = 0;

                //OnGameOver?.Invoke();

                OnGameOver?.Invoke();
            }
        }
    }
}
