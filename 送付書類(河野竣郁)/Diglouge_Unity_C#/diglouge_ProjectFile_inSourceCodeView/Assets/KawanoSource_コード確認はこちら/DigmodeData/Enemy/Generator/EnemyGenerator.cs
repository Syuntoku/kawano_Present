using UnityEngine;
using Syuntoku.DigMode.Wave;
using Syuntoku.DigMode.Settings;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// 敵を生成管理
    /// </summary>
    public class EnemyGenerator : MonoBehaviour
    {
        #region CashVariable
        [SerializeField] GameObject _playerObject;
        [SerializeField] WaveManage _waveManage;
        [SerializeField] GameObject _enemyGeneratePoint;
        [SerializeField] EnemyGeneratorScriptable _enemyGeneratorScriptable;
        [SerializeField] EnemyScriptable _enemyScriptable;
        [SerializeField] GameObject _enemyParent;
        [SerializeField] DamageText _damageText;
        #endregion
        public LayerMask _layerMask;
        bool _bStartGenerate;
        float _timer;
        public int _generateCount;
        const float NEXT_INTERVAL = 1.0f;

        float _reStartTimer;
        const float GENERARTE_RESTART_TIME = 5.0f;
        Ray _workRay;

        enum GenerateKind
        {
            Normal,
        }
        private void Start()
        {
            _workRay = new Ray();
        }
        //=========================
        //Unity
        //=========================
        void Update()
        {
            _generateCount = _enemyParent.transform.childCount;
            //バトルモード中のみ出現
            if (!_waveManage.IsBattleMode())
            {
                GeneratorReset();
                return;
            }

            if (_playerObject.transform.position.y >= _enemyGeneratorScriptable._generateHeight) return;

            //敵の生成が開始されるまで遅延する
            _reStartTimer += Time.deltaTime;
            if (_reStartTimer < GENERARTE_RESTART_TIME) return;

            GenerateSetting generateSetting = ActiveWaveSettingIndex();
            
            //最初だけたくさん出る
            if (!_bStartGenerate)
            {
                for (int i = 0; i < GeneratorCheckCount(generateSetting); i++)
                {

                    RandomGenerate(generateSetting);
                }
                _bStartGenerate = true;
            }

            _timer += Time.deltaTime;

            if (_timer > NEXT_INTERVAL) return;

            if (_generateCount <= generateSetting.maxEnemyCount)
            {
                RandomGenerate(generateSetting);
                _timer = 0.0f;
            }
#if UNITY_EDITOR
            Debug.Log("EnemyCout" + _generateCount);
#endif
        }

        //=========================
        //private
        //=========================
        void GeneratorReset()
        {
            _bStartGenerate = false;
        }

        /// <summary>
        /// 周りに敵を生成する
        /// </summary>
        void RandomGenerate(GenerateSetting generateSetting)
        {
            if (_playerObject.transform.position.y >= _enemyGeneratorScriptable._generateHeight) return;
            if (_generateCount >= generateSetting.maxEnemyCount)
            {
                return;
            }

            OutlineGenerate(generateSetting);
            _generateCount++;
        }

        /// <summary>
        /// Prefabから生成
        /// </summary>
        void Generate(Vector3 position, GenerateSetting generateSetting)
        {
            if (_playerObject.transform.position.y >= _enemyGeneratorScriptable._generateHeight) return;

            int generateRandom = Random.Range(0, _enemyGeneratorScriptable.generatPrefabs.Length);
            GameObject generatePrefab = _enemyGeneratorScriptable.generatPrefabs[generateRandom].generateEnemyPrf;
            int generateId = _enemyGeneratorScriptable.generatPrefabs[generateRandom].bindStatusId;

            GameObject generateObject = Instantiate(generatePrefab, position,Quaternion.Euler(Vector3.up));
            generateObject.transform.SetParent(_enemyParent.transform);
            generateObject.GetComponent<EnemyBase>().Initialize(_playerObject, generateSetting.changeStatus, _enemyScriptable.GetEnemySetting(generateId), _damageText);
        }

        /// <summary>
        /// 飛ばしたRayの先に敵を生成する
        /// </summary>
        void OutlineGenerate(GenerateSetting generateSetting)
        {
            Vector3 generatePos = Vector3.zero;
            SpawnOutlinePointCheck(out generatePos);
            if (generatePos == Vector3.zero) { return; }
            Generate(generatePos, generateSetting);
        }

        /// <summary>
        /// ランダムなRayを生成
        /// </summary>
        /// <returns></returns>
        void GenerateRandomRay(ref Ray work)
        {
            Vector3 randomRayDir = Vector3.zero;
            //ランダムなベクトルを求める
            randomRayDir.x = Random.Range(-1.0f, 1.0f);
            randomRayDir.y = Random.Range(_enemyGeneratorScriptable._ajustRandomDirY, 1.0f);
            randomRayDir.z = Random.Range(-1.0f, 1.0f);
            work.direction = randomRayDir.normalized;
            work.origin = _enemyGeneratePoint.transform.position;
        }

        /// <summary>
        /// 敵の生成数を決める
        /// </summary>
        /// <param name="generateSetting"></param>
        /// <returns></returns>
        int GeneratorCheckCount(GenerateSetting generateSetting)
        {
            //疑似的に範囲内にいるブロックを計算
            int generateCount = 0;
            float blockCount = generateSetting.range * generateSetting.range * generateSetting.range;

            //一定の確率を求める
            for (int i = 0; i < blockCount; i++)
            {
                if(GameUtility.CheckUnderParsent(generateSetting.ratePlibility)){ generateCount++; };      
            }
            return generateCount;
        }

        GenerateSetting ActiveWaveSettingIndex()
        {
            foreach (var item in _enemyGeneratorScriptable._spawnSetting)
            {
                if (item.activeWave == _waveManage._waveCount) return item;
            }
            return new GenerateSetting();
        }

        /// <summary>
        /// スポーンをする場所を調べる
        /// </summary>
        /// <returns></returns>
        void SpawnOutlinePointCheck(out Vector3 point)
        {
            int count = 0;
            while (true)
            {
                if (count >= 200) break;
                GenerateRandomRay(ref _workRay);
                count++;

                if (!WorldData.IsInWorld(_workRay.origin + _workRay.direction * ActiveWaveSettingIndex().range)) continue;

                float toPlayer = (_workRay.origin - _playerObject.transform.position).sqrMagnitude;
                if (toPlayer <= 3) continue;
                break;
            }
            point = _workRay.origin + _workRay.direction * ActiveWaveSettingIndex().range + _enemyGeneratorScriptable._ajustGeneratePos;
        }

        /// <summary>
        /// リスポーンする
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public void ReSpornEnemy(out Vector3 position)
        {       
           SpawnOutlinePointCheck(out position);
        }

        public void DespornAllEnemy()
        {
            foreach (Transform item in _enemyParent.transform)
            {
                _reStartTimer = 0.0f;
                Destroy(item.gameObject);
            }
        }
    }
}
