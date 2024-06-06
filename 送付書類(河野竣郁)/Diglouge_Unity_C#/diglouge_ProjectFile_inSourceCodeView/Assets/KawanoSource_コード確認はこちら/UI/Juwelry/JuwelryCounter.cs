using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Syuntoku.DigMode.Inventory.Juwelry;

namespace Syuntoku.DigMode.UI
{
    /// <summary>
    /// ��΂̏�������\������UI
    /// </summary>
    public class JuwelryCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text[] _juwelryText;
        [SerializeField] Material _fontMat;
        [SerializeField] JuwelryScriptable _juwelryScriptable;
        [SerializeField] Image[] _juwelryIcon; 

        private void Start()
        {     
            //�X�N���^�u���ɐݒ肳��Ă���C���X�g��ݒ肷��
            for (int i = 0; i < (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX; i++)
            {
                _juwelryIcon[i].sprite = _juwelryScriptable.GetIcon((JuwelryInventory.JUWELRY_KIND)i);
            }
        }

        /// <summary>
        /// ��΂̃J�E���g��ύX
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
        /// �W���G���[���ς���Ƃ��┃���Ȃ��Ƃ��̐F�ύX
        /// </summary>
        public void SetPurchaseStatusColor(bool isCanBuy)
        {
            //������Ƃ��̐F
            if(isCanBuy)
            {
                _fontMat.SetColor("Face Color", Color.white);
            }
            //�����Ȃ��Ƃ��̐F
            else
            {
                _fontMat.SetColor("Face Color", Color.red);
            }
        }
    }
}
