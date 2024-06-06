using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DEBUG;

namespace Syuntoku
{
    namespace DEBUG
    {
        public class CursorSystem : MonoBehaviour
        {

            [Header("カーソルシステム")]
            [Header("DEBUG : KEY_ESC  カーソルのオン・オフ")]
            public bool m_bNowCursorVisible;

            void Start()
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (Cursor.visible)
                    {
                        Cursor.visible = false;

                        // カーソルを画面中央にロックする
                        Cursor.lockState = CursorLockMode.Locked;
                        m_bNowCursorVisible = false;
                    }
                    else
                    {
                        Cursor.visible = true;

                        // カーソルを自由に動かせる
                        Cursor.lockState = CursorLockMode.None;
                        m_bNowCursorVisible = true;
                    }
                }
            }
        }
    }
}
