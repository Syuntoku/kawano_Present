using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Syuntoku.DigMode.Input;

namespace Syuntoku.DigMode.Settings
{
    //==============================================
    //�Q�[�����Ǘ�����N���X
    //==============================================
    public class GameSetting : MonoBehaviour
    {
        [SerializeField]
        BlockScriptable m_blockScriptable;

        public int nowWorldSeed;

        /// <summary>
        /// UI���o���Ƃ��̑���Ǘ��t���O
        /// </summary>
        public bool bStopGameAction;

        /// <summary>
        /// �̌@���[�h
        /// </summary>
        public bool bDigMode;

        private void Awake()
        {
            SeedSetting();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            bStopGameAction = false;
        }

        void SeedSetting()
        {
            if (nowWorldSeed == 0)
            {
                nowWorldSeed = DateTime.Now.Millisecond;
                UnityEngine.Random.InitState(nowWorldSeed);
                Debug.Log("SetSeed" + nowWorldSeed);
            }
            else
            {
                UnityEngine.Random.InitState(nowWorldSeed);
            }
        }

        /// <summary>
        /// UI���[�h�̐؂�ւ�
        /// </summary>
        /// <param name="set"></param>
        public void StopGameAndEnableCursor(bool set)
        {
            bStopGameAction = set;

            if (bStopGameAction)
            {
                CursorEnable();
            }
            else
            {
                CursorDisable();
            }
        }

        /// <summary>
        /// �J�[�\����L���ɂ���
        /// </summary>
        static public void CursorEnable()
        {
            Cursor.visible = true;

            // �J�[�\�������R�ɓ�������
            Cursor.lockState = CursorLockMode.None;

        }

        /// <summary>
        /// �J�[�\��������
        /// </summary>
        static public void CursorDisable()
        {
            Cursor.visible = false;

            // �J�[�\������ʒ����Ƀ��b�N����
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}