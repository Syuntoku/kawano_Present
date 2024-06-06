using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    [CreateAssetMenu(fileName = "EnemyGenerator", menuName = "Scriptable/Enemy/Create EnemyGeneratorScriptable")]
    public class EnemyGeneratorScriptable : ScriptableObject
    {
        [Tooltip("生成の間隔")]
        public float _spawnInterval;
        [Tooltip("ランダムな生成間隔の設定"),Range(0,10)]
        public float _spawnAddInterval;

        [Header("スポーン設定"), Tooltip("スポーン範囲")]
        public GenerateSetting[] _spawnSetting;
        [Tooltip("近くに沸く設定をする確率　％")]
        public int _spawanKindProbability;
        [Tooltip("プレイヤーどの深さでスポーンを開始するか"),Range(-1,-100)]
        public int _generateHeight;
        [Tooltip("プレイヤーの近さでスポーンしない距離")]
        public int _nearBlockSpawnLength; 
        [Tooltip("生成時の半円の平面の高さを調整")]
        public float _ajustRandomDirY;
        [Tooltip("生成するときの調整")]
        public Vector3 _ajustGeneratePos;

        public GeneratPrefab[] generatPrefabs;
    }

    [System.Serializable]
    public struct GeneratPrefab
    {
        public GameObject generateEnemyPrf;
        public int bindStatusId;
    }

    [System.Serializable]
    public struct GenerateSetting
    {
        public string name;
        public int activeWave;
        public int range;
        public float ratePlibility;
        public int maxEnemyCount;
        public WaveEnemyChangeStatus changeStatus;
    }

    [System.Serializable]
    public struct WaveEnemyChangeStatus
    {
        public float hpPribability;
        public float attackPribability;
    }
}

