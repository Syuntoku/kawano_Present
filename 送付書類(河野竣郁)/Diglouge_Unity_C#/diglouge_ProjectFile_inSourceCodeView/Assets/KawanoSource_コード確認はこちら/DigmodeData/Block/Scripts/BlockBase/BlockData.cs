using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Syuntoku.DigMode.Settings;
using Syuntoku.Status;
using static BlockScriptable;
using UnityEngine.UIElements;

namespace Syuntoku.DigMode
{
    public enum BlockState
    {
        ENABLE = 0x00,
        BREAKED = 0x02,
        PLAY_EFECT = 0x04,
        UNTACH = 0x08,
    }

    /// <summary>
    /// ブロックオブジェクト本体につけるクラス
    /// </summary>
    public class BlockData : MonoBehaviour
    {
        public BlockdataInfo _blockdataInfo { get; private set; }
        CancellationToken _token;

        readonly Vector3 DAMAGED_SCAL = new Vector3(1.02f, 1.02f, 1.02f);
        const int SCALE_DELAY = 50;

        public BlockData()
        {
        }

        //==========================
        //unity 
        //==========================
        private void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        //==========================
        //public 
        //==========================
        public void Initialize(ref BlockdataInfo blockData)
        {
            _token = this.GetCancellationTokenOnDestroy();
            _blockdataInfo = blockData;
            _blockdataInfo.SendDamage = Damage;
            _blockdataInfo.attathedObject = this;

            blockData.attathedObject = this;
        }

        /// <summary>
        /// ブロックにダメージを与える
        /// </summary>
        public virtual void Damage(DamageManager damageManager)
        {
            SendDamage(damageManager);
        }

        public async void SendDamage(DamageManager damageManager)
        {
            if (_blockdataInfo.hp <= 0) return;

            //即破壊
            if (damageManager.bBreak)
            {
                _blockdataInfo.hp = 0;
            }
            else
            {
                _blockdataInfo.hp -= damageManager.damage;
            }
#if UNITY_EDITOR
            if (_blockdataInfo.attathedObject == null)
            {
                Debug.Log("アタッチオブジェクトNULL");
            }
#endif
            if (_blockdataInfo.hp <= 0)
            {
                _blockdataInfo.bitFlagBlockState = (uint)BlockState.BREAKED;
            }

            if(_blockdataInfo.AdvansedCheckFlag(BlockAdvancedSetting.NOT_BLOCK_DAMAGE_SCALE))
            {
                if (_blockdataInfo.hp <= 0)
                {
                    _blockdataInfo.SetAirData();
                    BreakAction();
                    Destroy(gameObject);
                }
                return;
            }

            //ヒット時のブロック拡大
            
            _blockdataInfo.fieldBlockStatus.scale = DAMAGED_SCAL;
            await UniTask.Delay(SCALE_DELAY, cancellationToken: _token);
            _blockdataInfo.fieldBlockStatus.scale = Vector3.one;

            if (_blockdataInfo.hp <= 0)
            {
                BreakAction();
                _blockdataInfo.SetAirData();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// ブロックを破壊する
        /// </summary>
        public void Break()
        {
           if (_blockdataInfo.CategoryCheck(Category.STATIC)) return;
            _blockdataInfo.hp = 0;
            _blockdataInfo.bitFlagBlockState = (uint)BlockState.BREAKED;
            _blockdataInfo.bitFlagBlockState += (int)BlockState.UNTACH;
            InstanceBreakEfect(this, transform.position);
            BreakAction();
            _blockdataInfo.SetAirData();
            Destroy(gameObject);
        }

        /// <summary>
        /// 破壊後の動き
        /// </summary>
        virtual public void BreakAction()
        {

        }
#if UNITY_EDITOR
        /// <summary>
        /// デバッグ時に使用
        /// </summary>
        public void DebugBreak()
        {
            Destroy(gameObject);
        }
#endif
        /// <summary>
        /// アタッチされているオブジェクトのposを返す
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }

        public BlockdataInfo GetBlockDataInfo() 
        {
            return _blockdataInfo;
        }

        /// <summary>
        /// 設定されている破壊エフェクトを生成
        /// </summary>
        /// <param name="position"></param>
        public void InstanceBreakEfect(BlockData blockData, Vector3 position)
        {
            BlockdataInfo blockdataInfo = blockData._blockdataInfo;
            if (blockdataInfo == null) return;
            Instantiate(blockdataInfo.breakEfect, position, Quaternion.identity);
            blockdataInfo.bitFlagBlockState += (uint)BlockState.PLAY_EFECT;
        }
    }

    /// <summary>
    /// ブロックのデータ　ブロック生成時につかう
    /// </summary>
    [Serializable]
    public class BlockdataInfo
    {
        //オブジェクト情報
        public string name;
        public float hp;
        public float maxHp;
        public Category category;
        public BlockKind blockKind;
        public SpescialCategory spescialCategory;
        public uint bitFlagBlockState;

        public BlockData attathedObject;
        public DropSetting dropSetting;
        public FieldBlockStatus fieldBlockStatus;

        public GameObject breakEfect;
        public BlockAdvancedSetting advancedSetting;

