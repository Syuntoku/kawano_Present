using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using Battle.Systems;
using System.Collections.Generic;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;

        [SerializeField] private string _digSceneName = "Digmode";
        [SerializeField] private KeyCode _transitionKey = KeyCode.Escape;
        [SerializeField] private PlayerScriptable playerScriptable;
        [SerializeField] private Syuntoku.Status.StatusManage statusManage;
        public Player player;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public int KillCount
        {
            get => _killRP.Value;
            set => _killRP.Value = value;
        }

        public IReadOnlyReactiveProperty<int> CurrentKillCount => _killRP;
        private ReactiveProperty<int> _killRP = new ReactiveProperty<int>(0);

        public IReadOnlyReactiveProperty<int> WaveCount => _waveCountRP;
        private ReactiveProperty<int> _waveCountRP = new ReactiveProperty<int>(0);

        public IReadOnlyReactiveProperty<int> Remainingtime => _timer.Remainingtime;
        private Timer _timer = new Timer(0);

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (!player)
            {
                player = Instantiate(playerScriptable.playerPrefab, playerScriptable.spawnPoint.position, playerScriptable.spawnPoint.rotation);
                player.Init(playerScriptable);
            }
            statusManage.battleModeStatus.ConnectStatus(player.playerStatus);

            _killRP.AddTo(this);
            _waveCountRP.AddTo(this);

            LoopAsync(this.GetCancellationTokenOnDestroy()).Forget();

        }
        private List<Spawner> _spawners = new List<Spawner>();
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private Transform _enemyroot;
        [SerializeField] private Wave[] _waves;
        private Wave _currentWave;
        private void WaveInit(Wave wave)
        {
            _currentWave = wave;

            foreach (var spawnerData in wave.¶¬ƒe[ƒuƒ‹s)
            {
                _spawners.Add(new Spawner(spawnerData, _spawnPoints, _enemyroot));
            }
        }

        private async UniTask WaveStart(CancellationToken token)
        {
            foreach (var spawner in _spawners)
            {
                spawner.Start();
            }

            _timer.Initialize(_currentWave.battleTime);
            await _timer.CountStartAsync(token);

            foreach (var spawner in _spawners)
            {
                spawner.Dispose();
            }

            foreach (Transform child in _enemyroot)
            {
                Destroy(child.gameObject);
            }

            _spawners.Clear();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_transitionKey))
            {
                TransitionManager.LoadScene(_digSceneName, () => { });
            }
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {

            Debug.Log("StageStart...");
            foreach (var wave in _waves)
            {
                if (token.IsCancellationRequested) return;
                _waveCountRP.Value++;

                WaveInit(wave);
                await WaveStart(token);

                await UniTask.Delay(5000, cancellationToken: token);
            }
            Debug.Log("StageFinish...");
        }


        public void SpawnEnemy(int spawnCount)
        {
            //var count = spawnCount / _waveSpawners.Length;
            //foreach (var spawner in _waveSpawners) spawner.SpawnEnemy(count);
        }

        public void UIMode(bool isUiMode)
        {
            if (isUiMode)
            {
                CursorEnable();
            }
            else
            {
                CursorDisable();
            }
        }

        public static void CursorEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void CursorDisable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            _timer.Dispose();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (var spawnPoint in _spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.point.position, spawnPoint._radius);
            }
        }
    }

}