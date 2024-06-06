using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Syuntoku.DigMode.Input;

namespace Syuntoku.DigMode.Settings
{
    //==============================================
    //ゲームを管理するクラス
    //==============================================
    public class GameSetting : MonoBehaviour
    {
        [SerializeField]
        BlockScriptable m_blockScriptable;

        public int nowWorldSeed;

        /// <summary>
        /// UIが出たときの操作管理フラグ
        /// </summary>
        public bool bStopGameAction;

        /// <summary>
        /// 採掘モード
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
        /// UIモードの切り替え
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
        /// カーソルを有効にする
        /// </summary>
        static public void CursorEnable()
        {
            Cursor.visible = true;

            // カーソルを自由に動かせる
            Cursor.lockState = CursorLockMode.None;

        }

        /// <summary>
        /// カーソルを消す
        /// </summary>
        static public void CursorDisable()
        {
            Cursor.visible = false;

            // カーソルを画面中央にロックする
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}