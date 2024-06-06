using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Syuntoku.DigMode.UI
{
    public class TutorialManage : MonoBehaviour
    {
        [SerializeField] UIManage _uIManage;
        [SerializeField] Sprite[] tutorialImage;
        [SerializeField] GameObject _playerObject;
        [SerializeField] Sprite _tutorialImage;

        bool _bDigTutorial;
        public enum TutorialState
        { 
            GAME_START,
            DIG,
            PARK,
        }
        //============================================
        //Unity
        //============================================
        private void Start()
        {
            SetTutorialInfo(TutorialState.GAME_START);
        }

        //============================================
        //public
        //=============================================
        public void SetTutorialInfo(TutorialState tutorialState)
        {
            DialogUi dialog = _uIManage.DrawPopup();
            switch (tutorialState)
            {
                case TutorialState.GAME_START:
                    GameStartTutorial(dialog);
                    break;
                case TutorialState.DIG:
                    DigTutorial(dialog);
                    break;
                case TutorialState.PARK:
                    PartTutorial(dialog);
                    break;
                default:
                    break;
            }
        }

        //=======================================
        //private
        //=======================================
        void GameStartTutorial(DialogUi dialogUi)
        {
            dialogUi.SetTitleText("チュートリアル");
            //dialogUi.AddPaseText("画面の上にゲージがあります。WAVEゲージといいWAVEでゲームが進んでいきます。\n" +
            //                    "WAVEには採掘WAVEと戦闘WAVE があります。\n" +
            //                   "採掘WAVEは採掘に集中で切るWAVEです。採掘道具や戦闘道具をアップグレードして成長していきましょう\n" +
            //                   "戦闘WAVEは地下の生物達が襲ってきます。力尽きないように手持ちの銃で倒していきましょう");
            // dialogUi.AddPaseText("まずは穴に入って素材を集めましょう　未知の世界が待っています" +
            //                         "ある程度素材を集めたら拠点にある操作盤でツールのアップグレードをしましょう\n" +
            //                     "現Verでは素材を使用せずにアップグレードやショップキャラクターでの購入ができます。");
            dialogUi.InstanceImage(Vector3.zero, _tutorialImage);
        }

        void DigTutorial(DialogUi dialogUi)
        {
            dialogUi.SetTitleText("採掘について");
        }

        void PartTutorial(DialogUi dialogUi)
        {
            dialogUi.SetTitleText("パークについて");
        }

        void Reprodaction()
        {
            Time.timeScale = 1.0f;
        }
    }
}
