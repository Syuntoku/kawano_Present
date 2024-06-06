using UnityEngine;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode
{
    /// <summary>
    /// ブロックのダメージやエフェクトを管理するクラス
    /// </summary>
    public class BlockManage : MonoBehaviour
    {
        [SerializeField] ParkConditionsManage _parkConditionsManage;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] UICameraManage _uiCameraManage;
        [SerializeField] StatusManage _statusManage;
        [SerializeField] GameObject _soundObject;
        [SerializeField] static GameObject _soundManager;
        [SerializeField] InventoryManage _inventoryManage;
        [SerializeField] static GameObject _soundParent;
        [SerializeField] GameObject _soundParentNoramal;
        [SerializeField] DropManager _dropManager;
        [SerializeField] BlockScriptable blockScriptable;
        [SerializeField] DamageText _damageText;

        public static bool bBreaker;
        const int MAX_SOUND  =6;

        const float DAMAGE_TEXT_SIZE = 2.0f;
        const float Y_AJUST_AMOUNT = 0.5f;
        public const string BLOCK_TAG_NAME = "block";
        public const string BLOCK_LAYER_NAME = "Block";
        const float CHANGE_MESH_HP_PRIBABILITY_FIRST = 0.6f;　//1回目の破壊メッシュを切り替える倍率
        const float CHANGE_MESH_HP_PRIBABILITY_SECOND = 0.3f;//2回目の破壊メッシュを切り替える倍率

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _soundManager = _soundObject;
            _soundParent = _soundParentNoramal;
        }

        /// <summary>
        /// 対象のブロックにダメージを与える
        /// </summary>
        /// <param name="blockData"></param>
        /// <param name="damage"></param>
        /// <param name="toolInfo"></param>
        public void SendBreakDamage(ref BlockData blockData, DamageManager damage, ToolInfo toolInfo = null,bool bEnemyBreak = false )
        {
            //ブロック情報を取得
           BlockdataInfo workDataInfo = blockData.GetBlockDataInfo();

            if (workDataInfo.CategoryCheck(BlockScriptable.Category.STATIC)) return;
            if (workDataInfo.StateCheck(BlockState.BREAKED)) return;

            Vector3 hitPosition = blockData.gameObject.transform.position;
            AudioClip audioClip;

            if (!bEnemyBreak) _parkConditionsManage._oneSwingDamage++;

            //ブロックにダメージを与える
            DamageManager resltDamage = damage.DamageCalculation(_statusManage.digmodeStatus.digStatus);
            blockData.Damage(resltDamage);

            if (!bEnemyBreak)
            {
                _uiCameraManage.DrawStatusUi(hitPosition, workDataInfo.hp, workDataInfo.maxHp);
                _damageText.InstanceDamageText(hitPosition, resltDamage.damage, Y_AJUST_AMOUNT, DAMAGE_TEXT_SIZE);
            }

            if (toolInfo != null)
            {
                audioClip = damage.bBreak ? toolInfo._toolStatus.hitcriticalSound : toolInfo._toolStatus.hitSound;
               
                InstanceAudio(audioClip, hitPosition);

                if (toolInfo._toolStatus.hitBreakPointEfect != null)
                {
                    InstanceHitEfect(toolInfo._toolStatus.hitBreakPointEfect, blockData.transform.position);
                }
            }

            float work = workDataInfo.hp / workDataInfo.maxHp;

            //メッシュを変更する
            if (!workDataInfo.CategoryCheck(BlockScriptable.Category.PRIVATE_MESH)
                && !workDataInfo.CategoryCheck(BlockScriptable.Category.SPECIAL))
            {
                if (work <= CHANGE_MESH_HP_PRIBABILITY_FIRST)
                {
                    if (work <= CHANGE_MESH_HP_PRIBABILITY_SECOND)
                    {
                        blockData._blockdataInfo.fieldBlockStatus.nowMesh = blockScriptable.breakMesh_30;
                    }
                    else
                    {
                        blockData._blockdataInfo.fieldBlockStatus.nowMesh = blockScriptable.breakMesh_60;
                    }
                }
            }

            //ブロックが壊れたとき
            if (workDataInfo.StateCheck(BlockState.BREAKED))
            {
                workDataInfo.bitFlagBlockState += (int)BlockState.UNTACH;
                _dropManager.InstanceJuwelry( workDataInfo.dropSetting, blockData.gameObject.transform.position);
                blockData.InstanceBreakEfect(blockData, blockData.transform.position);

                _audioSource.Play();

                if (!bEnemyBreak)
                {
                    _parkConditionsManage.AddBreakCounter();
                    _statusManage.digmodeStatus.SetActionTrigger(_statusManage, PublicStatus.ActiveTrigger.Break);
                    _inventoryManage.BreakUpDate();
                }
            }
        }

        void InstanceHitEfect(GameObject hitPointEfect,Vector3 position)
        {
            Instantiate(hitPointEfect, position, Quaternion.identity);
        }

        /// <summary>
        /// ブロックに固定の倍率分のダメージを与える
        /// </summary>
        /// <param name="blockData"></param>
        /// <param name="magnification"></param>
        public void SendFixedMagnificationDamage(BlockData blockData,float magnification)
        {
            DamageManager damageManager = new DamageManager();
            damageManager.damage = blockData.GetBlockDataInfo().maxHp * magnification;
            SendBreakDamage(ref blockData, damageManager,bEnemyBreak:true);
        }

        static public bool IsBlock(RaycastHit raycastHit)
        {
            if (raycastHit.collider == null) return false;
            return raycastHit.collider.CompareTag(BLOCK_TAG_NAME);
        }

        static public void InstanceAudio(AudioClip audioClip, Vector3 position)
        {
            if (_soundParent.transform.childCount >= MAX_SOUND) return;
            SoundObject soundObject = Instantiate(_soundManager, position, Quaternion.identity, _soundParent.transform).GetComponent<SoundObject>();
            soundObject.SetAudio(audioClip, position);
        }
    }
}
