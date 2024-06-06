using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.Inventory.Juwelry;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Player;

namespace Syuntoku.DigMode.Shop
{
    /// <summary>
    /// �L�����N�^�[�{�̂ɕt����f�[�^
    /// ���i����UI�ɑ���e�L�X�g�f�[�^�������Ă�
    /// </summary>
    public class ShopManage : BaseUI
    {

        #region CashVariable
        [SerializeField] ShopCard[] _shopCards;
        [SerializeField] Image[] _shopIcon;
        [SerializeField] JuwelryCounter _juwelryCounter;
        [SerializeField] GameObject _weponParent;
        [SerializeField] GameObject _toolParent;
        [SerializeField] GameObject _baseParent;
        [SerializeField] RectTransform _buyText;
        [SerializeField] GameObject _selectedPointorObj;
        [SerializeField] GameObject NotSelectObj;
        [SerializeField] TMP_Text _rerollGem;
        [SerializeField] ShopCharactorScritable _charactorScritable;
        [SerializeField] DetailManagerInfo _detalManager;
        #endregion
        const int INDEX_MAX = 4;
        int enter;
        UIManage _uIManage;
        RectTransform _selectedPointor;
        RectTransform NotSelect;
        ShopMerChanfdiseSystem _chanfdiseSystem;
        JuwelryInventory _rerollUsedGem;
        ShopCharactor _shopCharactor;
        ShopData _shopData;
        InventoryManage _inventoryManage;
        ParkConditionsManage _parkConditionsManage;
        FirstPerson _firstPerson;
        ToolGenerater _toolGenerater;
        PlayerInventory _playerInventory;

        const float FIRST_MOTION_SIZE = 10.0f;
        const float MOTION_DELAY = 0.1f;
        const float SECOND_MOTION_DELAY = 0.4f;

        const string NOT_SELECT_OBJECT_NAME = "NotSelect";

        enum Kind
        {
            Tool,
            Battle,
            Base,
        }

        enum Step
        { 
            UP,
            MIDDLE,
            DOWN,
        }

        //================================
        //public
        //================================
        public void Initialize(UIManage uIManage, ShopData shopData, ShopCharactor shopCharactor)
        {
            _shopData = shopData;
            _uIManage = uIManage;
            _inventoryManage = _uIManage.GetInventoryData();
            _firstPerson = GameObject.Find(Player.Player.ATTACH_OBJECT_NAME).GetComponent<Player.Player>()._firstPerson;
            _playerInventory = GameObject.Find(Player.Player.ATTACH_OBJECT_NAME).GetComponent<Player.Player>().GetPlayerInventory();
            _toolGenerater = GameObject.Find(ToolGenerater.ATTACH_OBJECT_NAME).GetComponent<ToolGenerater>();
            _chanfdiseSystem = GameObject.Find(ToolGenerater.ATTACH_OBJECT_NAME).GetComponent<ShopMerChanfdiseSystem>();
            _shopCharactor = shopCharactor;
            _parkConditionsManage = GameObject.Find(ParkConditionsManage.ATTACH_OBJECT_NAME).GetComponent<ParkConditionsManage>();
            _selectedPointor = _selectedPointorObj.GetComponent<RectTransform>();
            NotSelect = NotSelectObj.GetComponent<RectTransform>();
            Transform scaleChild = gameObject.transform.GetChild(0);
            //�ŏ��̃A�j���[�V����
            scaleChild.gameObject.transform.localScale = new Vector3(1.0f, 0, 0);
            scaleChild.gameObject.transform.DOScale(Vector3.one / FIRST_MOTION_SIZE, MOTION_DELAY);
            scaleChild.gameObject.transform.DOScale(Vector3.zero, MOTION_DELAY).SetDelay(MOTION_DELAY);
            scaleChild.gameObject.transform.DOScale(Vector3.one, MOTION_DELAY).SetDelay(SECOND_MOTION_DELAY);
            _uIManage.OnUiMode();
            //�C���x���g���̃e�L�X�g�ύX
            _juwelryCounter.JuwelryCountTextUpdate(_playerInventory.GetjuwelryInventory());
            //�V���b�v�J�[�h�̃e�L�X�g
            ChangeText(_toolParent,(int)Step.UP);
            ChangeText(_weponParent,(int)Step.MIDDLE);
            ChangeText(_baseParent,(int)Step.DOWN);
            PointerEntor(NotSelect,0);
            SpriteUpdate();
            _rerollUsedGem = _shopData.rerollCost;
            //�ۑ�����Ă���w���󋵂ŃJ�[�h�̃A�N�e�B�u��ς���
            int activeFlag = 0x01;

            for (int i = 0; i < (int)ShopCharactor.SELECTFLAG.MAX; i++)
            {
                int index = 0;

                switch (activeFlag)
                {
                    case (int)ShopCharactor.SELECTFLAG.FIRST:
                        index = 0;
                        break;
                    case (int)ShopCharactor.SELECTFLAG.SECOND:
                        index = 1;
                        break;
                    case (int)ShopCharactor.SELECTFLAG.THIRD:
                        index = 2;
                        break;
                    case (int)ShopCharactor.SELECTFLAG.FORTH:
                        index = 3;
                        break;
                }

                if ((_shopCharactor._buyFlagUp & activeFlag) != 0)
                {
                    _shopCards[index].Buy();

                }
                if ((_shopCharactor._buyFlagMiddle & activeFlag) != 0)
                {
                    _shopCards[index + INDEX_MAX * (int)Step.MIDDLE].Buy();

                }
                if ((_shopCharactor._buyFlagDown & activeFlag) != 0)
                {
                    _shopCards[index + INDEX_MAX * (int)Step.DOWN].Buy();

                }
                activeFlag = activeFlag << 1;
            }
        }

