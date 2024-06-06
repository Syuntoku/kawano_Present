using System.Collections;
using System.Collections.Generic;
using Syuntoku.Status;
using UnityEngine;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.DigMode.Tool.Unique;

namespace Syuntoku.DigMode.Tool
{
    [System.Serializable]
    public class HammerInfo : ToolInfo
    {
        public float _middleDamage { get;private set; }
        public float _endflameDamage { get; private set; }
        public float _flameDamageMagnification;
        public float _breakRange;

       public const int DELAY_FIRST = 50;
       public const int DELAY_SECOND = 50;

        public void Setup(HammerInfoScriptable hammerInfoScriptable)
        {
            _toolStatus = hammerInfoScriptable.tooldata;
            _toolStatus.damageManager.damage = hammerInfoScriptable.damageInfos[0].centerDamage;
            _middleDamage = hammerInfoScriptable.damageInfos[0].middleLineDamage;
            _endflameDamage = hammerInfoScriptable.damageInfos[0].outlineDamage;
            _toolStatus.interval = hammerInfoScriptable.damageInfos[0].interval;

            _toolUpgladeStatus = new HammerUpgrade();
            _toolUpgladeStatus.SetToolIcon(hammerInfoScriptable.upgradeIcon);
            _uniqueCharacteristics = new Unique_Hammer();

            _flameDamageMagnification = 1.0f;
        }

        public override void Initialize(UniqueCharacteristicsScriptable uniqueCharacteristicsScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus, GameObject playerObject)
        {
            _uniqueCharacteristics.Initialize(uniqueCharacteristicsScriptable, parkConditionsManage, baseStatus);
        }

        public DamageManager[] GetDamage()
        {
            DamageManager[] damage = new DamageManager[3];

            for (int i = 0; i < 3; i++)
            {
                damage[i] = new DamageManager();
            }

            damage[0].damage = _toolStatus.damageManager.damage * _flameDamageMagnification;
            damage[1].damage = _middleDamage * _flameDamageMagnification;
            damage[2].damage = _endflameDamage * _flameDamageMagnification;

            return damage;
        }

        public void AddBreakRange(int breakRange)
        {
            _breakRange += breakRange;
        }
    }
}
