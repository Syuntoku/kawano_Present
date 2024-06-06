using UnityEngine;
using Syuntoku.Status;
using Syuntoku.DigMode.Enemy;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //�s�b�P���@�ڂ̑O�̃u���b�N��}�X
    //======================================
    class PickAxe : ToolBase
    {

        //�R���X�g���N�^
        public PickAxe()
        {

        }

        /// <summary>
        /// Update�@�@��C���^�[�o���͕W��
        /// </summary>
        public override void ToolUpdate()
        {
            base.ToolUpdate();
        }

        /// <summary>
        ///�@��@
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

            //�G�Ƀ_���[�W��^����
            raycastHit.collider.GetComponent<EnemyBase>().SendDamage(_toolData._toolStatus.enemyAttack, _toolData._toolStatus.knockBackPower, _playerObject.transform.forward);

            return false;
        }
    }
}
