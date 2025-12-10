using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;


    private void Start()
    {
        _checkTapAction.OnAnimation += SetPosition;
    }

    private void OnDisable()
    {
        _checkTapAction.OnAnimation -= SetPosition;
    }

    private void SetPosition(float x)
    {
        transform.position = new(x, transform.position.y);
    }
}
