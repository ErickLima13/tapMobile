using UnityEngine;
using Zenject;

public class PrincipalInstaller : MonoInstaller
{
    [SerializeField] private CheckTapAction _checkTapAction;
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private DamagePlayer _damagePlayer;


    public override void InstallBindings()
    {
        Container.Bind<CheckTapAction>().FromInstance(_checkTapAction);
        Container.Bind<PlayerStatus>().FromInstance(_playerStatus);
        Container.Bind<DamagePlayer>().FromInstance(_damagePlayer);
    }
}