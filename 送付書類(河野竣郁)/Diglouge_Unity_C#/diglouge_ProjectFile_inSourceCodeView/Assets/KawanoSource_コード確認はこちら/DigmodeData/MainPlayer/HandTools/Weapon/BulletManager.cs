using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BulletManager : MonoBehaviour
{
    Queue<BulletBase> _queBulletData;
    const int DRAW_MAX = 30;

    private void Start()
    {
        _queBulletData = new Queue<BulletBase>(DRAW_MAX);
    }

    private void Update()
    {
        UniTask.SwitchToThreadPool();

        while (true)
        {
            BulletBase work;

            if (!_queBulletData.TryPeek(out work)) break;

            if (!work.IsActive())
            {
                _queBulletData.Dequeue();
            }
            else
            {
                break;
            }
        }

        foreach (BulletBase item in _queBulletData)
        {
            item.Update();
        }
    }

    public void AddBulletObject(BulletBase bulletBase)
    {
        //Å‘å•\Ž¦‚ª’´‚¦‚½ê‡‚ÍÁ‚·
        if (_queBulletData.Count >= DRAW_MAX)
        {
            _queBulletData.Peek().DestroyObject();
            _queBulletData.Dequeue();
        }

        _queBulletData.Enqueue(bulletBase);
    }
}


