using Zenject;

public class PrincipalInstaller : MonoInstaller
{
    public EnemyCollider[] _areasColliders;

    public override void InstallBindings()
    {
        Container.Bind<EnemyCollider>().FromInstance(_areasColliders[0]);
        Container.Bind<EnemyCollider>().FromInstance(_areasColliders[1]);
    }
}