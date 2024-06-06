using System.Collections;
using System.Collections.Generic;
using Syuntoku.DigMode.Enemy;
using Syuntoku.DigMode;
using UnityEngine;
using Syuntoku.Status;
using DG.Tweening;

public class EnemyBulletManager : MonoBehaviour
{
    public List<BulletStatus> _bullets;
    public const int BULLET_MAX = 100;
    static public BlockManage _blockManage;
    public const float DEFAULT_SIZE = 0.01f;
    void Start()
    {
        _bullets = new List<BulletStatus>(BULLET_MAX);
        _blockManage = GameObject.Find("BlockManage").GetComponent<BlockManage>();
    }

    void Update()
    {
        
        foreach (BulletStatus item in _bullets)
        {
            item.Update();        
        }

        for (int i = 0; i < _bullets.Count; i++)
        {
            if (!_bullets[i]._bDestroy) continue;

            Destroy(_bullets[i]._instanceBullet);
            _bullets.Remove(_bullets[i]);
        }
    }

    public void InstanceBullet(BulletStatus bulletStatus,Vector3 startPos,Vector3 direction)
    {
        bulletStatus.InstanceBullet(startPos, direction);
        _bullets.Add(bulletStatus);
    }

    /// <summary>
    /// ダメージを与えるオブジェクトを自動検出してダメージを与える
    /// </summary>
    public static void SendUseRayDamage(RaycastHit hit,float damage,float knowkBackPower,Transform transform,bool enemyBreak)
    {
        if (hit.collider == null) return;

        if(hit.collider.CompareTag(Syuntoku.DigMode.Player.Player.PLAYER_TAG))
        {
            hit.collider.GetComponent<Syuntoku.DigMode.Player.Player>().SendDamage(damage, transform);
        }

        if (hit.collider.CompareTag(BlockManage.BLOCK_TAG_NAME))
        {
            DamageManager damageManager = new DamageManager();
            damageManager.damage = damage;
            BlockData blockData = hit.collider.GetComponent<BlockData>();
            _blockManage.SendBreakDamage(ref blockData, damageManager, bEnemyBreak: enemyBreak);
        }
    }
}

[System.Serializable]
public class BulletStatus
{
    public BulletStatus()
    {

    }

    public BulletStatus(BulletStatus bulletStatus)
    {
        _bActive = true;
        _activeTime = bulletStatus._activeTime;
        _bulletSpeed = bulletStatus._bulletSpeed;
        _hitMask = bulletStatus._hitMask;
        _sendDamage = bulletStatus._sendDamage;
        _hitEfect = bulletStatus._hitEfect;
        _muzzleFlash = bulletStatus._muzzleFlash;
        _bulletPrefab = bulletStatus._bulletPrefab;
        _knockBackPower = bulletStatus._knockBackPower;

    }

    [Header("設定項目")]
    public bool _bActive;
    [Tooltip("弾の有効時間")]
    public float _activeTime;
    [Tooltip("弾の速度")]
    public float _bulletSpeed;

    [Tooltip("弾の当たり判定")]
    public LayerMask _hitMask;
    [Tooltip("弾のダメージ")]
    public float _sendDamage;
    [Tooltip("弾のヒットエフェクト")]
    public GameObject _hitEfect;
    [Tooltip("弾のプレファブ")]
    public GameObject _bulletPrefab;
    [Tooltip("弾のプレファブ")]
    public GameObject _muzzleFlash;
    [Tooltip("ヒット時のノックバック量")]
    public float _knockBackPower;

    public bool _bDestroy;
    static float DESTROY_ZERO_SCALE_MOTION_TIME = 1.0f;  //消えるときのアニメーション時間

    [Header("設定の必要無し")]
    public GameObject _instanceBullet;

    Ray _bulletRay;
    float timer;

    public void InstanceBullet(Vector3 instancePos,Vector3 direction)
    {
        _instanceBullet = GameObject.Instantiate(_bulletPrefab, instancePos ,Quaternion.identity);
         GameObject.Instantiate(_muzzleFlash, instancePos ,Quaternion.identity);
        _instanceBullet.transform.forward = direction;
    }

    /// <summary>
    /// 弾の更新
    /// </summary>
    /// <returns>弾が無効：false 弾が有効:true</returns>
    public void Update()
    {
        RaycastHit hit;
        timer += Time.deltaTime;

        if (timer >= _activeTime)
        {
            _bActive = false;
            _instanceBullet.transform.DOScale(Vector3.zero, DESTROY_ZERO_SCALE_MOTION_TIME);
            timer = 0.0f;
            return;
        }
        _instanceBullet.transform.position += _instanceBullet.transform.forward * _bulletSpeed * Time.deltaTime;

        _bulletRay.origin = _instanceBullet.transform.position;
        _bulletRay.direction = -_instanceBullet.transform.forward;

            Debug.DrawRay(_bulletRay.origin, _bulletRay.direction * (_bulletSpeed * Time.deltaTime));
        if (Physics.SphereCast(_bulletRay.origin, EnemyBulletManager.DEFAULT_SIZE, _bulletRay.direction, out hit, _bulletSpeed * Time.deltaTime, _hitMask))
        {
            _bActive = false;
            EnemyBulletManager.SendUseRayDamage(hit, _sendDamage, _knockBackPower, _instanceBullet.transform, true);
             GameObject.Instantiate(_muzzleFlash, hit.point, Quaternion.identity);
            _instanceBullet.transform.localScale = Vector3.zero;
        }

        if(_instanceBullet.transform.localScale == Vector3.zero)
        {
            _bDestroy = true;
        }
    }
}