        /// <summary>
        /// �Ē��I������
        /// </summary>
        public void Reroll()
        {
            if (!_inventoryManage._juwelryInventoryData.UseCheck(_rerollUsedGem))
            {
                _juwelryCounter.SetPurchaseStatusColor(false);
#if UNITY_EDITOR
                Debug.Log("�ύX�ł��܂���");
#endif
                return;
            }
            else
            {
                _juwelryCounter.SetPurchaseStatusColor(false);
                _inventoryManage._juwelryInventoryData -= _rerollUsedGem;
                //�ύX��
                _shopData = _chanfdiseSystem.SetMerchandiseData();
                for (int i = 0; i < _shopCards.Length; i++)
                {
                    _shopCards[i].DataReset();
                }
                SpriteUpdate();
            }
           //�Ē��I�R�X�g�𑝂₷
            _rerollUsedGem *= _charactorScritable.RerollIncreaseAmount;
            _shopCharactor.FlagReset();
        }

        void SpriteUpdate()
        {
            for (int i = 0; i < INDEX_MAX; i++)
            {
                _shopIcon[i].sprite = _toolGenerater.InstanceToolData((ToolGenerater.ToolName)_shopData.shopData_Tools[i].index)._toolStatus.toolIcon;
                //�f�U�C���҂�
                //_shopCards[i + INDEX_MAX].transform.GetChild(1).GetComponent<Image>().sprite = _toolGenerater.InstanceWeaponData((ToolGenerater.WeaponName)_shopData.shopData_Battle[i].index)._weaponBaseStatus;
                // _shopCards[i + INDEX_MAX * 2].transform.GetChild(1).GetComponent<Image>().sprite = _toolGenerater.InstanceToolData((ToolGenerater.ToolName)_shopData.shopData_Base[i].index)._toolStatus.toolIcon;
            }
        }

        /// <summary>
        /// �󂯎������������ǂ̃J�[�h��I��������
        /// </summary>
        /// <param name="num"></param>
        public void SetSelectNum(int num)
        {
            int kindSelect = num % INDEX_MAX;
            int kind = num / INDEX_MAX;
#if UNITY_EDITOR
            Debug.Log("�I���F" + kindSelect);
#endif
            if (kind == (int)Kind.Tool)
            {
                ExplainSet(kind, kindSelect);
            }
            else if (kind == (int)Kind.Battle)
            {
                ExplainSet(kind, kindSelect);
            }
            else
            {
                ExplainSet(kind, kindSelect);
            }
        }
        /// <summary>
        /// �J�[�\�����J�[�h�̏�ɏ�����Ƃ��ɑI���󋵂�ς���
        /// </summary>
        /// <param name="position"></param>
        public void PointerEntor(RectTransform position, int id)
        {
            if (position.gameObject.name == NOT_SELECT_OBJECT_NAME)
            {
                _selectedPointor.gameObject.SetActive(false);
                _juwelryCounter.SetPurchaseStatusColor(true);
                return;
            }
            else
            {
                _selectedPointor.gameObject.SetActive(true);
                _buyText.gameObject.SetActive(false);
            }

            _buyText.position = position.position;
            _selectedPointor.position = position.position;
            _juwelryCounter.SetPurchaseStatusColor(CostCheck(id / INDEX_MAX, id % INDEX_MAX));
        }

