using UnityEngine;
using Zenject;

public class PrincipalInstaller : MonoInstaller
{
    [SerializeField] private CheckTapAction _checkTapAction;
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private DamagePlayer _damagePlayer;
    [SerializeField] private ScreenLimits _screenLimits;
    [SerializeField] private ObjectPooler _objectPooler;
    [SerializeField] private WaveController _waveController;

    public override void InstallBindings()
    {
        //Container.BindFactory<GameObject, Enemy, Vector3, EnemyCollider, EnemyFactory>().FromFactory<PrefabEnemyfactory>();

        Container.Bind<CheckTapAction>().FromInstance(_checkTapAction);
        Container.Bind<PlayerStatus>().FromInstance(_playerStatus);
        Container.Bind<DamagePlayer>().FromInstance(_damagePlayer);
        Container.Bind<ScreenLimits>().FromInstance(_screenLimits);
        Container.Bind<ObjectPooler>().FromInstance(_objectPooler);
        Container.Bind<WaveController>().FromInstance(_waveController);
    }
}