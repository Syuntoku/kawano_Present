using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode.UI
{
    /// <summary>
    /// 宝石の所持数を表示するUI
    /// </summary>
    public class JuwelryCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text[] _juwelryText;
        [SerializeField] Material _fontMat;
        [SerializeField] JuwelryScriptable _juwelryScriptable;
        [SerializeField] Image[] _juwelryIcon; 

        private void Start()
        {     
            //スクリタブルに設定されているイラストを設定する
            for (int i = 0; i < (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX; i++)
            {
                _juwelryIcon[i].sprite = _juwelryScriptable.GetIcon((JuwelryInventory.JUWELRY_KIND)i);
            }
        }

        /// <summary>
        /// 宝石のカウントを変更
        /// </summary>
        public void JuwelryCountTextUpdate(JuwelryInventory juwelryInventory)
        {
            int count = 0;
            foreach (TMP_Text item in _juwelryText)
            {
                item.SetText(juwelryInventory.GetjuwelryData(count).ToString());
                count++;
            }
        }

        /// <summary>
        /// ジュエリーが変えるときや買えないときの色変更
        /// </summary>
        public void SetPurchaseStatusColor(bool isCanBuy)
        {
            //買えるときの色
            if(isCanBuy)
            {
                _fontMat.SetColor("Face Color", Color.white);
            }
            //買えないときの色
            else
            {
                _fontMat.SetColor("Face Color", Color.red);
            }
        }
    }
}
