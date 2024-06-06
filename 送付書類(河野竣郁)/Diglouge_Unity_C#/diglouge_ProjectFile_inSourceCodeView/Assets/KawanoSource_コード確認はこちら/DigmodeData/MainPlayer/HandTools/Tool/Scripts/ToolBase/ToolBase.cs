using UnityEngine;
using Cysharp.Threading.Tasks;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //�c�[���p�@�e�N���X
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
            //�@������̃C���^�[�o��
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
        /// �����̐��\���󂯎��
        /// </summary>
        /// <param name="data"�@></param>
        /// <param name="bNextLevel"></param>
        virtual public string GetLevelInDescription(int level)
        {
            return "";
        }
        /// <summary>
        /// �����̐��\���󂯎��
        /// </summary>
        /// <param name="data"�@></param>
        /// <param name="bNextLevel"></param>
        virtual public string GetLevelAmountInDescription(int level,int compareLevel)
        {
            return "";
        }

        /// <summary>
        /// ��Ԃ̃`�F�b�N
        /// </summary>
        /// <returns>�@�@����� :�@false  �C���^�[�o���� : true</returns>
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
        /// �C���^�[�o���@Update�ł��łɌĂяo���ς�
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


    //Ray�ɂ����鎲
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
    //�c�[���p���
    //======================================
    class ToolWorks
    {
        Ray _ray = new Ray();
        RaycastHit _hit;

        float DISTANCE = 1.0f;

        //�ݒ肵����������Ray���΂��u���b�N���擾
        public GameObject RayBlocks(Vector3 block, int axis)
        {
            //ray�̊J�n�n�_
            _ray.origin = block;

            //���͂����Ƃɕ������Z�b�g
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


            //Ray���΂�
            if (Physics.Raycast(_ray, out _hit, DISTANCE))
            {
                //�Ƃ�Ă��邩
                if (_hit.collider.gameObject == null) return null;
                //�u���b�N�̃^�O���t���Ă���
                if (!_hit.collider.CompareTag(BlockManage.BLOCK_TAG_NAME)) return null;
                return _hit.collider.gameObject;
            }
            return null;
        }

        /// <summary>
        /// �z��̒��ɓ����Ă���u���b�N���w�肵���z�񂾂��_���[�W��^����@
        /// 1. �u���b�N�̔z��@2.BlockManage�N���X�@3.�_���[�W�ʁ@4.�z��̍ŏ��@5.�z��̍Ō� 6.�ҋ@���鎞��
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