        /// <summary>
        /// 生成設定の有効をチェックする
        /// </summary>
        /// <returns>true:有効　false:無効</returns>
        public bool AdvansedCheckFlag(BlockAdvancedSetting check)
        {
            if (((uint)advancedSetting & (uint)check) == (uint)check) return true;
            return false;
        }

        public delegate void Damage(DamageManager damageManager);
        /// <summary>
        /// ブロックにダメージを与える
        /// </summary>
        public Damage SendDamage;

        public BlockdataInfo()
        {
            fieldBlockStatus = new FieldBlockStatus();
        }

        /// <summary>
        /// コピー用コンストラクタ
        /// </summary>
        /// <param name="blockData">コピーするデータ</param>
        /// <param name="blockPosition">インスタンスする前のポジションデータ</param>
        public BlockdataInfo(BlockdataInfo blockData, BlockPositionInfo blockPosition)
        {
            name = blockData.name;
            hp = blockData.hp;
            maxHp = blockData.hp;
            category = blockData.category;
            SendDamage = blockData.SendDamage;
            attathedObject = blockData.attathedObject;
            dropSetting = blockData.dropSetting;
            spescialCategory = blockData.spescialCategory;
            breakEfect = blockData.breakEfect;
            advancedSetting = blockData.advancedSetting;
            fieldBlockStatus =FieldBlockStatus.CopyData(blockData.fieldBlockStatus);
            fieldBlockStatus.positionInfo = new BlockPositionInfo(blockPosition);
        }

        //================================================
        //public
        //================================================
        public void Initialize(string name, int hp, Category category, BlockGenerateData blockGenerate,Mesh defaultMesh)
        {
            this.name = name;
            this.hp = hp;
            maxHp = hp;
            this.category = category;
            breakEfect = blockGenerate.breakEfectPrefab;
            fieldBlockStatus.useMaterial = blockGenerate.material;
            advancedSetting = blockGenerate.advancedSetting;
            fieldBlockStatus.nowMesh = defaultMesh;
            bitFlagBlockState = 0x00;
        }
        public void Initialize(string name, int hp, Category category, BlockKind blockKind, BlockGenerateData blockGenerate, Mesh defaultMesh)
        {
            this.name = name;
            this.hp = hp;
            maxHp = hp;
            this.category = category;
            this.blockKind = blockKind;
            breakEfect = blockGenerate.breakEfectPrefab;
            fieldBlockStatus.useMaterial = blockGenerate.material;
            advancedSetting = blockGenerate.advancedSetting;
            fieldBlockStatus.nowMesh = defaultMesh;
            bitFlagBlockState = 0x00;
        }
        public void Initialize(string name, int hp, Category category, SpescialCategory spescialCategory, BlockGenerateData blockGenerate, Mesh defaultMesh)
        {
            this.name = name;
            this.hp = hp;
            maxHp = hp;
            this.category = category;
            this.spescialCategory = spescialCategory;
            breakEfect = blockGenerate.breakEfectPrefab;
            fieldBlockStatus.useMaterial = blockGenerate.material;
            advancedSetting = blockGenerate.advancedSetting;
            fieldBlockStatus.nowMesh = defaultMesh;
            bitFlagBlockState = 0x00;
        }
        public void Initialize(string name, int hp, Category category,DropSetting dropSetting )
        {
            this.name = name;
            this.hp = hp;
            maxHp = hp;
            this.category = category;
            this.dropSetting = dropSetting;
            bitFlagBlockState = 0x00;
        }

        /// <summary>
        /// 初期化３　配列ポジションを入れる
        /// </summary>
        public void Initialize(Vector3 pos)
        {
            fieldBlockStatus.positionInfo.SetIndex(pos);
        }

        public void SetMethot(Damage damage)
        {
            SendDamage = new Damage(damage);
        }

        public void SendBlockDamage(DamageManager damageManager)
        {
#if UNITY_EDITOR
            if (attathedObject == null)
            {
                Debug.Log("block:AttachNull");
                return;
            }
#endif
            attathedObject.Damage(damageManager);
        }

        /// <summary>
        /// ブロックのステート状態をチェック
        /// </summary>
        /// <param name="checkState"></param>
        /// <returns>true:引数のステート中　false:別ステート中</returns>
        public bool StateCheck(BlockState checkState)
        {
            if (((BlockState)bitFlagBlockState & checkState) == checkState) return true;

            return false;
        }

        /// <summary>
        /// ブロックのカテゴリーを調べる
        /// </summary>
        /// <param name="category"></param>
        /// <returns>true:設定カテゴリー　false:別カテゴリー</returns>
        public bool CategoryCheck(Category category)
        {
            return (this.category & category) == category;
        }

        public void AddState(uint addState)
        {
            bitFlagBlockState += addState;
        }

        public void SetAirData()
        {
            category = Category.AIR;
        }

        public void SetBedRock(BlockGenerateData blockData)
        {
          
            category = Category.STATIC;
            blockKind = BlockKind.BED_ROCK;
            fieldBlockStatus.useMaterial = blockData.material;
        }
    }
}
