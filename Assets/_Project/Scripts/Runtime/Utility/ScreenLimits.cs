using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenLimits : MonoBehaviour
{
    [SerializeField] private Vector2 screenBounds;

    private Camera main;

    public GameObject _testArea;

    private void Awake()
    {
        main = Camera.main;

        screenBounds = main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, main.transform.position.z));

        _ = Delay();
    }

    private async Task Delay()
    {
        await UniTask.WaitForEndOfFrame();

        float x = screenBounds.x * 2 - 1f;
        _testArea.transform.localScale = new Vector3(x, screenBounds.y);

    }

    public Vector2 GetLimits()
    {
        return screenBounds;
    }

}
