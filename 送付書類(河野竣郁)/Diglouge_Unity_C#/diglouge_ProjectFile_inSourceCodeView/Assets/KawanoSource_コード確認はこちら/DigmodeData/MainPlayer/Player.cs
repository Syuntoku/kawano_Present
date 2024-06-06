using UnityEngine;
using Syuntoku.Status;
using Syuntoku.DigMode.Player.MainSkill;
using Syuntoku.DigMode.Input;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Weapon;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode.Tool.Swing;
using Battle;
using Syuntoku.DigMode.Enemy;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode.Player
{
    public class Player : MonoBehaviour
    {
        #region CashVariable
        [SerializeField] Light _suitLight;
        [SerializeField] ParkConditionsManage _parkConditions;
        [SerializeField] StatusManage _statusManage;
        [SerializeField] IsGraundedCheck _isGraundedCheck;
        [SerializeField] GameObject _cameraObject;
        [SerializeField] BlockManage _blockManage;
        [SerializeField] ToolGenerater _toolGenerater;
        [SerializeField] Transform _toolPoket;
        [SerializeField] Transform _weaponPoket;
        [SerializeField] InventoryManage _inventoryManage;
        [SerializeField] WorldData _worldSetting;

        [SerializeField] UIManage _uiManage;
        [SerializeField] GameSetting _gameSetting;
        [SerializeField] GameObject _respornEfect;
        [SerializeField] GameObject _blackSphere;
        [SerializeField] EnemyGenerator _enemyGenerater;
        [SerializeField] MainUI _mainUI;
        #endregion

        public GameObject attackEnemy;

        public const string PLAYER_TAG = "Player";
        public const string ATTACH_OBJECT_NAME = "Player";

        public FirstPerson _firstPerson;
        public MainSkillManage _mainSkillManage;
        PlayerInventory _playerInventory;
        SwingTool _swingTool;

        public float _tall;
        public float _lineOfsightLength;
        public LayerMask _rayLayerMask;

        const int ON_HAND_TOOL_COUNT = 2;
        const int ON_WEAPON_COUNT = 2;
        const int ACTIVE_LENGTH = -2;

        //�c�[�����
        ToolInfo[] _handToolData;
        ToolBase[] _handToolObject;
        WeaponBase[] _handWeaponObject;

        int _selectToolIndex;
        int _selectWeaponIndex;

        Ray _playerRay;
        bool _bDigmode;
        bool _bHold;

        public static bool binBase;

        [Header("�X�e�[�^�X")]
        public float hp;
        public float maxHp;

        public Player()
        {
            _firstPerson = new FirstPerson();
            _mainSkillManage = new MainSkillManage();
            _playerInventory = new PlayerInventory();
            _swingTool = new SwingTool();

            _handToolData = new ToolInfo[ON_HAND_TOOL_COUNT];
            _handToolObject = new ToolBase[ON_HAND_TOOL_COUNT];
            _handWeaponObject = new WeaponBase[ON_WEAPON_COUNT];
        }

        //======================================
        //Unity
        //======================================
        void Start()
        {
            _firstPerson.Initalize(gameObject, GetComponent<Animator>(), _statusManage);
            _mainSkillManage.Initialize(gameObject, _respornEfect);
            _playerInventory.SetMainUI(_mainUI);
            _isGraundedCheck.Initialize(_firstPerson);

            _playerInventory.AddTool(_toolGenerater.InstanceToolData(ToolGenerater.ToolName.PICK_AXE));
            _playerInventory.AddTool(_toolGenerater.InstanceToolData(ToolGenerater.ToolName.HAMMER));
            _playerInventory.AddTool(_toolGenerater.InstanceToolData(ToolGenerater.ToolName.GUN));
            _playerInventory.AddWeaon(_toolGenerater.InstanceWeaponData(ToolGenerater.WeaponName.Revolver));
            _playerInventory.AddWeaon(_toolGenerater.InstanceWeaponData(ToolGenerater.WeaponName.Shotgun));

            UpdateToolEquipment();
            _swingTool.SetInitPosition(_toolPoket.transform.localPosition);
            _mainUI.SetEquipmentText(_handToolData[_selectToolIndex]._toolStatus.toolName);
            _toolPoket.GetChild(_selectToolIndex).gameObject.SetActive(true);
            _mainUI.ActiveToolHUD();
            _bDigmode = true;
            _mainUI.ChangeHpUI(hp, maxHp);
        }

        private void FixedUpdate()
        {
            if (_gameSetting.bStopGameAction) return;
            _firstPerson.TranfromUpdate();
        }

        void Update()
        {
            if (_gameSetting.bStopGameAction) return;

            ModeChange();
            _mainSkillManage.Update();
            HandAction();
            SelectToolChange();
            EquipmentUpdate();

            if (_bDigmode)
            {
                _handToolObject[_selectToolIndex].ToolUpdate();
            }
            else
            {
                _mainUI.SetMagazineSize((int)_handWeaponObject[_selectWeaponIndex]._ammunitionRemaining);
            }
            //�p�[�N�p�̃f�[�^��ύX
            ChangeConditionStatus();

            //���E�𐧌�����I�u�W�F�N�g��L���ɂ���
            if (gameObject.transform.position.y <= ACTIVE_LENGTH)
            {
                _worldSetting.ChangeDigLightPower();
                _blackSphere.SetActive(true);
            }
            else
            {
                _worldSetting.ChangeHomeLightPower();
                _blackSphere.SetActive(false);
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.Z))
            {
                attackEnemy.SetActive(true);
            }
        }

        //===========================================
        //public
        //===========================================
        /// <summary>
        /// �_���[�W�����炤
        /// </summary>
        /// <param name="damage">�_���[�W��</param>
        /// <param name="damageSource">�_���[�W��^�����G��Transform</param>
        public void SendDamage(float damage, Transform damageSource)
        {
            if (damage < 0) return;
            DI_System.CreateIndicator(damageSource);
            hp -= damage;
            _mainUI.ChangeHpUI(hp, maxHp);
            if (hp <= 0)
            {
                hp = maxHp;
                _firstPerson.Resporn();
                _enemyGenerater.DespornAllEnemy();
            }
        }
        /// <summary>
        /// HP���񕜂���
        /// </summary>
        public void HealHp(float healPower, GameObject healEfect = null)
        {
            if (healPower < 0) return;

            hp += healPower;
            if(hp >= maxHp)
            {
                hp = maxHp;
            }
            _mainUI.ChangeHpUI(hp, maxHp);

            if (healEfect == null) return;
            Instantiate(healEfect,transform.position, Quaternion.identity,transform);
        }

        /// <summary>
        /// �ڐ��ɂ���u���b�N�̃f�[�^���擾
        /// </summary>
        /// <returns></returns>
        public RaycastHit GetFowordRaycastData()
        {
            RaycastHit raycastHit;
            Physics.Raycast(_playerRay, out raycastHit, _lineOfsightLength, _rayLayerMask);
            return raycastHit;
        }

        /// <summary>
        /// �X�[�c�̃��C�g�p���[��ύX����
        /// </summary>
        public void ChangeSuitLightPower(float magnifacation)
        {
            _suitLight.intensity = _suitLight.intensity * magnifacation;
        }

        /// <summary>
        /// �v���C���[�C���x���g���ɐݒ肳�ꂽ������ݒ肷��
        /// </summary>
        public void UpdateToolEquipment()
        {
            _handToolData = _playerInventory.GetEquipmentTools();
            WorldInstanceTool(_playerInventory.GetEquipmentTools());
            WorldInstanceWeapon(_playerInventory.GetEquipmentWeapons());
        }

        /// <summary>
        /// �ēx���������ăX�e�[�^�X���v�Z���Ȃ���
        /// </summary>
        public void ChangeHandUpdate()
        {
            if (_bDigmode)
            {
                _handToolData[_selectToolIndex].SetHandSetting(_statusManage);
            }
            else
            {
                _handWeaponObject[_selectToolIndex].SetHoldTool();
            }
        }

        /// <summary>
        /// ���������Ă��Ȃ��X�e�[�^�X�ɖ߂�
        /// </summary>
        public void NeutralHand()
        {
            if (_bDigmode)
            {
                _handToolData[_selectToolIndex].OutHandSetting(_statusManage);
            }
            else
            {
                _handWeaponObject[_selectToolIndex].PutItAway();
            }
        }

        /// <summary>
        /// ������؂�ւ�����
        /// </summary>
        public void EquipmentUpdate(bool bReset = false)
        {
            if (bReset)
            {
                DisableTools();
            }
            if (_bDigmode)
            {
                if (_toolPoket.GetChild(_selectToolIndex).gameObject.activeSelf) return;
                _mainUI.SetEquipmentText(_handToolData[_selectToolIndex]._toolStatus.toolName);
                _toolPoket.GetChild(_selectToolIndex).gameObject.SetActive(true);
            }
            else
            {
                if (_weaponPoket.GetChild(_selectToolIndex).gameObject.activeSelf) return;
                _mainUI.SetEquipmentText(_handWeaponObject[_selectWeaponIndex]._toolName);
                _weaponPoket.GetChild(_selectWeaponIndex).gameObject.SetActive(true);
            }
        }

        public void DisableTools()
        {
            foreach (Transform item in _toolPoket.transform)
            {
                item.gameObject.SetActive(false);
            }
            foreach (Transform item in _weaponPoket.transform)
            {
                item.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// �v���C���[�̃C���x���g�����擾����
        /// </summary>
        /// <returns></returns>
        public PlayerInventory GetPlayerInventory()
        {
            return _playerInventory;
        }
        /// <summary>
         /// �v���C���[�̃C���x���g�����擾����
         /// </summary>
         /// <returns></returns>
        public JuwelryInventory GetJuwelryInventory()
        {
            return _playerInventory.GetjuwelryInventory();
        }

        //===========================================
        //private
        //===========================================
        /// <summary>
        /// �c�[����Ԃƕ����Ԃ�؂�ւ���
        /// </summary>
        void ModeChange()
        {
            if (InputData._bModeChange)
            {
                if (_bHold) return;

                NeutralHand();

                //���킪��������Ă��Ȃ������ꍇ�͐؂�ւ��Ȃ�
                if (!_bDigmode)
                {
                    if (_weaponPoket.childCount == 0) return;
                    _mainUI.ActiveToolHUD();
                }
                else
                {
                    _mainUI.ActiveWeaponHUD();
                }
                _bDigmode = !_bDigmode;
                _bHold = true;
                EquipmentUpdate(true);
                ChangeHandUpdate();
            }
            else
            {
                _bHold = false;
            }
        }

        void RayUpdate()
        {
            _playerRay.origin = _cameraObject.transform.position;
            _playerRay.direction = _cameraObject.transform.forward;
#if UNITY_EDITOR
            Debug.DrawRay(_playerRay.origin, _playerRay.direction * _lineOfsightLength);
#endif
        }

        void ChangeConditionStatus()
        {
            if (transform.position.y <= 0)
            {
                _parkConditions.hierarchy = (int)Mathf.Abs(transform.position.y + _tall);
            }
            else
            {
                _parkConditions.hierarchy = 0;
            }

            _parkConditions.IsJump = !_firstPerson.IsGround();
        }

        /// <summary>
        /// �c�[����؂�ւ���
        /// </summary>
        /// <returns>�ύX�@true �ύX�Ȃ��@false</returns>
        bool SelectToolChange()
        {
            if (InputData._middleHweel.y == 0) return false;
            int work;
            NeutralHand();

            //����̎莝����ύX
            if (_bDigmode)
            {
                //�������Ă��铹���̏ꍇ�͕ύX���Ȃ�
                if (_toolPoket.childCount == 1) return false;
                work = (int)Mathf.Sign(InputData._middleHweel.y);
                _selectToolIndex += work;
                if (_selectToolIndex < 0) _selectToolIndex = ON_HAND_TOOL_COUNT - 1;
                _selectToolIndex = _selectToolIndex % ON_HAND_TOOL_COUNT;
            }
            //����̎莝����ύX
            else
            {
                if (_weaponPoket.childCount == 1) return false;
                work = (int)Mathf.Sign(InputData._middleHweel.y);
                _selectWeaponIndex += work;
                if (_selectWeaponIndex < 0) _selectWeaponIndex = ON_HAND_TOOL_COUNT - 1;
                _selectWeaponIndex = _selectWeaponIndex % ON_HAND_TOOL_COUNT;
            }

            EquipmentUpdate(true);
            ChangeHandUpdate();
            return true;
        }

        /// <summary>
        /// �u���b�N��K�v�Ƃ���̌@
        /// </summary>
        /// <param name="hitBlock"></param>
        bool UseBlockDig(RaycastHit hitBlock)
        {
            if (!BlockManage.IsBlock(hitBlock)) return false;
            //�@��
            BlockData data = hitBlock.collider.gameObject.GetComponent<BlockData>();

            if (!data._blockdataInfo.StateCheck(BlockState.ENABLE)) return false;

            _inventoryManage.SwingUpdate();
            //�G�����u���b�N�̕����̌v�Z
            RunCalculation(hitBlock);
            _statusManage.digmodeStatus.SetActionTrigger(_statusManage, PublicStatus.ActiveTrigger.Call);
            _handToolObject[_selectToolIndex].HitPointEfect(hitBlock.point, hitBlock.normal);
            bool result = _handToolObject[_selectToolIndex].Dig(hitBlock.collider.gameObject, _statusManage.digmodeStatus.digStatus, _handToolData[_selectToolIndex]);
            if (result) _inventoryManage.OnHitBlock();
            _statusManage.digmodeStatus.DisableAction(_statusManage, PublicStatus.KeyAction.Probability);
            _inventoryManage.EndSwing();
            return result;
        }

        /// <summary>
        /// �G�����u���b�N�̌v�Z
        /// </summary>
        /// <param name="hit"></param>
        void RunCalculation(RaycastHit hit)
        {
            if (hit.collider == null) return;

            Vector3 targetPosition = hit.collider.gameObject.transform.position;
            float result = gameObject.transform.position.y - targetPosition.y;

            //�u���b�N�ƃv���C���[�̍����̍����P�ȏゾ�Ə㉺�ǂ��炩
            if (Mathf.Abs(result) >= 1.0f)
            {
                if (Mathf.Sign(result) < 0)
                {
                    _parkConditions.blockState_Direction_virtical = (int)ParkConditionsManage.BlockState_Direction.DOWN;
                    return;
                }
                else
                {
                    _parkConditions.blockState_Direction_virtical = (int)ParkConditionsManage.BlockState_Direction.UP;
                    return;
                }
            }

            //�����̍����Ȃ���
            Vector3 playerPos = gameObject.transform.position;
            playerPos.y = 0.0f;
            targetPosition.y = 0.0f;

            //���E�����߂�
            float dot = Vector3.Dot(Vector3.forward, playerPos);

            if (dot < 0)
            {
                _parkConditions.blockState_Direction_horizontal = (int)ParkConditionsManage.BlockState_Direction.RIGHT;
                return;
            }
            else
            {
                _parkConditions.blockState_Direction_horizontal = (int)ParkConditionsManage.BlockState_Direction.LEFT;
                return;
            }
        }

        /// <summary>
        /// ��Ɏ����Ă��鑕���̃A�N�V����
        /// </summary>
        void HandAction()
        {
            //�@���p�̃c�[��
            if (_bDigmode)
            {
                if (InputData._bAction)
                {
                    RaycastHit hit;
                    RayUpdate();
                    hit = GetFowordRaycastData();

                    if (!_handToolData[_selectToolIndex]._toolStatus.bUseBlock)
                    {
                        if (gameObject.transform.position.y >= 0) return;
                        bool result = _handToolObject[_selectToolIndex].Dig(null, _statusManage.digmodeStatus.digStatus, _handToolData[_selectToolIndex]);
                    }
                    else
                    {
                        if (UseBlockDig(hit))
                        {
                            _swingTool.Swing(_toolPoket.gameObject);
                        }
                    }

                    if (hit.collider == null) return;
                    //�A�N�V�����o����I�u�W�F�N�g�ɃA�N�Z�X����
                    if (hit.collider.CompareTag(ActionObject.ACTION_OBJECT_TAG_NAME))
                    {
                        hit.collider.GetComponent<ActionObject>().OnAction(_uiManage);
                    }
                }
            }
            else
            {
                _handWeaponObject[_selectWeaponIndex].ShotBullet();
            }
        }

        /// <summary>
        /// ���[���h�ɕ\���p�̃c�[���𐶐�
        /// </summary>
        /// <param name="ToolData"></param>
        void WorldInstanceTool(ToolInfo[] ToolData)
        {
            foreach (Transform item in _toolPoket.transform)
            {
                Destroy(item.gameObject);
            }

            for (int i = 0; i < ON_HAND_TOOL_COUNT; i++)
            {
                if (ToolData[i] == null) return;
                GameObject generateObject = _toolGenerater.InstanceEmptyDataTool(ToolData[i]._toolStatus.toolKind, _toolPoket.transform.position, _toolPoket.rotation, _toolPoket);
                ToolBase toolBase = generateObject.GetComponent<ToolBase>();
                toolBase.SetStatus(ToolData[i]);
                toolBase.Initialize(_blockManage, gameObject, _statusManage);
                _handToolObject[i] = toolBase;
                generateObject.SetActive(false);
            }
        }
        /// <summary>
        /// ���[���h�ɕ\���p�̕���𐶐�
        /// </summary>
        /// <param name="weaponInfos"></param>
        void WorldInstanceWeapon(WeaponInfo[] weaponInfos)
        {
            foreach (Transform item in _weaponPoket.transform)
            {
                Destroy(item.gameObject);
            }

            for (int i = 0; i < ON_WEAPON_COUNT; i++)
            {
                if (weaponInfos[i] == null) return;
                GameObject generateObject = _toolGenerater.InstanceWeapon(weaponInfos[i], _weaponPoket.position, _weaponPoket.rotation, _weaponPoket);
                WeaponBase weaponBase = generateObject.GetComponent<WeaponBase>();
                _handWeaponObject[i] = weaponBase;
                generateObject.SetActive(false);
            }
        }
    }
}
