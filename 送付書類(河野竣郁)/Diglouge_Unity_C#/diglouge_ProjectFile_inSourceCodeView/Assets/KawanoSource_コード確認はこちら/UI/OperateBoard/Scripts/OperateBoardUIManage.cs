using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Settings;

namespace Syuntoku.DigMode.UI
{
    public class OperateBoardUIManage : BaseUI
    {
        [SerializeField] JuwelryCounter _juwelryCounter;
        UIManage _uiManage;

        override public void Initialize(UIManage uiManage)
        {
            Player.Player player = GameObject.Find(Player.Player.PLAYER_TAG).GetComponent<Player.Player>();
            _uiManage = uiManage;
            _uiManage.OnUiMode();

            _juwelryCounter.JuwelryCountTextUpdate(player.GetJuwelryInventory());
        }

        public void LoadUIScene()
        {
            SceneManager.LoadScene("UIScene");
        }

        public void DrawUpGradeUI()
        {
            _uiManage.OutUiMode(gameObject);
            _uiManage.DrawUpGradeUI();
        }

        public void Back()
        {
            _uiManage.OutUiMode(gameObject);
        }
    }
}
