using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace Battle.Weapons
{

    public class Shotgun : BaseWeapon
    {
        [SerializeField] private Vector2 _size = Vector2.one;
        [SerializeField] private float _range = 1.0f;

        [Header("散弾数")]
        [SerializeField] private Vector2 _pelletCount = new Vector2(4, 4);
        public int _damage = 10;

        [SerializeField] private bool _isDrawGizmo = true;

        private List<Vector3> _layPoslist = new List<Vector3>();

        protected override void Fire()
        {
            _layPoslist.Clear();

            var damageEnemies = new List<DamageEnemy>();

            // 散弾を等間隔で配置するための座標を計算: 余白の計算　長さ/余白の数
            var sX = Camera.main.transform.TransformDirection(new Vector3(_size.x / (_pelletCount.x * 2), 0));
            var sY = Camera.main.transform.TransformDirection(new Vector3(0, _size.y / (_pelletCount.y * 2)));

            var x = _size.x / (_pelletCount.x * 2);
            var y = _size.y / (_pelletCount.y * 2);
            Vector3 range = Camera.main.transform.position + Camera.main.transform.forward * _range;
            // 回転をかけて傾きを考慮する
            Vector3 initPos = range - Camera.main.transform.rotation * ((Vector3)_size / 2);


            for (int i = 1; i <= _pelletCount.y; i++)
            {

                initPos += sY;
                var pos = initPos;

                for (int j = 1; j <= _pelletCount.x; j++)
                {
                    pos += sX;

                    var rand = new Vector3(Random.Range(-x, x), Random.Range(-y, y));
                    var rayPos = pos + _cam.transform.TransformDirection(rand);

                    // レイキャストを実行
                    Vector3 rayStart = _cam.transform.position;
                    Vector3 rayDirection = rayPos - _cam.transform.position;

                    var ray = new Ray(rayStart, rayDirection);
                    RaycastHit hit;
                    Vector3 RayPos = rayPos;
                    if (Physics.Raycast(ray, out hit, _range))
                    {

                        var enemy = hit.collider.GetComponentInParent<Enemy>();
                        if (enemy)
                        {
                            // 散弾が命中した敵ごとに与えるダメージを計算する

                            enemy.TakeDamage(new Game.Damage(_damage, this.gameObject, false));

                            var damageEnemy = damageEnemies.FirstOrDefault(x => x.enemy == enemy);

                            if (damageEnemy == null)
                                damageEnemies.Add(new DamageEnemy(enemy, _damage));
                            else
                                damageEnemy.AddDamage(_damage);
                        }
                        RayPos = hit.point;
                    }
                    else
                    {
                        // レイキャストがヒットしなかった場合の処理

                    }

                    _layPoslist.Add(RayPos);

                    pos += sX;
                }

                initPos += sY;
            }

            //// 散弾が命中した敵にダメージを当たる
            //foreach (var damageEnemy in damageEnemies)
            //{
            //    damageEnemy.enemy.TakeDamage(new Game.Damage(damageEnemy.damage, this.gameObject, false));
            //}

            ShowBulletLine().Forget();
        }

        [SerializeField] private GameObject _trailRoot;
        [SerializeField] LineRenderer _trailPrefab;
        private async UniTaskVoid ShowBulletLine()
        {
            foreach (var pos in _layPoslist)
            {
                var lineRenderer = Instantiate(_trailPrefab, _settings.weaponMuzzle.position, Quaternion.identity, _trailRoot.transform);
                lineRenderer.SetPosition(0, _settings.weaponMuzzle.position);
                lineRenderer.SetPosition(1, pos);
            }

            await UniTask.Delay(500);

            foreach (Transform child in _trailRoot.transform)
            {
                //自分の子供をDestroyする
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        ///  散弾ごとに命中する座標を計算
        /// </summary>
        private Vector3 CalculateHitPosition(Transform shootTransform)
        {

            Vector3 randomPosition = Random.insideUnitCircle * _size;
            Vector3 range = shootTransform.position + shootTransform.forward * _range; // 自分の前方向に射程分離れた座標を計算

            // ローカル座標系からワールド座標系へ変換し、Y軸回転に合わせて位置を調整
            Vector3 adjustedPosition = range + shootTransform.rotation * randomPosition;
            return adjustedPosition;
        }

        private void OnDrawGizmosSelected()
        {

            if (_isDrawGizmo)
            {
                //// 散弾を等間隔で配置するための座標を計算: 余白の計算　長さ/余白の数
                //var sX = Camera.main.transform.TransformDirection(new Vector3(_size.x / (_pelletCount.x * 2), 0));
                //var sY = Camera.main.transform.TransformDirection(new Vector3(0, _size.y / (_pelletCount.y * 2)));

                //Vector3 range = Camera.main.transform.position + Camera.main.transform.forward * _range;
                //// 回転をかけて傾きを考慮する
                //Vector3 initPos = range - Camera.main.transform.rotation * ((Vector3)_size / 2);


                //for (int i = 1; i <= _pelletCount.y; i++)
                //{

                //    initPos += sY;
                //    var pos = initPos;

                //    for (int j = 1; j <= _pelletCount.x; j++)
                //    {
                //        pos += sX;

                //        Gizmos.color = Color.red;
                //        Gizmos.DrawLine(Camera.main.transform.position, pos);

                //        pos += sX;
                //    }

                //    initPos += sY;
                //}

                foreach (var pos in _layPoslist)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(Camera.main.transform.position, pos);
                }
            }
        }

        class DamageEnemy
        {
            public Enemy enemy;
            public int damage;

            public DamageEnemy(Enemy enemy, int damage)
            {
                this.enemy = enemy;
                this.damage = damage;
            }

            public void AddDamage(int damage)
            {
                this.damage += damage;
            }
        }
    }

}