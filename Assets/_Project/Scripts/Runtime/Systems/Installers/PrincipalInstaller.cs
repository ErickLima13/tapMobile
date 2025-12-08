using UnityEngine;
using Zenject;

public class PrincipalInstaller : MonoInstaller
{
    [SerializeField] private EnemyCollider[] _areasColliders;
    [SerializeField] private CheckTapAction _checkTapAction;
    [SerializeField] private PlayerStatus _playerStatus;


    public override void InstallBindings()
    {
        Container.Bind<EnemyCollider>().FromInstance(_areasColliders[0]);

        Container.Bind<CheckTapAction>().FromInstance(_checkTapAction);
        Container.Bind<PlayerStatus>().FromInstance(_playerStatus);
    }
}