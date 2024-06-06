using UnityEngine;
using TMPro;
using DG.Tweening;
using Syuntoku.DigMode.Tool;
using UnityEngine.UI;

namespace Syuntoku.DigMode.UI
{
    /// <summary>
    /// UPGrade2UI
    /// </summary>
    public class Upglade2 : MonoBehaviour
    {
        public bool _bDebug;
        const int UPGLADE_MAX = 4;

        [SerializeField] Upglade2Button[] _levelupCard_Up;
        [SerializeField] Upglade2Button[] _levelupCard_Middle;
        [SerializeField] Upglade2Button[] _levelupCard_Down;

        [SerializeField] TMP_Text[] _levelupText_Up;
        [SerializeField] TMP_Text[] _levelupText_Middle;
        [SerializeField] TMP_Text[] _levelupText_Down;
        [SerializeField] TMP_Text _toolStatus_Info;
        [SerializeField] GameObject uparrowParent;

        [SerializeField] GameObject _outlineObject;
        [SerializeField] GameObject _detailObject;
        [SerializeField] Transform _detailTransform;
        [SerializeField] GameObject _detailLine;
        [SerializeField] public DetailManager _detailManager;
        [SerializeField] JuwelryCounter _juwelryCounter;

        [SerializeField] Image _selectToolIcon;
        Player.Player _player;
        UIManage _uIManage;
        ToolUpgrade _toolUpgladeStatus;

        public bool _bDeailDraw;

        const int DETAIL_UP_SLIDE = 150;
        const int DETAIL_DOWN_SLIDE = 80;
        const int DETAIL_RIGHT_SLIDE = 270;
        const int DETAIL_LEFT_SLIDE = 270;
        readonly Vector3 OUTLINE_AJUST = new Vector3(0.0f, 0.1f, 0.0f);
        const float ACTIVE_DETAIL_DELAY = 0.2f;

        const string UPGLADE_TEXT = "����";
        const string SPLIT_WORD = ",";

        public enum DetailDrawDirection
        {
            UP,
            DOWN,
            RIGHT,
            LEFT,
        }
        public enum RightCount
        {
            FIRST,
            SECOND,
            THERD,
            FOUR,
        }

        public enum Step
        {
            UP,
            MIDDLE,
            DOWN,
        }

        private void Update()
        {
            if(Input.InputData._bMenu)
            {
                Back();
            }
        }

        /// <summary>
        /// �A�b�v�O���[�h�̃e�L�X�g�⋭����Ԃ�ݒ肷��
        /// </summary>
        /// <param name="toolKind">�����̎�ށ@ToolKind</param>
        public void Initialize(UIManage uiManage, ToolUpgrade toolUpgrade)
        {
            _player = GameObject.Find(Player.Player.PLAYER_TAG).GetComponent<Player.Player>();

            _toolUpgladeStatus = toolUpgrade;

            //�e�J�[�h�ɋ������e��\������
            string[] upgladeInfo = toolUpgrade.GetToolUpgladeInfo();

            string[] split = upgladeInfo[(int)RightCount.FIRST].Split(SPLIT_WORD);

            for (int i = 0; i < split.Length; i++)
            {
                _levelupText_Up[i].SetText(split[i] + UPGLADE_TEXT);
            }
            split = upgladeInfo[(int)RightCount.SECOND].Split(SPLIT_WORD);

            for (int i = 0; i < split.Length; i++)
            {
                _levelupText_Middle[i].SetText(split[i] + UPGLADE_TEXT);
            }
            split = upgladeInfo[(int)RightCount.THERD].Split(SPLIT_WORD);

            for (int i = 0; i < split.Length; i++)
            {
                _levelupText_Down[i].SetText(split[i] + UPGLADE_TEXT);
            }

            _toolStatus_Info.SetText(_toolUpgladeStatus.GetNowStatusText());

            for (int i = 0; i < _toolUpgladeStatus._statusDrawCount; i++)
            {
                uparrowParent.transform.GetChild(i).gameObject.SetActive(true);
            }

            //�J�[�h�̏�����
            for (int i = 0; i < _levelupCard_Up.Length; i++)
            {
                _levelupCard_Up[i].Initialize();
                _levelupCard_Up[i]._iconManage.SetIconActive(false);
                _levelupCard_Up[i]._iconManage.SetMainIcon(_toolUpgladeStatus._upgradeIcon._firstIcon);
            }

            for (int i = 0; i < _levelupCard_Middle.Length; i++)
            {
                _levelupCard_Middle[i].Initialize();
                _levelupCard_Middle[i]._iconManage.SetIconActive(false);
                _levelupCard_Middle[i]._iconManage.SetMainIcon(_toolUpgladeStatus._upgradeIcon._secondIcon);
            }

            for (int i = 0; i < _levelupCard_Down.Length; i++)
            {
                _levelupCard_Down[i].Initialize();
                _levelupCard_Down[i]._iconManage.SetIconActive(false);
                _levelupCard_Down[i]._iconManage.SetMainIcon(_toolUpgladeStatus._upgradeIcon._thirdIcon);
            }

            SetCardActiveSetting(Step.UP);
            SetCardActiveSetting(Step.MIDDLE);
            SetCardActiveSetting(Step.DOWN);

            _detailObject.SetActive(false);
            _selectToolIcon.sprite = _toolUpgladeStatus._upgradeIcon._mainIcon;
            _uIManage = uiManage;
            _juwelryCounter.JuwelryCountTextUpdate(_player.GetJuwelryInventory());
        }

