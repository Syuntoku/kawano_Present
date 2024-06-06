using UnityEngine;
using Cysharp.Threading.Tasks;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //ツール用　親クラス
    //======================================
    public class ToolBase : MonoBehaviour
    {
        float _time;
        public bool _bDig;

        protected GameObject _playerObject;
        protected BlockManage _blockManage;
        protected StatusManage _statusManage;

        protected ToolInfo _toolData;

        public ToolBase()
        {
            _time = 0;
            _bDig = false;
        }

        public void Initialize(BlockManage blockManage, GameObject playerObject , StatusManage statusManage)
        {
            _blockManage = blockManage;
            _playerObject = playerObject;
            _statusManage = statusManage;
        }

        public void SetStatus(ToolInfo toolInfo)
        {
            _toolData = toolInfo;
        }

        virtual public void ToolUpdate()
        {
            //掘った後のインターバル
            Interval(_toolData);
        }

        virtual public bool Dig(GameObject gameObject, DigStatus digStatus, ToolInfo toolData)
        {
            return false;
        }

        virtual public bool AttackEnemy(Ray playerDigReach, float distance, StatusManage statusManage)
        {
            return false;
        }

        public void HitPointEfect(Vector3 position, Vector3 normal)
        {
            if (_toolData._toolStatus.hitContactPointEfect == null) return;
            Instantiate(_toolData._toolStatus.hitContactPointEfect, position, Quaternion.Euler(normal));
        }

        /// <summary>
        /// 装備の性能を受け取る
        /// </summary>
        /// <param name="data"　></param>
        /// <param name="bNextLevel"></param>
        virtual public string GetLevelInDescription(int level)
        {
            return "";
        }
        /// <summary>
        /// 装備の性能を受け取る
        /// </summary>
        /// <param name="data"　></param>
        /// <param name="bNextLevel"></param>
        virtual public string GetLevelAmountInDescription(int level,int compareLevel)
        {
            return "";
        }

        /// <summary>
        /// 状態のチェック
        /// </summary>
        /// <returns>　掘れる状態 :　false  インターバル中 : true</returns>
        public bool IsDig()
        {
            return _bDig;
        }

        public ToolUpgrade GetUpgradeData()
        {
            return _toolData._toolUpgladeStatus;
        }

        virtual public float GetDamage()
        {
            return 0.0f;
        }

        /// <summary>
        /// インターバル　Updateですでに呼び出し済み
        /// </summary>
        void Interval(ToolInfo toolInfo)
        {
            if (_bDig)
            {
                _time += Time.deltaTime;

                if (_time >= toolInfo._toolStatus.interval * _statusManage.digmodeStatus.digStatus.ToolIntervalMagnification)
                {
                    _time = 0;
                    _bDig = false;
                }
            }
        }
    }


    //Rayにおける軸
    enum Axis
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        FORWARD,
        BACK,

    }



    //======================================
    //ツール用作業
    //======================================
    class ToolWorks
    {
        Ray _ray = new Ray();
        RaycastHit _hit;

        float DISTANCE = 1.0f;

        //設定した軸方向にRayを飛ばしブロックを取得
        public GameObject RayBlocks(Vector3 block, int axis)
        {
            //rayの開始地点
            _ray.origin = block;

            //入力をもとに方向をセット
            switch (axis)
            {
                case (int)Axis.UP:
                    _ray.direction = Vector3.up;
                    break;
                case (int)Axis.DOWN:
                    _ray.direction = Vector3.down;
                    break;
                case (int)Axis.RIGHT:
                    _ray.direction = Vector3.right;
                    break;
                case (int)Axis.LEFT:
                    _ray.direction = Vector3.left;
                    break;
                case (int)Axis.FORWARD:
                    _ray.direction = Vector3.forward;
                    break;
                case (int)Axis.BACK:
                    _ray.direction = Vector3.back;
                    break;
            }


            //Rayを飛ばす
            if (Physics.Raycast(_ray, out _hit, DISTANCE))
            {
                //とれているか
                if (_hit.collider.gameObject == null) return null;
                //ブロックのタグが付いている
                if (!_hit.collider.CompareTag(BlockManage.BLOCK_TAG_NAME)) return null;
                return _hit.collider.gameObject;
            }
            return null;
        }

        /// <summary>
        /// 配列の中に入っているブロックを指定した配列だけダメージを与える　
        /// 1. ブロックの配列　2.BlockManageクラス　3.ダメージ量　4.配列の最初　5.配列の最後 6.待機する時間
        /// </summary>
        async public UniTask ArrayBreak(GameObject[] Obj,DamageManager damageManager,BlockManage blockManage,ToolInfo toolInfo, int arrayStart, int arrayEnd, int waitTime)
        {
            await UniTask.Delay(waitTime);

            for (int i = arrayStart; i < arrayEnd; i++)
            {
                if (Obj[i] == null) continue;

                BlockData blockData = Obj[i].GetComponent<BlockData>();

                if (blockData == null) continue;
                blockManage.SendBreakDamage(ref blockData, damageManager, toolInfo);
            }
        }
    }
}