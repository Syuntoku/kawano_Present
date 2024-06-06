using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    /// <summary>
    /// É{Å[ÉãÇÃïêäÌèÓïÒ
    /// </summary>
    [System.Serializable]
    public class GunInfo : ToolInfo
    {
        public int _ballDamage { get; private set; }
        public int _shotCount;
        public float _shotSpeed { get; private set; }
        public float _shotSpeedMagnification;
        public GameObject _shotPrefab;
        public float _destroyTime { get; private set; }

        public void Setup(GunInfoScriptable gunInfoScriptable)
        {
            _toolStatus = gunInfoScriptable.tooldata;
            _toolStatus.damageManager.damage = gunInfoScriptable.damageInfos[0].damage;
            _toolStatus.interval = gunInfoScriptable.damageInfos[0].interval;
            _shotSpeed = gunInfoScriptable.shotSpeed;

            _shotPrefab = gunInfoScriptable.shotPrefab;
            _shotCount = gunInfoScriptable.shotCount;
            _destroyTime = gunInfoScriptable.destroyTime;
            _toolUpgladeStatus = new GunUpgrade();
            _toolUpgladeStatus.SetToolIcon(gunInfoScriptable.upgradeIcon);
            _uniqueCharacteristics = new Unique_Gun();
            _shotSpeedMagnification = 1.0f;
        }

        public override void Initialize(UniqueCharacteristicsScriptable uniqueCharacteristicsScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus,GameObject playerObject)
        {
            _uniqueCharacteristics.Initialize(uniqueCharacteristicsScriptable, parkConditionsManage, baseStatus);
        }

        public void AddShotCount(int count)
        {
            _shotCount += count;
        }
    }
}
