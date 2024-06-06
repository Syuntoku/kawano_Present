using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase
{
    bool _bActive;
    public GameObject _connectObject;
    public float _speed;
    public float _timer;
    public float _destroyTime;
    public GameObject _hitEfect;
    static float AJUST_DESTROY_TIME = 0.1f;

    public BulletBase()
    {
        _bActive = true;
    }

    public bool IsActive()
    {
        return _bActive;
    }

    public void Update()
    {
        if (_connectObject == null) return;

        _connectObject.transform.position += _connectObject.transform.forward * _speed * Time.deltaTime;

        _timer += Time.deltaTime;

        if (_timer + AJUST_DESTROY_TIME >= _destroyTime)
        {
            DestroyObject();
            _bActive = false;
        }
    }

    public void DestroyObject()
    {

        // 何にも当たらない場合HitエフェクトはNullである
        if (_hitEfect != null)
        {
            Object.Instantiate(_hitEfect, _connectObject.transform.position, Quaternion.identity);
        }

        // 弾を消しても弾道エフェクトは残す、トレイルの設定から自動で破棄される
        var trail = _connectObject.transform.Find("Trail");
        trail.SetParent(null);
        //GameObject.Destroy(trail.gameObject, 0.5f);
        GameObject.Destroy(_connectObject);
    }
}
