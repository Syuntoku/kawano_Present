using UnityEngine;
using Syuntoku.Status;
using DG.Tweening;
using Syuntoku.DigMode.Enemy;

namespace Syuntoku.DigMode.Tool
{
    //===================================
    //ボールを管理するクラス
    //===================================
    public class DigBall : MonoBehaviour
    {
        Vector3 _direction;
        BlockManage _blockManage;
        BallStatus _ballStatus;
        GunInfo _gunInfo;
        float _time;
        int _boundCount;
        [Tooltip("弾の最終的なサイズ")]
        readonly Vector3 BALL_SCALE = new Vector3(0.2f,0.2f,0.2f);
        const float END_SCALE_TIME = 0.1f;

        //================================
        //Unity
        //================================
        void FixedUpdate()
        {
            //ベクトル方向に移動させる
            transform.position += _direction *_gunInfo._shotSpeed  * _ballStatus._speedMagnification;
            _time += Time.deltaTime;

            if (_time > _gunInfo._destroyTime || _boundCount > _ballStatus._reflectionCount)
            {
                Destroy(gameObject);
            }

            _ballStatus.Update();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(BlockManage.BLOCK_TAG_NAME))
            {
                BlockData blockData = collision.gameObject.GetComponent<BlockData>();
                if (blockData == null) return;
                _blockManage.SendBreakDamage(ref blockData,_gunInfo._toolStatus.damageManager,_gunInfo);
                // 衝突した面の、接触した点における法線ベクトルを取得
                Vector3 normal = collision.contacts[0].normal;
                //反射させる
                _direction = Vector3.Reflect(_direction, normal);
                _boundCount++;
                //反射時の追加効果
                _ballStatus.ReflectActive(_ballStatus);
            }
            else if(collision.gameObject.CompareTag(EnemyBase.ENEMY_TAG))
            {
                collision.gameObject.GetComponent<EnemyBase>().SendDamage(_ballStatus._enemyDamage, _ballStatus._knockBackPower, transform.forward);
            }
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        //================================
        //public
        //================================
        public void Initialzie(BlockManage blockManage, Vector3 direction, BallStatus ballStatus ,GunInfo toolInfo)
        {
            //方向を取得
            _direction = direction;
            //正規化
            _direction = _direction.normalized;
            _blockManage = blockManage;
            _ballStatus = new BallStatus(ballStatus);
            _gunInfo = toolInfo;

            transform.localScale = Vector3.zero;
            transform.DOScale(BALL_SCALE, END_SCALE_TIME).SetEase(Ease.InElastic);
        }
    }
}