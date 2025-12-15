using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;
    [Inject] private ObjectPooler _objectPooler;


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
        float currentX = transform.position.x;

        if (x != currentX)
        {
            _objectPooler.SpawnFromPoolWithReturn("smoke", transform.position, Quaternion.identity);
        }

        transform.position = new(x, transform.position.y);

    }
}
