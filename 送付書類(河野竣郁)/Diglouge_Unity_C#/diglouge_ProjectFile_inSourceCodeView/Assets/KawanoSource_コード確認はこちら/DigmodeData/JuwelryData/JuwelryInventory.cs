using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode.Inventory.Juwelry
{
    [System.Serializable]
    public class JuwelryInventory
    {
        [SerializeField] static JuwelryScriptable _juwelryScriptable;

        public enum JUWELRY_KIND
        {
            SKY_LIGHT,
            AMPLIROZE,
            LINQHONEY,
            HEXAHOPE,
            FLORAREAF,
            SEEFORCESTER,
            JUWELRY_MAX,
            RANDOM_DROP,
        }

        public uint _skyLight;
        public uint _ampliroze;
        public uint _linqhony;
        public uint _hexahope;
        public uint _florareaf;
        public uint _seeforcester;

        public JuwelryInventory()
        {
            _skyLight = 0;
            _ampliroze = 0;
            _linqhony = 0;
            _hexahope = 0;
            _florareaf = 0;
            _seeforcester = 0;
        }

        //======================================
        //public
        //======================================
        public void Initialize(JuwelryScriptable juwelryScriptable)
        {
            _juwelryScriptable = juwelryScriptable;
        }


        /// <summary>
        /// インベントリのインデックスから宝石のデータを取得する
        /// </summary>
        public uint GetjuwelryData(int index)
        {
            switch (index)
            {
                case (int)JUWELRY_KIND.SKY_LIGHT:
                    return _skyLight;
                case (int)JUWELRY_KIND.AMPLIROZE:
                    return _ampliroze;
                case (int)JUWELRY_KIND.LINQHONEY:
                    return _linqhony;
                case (int)JUWELRY_KIND.HEXAHOPE:
                    return _hexahope;
                case (int)JUWELRY_KIND.FLORAREAF:
                    return _florareaf;
                case (int)JUWELRY_KIND.SEEFORCESTER:
                    return _seeforcester;
            }

            return uint.MinValue;
        }

        public uint GetjuwelryData(JUWELRY_KIND index)
        {
            switch (index)
            {
                case JUWELRY_KIND.SKY_LIGHT:
                    return _skyLight;
                case JUWELRY_KIND.AMPLIROZE:
                    return _ampliroze;
                case JUWELRY_KIND.LINQHONEY:
                    return _linqhony;
                case JUWELRY_KIND.HEXAHOPE:
                    return _hexahope;
                case JUWELRY_KIND.FLORAREAF:
                    return _florareaf;
                case JUWELRY_KIND.SEEFORCESTER:
                    return _seeforcester;
            }

            return uint.MinValue;
        }

        public void RemoveJuwelry(JUWELRY_KIND index,uint removeCount)
        {
            switch (index)
            {
                case JUWELRY_KIND.SKY_LIGHT:
                    _skyLight -= removeCount;
                    break;
                case JUWELRY_KIND.AMPLIROZE:
                    _ampliroze -= removeCount;
                    break;
                case JUWELRY_KIND.LINQHONEY:
                    _linqhony -= removeCount;
                    break;
                case JUWELRY_KIND.HEXAHOPE:
                    _hexahope -= removeCount;
                    break;
                case JUWELRY_KIND.FLORAREAF:
                    _florareaf -= removeCount;
                    break;
                case JUWELRY_KIND.SEEFORCESTER:
                    _seeforcester -= removeCount;
                    break;
            }
        }

        public bool AddJuwelryData(int juwelryIndex, uint num)
        {
            if (juwelryIndex >= (int)JUWELRY_KIND.JUWELRY_MAX) return false;

            switch ((JUWELRY_KIND)juwelryIndex)
            {
                case JUWELRY_KIND.SKY_LIGHT:
                    _skyLight += num;
                    break;
                case JUWELRY_KIND.AMPLIROZE:
                    _ampliroze += num;
                    break;
                case JUWELRY_KIND.LINQHONEY:
                    _linqhony += num;
                    break;
                case JUWELRY_KIND.HEXAHOPE:
                    _hexahope += num;
                    break;
                case JUWELRY_KIND.FLORAREAF:
                    _florareaf += num;
                    break;
                case JUWELRY_KIND.SEEFORCESTER:
                    _seeforcester += num;
                    break;
            }

            return true;
        }

        public bool AddJuwelryData(JUWELRY_KIND juwelryIndex, uint num = 1)
        {
            switch (juwelryIndex)
            {
                case JUWELRY_KIND.SKY_LIGHT:
                    _skyLight += num;
                    break;
                case JUWELRY_KIND.AMPLIROZE:
                    _ampliroze += num;
                    break;
                case JUWELRY_KIND.LINQHONEY:
                    _linqhony += num;
                    break;
                case JUWELRY_KIND.HEXAHOPE:
                    _hexahope += num;
                    break;
                case JUWELRY_KIND.FLORAREAF:
                    _florareaf += num;
                    break;
                case JUWELRY_KIND.SEEFORCESTER:
                    _seeforcester += num;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 今のインベントリで買う・使える状態にあるか
        /// </summary>
        /// <param name="check"></param>
        /// <returns>足りる　true 足りない　false</returns>
        public bool UseCheck(JuwelryInventory check)
        {
            if (_skyLight < check._skyLight) return false;
            if (_ampliroze < check._ampliroze) return false;
            if (_linqhony < check._linqhony) return false;
            if (_hexahope < check._hexahope) return false;
            if (_florareaf < check._florareaf) return false;
            if (_seeforcester < check._seeforcester) return false;
            return true;
        }
        /// <summary>
        /// 今のインベントリで買う・使える状態にあるか
        /// </summary>
        /// <param name="check"></param>
        /// <returns>足りる　true 足りない　false</returns>
        public bool CanBuyCheck(JuwelryCost check)
        {
            foreach (Cost cost in check.costs)
            {
                switch (cost.useKind)
                {
                    case JUWELRY_KIND.SKY_LIGHT:
                        if (_skyLight < cost.useCount) return false;
                        break;
                    case JUWELRY_KIND.AMPLIROZE:
                        if (_ampliroze < cost.useCount) return false;
                        break;
                    case JUWELRY_KIND.LINQHONEY:
                        if (_linqhony < cost.useCount) return false;
                        break;
                    case JUWELRY_KIND.HEXAHOPE:
                        if (_hexahope < cost.useCount) return false;
                        break;
                    case JUWELRY_KIND.FLORAREAF:
                        if (_florareaf < cost.useCount) return false;
                        break;
                    case JUWELRY_KIND.SEEFORCESTER:
                        if (_seeforcester < cost.useCount) return false;
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// コストを使用して支払う
        /// </summary>
        /// <param name="cost"></param>
        public void Buy(JuwelryCost cost)
        {
            foreach (Cost usecost in cost.costs)
            {
                RemoveJuwelry(usecost.useKind, (uint)usecost.useCount);
            }
        }

        /// <summary>
        /// 各宝石の重さを計算
        /// </summary>
        /// <param name="juwelryKind"></param>
        /// <returns></returns>
        public int GetJuwelryWeight(JUWELRY_KIND juwelryKind)
        {
           return _juwelryScriptable.JuwelryWeight(juwelryKind);
        }

        /// <summary>
        /// アイコンのデータをすべて取得
        /// </summary>
        /// <returns></returns>
        public Sprite[] GetIconDataFull()
        {
            Sprite[] sprites = new Sprite[(int)JUWELRY_KIND.JUWELRY_MAX];

            for (int i = 0; i < (int)JUWELRY_KIND.JUWELRY_MAX; i++)
            {
                sprites[i] = _juwelryScriptable.GetIcon((JUWELRY_KIND)i);
            }
            return sprites;
        }
        /// <summary>
         /// アイコンのデータをすべて取得
         /// </summary>
         /// <returns></returns>
        public Sprite GetIconData(JUWELRY_KIND juwelryKind)
        {
            switch (juwelryKind)
            {
                case JUWELRY_KIND.SKY_LIGHT:
                    return _juwelryScriptable.skylightIcon;

                case JUWELRY_KIND.AMPLIROZE:
                    return _juwelryScriptable.amplirozeIcon;

                case JUWELRY_KIND.LINQHONEY:
                    return _juwelryScriptable.linqhoneyIcon;

                case JUWELRY_KIND.HEXAHOPE:
                    return _juwelryScriptable.hexahopeIcon;

                case JUWELRY_KIND.FLORAREAF:
                    return _juwelryScriptable.floraReafIcon;

                case JUWELRY_KIND.SEEFORCESTER:
                    return _juwelryScriptable.seeforcestarIcon;
            }
            return null;
        }

        /// <summary>
        /// ランダムな宝石の種類を取得する
        /// </summary>
        /// <returns></returns>
        static public JUWELRY_KIND GetRandomJuwelryKind()
        {
            int count = Random.Range((int)JUWELRY_KIND.SKY_LIGHT, (int)JUWELRY_KIND.JUWELRY_MAX);
            switch (count)
            {
                case (int)JUWELRY_KIND.SKY_LIGHT:
                    return JUWELRY_KIND.SKY_LIGHT;

                case (int)JUWELRY_KIND.AMPLIROZE:
                    return JUWELRY_KIND.AMPLIROZE;

                case (int)JUWELRY_KIND.LINQHONEY:
                    return JUWELRY_KIND.LINQHONEY;

                case (int)JUWELRY_KIND.HEXAHOPE:
                    return JUWELRY_KIND.HEXAHOPE;

                case (int)JUWELRY_KIND.FLORAREAF:
                    return JUWELRY_KIND.FLORAREAF;

                case (int)JUWELRY_KIND.SEEFORCESTER:
                    return JUWELRY_KIND.SEEFORCESTER;
            }
            return 0;
        }

        public JuwelryInventory(JuwelryInventory copy)
        {
            _skyLight = copy._skyLight;
            _ampliroze = copy._ampliroze;
            _linqhony = copy._linqhony;
            _hexahope = copy._hexahope;
            _florareaf = copy._florareaf;
            _seeforcester = copy._seeforcester;
        }

        //演算時オーバーライド +
        public static JuwelryInventory operator +(JuwelryInventory baseData, JuwelryInventory plusData)
        {
            baseData._skyLight += plusData._skyLight;
            baseData._ampliroze += plusData._ampliroze;
            baseData._linqhony += plusData._linqhony;
            baseData._hexahope += plusData._hexahope;
            baseData._florareaf += plusData._florareaf;
            baseData._seeforcester += plusData._seeforcester;

            return baseData;
        }

        //演算時オーバーライド -
        public static JuwelryInventory operator -(JuwelryInventory baseData, JuwelryInventory plusData)
        {
            baseData._skyLight -= plusData._skyLight;
            baseData._ampliroze -= plusData._ampliroze;
            baseData._linqhony -= plusData._linqhony;
            baseData._hexahope -= plusData._hexahope;
            baseData._florareaf -= plusData._florareaf;
            baseData._seeforcester -= plusData._seeforcester;

            return baseData;
        }
        public static JuwelryInventory operator -(JuwelryInventory baseData, int data)
        {
            baseData._skyLight -= (uint)data;
            baseData._ampliroze -= (uint)data;
            baseData._linqhony -= (uint)data;
            baseData._hexahope -= (uint)data;
            baseData._florareaf -= (uint)data;
            baseData._seeforcester -= (uint)data;

            return baseData;
        }

        //演算子オーバーライド　*
        public static JuwelryInventory operator *(JuwelryInventory baseData, int data)
        {
            baseData._skyLight *= (uint)data;
            baseData._ampliroze *= (uint)data;
            baseData._linqhony *= (uint)data;
            baseData._hexahope *= (uint)data;
            baseData._florareaf *= (uint)data;
            baseData._seeforcester *= (uint)data;

            return baseData;
        }
        public static JuwelryInventory operator *(JuwelryInventory baseData, float data)
        {
            baseData._skyLight = (uint)(baseData._skyLight * data);
            baseData._ampliroze = (uint)(baseData._ampliroze * data);
            baseData._linqhony = (uint)(baseData._linqhony * data);
            baseData._hexahope = (uint)(baseData._hexahope * data);
            baseData._florareaf = (uint)(baseData._florareaf * data);
            baseData._seeforcester = (uint)(baseData._seeforcester * data);

            return baseData;
        }
    }
}
