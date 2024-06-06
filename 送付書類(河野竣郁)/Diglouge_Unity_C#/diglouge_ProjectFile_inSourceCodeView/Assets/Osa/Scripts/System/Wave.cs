using UnityEngine;

namespace Battle.Systems
{
    [CreateAssetMenu]
    public class Wave : ScriptableObject
    {
        public 生成テーブル[] 生成テーブルs;

        [Header("Waveごとの戦闘時間")]
        public int battleTime;
    }
}