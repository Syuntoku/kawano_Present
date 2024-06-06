using UnityEngine;
using System;

namespace Battle.Systems
{
    [CreateAssetMenu]
    public class 生成テーブル : ScriptableObject
    {
        public enum 生成区分 { 単体, 集団 }

        public EnemyParameter[] spawnEnemies;

        [Header("次の敵を生成するまでの時間")]
        public int coolTime = 5;

        [Header("前に生成した敵が死亡した場合に上書きするときの時間")]
        public int aftercoolTime = 5;

        public 生成区分 type;

        [Header("区分が集団の場合の生成数")]
        public int 生成数;
    }

    [Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public int _radius;
    }
}

