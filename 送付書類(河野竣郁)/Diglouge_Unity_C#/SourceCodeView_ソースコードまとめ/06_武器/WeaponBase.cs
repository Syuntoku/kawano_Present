using UnityEngine;
using Syuntoku.Status;
using Cysharp.Threading.Tasks;
using System.Threading;
using Syuntoku.DigMode.Enemy;
using Syuntoku.DigMode.Input;
using Syuntoku.DigMode.Tool;
using DG.Tweening;

namespace Syuntoku.DigMode.Weapon
{
    public class WeaponBase : MonoBehaviour
    {
        #region CashVariabe
        [Header("弾の出る場所を設定")]
        [SerializeField] Transform _shotPoint;
        [Header("リロード時の回転量")]
        [SerializeField] Vector3 _reroadRotateEndvalue;
        [Header("ツールの状態")]
        public string _toolName;
        #endregion

        public WeaponInfo _weaponInfo;
        protected StatusManage _statusManage;

        protected const float REROADING_MOSION_TIME = 0.5f;
        const float END_REROADING_MOSION_TIMER = 0.3f;

        Transform _bulletParent;
        BulletManager _bulletManager;

        bool _bReroading;
        public bool _bShot;
        public float _ammunitionRemaining;

        Ray _workShotRay;
        float _holdTime;
        const int ONE_SECOND = 1000;

        const int NEAR_BULLET_HIT_LENGTH = 100;
        const float RESET_TRANSFORM_PRIBILITY = 10;
        const float RECOIL_POWER = 10.0f;

        CancellationToken cts;

        public WeaponBase()
        {
            _weaponInfo = new WeaponInfo();
            _statusManage = null;
            _bulletManager = null;
        }
        //=================================================
        //Unity
        //=================================================
        private void OnDisable()
        {
            gameObject.transform.DOKill();
        }
        //=================================================
        //public
        //=================================================
        public void Initialize(StatusManage statusManage)
        {
            GameObject bulletParent = GameObject.Find("BulletParent");
            _bulletManager = bulletParent.GetComponent<BulletManager>();
            _bulletParent = bulletParent.transform;

            cts = this.GetCancellationTokenOnDestroy();
            _statusManage = statusManage;
            ShotReset();
        }

        public virtual void SetStatus(WeaponInfo weaponInfo)
        {
            _weaponInfo = weaponInfo;
            AmmoReroad();
        }

        public virtual void ShotUpdate()
        {
            ShotBullet();
            transform.eulerAngles -= transform.eulerAngles / RESET_TRANSFORM_PRIBILITY;
        }

        public virtual void ShotBullet()
        {
            if (ReroadUpdate()) return;

            if (InputData._bAction)
            {
                if (IsShoot())
                {
                    OnCallShot(_weaponInfo._weaponBaseStatus.bulletPrf, _shotPoint.transform.position, transform.forward);
                };
            }
            else
            {
                if (!_weaponInfo._weaponBaseStatus.bFullOuto && _bShot) ShotReset();
            }

            //発射レート
            if (!IsShoot())
            {
                if (_bReroading) return;

                _holdTime += Time.deltaTime;

                if (_holdTime >= _weaponInfo._weaponBaseStatus.shotRate)
                {
                    ShotReset();
                }
            }
        }

        /// <summary>
        /// 撃った時の処理
        /// </summary>
        public virtual void OnCallShot(GameObject bulletPrefab, Vector3 instancePosition, Vector3 targetDir, bool activeShotInterval = true)
        {
            Shot(bulletPrefab, instancePosition, targetDir, Vector3.zero, activeShotInterval);
        }

        /// <summary>
        /// 弾を撃つ /
        /// 通常のショットの状態 
        /// </summary>
        /// <param name="bulletPrefab"></param>
        /// <param name="instancePosition"></param>
        /// <param name="targetDir"></param>
        public virtual void Shot(GameObject bulletPrefab, Vector3 instancePosition, Vector3 targetDir, Vector3 addDir, bool activeShotInterval = true)
        {
            RaycastHit hit;
            GameObject generateObject = Instantiate(bulletPrefab, instancePosition, Quaternion.LookRotation(targetDir + addDir), _bulletParent);
            var muzzuleFlash = Instantiate(_weaponInfo._weaponBaseStatus.muzzuleFlash, _shotPoint.transform.position, _shotPoint.transform.rotation, gameObject.transform).GetComponent<ParticleSystem>();
            muzzuleFlash.Play();

            if (activeShotInterval)
            {
                _ammunitionRemaining--;
                _bShot = true;
            }

            if (_ammunitionRemaining <= 0)
            {
                Reroad();
                SetReroadMosion();
                ReroadStart();
            }

            //銃のリコイル
            // StartshotRecoil();

            //ダメージはRayで行う
            _workShotRay.origin = instancePosition;
            _workShotRay.direction = targetDir + addDir;

            Debug.DrawRay(_workShotRay.origin, _workShotRay.direction * _weaponInfo._weaponBaseStatus.shotHitLength, Color.gray, 2f);

            int destroyTime = ONE_SECOND;

            GameObject hitEfect = null;
            if (Physics.Raycast(_workShotRay, out hit, _weaponInfo._weaponBaseStatus.shotHitLength, _weaponInfo._weaponBaseStatus.layerMask))
            {
                hitEfect = _weaponInfo._weaponBaseStatus.hitEfect;
                destroyTime = CheckHitTime(hit);

                if (destroyTime <= 0) destroyTime = 0;

                if (destroyTime <= NEAR_BULLET_HIT_LENGTH)
                {
                    Instantiate(hitEfect, hit.point, Quaternion.identity);
                    hitEfect = null;
                }
                SendDamage(hit, destroyTime);
            }
            else // 何も当たらなかった場合の破壊時間の計算
                destroyTime = (int)(ONE_SECOND * (_weaponInfo._weaponBaseStatus.shotHitLength / _weaponInfo._weaponBaseStatus.shotSpeed));

            if (destroyTime != 0)
            {
                BulletBase bulletData = new BulletBase();
                bulletData._connectObject = generateObject;
                bulletData._destroyTime = (float)destroyTime / ONE_SECOND;
                bulletData._speed = _weaponInfo._weaponBaseStatus.shotSpeed;
                bulletData._hitEfect = hitEfect;
                _bulletManager.AddBulletObject(bulletData);
            }
        }