        /// <summary>
        /// �c�[���̃��x���ɂ���ăJ�[�h���I���ł��邩��ς���
        /// </summary>
        /// <param name="step"></param>
        void SetCardActiveSetting(Step step)
        {
            switch (step)
            {
                case Step.UP:
                    for (int i = 0; i < _levelupCard_Up.Length; i++)
                    {
                        //�ŏ��ɂ��ׂđI���ł��Ȃ�����
                        if (i < _toolUpgladeStatus._firstUpglade_level + 1)
                        {
                            _levelupCard_Up[i]._iconManage.SetState(IconManage.State.ONNOSELECT);
                        };
                    }

                    //�c�[���̃��x���́{�P�͔�����悤�ɕ\�����o��
                    if (_toolUpgladeStatus._firstUpglade_level + 1 <= UPGLADE_MAX)
                    {
                        _levelupCard_Up[_toolUpgladeStatus._firstUpglade_level + 1]._iconManage.SetIconActive(true);
                    }

                    break;
                case Step.MIDDLE:
                    for (int i = 0; i < _levelupCard_Middle.Length; i++)
                    {
                        if (i <= _toolUpgladeStatus._secondUpglade_level + 1)
                        {
                            _levelupCard_Middle[i]._iconManage.SetState(IconManage.State.ONNOSELECT);
                        }
                    }

                    if (_toolUpgladeStatus._secondUpglade_level + 1 <= UPGLADE_MAX)
                    {
                        _levelupCard_Middle[_toolUpgladeStatus._secondUpglade_level + 1]._iconManage.SetIconActive(true);
                    }

                    break;
                case Step.DOWN:
                    for (int i = 0; i < _levelupCard_Up.Length; i++)
                    {
                        if (i <= _toolUpgladeStatus._thirdUpglade_level + 1)
                        {
                            _levelupCard_Down[i]._iconManage.SetState(IconManage.State.ONNOSELECT);
                        }
                    }

                    if (_toolUpgladeStatus._thirdUpglade_level + 1 <= UPGLADE_MAX)
                    {
                        _levelupCard_Down[_toolUpgladeStatus._thirdUpglade_level + 1]._iconManage.SetIconActive(true);
                    }
                    break;
            }
        }

        /// <summary>
        /// �A�E�g���C�����X�V����
        /// </summary>
        public void SetOutlineTransform(Vector3 position, bool visible = true)
        {
            _outlineObject.transform.position = position + OUTLINE_AJUST;
            _outlineObject.SetActive(visible);
        }

        /// <summary>
        /// �f�^�C��������
        /// </summary>
        public void EndDetailDraw()
        {
            _detailManager.EndDraw();
        }

