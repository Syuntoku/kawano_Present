using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Random = UnityEngine.Random;

namespace Battle.Systems
{

    public class Spawner : IDisposable
    {
        private 生成テーブル _生成data;
        private SpawnPoint[] _spawnPoints;
        private Transform _roots;

        private float _timer;
        private Enemy _observeEnemy;
        private CancellationTokenSource _cts;

        public Spawner(生成テーブル 生成Data, SpawnPoint[] spawnPoints, Transform root)
        {
            _生成data = 生成Data;
            _spawnPoints = spawnPoints;
            _roots = root;

            _cts = new CancellationTokenSource();
        }

        public void Start()
        {
            _observeEnemy = SpawnEnemy();
            _timer = _生成data.coolTime;
            LoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                OnDeadAsync(_observeEnemy, cts.Token).Forget();

                while (_timer > 0)
                {
                    await UniTask.Yield(token);
                    _timer -= Time.deltaTime;

                    //Debug.Log(_timer);
                }

                cts.Cancel();
                _observeEnemy = SpawnEnemy();
                _timer = _生成data.coolTime;
            }
        }

        private async UniTaskVoid OnDeadAsync(Enemy enemy, CancellationToken token)
        {

            while (enemy.IsAlive)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Yield(token);
            }
            Debug.Log("前の敵が死んだためクールタイムを上書き");
            _timer = _生成data.aftercoolTime;
        }

        private Enemy SpawnEnemy()
        {

            var enemyIndex = Random.Range(0, _生成data.spawnEnemies.Length - 1);
            var spawnEnemy = _生成data.spawnEnemies[enemyIndex];

            var spawnPosIndex = Random.Range(0, _spawnPoints.Length - 1);
            var spawnPos = _spawnPoints[spawnPosIndex];


            var pos = GetPosition(spawnEnemy, spawnPos);
            Debug.Log(pos);
            pos += spawnPos.point.position;
            var enemy = GameObject.Instantiate(spawnEnemy.enemyPrefab, pos, Quaternion.identity);

            if (_roots != null)
                enemy.transform.SetParent(_roots);

            return enemy.GetComponent<Enemy>();
        }

        private void Loop()
        {
            if (_生成data.type == 生成テーブル.生成区分.単体)
            {
                SpawnEnemy();
            }
            else
            {
                for (int i = 0; i < _生成data.生成数; i++)
                {
                    SpawnEnemy();
                }
            }
        }

        private Vector3 GetPosition(EnemyParameter spawnEnemy, SpawnPoint spawnPoint)
        {
            // 円周上のランダムな座標0,360のランダム出して半径を考慮したときの位置を計算
            var randomdeg = Random.Range(0, Mathf.PI * 2);
            var pos = new Vector3(Mathf.Cos(randomdeg) * spawnPoint._radius, 0f, Mathf.Sin(randomdeg) * spawnPoint._radius);

            pos += spawnPoint.point.position;
            pos.y = 0;

            // 歩行敵は地面に接地するよう地面の位置を計算
            if (spawnEnemy.Type != EnemyType.Fly)
            {
                //var rayPos = pos + new Vector3(0, 100, 0);
                //if (Physics.Raycast(rayPos, Vector3.down, out var hit, Mathf.Infinity))
                //{
                //    pos.y = hit.point.y;
                //}

                return pos;
            }

            // 飛行敵の場合、高さもランダムにする
            var y = Random.Range(spawnEnemy._minHeight, spawnEnemy._maxHeight);
            pos.y = y;
            return pos;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

}
