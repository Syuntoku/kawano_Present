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

        //ツール情報
        ToolInfo[] _handToolData;
        ToolBase[] _handToolObject;
        WeaponBase[] _handWeaponObject;

        int _selectToolIndex;
        int _selectWeaponIndex;

        Ray _playerRay;
        bool _bDigmode;
        bool _bHold;

        public static bool binBase;

        [Header("ステータス")]
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
            //パーク用のデータを変更
            ChangeConditionStatus();

            //視界を制限するオブジェクトを有効にする
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
        /// ダメージをもらう
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        /// <param name="damageSource">ダメージを与えた敵のTransform</param>
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
        /// HPを回復する
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
        /// 目線にあるブロックのデータを取得
        /// </summary>
        /// <returns></returns>
        public RaycastHit GetFowordRaycastData()
        {
            RaycastHit raycastHit;
            Physics.Raycast(_playerRay, out raycastHit, _lineOfsightLength, _rayLayerMask);
            return raycastHit;
        }

        /// <summary>
        /// スーツのライトパワーを変更する
        /// </summary>
        public void ChangeSuitLightPower(float magnifacation)
        {
            _suitLight.intensity = _suitLight.intensity * magnifacation;
        }

        /// <summary>
        /// プレイヤーインベントリに設定された装備を設定する
        /// </summary>
        public void UpdateToolEquipment()
        {
            _handToolData = _playerInventory.GetEquipmentTools();
            WorldInstanceTool(_playerInventory.GetEquipmentTools());
            WorldInstanceWeapon(_playerInventory.GetEquipmentWeapons());
        }

        /// <summary>
        /// 再度持ち直してステータスを計算しなおす
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
        /// 何も持っていないステータスに戻す
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
        /// 装備を切り替えた時
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
        /// プレイヤーのインベントリを取得する
        /// </summary>
        /// <returns></returns>
        public PlayerInventory GetPlayerInventory()
        {
            return _playerInventory;
        }
        /// <summary>
         /// プレイヤーのインベントリを取得する
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
        /// ツール状態と武器状態を切り替える
        /// </summary>
        void ModeChange()
        {
            if (InputData._bModeChange)
            {
                if (_bHold) return;

                NeutralHand();

                //武器が装備されていなかった場合は切り替えない
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
        /// ツールを切り替える
        /// </summary>
        /// <returns>変更　true 変更なし　false</returns>
        bool SelectToolChange()
        {
            if (InputData._middleHweel.y == 0) return false;
            int work;
            NeutralHand();

            //道具の手持ちを変更
            if (_bDigmode)
            {
                //装備している道具が一つの場合は変更しない
                if (_toolPoket.childCount == 1) return false;
                work = (int)Mathf.Sign(InputData._middleHweel.y);
                _selectToolIndex += work;
                if (_selectToolIndex < 0) _selectToolIndex = ON_HAND_TOOL_COUNT - 1;
                _selectToolIndex = _selectToolIndex % ON_HAND_TOOL_COUNT;
            }
            //武器の手持ちを変更
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
        /// ブロックを必要とする採掘
        /// </summary>
        /// <param name="hitBlock"></param>
        bool UseBlockDig(RaycastHit hitBlock)
        {
            if (!BlockManage.IsBlock(hitBlock)) return false;
            //掘る
            BlockData data = hitBlock.collider.gameObject.GetComponent<BlockData>();

            if (!data._blockdataInfo.StateCheck(BlockState.ENABLE)) return false;

            _inventoryManage.SwingUpdate();
            //触ったブロックの方向の計算
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
        /// 触ったブロックの計算
        /// </summary>
        /// <param name="hit"></param>
        void RunCalculation(RaycastHit hit)
        {
            if (hit.collider == null) return;

            Vector3 targetPosition = hit.collider.gameObject.transform.position;
            float result = gameObject.transform.position.y - targetPosition.y;

            //ブロックとプレイヤーの高さの差が１以上だと上下どちらか
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

            //高さの差をなくす
            Vector3 playerPos = gameObject.transform.position;
            playerPos.y = 0.0f;
            targetPosition.y = 0.0f;

            //左右を求める
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
        /// 手に持っている装備のアクション
        /// </summary>
        void HandAction()
        {
            //掘る専用のツール
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
                    //アクション出来るオブジェクトにアクセスする
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
        /// ワールドに表示用のツールを生成
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
        /// ワールドに表示用の武器を生成
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
