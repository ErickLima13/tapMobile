using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PoolListController : MonoBehaviour
{
    [Inject] private ObjectPooler _objectPooler;

    public List<GameObject> _enemies;


    private void Start()
    {
        _ = StartDelay();
    }

    private async Task StartDelay()
    {
        await UniTask.WaitForEndOfFrame();

        foreach(GameObject g in _objectPooler.poolDictionary["enemy"])
        {
            _enemies.Add(g);
        }

        print(_objectPooler.poolDictionary["enemy"].Count);
    }

}
