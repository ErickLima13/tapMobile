using UnityEngine;

public class ScreenLimits : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private float offset = 0.60f;
    private Camera main;

    [SerializeField] private Sprite _spriteReference;

    private void Awake()
    {
        main = Camera.main;

        screenBounds = main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, main.transform.position.z));
        objectWidth = _spriteReference.bounds.size.x / 2;
        objectHeight = _spriteReference.bounds.size.y / 2;
    }

    public Vector2 GetScreenLimits()
    {
        return new Vector2((screenBounds.x - objectWidth) - offset, 2.8f);
    }

}
