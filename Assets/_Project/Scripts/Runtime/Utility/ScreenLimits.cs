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

        float x = screenBounds.x * 2 - 1f;
        _testArea.transform.localScale = new Vector3(x, screenBounds.y + 1);
    }

    private void Start()
    {
      
    }
}
