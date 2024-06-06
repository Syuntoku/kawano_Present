using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Syuntoku.DigMode.UI
{

    public class StartOperationBoard : MonoBehaviour
    {
        [SerializeField] GameObject[] content;
        [SerializeField] Scrollbar[] scrollbar;

        [SerializeField] GameObject[] ArrowObject;
        [SerializeField] Sprite[] arrowOnOff;
        [SerializeField] Sprite[] cardImage;

        const int ON = 0;
        const int OFF = 1;
        const int LOCK = 2;

        const int TOOL_MAX_COUNT = 5;
        const int TOOL_UNLOCK_COUNT = 2;


        float[] _oneAmountData;

        void Start()
        {
            _oneAmountData = new float[2];

            _oneAmountData[0] = (float)(1.0f / content[0].transform.childCount);
            _oneAmountData[1] = (float)(1.0f / content[0].transform.childCount);

            //スクロールの一回の移動量を計算し、移動させる
            MoveOneAmount(0, 0);
            MoveOneAmount(1, 0);
        }

        public void MoveOneAmount(int barCount, int scale)
        {
            scrollbar[barCount].value += scale * _oneAmountData[barCount] * 3;
            if (barCount == 0)
            {

                if (scrollbar[barCount].value >= 1)
                {
                    scrollbar[barCount].value = 1.0f;
                    ArrowObject[barCount].GetComponent<Image>().sprite = arrowOnOff[barCount + OFF];
                    ArrowObject[barCount + 1].GetComponent<Image>().sprite = arrowOnOff[barCount + 2 + ON];
                }
                else if (scrollbar[barCount].value <= 0)
                {
                    scrollbar[barCount].value = 0.0f;
                    ArrowObject[barCount].GetComponent<Image>().sprite = arrowOnOff[ON];
                    ArrowObject[barCount + 1].GetComponent<Image>().sprite = arrowOnOff[barCount + 2 + OFF];
                }
                else
                {
                    ArrowObject[barCount].GetComponent<Image>().sprite = arrowOnOff[ON];
                    ArrowObject[barCount + 1].GetComponent<Image>().sprite = arrowOnOff[barCount + 2 + ON];
                }
            }

            if (barCount == 1)
            {

                if (scrollbar[barCount].value >= 1)
                {
                    scrollbar[barCount].value = 1.0f;
                    int work = barCount + 1;
                    ArrowObject[work].GetComponent<Image>().sprite = arrowOnOff[OFF];
                    ArrowObject[work + 1].GetComponent<Image>().sprite = arrowOnOff[work + ON];
                }
                else if (scrollbar[barCount].value <= 0)
                {
                    scrollbar[barCount].value = 0.0f;
                    int work = barCount + 1;
                    ArrowObject[work].GetComponent<Image>().sprite = arrowOnOff[ON];
                    ArrowObject[work + 1].GetComponent<Image>().sprite = arrowOnOff[work + OFF];
                }
                else
                {
                    int work = barCount + 1;
                    ArrowObject[work].GetComponent<Image>().sprite = arrowOnOff[ON];
                    ArrowObject[work + 1].GetComponent<Image>().sprite = arrowOnOff[work + ON];
                }
           
            }
        }
        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }
    }
}
