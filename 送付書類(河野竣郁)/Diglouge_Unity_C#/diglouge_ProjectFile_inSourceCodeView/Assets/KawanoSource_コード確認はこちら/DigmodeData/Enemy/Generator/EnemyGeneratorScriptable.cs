using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode.Enemy
{
    [CreateAssetMenu(fileName = "EnemyGenerator", menuName = "Scriptable/Enemy/Create EnemyGeneratorScriptable")]
    public class EnemyGeneratorScriptable : ScriptableObject
    {
        [Tooltip("�����̊Ԋu")]
        public float _spawnInterval;
        [Tooltip("�����_���Ȑ����Ԋu�̐ݒ�"),Range(0,10)]
        public float _spawnAddInterval;

        [Header("�X�|�[���ݒ�"), Tooltip("�X�|�[���͈�")]
        public GenerateSetting[] _spawnSetting;
        [Tooltip("�߂��ɕ����ݒ������m���@��")]
        public int _spawanKindProbability;
        [Tooltip("�v���C���[�ǂ̐[���ŃX�|�[�����J�n���邩"),Range(-1,-100)]
        public int _generateHeight;
        [Tooltip("�v���C���[�̋߂��ŃX�|�[�����Ȃ�����")]
        public int _nearBlockSpawnLength; 
        [Tooltip("�������̔��~�̕��ʂ̍����𒲐�")]
        public float _ajustRandomDirY;
        [Tooltip("��������Ƃ��̒���")]
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