        /// <summary>
        /// �f�^�C����ʂ�`��
        /// </summary>
        public void DrawDetail(Upglade2Button connnectObject, Vector3 pivotPos, int step, int selectLevel, DetailDrawDirection drawDirectionVirtical, DetailDrawDirection drawDirectionHorizontal)
        {
            _detailObject.transform.localScale = Vector3.right;
            _detailObject.transform.DOScale(Vector3.one, ACTIVE_DETAIL_DELAY);
            Vector3 drawPos = pivotPos;

            foreach (Transform item in _detailLine.transform)
            {
                item.gameObject.SetActive(false);
            }

            //�\������������C���f�b�N�X�ŕ\��
            int activeLine = 0;

            if (drawDirectionHorizontal == DetailDrawDirection.RIGHT)
            {
                activeLine += (int)DetailDrawDirection.RIGHT;
            }

            if (drawDirectionVirtical == DetailDrawDirection.UP)
            {
                activeLine += (int)DetailDrawDirection.UP;
            }

            //�������̏o������
            _detailLine.transform.GetChild(activeLine).gameObject.SetActive(true);
            if (drawDirectionVirtical == DetailDrawDirection.UP) drawPos.y += DETAIL_UP_SLIDE;
            if (drawDirectionVirtical == DetailDrawDirection.DOWN) drawPos.y -= DETAIL_DOWN_SLIDE;
            if (drawDirectionHorizontal == DetailDrawDirection.RIGHT) drawPos.x += DETAIL_RIGHT_SLIDE;
            if (drawDirectionHorizontal == DetailDrawDirection.LEFT) drawPos.x -= DETAIL_LEFT_SLIDE;

            //�f�^�C�����o�Ă���r���̓A�C�R����I����Ԃɂ���
            connnectObject.SetState(IconManage.State.ONPUSH);

            _detailObject.SetActive(true);

            //�f�^�C����\��
            drawPos.z = 0.0f;
            _detailTransform.localPosition = drawPos;
            _detailManager.Initialize(this, connnectObject, _toolUpgladeStatus.GetUpGladeAmount(step, selectLevel), (Step)step);
        }

        /// <summary>
        /// UI�����
        /// </summary>
        public void Back()
        {
            _uIManage.OutUiMode(gameObject);
            _uIManage.DrawUpGradeUI();
        }

        /// <summary>
        /// �c�[������������
        /// </summary>
        /// <param name="step">��������i��</param>
        public void UpgladeTool(Step step)
        {
            switch (step)
            {
                case Step.UP:
                    FirstUpGrade();
                    break;
                case Step.MIDDLE:
                    SecondUpGrade();
                    break;
                case Step.DOWN:
                    ThirdUpGrade();
                    break;
            }
#if UNITY_EDITOR
            //�f�o�b�O�t����������
            if (_bDebug)
            {
                for (int i = 0; i < 5; i++)
                {
                    FirstUpGrade();
                    SecondUpGrade();
                    ThirdUpGrade();
                }
                SetCardActiveSetting(Step.UP);
                SetCardActiveSetting(Step.MIDDLE);
                SetCardActiveSetting(Step.DOWN);
            }
#endif
            SetCardActiveSetting(step);
        }

        void FirstUpGrade()
        {
            _toolUpgladeStatus.FirstUpGlade();
            _toolStatus_Info.SetText(_toolUpgladeStatus.GetNowStatusText());
            
            _juwelryCounter.JuwelryCountTextUpdate(_player.GetJuwelryInventory());
        }

        void SecondUpGrade()
        {
            _toolUpgladeStatus.SecondUpGlade();
            _toolStatus_Info.SetText(_toolUpgladeStatus.GetNowStatusText());

            _juwelryCounter.JuwelryCountTextUpdate(_player.GetJuwelryInventory());
        }
        void ThirdUpGrade()
        {
            _toolUpgladeStatus.ThirdUpGlade();
            _toolStatus_Info.SetText(_toolUpgladeStatus.GetNowStatusText());

            _juwelryCounter.JuwelryCountTextUpdate(_player.GetJuwelryInventory());
        }

        void CostCheck(Step step)
        {
        }
    }
}
