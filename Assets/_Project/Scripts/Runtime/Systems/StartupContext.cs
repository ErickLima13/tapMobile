using UnityEngine.SceneManagement;
using Zenject;

public class StartupContext : MonoInstaller
{
    public override void InstallBindings()
    {
        SceneManager.LoadScene(0);
    }
}