        /// <summary>
        /// 弾が打てる状態にする
        /// </summary>
        public void ShotReset()
        {
            _holdTime = 0.0f;
            _bShot = false;
        }

        public void AmmoReroad()
        {
            _ammunitionRemaining = _weaponInfo._weaponBaseStatus.magazineSize;
        }

        //=======================================
        //ステータス変更 public
        //=======================================
        /// <summary>
        /// 手に持った時の変更
        /// </summary>
        public virtual void SetHoldTool()
        {
            _statusManage.digmodeStatus.AddSpeedMagnification(-_weaponInfo._weaponBaseStatus.HandMovementSpeedDecreased);

            if (_bReroading)
            {
                ReroadStart();
            }
        }

        /// <summary>
        /// 手持ちをやめたときの変更
        /// </summary>
        public virtual void PutItAway()
        {
            _statusManage.digmodeStatus.AddSpeedMagnification(_weaponInfo._weaponBaseStatus.HandMovementSpeedDecreased);

            if (_bReroading)
            {
                ReroadEnd();
            }
        }

        /// <summary>
        /// リロードをした最初の変更
        /// </summary>
        public virtual void ReroadStart()
        {
            //  _statusManage.digmodeStatus.AddSpeedMagnification(-_weaponInfo._weaponBaseStatus.ReloadMovementSpeedDecreased);
        }

        /// <summary>
        /// リロードが終わった後の変更
        /// </summary>
        public virtual void ReroadEnd()
        {
            //  _statusManage.digmodeStatus.AddSpeedMagnification(_weaponInfo._weaponBaseStatus.ReloadMovementSpeedDecreased);

            if (_bReroading) return;
            SetShotMosion();
        }

        public virtual void StartshotRecoil()
        {
            transform.localEulerAngles -= Vector3.right;

            transform.DOLocalRotate(Vector3.zero, 0.1f);
        }

        public virtual ToolUpgrade GetUpgradeData()
        {
            return null;
        }

        //=================================================
        //private
        //=================================================
        void SendDamage(RaycastHit hit, int delayTime)
        {
            if (BlockManage.IsBlock(hit) || hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            UniTask.Delay(delayTime, cancellationToken: cts);
            EnemyBase enemyBase = hit.collider.GetComponent<EnemyBase>();
            enemyBase.SendDamage(_weaponInfo._weaponBaseStatus.bulletDamage, 0, Vector3.forward);
        }

        int CheckHitTime(RaycastHit hit)
        {
            return (int)(ONE_SECOND * (hit.distance / _weaponInfo._weaponBaseStatus.shotSpeed));
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <returns>リロード中　true</returns>
        bool ReroadUpdate()
        {
            if (!_bReroading) return false;

            _holdTime += Time.deltaTime;

            if (_holdTime >= _weaponInfo._weaponBaseStatus.reloadTime)
            {
                _bReroading = false;
                ReroadEnd();
            }
            return true;
        }

        void Reroad()
        {
            _bReroading = true;
            _holdTime = 0.0f;
        }

        void SetReroadMosion()
        {
            transform.DOLocalRotate(_reroadRotateEndvalue, REROADING_MOSION_TIME);
        }

        void SetShotMosion()
        {
            transform.DOLocalRotate(Vector3.zero, END_REROADING_MOSION_TIMER).OnComplete(() => { AmmoReroad(); });
        }

        /// <summary>
        /// 撃てる状態か
        /// </summary>
        /// <returns></returns>
        bool IsShoot()
        {
            if (_bShot) return false;
            if (_ammunitionRemaining == 0) return false;
            if (_bReroading) return false;
            return true;
        }
    }
}
