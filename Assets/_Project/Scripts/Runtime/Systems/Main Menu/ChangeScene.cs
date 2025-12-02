using UnityEngine;
using UnityEngine.SceneManagement;

public enum ScenesInGame
{
    MainMenu = 0,
    Gameplay = 1
}

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private ScenesInGame _myScene;


    public void LoadScene()
    {
        SceneManager.LoadScene(_myScene.ToString());
    }
}
