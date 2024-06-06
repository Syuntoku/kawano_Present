using UnityEngine;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode.Input;
using Syuntoku.DigMode.Shop;
using Syuntoku.DigMode.Tool;
using DG.Tweening;

namespace Syuntoku.DigMode.UI
{
    public class UIManage : MonoBehaviour
    {
        #region UI_Prefab
        [SerializeField] GameObject ShopUI;
        [SerializeField] GameObject ParkUI;
        [SerializeField] GameObject MenuUI;
        [SerializeField] GameObject OptionUI;
        [SerializeField] GameObject UpGladeUI;
        [SerializeField] GameObject OperateBoradUI;
        [SerializeField] GameObject Upglade2UI;
        [SerializeField] GameObject PopupUI;
        [SerializeField] GameObject debugUI;
        #endregion

        #region CashVariable
        [SerializeField] GameSetting _gameSetting;
        [SerializeField] DigCount _digCount;
        [SerializeField] Inventory.InventoryManage _inventoryManage;
        [SerializeField] GameObject MainUi;
        [SerializeField] Camera _uiCamera;
        [SerializeField] GameObject _player;
        [SerializeField] Player.Player _playerData;
        [SerializeField] Camera _mainCamera;
        #endregion
        public bool _bInMenu;
        public bool _binOtherMenu;
        bool _bHold;
        //=============================
        //Unity
        //=============================
        private void Start()
        {
            debugUI.SetActive(false);
            //MainUi.SetActive(false);
        }

        public void Update()
        {
            if (InputData._bMenu)
            {
                if (_bHold) return;
                DrawMenuUI();
            }
            else
            {
                _bHold = false;
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.U))
            {
                if (_bHold) return;
                DrawUpGradeUI(true);
            }
            else
            {
                _bHold = false;
            }
          //  DrawDebugUI();
        }

        //=============================
        //public
        //=============================
        /// <summary>
        /// UI��\����
        /// </summary>
        public void OnUiMode()
        {
            _binOtherMenu = true;
            _gameSetting.StopGameAndEnableCursor(true);
            MainUi.SetActive(false);
        }

        /// <summary>
        /// UI��������
        /// </summary>
        public void OutUiMode(GameObject destroyObject)
        {
            Destroy(destroyObject);
            _binOtherMenu = false;
            _gameSetting.StopGameAndEnableCursor(false);
            MainUi.SetActive(true);
            Time.timeScale = 1.0f;
            _bHold = true;

            if(!_uiCamera.orthographic)
            {
                _uiCamera.orthographic = true;
            }
        }

        public void OnHold()
        {
            _bHold = true;
        }

        /// <summary>
        /// �V���b�v��UI��`��
        /// </summary>
        public void DrawShopUI(ShopData shopData, ShopCharactor shopCharactor)
        {
            if (_binOtherMenu) return;

            GameObject generateObject = CanvasSeting(ShopUI);
            ShopManage shopManage = generateObject.GetComponent<ShopManage>();
            shopManage.Initialize(this,shopData,shopCharactor);
            OnUiMode();
        }

        /// <summary>
        /// �I�v�V������ʂ�`��
        /// </summary>
        public void DrawOptionUI()
        {
            if (_binOtherMenu) return;
            
            CanvasSeting(OptionUI);
            OnUiMode();
        }

        /// <summary>
        /// ���j���[��`��
        /// </summary>
        public void DrawMenuUI()
        {
            if (_binOtherMenu) return;

            GameObject generateObject = CanvasSeting(MenuUI);
            PauseUI menuManage = generateObject.GetComponent<PauseUI>();
            menuManage.Initialize(this);
            OnUiMode();
        }

        /// <summary>
        /// �A�b�v�O���[�h��`��
        /// </summary>
        public void DrawUpGradeUI(bool bChangeStatus = false)
        {
            if (_binOtherMenu) return;

            GameObject generateObject = CanvasSeting(UpGladeUI);
            generateObject.transform.localScale = Vector3.right;
            generateObject.transform.DOScale(Vector3.one, 0.2f);
            UpgladeUI upGladeUI = generateObject.GetComponent<UpgladeUI>();
            upGladeUI.Initialize(this);
            if(bChangeStatus)
            {
                upGladeUI.NutoralStatus();
            }
            OnUiMode();
        }
        /// <summary>
        /// �A�b�v�O���[�h��`��
        /// </summary>
        public void DrawUpGrade2UI(ToolUpgrade toolUpgrade)
        {
            GameObject generateObject = CanvasSeting(Upglade2UI);
            OnUiMode();
            generateObject.transform.localScale = Vector3.right;
            generateObject.transform.DOScale(Vector3.one, 0.2f);
            Upglade2 upGladeUI = generateObject.GetComponent<Upglade2>();
            upGladeUI.Initialize(this,toolUpgrade);
        }

        /// <summary>
        /// �A�b�v�O���[�h��`��
        /// </summary>
        public void DrawOperateBoardUI()
        {
            if (_binOtherMenu) return;

            GameObject generateObject = CanvasSeting(OperateBoradUI);
            OperateBoardUIManage operateBoradUI = generateObject.GetComponent<OperateBoardUIManage>();
            operateBoradUI.Initialize(this);
            OnUiMode();
        }

        /// <summary>
        /// �|�b�v�A�b�v��\��
        /// </summary>
        public DialogUi DrawPopup()
        {
            if (_binOtherMenu) return null;
            GameObject generateObject = CanvasSeting(PopupUI);
            generateObject.transform.localScale = Vector3.right;
            generateObject.transform.DOScale(Vector3.one, 0.2f);
            OnUiMode();
            DialogUi dialogUi = generateObject.GetComponent<DialogUi>();
            dialogUi.Initialize(this);
            return dialogUi;
        }

        /// <summary>
        /// �肩��z���N�������o��
        /// </summary>
        /// <param name="popMode"></param>
        /// <param name="holdParkCount"></param>
        public void DrawParkCanvas(int popMode, int holdParkCount)
        {
            if (_gameSetting.bStopGameAction) return;

            OnUiMode();

            _mainCamera.transform.DOLocalRotate(Vector3.zero, 0.7f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                GameObject generate = Instantiate(ParkUI);
                Canvas canvas= generate.GetComponent<Canvas>();
                //�L�����o�X���J�����ɍ��킹��ݒ�ɂ���
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = _uiCamera;
                _uiCamera.orthographic = false;
                _uiCamera.fieldOfView = 60.0f;
                UI_ParkManage uI_ParkManage = generate.transform.GetChild(0).GetComponent<UI_ParkManage>();
                uI_ParkManage.Initialize(_gameSetting, _digCount);
            });
        }

        /// <summary>
        ///�@��������L�����o�X�̐ݒ���s��
        /// </summary>
        /// <param name="drawUi"></param>
        /// <returns>��������Canvas�I�u�W�F�N�g</returns>
        GameObject CanvasSeting(GameObject drawUi)
        {
            if (_gameSetting.bStopGameAction) return null;

            GameObject GenerateObject = Instantiate(drawUi);
            Canvas canvas = GenerateObject.GetComponent<Canvas>();
            //�L�����o�X���J�����ɍ��킹��ݒ�ɂ���
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _uiCamera;
            return GenerateObject;
        }

        public Inventory.InventoryManage GetInventoryData()
        {
            return _inventoryManage;
        }
    }
}