using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    public class Shotgun : WeaponBase
    {
        [SerializeField] ShotgunScriptable _shotgunScriptable;
        ShotgunInfo _shotgunInfo;

        public Shotgun()
        {
        }

        public override void OnCallShot(GameObject bulletPrefab, Vector3 instancePosition, Vector3 targetDir, bool activeShotInterval = true)
        {
            Vector3 addDir = Vector3.zero;

            for (int i = 0; i < _shotgunScriptable.shellInBulletCount; i++)
            {
                addDir.x = Random.Range(-_shotgunScriptable.shotBulletRange, _shotgunScriptable.shotBulletRange);
                addDir.y = Random.Range(-_shotgunScriptable.shotBulletRange, _shotgunScriptable.shotBulletRange);

                //最後の弾を発射したときにインターバルを付ける
                if(i == _shotgunScriptable.shellInBulletCount -1)
                {
                    base.Shot(bulletPrefab, instancePosition, targetDir,addDir, activeShotInterval);
                }
                base.Shot(bulletPrefab, instancePosition, targetDir,addDir, false);
            }
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _shotgunInfo = (ShotgunInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _shotgunInfo._weaponUpgrade.ChangeStatus(base._statusManage, _shotgunInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _shotgunInfo._weaponUpgrade.DisableStatus(base._statusManage, _shotgunInfo._weaponBaseStatus);
        }
    }
}
