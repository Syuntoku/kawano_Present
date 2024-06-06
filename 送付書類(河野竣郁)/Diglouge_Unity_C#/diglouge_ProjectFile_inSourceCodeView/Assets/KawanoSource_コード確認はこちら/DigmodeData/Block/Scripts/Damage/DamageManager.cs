using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Status
{
    [System.Serializable]
    public class DamageManager
    {
        public float damage;

        /// <summary>
        /// ���j�󎞃t���O
        /// </summary>
        public bool bBreak;

        /// <summary>
        /// �_���[�W�v�Z����
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public DamageManager DamageCalculation(DigStatus digStatus)
        {
            DamageManager damageManager = new DamageManager();
            if (GameUtility.GetRandomParsent() <= digStatus.ImmediatelyBreakMagnification)
            {
                damageManager.bBreak = true;
                return damageManager;
            }
            else
            {
                bBreak = false;
            }

            damageManager.damage = damage * digStatus.DamagePlibility;
            return damageManager;
        }
    }
}
