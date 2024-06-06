using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.DigMode.ParkData;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode.Shop
{

    /// <summary>
    /// キャラクターごとの商品情報
    /// </summary>
    public class ShopMerChanfdiseSystem : MonoBehaviour
    {
        #region CashVariable

        [SerializeField] ShopCharactorScritable _shopScritable;
        [SerializeField] ParkConditionsManage _parkConditions;
        [SerializeField] UniqueCharacteristicsScriptable _uniqueCharacteristics;
        [SerializeField] Inventory.InventoryManage _inventoryManage;
        #endregion 
        public const int CARD_MAX = 4;
        public const string ATTACH_OBJECT_NAME = "ShopManage";
        public const string OTHERS_NAME = "その他";

        enum WaveIndex
        {
            FIRST = 1,
            SECOND,
            THIRD,
        }

        //======================================
        //public
        //======================================
        public void Initialize(Inventory.InventoryManage inventoryManage)
        {
            _inventoryManage = inventoryManage;
        }

        /// <summary>
        /// 商品情報を初期化・更新する
        /// </summary>
        public ShopData SetMerchandiseData()
        {
            ShopData newShopData = new ShopData();
            WeponInitialize(newShopData);
            ToolInitialize(newShopData);
            BaseInitialize(newShopData);

            return newShopData;
        }

        //======================================
        //private
        //======================================
        void WeponInitialize(ShopData newShopData)
        {
        }

        void ToolInitialize(ShopData newShopData)
        {
            for (int i = 0; i < CARD_MAX; i++)
            {
                int random = Random.Range(0, (int)ToolGenerater.ToolName.MAX);
                newShopData.shopData_Tools[i].index = random;
                newShopData.shopData_Tools[i].cost = _shopScritable.ToolCost;
                newShopData.shopData_Tools[i].uniqueCharacteristics = _inventoryManage.InstanceUniqueData((ToolGenerater.ToolName)random);
            }
        }

        void BaseInitialize(ShopData newShopData)
        {
            int random;
            int waveIndex = 0;

            //設定したウェーブによって出現するスキルを変える
           if(_parkConditions.waveCount >= _shopScritable.waveCount[0]&& _parkConditions.waveCount <= _shopScritable.waveCount[1] )
            {
                waveIndex = 1;
            }
            else if(_parkConditions.waveCount >= _shopScritable.Second_waveCount[0] && _parkConditions.waveCount <= _shopScritable.Second_waveCount[1]) 
            {
                waveIndex = 2; 
            }
           else if(_parkConditions.waveCount >= _shopScritable.Third_waveCount[0] && _parkConditions.waveCount <= _shopScritable.Third_waveCount[1])
            {
                waveIndex = 3; 
            }

            for (int i = 0; i < CARD_MAX; i++)
            {
                if (waveIndex == 2 || waveIndex == 3)
                {
                    random = Random.Range((int)ShopCharactorScritable.OTHER_ID.BUY_PARKRANDOM, (int)ShopCharactorScritable.OTHER_ID.MAX_OTHER);
                }
                else
                {
                    random = Random.Range((int)ShopCharactorScritable.OTHER_ID.BUY_PARKRANDOM, (int)ShopCharactorScritable.OTHER_ID.BUY_RETURN_TO_BASE);

                }
                //取得できるパークがない場合は別の商品を受け取る
                if (random == (int)ShopCharactorScritable.OTHER_ID.BUY_PARKRANDOM && _inventoryManage.ParkActiveCount((int)POPKIND.SHARE) == 0) continue;

                newShopData.shopData_Base[i].index = random;

                switch (random)
                {
                    case (int)ShopCharactorScritable.OTHER_ID.BUY_PARKRANDOM:

                        if (waveIndex == (int)WaveIndex.FIRST)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.ParkRandomCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_ParkCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_ParkRamdom;
                        }
                        else if (waveIndex == (int)WaveIndex.SECOND)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Second_ParkRandomCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Second_ParkCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_ParkRamdom;
                        }
                        else if (waveIndex == (int)WaveIndex.THIRD)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Third_ParkRandomCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Third_ParkCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_ParkRamdom;
                        }

                        break;
                    case (int)ShopCharactorScritable.OTHER_ID.BUY_HEALBASE:

                        if (waveIndex == (int)WaveIndex.FIRST)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.HealHpCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_HealCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_HealBase;
                        }
                        else if (waveIndex == (int)WaveIndex.SECOND)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Second_HealHpCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Second_Second_HealCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_HealBase;
                        }
                        else if (waveIndex == (int)WaveIndex.THIRD)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Third_HealHpCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Third_HealHpCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_HealBase;
                        }

                        break;
                    case (int)ShopCharactorScritable.OTHER_ID.BUY_RETURN_TO_BASE:

                        if (waveIndex == (int)WaveIndex.SECOND)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Second_ReturnToBaseCost;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Second_ReturnToBaseCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_ReturnToBaseCost;
                        }
                        else if (waveIndex == (int)WaveIndex.THIRD)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Third_ReturnToBase;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Third_ReturnToBaseCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_ReturnToBaseCost;
                        }
                        break;
                    case (int)ShopCharactorScritable.OTHER_ID.BUY_GAMBLING:

                        if (waveIndex == (int)WaveIndex.SECOND)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Second_Gambling;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Second_GamblingCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_GumblingCost;

                        }
                        else if (waveIndex == (int)WaveIndex.THIRD)
                        {
                            newShopData.shopData_Base[i].cost = _shopScritable.Third_Gambling;
                            newShopData.shopData_Base[i].explain = _shopScritable.explanation_Third_GumblingCost;
                            newShopData.shopData_Base[i].performanceText = _shopScritable.paformance_GumblingCost;
                        }

                        break;
                }
            }
        }

    }

    public class ShopData
    {

        public ShopData()
        {
            for (int i = 0; i < ShopMerChanfdiseSystem.CARD_MAX; i++)
            {
                shopData_Tools[i] = new ShopData_Tool();
                shopData_Battle[i] = new ShopData_Battle();
                shopData_Base[i] = new ShopData_Base();
            }
        }

        public ShopData_Tool[] shopData_Tools = new ShopData_Tool[4];
        public ShopData_Battle[] shopData_Battle = new ShopData_Battle[4];
        public ShopData_Base[] shopData_Base = new ShopData_Base[4];
        public JuwelryInventory rerollCost;
    }

    public class ShopData_Tool
    {
        public ShopData_Tool()
        {
            cost = new JuwelryInventory();
            index = 0;
            uniqueCharacteristics = null;
        }

        public JuwelryInventory cost;
        public int index;
        public UniqueCharacteristics uniqueCharacteristics;
    }

    public class ShopData_Battle
    {
        public ShopData_Battle()
        {
            cost = new JuwelryInventory();
            index = 0;
            uniqueCharacteristics = null;
        }

        public JuwelryInventory cost;
        public int index;
        public UniqueCharacteristics uniqueCharacteristics;
    }

    public class ShopData_Base
    {
        public ShopData_Base()
        {
            cost = new JuwelryInventory();
            index = 0;
            name = ShopMerChanfdiseSystem.OTHERS_NAME;
        }

        public string name;
        public JuwelryInventory cost;
        public int index;
        public string explain;
        public string performanceText;
    }
}
