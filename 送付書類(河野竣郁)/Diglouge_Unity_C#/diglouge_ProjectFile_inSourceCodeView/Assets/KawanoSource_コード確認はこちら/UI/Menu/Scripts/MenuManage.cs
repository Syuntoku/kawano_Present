using UnityEngine.SceneManagement;
using Syuntoku.DigMode.Sound;
using UnityEngine;

namespace Syuntoku.DigMode.UI
{

    public class MenuManage : BaseUI
    {
        UIManage _uIManage;
        SoundManager _soundManager;

        bool bHold;
        bool bInitialize;

        override public void Initialize(UIManage uIManage)
        {
            _uIManage = uIManage;
            _uIManage.OnUiMode();
            _uIManage.OnHold();
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        private void Update()
        {
            if(!Input.InputData._bMenu && bHold)
            {
                bHold = false;
            }

            if (bInitialize)
            {
                if (Input.InputData._bMenu && !bHold)
                {
                    OnBack();
                }
            }
        }

        public void OnBackToTitle()
        {
            SceneManager.LoadScene("0_Title");
        }

        public void OnBack()
        {
            Destroy(gameObject);
            _uIManage.OnHold();
            _uIManage.OutUiMode(gameObject);
        }

        public void OnEndGame()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }
}
