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
            dialogUi.SetTitleText("�`���[�g���A��");
            //dialogUi.AddPaseText("��ʂ̏�ɃQ�[�W������܂��BWAVE�Q�[�W�Ƃ���WAVE�ŃQ�[�����i��ł����܂��B\n" +
            //                    "WAVE�ɂ͍̌@WAVE�Ɛ퓬WAVE ������܂��B\n" +
            //                   "�̌@WAVE�͍̌@�ɏW���Ő؂�WAVE�ł��B�̌@�����퓬������A�b�v�O���[�h���Đ������Ă����܂��傤\n" +
            //                   "�퓬WAVE�͒n���̐����B���P���Ă��܂��B�͐s���Ȃ��悤�Ɏ莝���̏e�œ|���Ă����܂��傤");
            // dialogUi.AddPaseText("�܂��͌��ɓ����đf�ނ��W�߂܂��傤�@���m�̐��E���҂��Ă��܂�" +
            //                         "������x�f�ނ��W�߂��狒�_�ɂ��鑀��ՂŃc�[���̃A�b�v�O���[�h�����܂��傤\n" +
            //                     "��Ver�ł͑f�ނ��g�p�����ɃA�b�v�O���[�h��V���b�v�L�����N�^�[�ł̍w�����ł��܂��B");
            dialogUi.InstanceImage(Vector3.zero, _tutorialImage);
        }

        void DigTutorial(DialogUi dialogUi)
        {
            dialogUi.SetTitleText("�̌@�ɂ���");
        }

        void PartTutorial(DialogUi dialogUi)
        {
            dialogUi.SetTitleText("�p�[�N�ɂ���");
        }

        void Reprodaction()
        {
            Time.timeScale = 1.0f;
        }
    }
}
