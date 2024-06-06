using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.ParkData;
using UnityEngine.UI;
using TMPro;

namespace Syuntoku.DigMode
{

    public class MemoryManage : MonoBehaviour
    {
        [SerializeField]
        GameObject _memoryNormal;
        [SerializeField]
        GameObject _memoryGold;
        [SerializeField]
        GameObject _memoryRainBow;

        [SerializeField]
        TMP_Text _nameText;

        [SerializeField]
        TMP_Text _explainText;

        [SerializeField]
        public GameObject _memoryCollider;

        [SerializeField]
        Image _parkImage;

        public void Inialize(Park data,Image image = null)
        {
            _memoryNormal.SetActive(false);
            _memoryGold.SetActive(false);
            _memoryRainBow.SetActive(false);
            _nameText.SetText(data.name);

            _explainText.SetText(data.levelParkData[data.nowLevel+1].explanation);

            if (image == null) return;

            _parkImage = image;            
        }

        public void NotData()
        {
            _memoryNormal.SetActive(true);
            _nameText.SetText("NoData");

            _explainText.SetText("éÊìæÇ≈Ç´ÇÈÉpÅ[ÉNÇ™Ç†ÇËÇ‹ÇπÇÒ");
        }
    }
}
