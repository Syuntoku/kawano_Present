using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Settings;

namespace Syuntoku
{
    namespace DigMode
    {
        namespace UI
        {
            public class UI_OptionManage : MonoBehaviour
            {
                GameSetting gameSetting;

                private void Start()
                {
                    gameSetting = GameObject.Find("GameSetting").GetComponent<GameSetting>();
                }


                public void Save()
                {

                }


                public void NotSaveEnd()
                {
                    gameSetting.StopGameAndEnableCursor(false);
                    Destroy(gameObject);
                }


            }
        }
    }
}
