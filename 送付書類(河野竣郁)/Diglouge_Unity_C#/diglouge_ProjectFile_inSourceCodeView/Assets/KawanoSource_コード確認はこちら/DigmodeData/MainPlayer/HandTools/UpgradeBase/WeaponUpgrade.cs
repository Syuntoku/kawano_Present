using Syuntoku.Status;
using Syuntoku.DigMode.Weapon;

namespace Syuntoku.DigMode.Tool
{
    public class WeaponUpgrade : ToolUpgrade
    {
        protected float _attackPower;
        protected float _reroadSpeedUpMagnification;
        protected int _addMagazinSize;

        public virtual void ChangeStatus(StatusManage baseStatus, WeaponBaseStatus weaponBaseStatus)
        {
            weaponBaseStatus.magazineSize += _addMagazinSize;
            baseStatus.battleModeStatus.AttackPowerMagnification += _attackPower;
            baseStatus.battleModeStatus.AddReloadSpeedMagnification(-_reroadSpeedUpMagnification);
        }
        public virtual void DisableStatus(StatusManage baseStatus, WeaponBaseStatus weaponBaseStatus)
        {
            weaponBaseStatus.magazineSize -= _addMagazinSize;
            baseStatus.battleModeStatus.AttackPowerMagnification -= _attackPower;
            baseStatus.battleModeStatus.AddReloadSpeedMagnification(_reroadSpeedUpMagnification);
        }
    }
}
