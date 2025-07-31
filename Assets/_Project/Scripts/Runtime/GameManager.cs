using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AreaCollider[] _areasColliders;

    public CheckTapAction _checkTapAction;

    private void Start()
    {
        foreach(AreaCollider a in _areasColliders)
        {
            a.OnDisableCollider += CheckDisable;
        }

        _checkTapAction.OnTapCollider += CheckTap;
    }

    private void CheckTap()
    {
        print("tap in time");
    }

    private void CheckDisable()
    {
        print("disable" + _areasColliders.Length);
    }

    private void OnDisable()
    {
        foreach (AreaCollider a in _areasColliders)
        {
            a.OnDisableCollider -= CheckDisable;
        }

        _checkTapAction.OnTapCollider -= CheckTap;
    }
}
