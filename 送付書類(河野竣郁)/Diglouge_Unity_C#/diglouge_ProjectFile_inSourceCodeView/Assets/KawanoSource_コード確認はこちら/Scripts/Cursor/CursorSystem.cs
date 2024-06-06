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

            [Header("�J�[�\���V�X�e��")]
            [Header("DEBUG : KEY_ESC  �J�[�\���̃I���E�I�t")]
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

                        // �J�[�\������ʒ����Ƀ��b�N����
                        Cursor.lockState = CursorLockMode.Locked;
                        m_bNowCursorVisible = false;
                    }
                    else
                    {
                        Cursor.visible = true;

                        // �J�[�\�������R�ɓ�������
                        Cursor.lockState = CursorLockMode.None;
                        m_bNowCursorVisible = true;
                    }
                }
            }
        }
    }
}
