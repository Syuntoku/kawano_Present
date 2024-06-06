using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    [CreateAssetMenu(fileName = "EnemySetting",menuName = "Scriptable/Enemy/Create EnemySetting")]
    public class EnemyScriptable : ScriptableObject
    {
        public EnemySetting[] enemySettings;

        public EnemyStatus GetEnemySetting(int bindId)
        {
            for (int i = 0; i < enemySettings.Length; i++)
            {
                if(enemySettings[i].statusID == bindId)
                {
                   return enemySettings[i].statusData;
                }
            }
            Debug.Log("EnemyBindError" + bindId);
            return new EnemyStatus();
        }
    }

    [System.Serializable]
    public struct EnemySetting
    {
        [Tooltip("�ݒ���o�C���h����Ƃ���ID")]
        public int statusID;
        [Tooltip("�K������X�e�[�^�X")]
        public EnemyStatus statusData;
    }

}
