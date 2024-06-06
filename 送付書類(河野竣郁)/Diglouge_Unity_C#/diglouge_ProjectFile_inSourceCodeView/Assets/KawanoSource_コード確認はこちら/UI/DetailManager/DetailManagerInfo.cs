using UnityEngine;
using UnityEngine.UI;
using Syuntoku.DigMode.Tool;
using TMPro;

namespace Syuntoku.DigMode.UI
{

    public class DetailManagerInfo : MonoBehaviour
    {
        #region CashVariable
        [SerializeField] TMP_Text _toolName;
        [SerializeField] TMP_Text _levelText;
        [SerializeField] TMP_Text _infoText;
        [SerializeField] TMP_Text _amountText;
        [SerializeField] TMP_Text _explainText;
        [SerializeField] Image _image;
        [SerializeField] Sprite _defaultImage;
        #endregion

        const string LEVEL_TEXT = "Lv";

        //===========================================
        //public
        //===========================================
        public void SetToolData(ToolInfo toolInfo)
        {
            _toolName.SetText(toolInfo._toolStatus.toolName);
            _levelText.SetText(LEVEL_TEXT + toolInfo._toolState.ToString());
            _infoText.SetText(toolInfo._toolStatus.explanation);
            _amountText.SetText(toolInfo.GetToolInfoText());
            _explainText.SetText(toolInfo._uniqueCharacteristics.ExplainSet());
            _image.sprite = toolInfo._toolStatus.toolSquareIcon;
        }

        public void SetWeaponData(WeaponInfo weaponInfo)
        {
            _toolName.SetText(weaponInfo._weaponBaseStatus.weaponName);
            _levelText.SetText(LEVEL_TEXT + "1");
            _infoText.SetText(weaponInfo._weaponBaseStatus.exlain);
            _amountText.SetText(weaponInfo.GetToolInfoText());
            _image.sprite = weaponInfo._weaponBaseStatus.icon;
        }

        public void ResetText()
        {
            _toolName.SetText("");
            _levelText.SetText("");
            _infoText.SetText("");
            _amountText.SetText("");
            _explainText.SetText("");
            _image.sprite = _defaultImage;
        }

        public void SetNameText(string name)
        {
            _toolName.SetText(name);
        }

        public void SetLevelText(string level)
        {
            _levelText.SetText(LEVEL_TEXT + level.ToString());
        }

        public void SetInfoText(string infoText)
        {
            _infoText.SetText(infoText);
        }

        public void SetAmountText(string amountText)
        {
            _amountText.SetText(amountText);
        }

        public void SetExplainText(string uniqueText)
        {
            _explainText.SetText(uniqueText);
        }
    
        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}
