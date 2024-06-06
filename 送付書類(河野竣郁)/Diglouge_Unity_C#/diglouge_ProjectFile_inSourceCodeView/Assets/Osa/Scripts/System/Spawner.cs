using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Random = UnityEngine.Random;

namespace Battle.Systems
{

    public class Spawner : IDisposable
    {
        private �����e�[�u�� _����data;
        private SpawnPoint[] _spawnPoints;
        private Transform _roots;

        private float _timer;
        private Enemy _observeEnemy;
        private CancellationTokenSource _cts;

        public Spawner(�����e�[�u�� ����Data, SpawnPoint[] spawnPoints, Transform root)
        {
            _����data = ����Data;
            _spawnPoints = spawnPoints;
            _roots = root;

            _cts = new CancellationTokenSource();
        }

        public void Start()
        {
            _observeEnemy = SpawnEnemy();
            _timer = _����data.coolTime;
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
                _timer = _����data.coolTime;
            }
        }

        private async UniTaskVoid OnDeadAsync(Enemy enemy, CancellationToken token)
        {

            while (enemy.IsAlive)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Yield(token);
            }
            Debug.Log("�O�̓G�����񂾂��߃N�[���^�C�����㏑��");
            _timer = _����data.aftercoolTime;
        }

        private Enemy SpawnEnemy()
        {

            var enemyIndex = Random.Range(0, _����data.spawnEnemies.Length - 1);
            var spawnEnemy = _����data.spawnEnemies[enemyIndex];

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
            if (_����data.type == �����e�[�u��.�����敪.�P��)
            {
                SpawnEnemy();
            }
            else
            {
                for (int i = 0; i < _����data.������; i++)
                {
                    SpawnEnemy();
                }
            }
        }

        private Vector3 GetPosition(EnemyParameter spawnEnemy, SpawnPoint spawnPoint)
        {
            // �~����̃����_���ȍ��W0,360�̃����_���o���Ĕ��a���l�������Ƃ��̈ʒu���v�Z
            var randomdeg = Random.Range(0, Mathf.PI * 2);
            var pos = new Vector3(Mathf.Cos(randomdeg) * spawnPoint._radius, 0f, Mathf.Sin(randomdeg) * spawnPoint._radius);

            pos += spawnPoint.point.position;
            pos.y = 0;

            // ���s�G�͒n�ʂɐڒn����悤�n�ʂ̈ʒu���v�Z
            if (spawnEnemy.Type != EnemyType.Fly)
            {
                //var rayPos = pos + new Vector3(0, 100, 0);
                //if (Physics.Raycast(rayPos, Vector3.down, out var hit, Mathf.Infinity))
                //{
                //    pos.y = hit.point.y;
                //}

                return pos;
            }

            // ��s�G�̏ꍇ�A�����������_���ɂ���
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