        /// <summary>
        /// �J�[�\�����N���b�N�����Ƃ�
        /// </summary>
        /// <param name="num"></param>
        public void Selected(int num)
        {
            //�I�𒆂ɂ�����x�I������ƍw��
            if (enter == num)
            {
                int step = num / INDEX_MAX;
                int indexSelect = num % INDEX_MAX;
                int kindSelect;
                _buyText.gameObject.SetActive(false);

                if (step == (int)Kind.Tool)
                {
                    kindSelect = _shopData.shopData_Tools[indexSelect].index;
                }
                else if(step == (int)Kind.Battle)
                {
                    kindSelect = _shopData.shopData_Battle[indexSelect].index;
                }
                else
                {
                    kindSelect = _shopData.shopData_Base[indexSelect].index;

                }
                Buy(step, kindSelect,indexSelect);
            }
            else
            {
                enter = num;
                _buyText.gameObject.SetActive(true);
            }
        }

        //================================
        //private
        //================================
        /// <summary>
        /// �J�[�h�ɃR�X�g���e�L�X�g�ŕ\������@
        /// </summary>
        /// <param name="parent"></param>
        void ChangeText(GameObject parent,int kind)
        {
            int shopCount = 0;
            int countThrough = 0;
            foreach (Transform item in parent.transform)
            {
                countThrough++;
                if (countThrough <= 4)
                {
                    continue;
                }
                TMP_Text text = item.GetChild(0).GetComponent<TMP_Text>();
                JuwelryInventory inventory = null;

                if (kind == (int)Step.UP)
                {
                    inventory = _shopData.shopData_Tools[shopCount].cost;
                }
                else if(kind == (int)Step.MIDDLE)
                {
                    inventory = _shopData.shopData_Battle[shopCount].cost;

                }
                else if(kind == (int)Step.DOWN)
                {
                    inventory = _shopData.shopData_Base[shopCount].cost;

                }
                List<string> data = new List<string>();

                int count = 0;

      
                for (int j = 0; j < (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX; j++)
                {
                    switch (j)
                    {
                        case (int)JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                            if (inventory._skyLight > 0)
                            {
                                count++;
                                data.Add(inventory._skyLight.ToString());
                            }

                            break;
                        case (int)JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                            if (inventory._ampliroze > 0)
                            {
                                count++;
                                data.Add(inventory._ampliroze.ToString());
                            }

                            break;
                        case (int)JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                            if (inventory._linqhony > 0)
                            {
                                count++;
                                data.Add(inventory._linqhony.ToString());
                            }

                            break;
                        case (int)JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                            if (inventory._hexahope > 0)
                            {
                                count++;
                                data.Add(inventory._hexahope.ToString());
                            }

                            break;
                        case (int)JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                            if (inventory._florareaf > 0)
                            {
                                count++;
                                data.Add(inventory._florareaf.ToString());
                            }

                            break;
                        case (int)JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                            if (inventory._seeforcester > 0)
                            {
                                count++;
                                data.Add(inventory._seeforcester.ToString());
                            }

                            break;
                    }
                }

                string costText = "�@";

                for (int j = 0; j < data.Count; j++)
                {
                    costText += "�@";
                    costText += data[j];
                }

                if (_inventoryManage._juwelryInventoryData.UseCheck(inventory))
                {
                    text.SetText(costText);
                }
                else
                {
                    text.SetText(costText);
                    text.color = Color.red;
                }
                shopCount++;
            }
        }

        /// <summary>
        /// ��������ύX����
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="kindSelect"></param>
        void ExplainSet(int kind , int kindSelect)
        {
            _detalManager.ResetText();
            switch(kind)
            {
                case (int)Kind.Tool:
                    ToolInfo toolInfo = _toolGenerater.InstanceToolData((ToolGenerater.ToolName)_shopData.shopData_Tools[kindSelect].index);
                    toolInfo.SetUniqueData(_shopData.shopData_Tools[kindSelect].uniqueCharacteristics);
                    _detalManager.SetToolData(toolInfo);
                    break;
                case (int)Kind.Battle:
                    break;
                case (int)Kind.Base:
                    _detalManager.SetNameText(_shopData.shopData_Base[kindSelect].name);
                    _detalManager.SetLevelText("");
                    _detalManager.SetExplainText(_shopData.shopData_Base[kindSelect].explain);
                    _detalManager.SetInfoText(_shopData.shopData_Base[kindSelect].performanceText);
                    break;
            }
        }

        /// <summary>
        /// ���i�𔃂�
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="id"></param>
        void Buy(int kind, int id,int index)
        {
            ShopCharactor.SELECTFLAG select = ShopCharactor.SELECTFLAG.FIRST;

            switch (index)
            {
                case 0:
                    select = ShopCharactor.SELECTFLAG.FIRST;
                    break;
                case 1:
                    select = ShopCharactor.SELECTFLAG.SECOND;
                    break;
                case 2:
                    select = ShopCharactor.SELECTFLAG.THIRD;
                    break;
                case 3:
                    select = ShopCharactor.SELECTFLAG.FORTH;
                    break;
            }
            /*
            if (!CostCheck(kind, id))
            {
                Debug.Log("�����܂���ł���");
                return;
            }
            */
            _shopCards[enter].Buy();

            if (kind == (int)Kind.Tool)
            {
                ToolInfo toolInfo = _toolGenerater.InstanceToolData((ToolGenerater.ToolName)_shopData.shopData_Tools[id].index);
                toolInfo.SetUniqueData(_shopData.shopData_Tools[id].uniqueCharacteristics);

                _playerInventory.AddTool(toolInfo);
#if UNITY_EDITOR
                Debug.Log("�c�[�����w�����܂����F");
#endif
            }
            else if (kind == (int)Kind.Battle)
            {

            }
            else if (kind == (int)Kind.Base)
            {
                switch (id)
                {
                    //�����_���ȃp�[�N���擾
                    case 0:
                        ParkData.Park work = _inventoryManage.GetRandomPark();
                        //�p�[�N�����x���A�b�v����
                        if (work.nowLevel != -1)
                        {
                            break;
                        }
                        //���߂Ď擾
                        _inventoryManage.AddPark(work);
#if UNITY_EDITOR
                        Debug.Log("�w�����܂����F�p�[�N");
#endif
                        break;
                    //Hp���񕜂�����
                    case 1:
                        _parkConditionsManage.HealBaseHPPribility(0.25f);
#if UNITY_EDITOR
                        Debug.Log("�w�����܂����F��");
#endif
                        break;
                    //�e���|�[�g������
                    case 2:
                        DestroyCanvas();
                        _firstPerson.Resporn();
#if UNITY_EDITOR
                        Debug.Log("�w�����܂����F�e���|�[�g");
#endif
                        break;
                    //�M�����u��
                    case 3:
                        //�����_���ȃJ�[�h���_�u���N���b�N�������Ƃɂ���
                        int num = Random.Range(0, 11);
                        Selected(num);
                        Selected(num);
#if UNITY_EDITOR
                        Debug.Log("�w�����܂����F�����_��");
#endif
                        break;
                }
            }
            _shopCharactor.SetBuyFlag(kind, select);
        }

        /// <summary>
        /// id�ɓK�����ăR�X�g���`�F�b�N���g�p����ꍇ�͎g�p����
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CostCheck(int kind,int id)
        {
            //�J�����̓R�X�g���g��Ȃ�
            return true;
            /*
            //�R�X�g�`�F�b�N
            if (kind == (int)Kind.Tool)
            {
                if (_inventoryManage._juwelryInventoryData.UseCheck(_shopData.shopData_Tools[id].cost))
                {
                    _inventoryManage._juwelryInventoryData -= _shopData.shopData_Tools[id].cost;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (kind == (int)Kind.Battle)
            {
                if (_inventoryManage._juwelryInventoryData.UseCheck(_shopData.shopData_Battle[id].cost))
                {
                    _inventoryManage._juwelryInventoryData -= _shopData.shopData_Tools[id].cost;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (kind == (int)Kind.Base)
            {
                if (_inventoryManage._juwelryInventoryData.UseCheck(_shopData.shopData_Base[id].cost))
                {
                    _inventoryManage._juwelryInventoryData -= _shopData.shopData_Base[id].cost;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;*/
        }

        public void Notselecteed()
        {
            enter = int.MaxValue;
        }

        public void DestroyCanvas()
        {
            Transform scaleChild = gameObject.transform.GetChild(0);
           
            scaleChild.gameObject.transform.DOScale(new Vector3(0.9f, 0, 0), MOTION_DELAY).OnKill(() =>
            {
                scaleChild.gameObject.transform.DOScale(Vector3.right, 0.2f).SetDelay(MOTION_DELAY).OnKill(() =>
                {
                    scaleChild.gameObject.transform.DOScale(Vector3.zero, MOTION_DELAY).SetDelay(SECOND_MOTION_DELAY).OnKill(() => {

                        _uIManage.OutUiMode(gameObject);
                    });
                });
            });
        }
    }
}