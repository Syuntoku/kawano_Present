using UnityEngine;
using Syuntoku.Status;
using Syuntoku.DigMode.Enemy;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //ピッケル　目の前のブロック一マス
    //======================================
    class PickAxe : ToolBase
    {

        //コンストラクタ
        public PickAxe()
        {

        }

        /// <summary>
        /// Update　掘るインターバルは標準
        /// </summary>
        public override void ToolUpdate()
        {
            base.ToolUpdate();
        }

        /// <summary>
        ///掘る　
        /// </summary>
        public override bool Dig(GameObject gameObject,DigStatus digStatus,ToolInfo toolData)
        {
            if (!_bDig)
            {
                _bDig = true;
                BlockData blockData = gameObject.GetComponent<BlockData>();
               _blockManage.SendBreakDamage(ref blockData, _toolData._toolStatus.damageManager, _toolData);
                return true;   
            }
            return false;

        }

        public override bool AttackEnemy(Ray playerDigReach,float distance, StatusManage statusManage)
        {
            RaycastHit raycastHit;

            if(!Physics.Raycast(playerDigReach,out raycastHit, distance, LayerMask.NameToLayer("Enemy")))return false;

            //敵にダメージを与える
            raycastHit.collider.GetComponent<EnemyBase>().SendDamage(_toolData._toolStatus.enemyAttack, _toolData._toolStatus.knockBackPower, _playerObject.transform.forward);

            return false;
        }
    }
}
