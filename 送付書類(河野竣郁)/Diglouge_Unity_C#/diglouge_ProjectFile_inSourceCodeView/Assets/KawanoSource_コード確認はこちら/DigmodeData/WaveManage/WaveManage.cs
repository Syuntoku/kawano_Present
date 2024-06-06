using UnityEngine;
using Syuntoku.DigMode.Inventory;
using UnityEngine.UI;
using TMPro;
using Syuntoku.DigMode.Settings;

namespace Syuntoku.DigMode.Wave
{
    public class WaveManage : MonoBehaviour
    {
        #region　CashVariable
        [SerializeField] InventoryManage _inventoryManage;
        [SerializeField] ParkConditionsManage _parkConditions;
        [SerializeField] WaveManageScriptable _waveManageScriptable;
        [SerializeField] BGMManage _bgmManage;
        [SerializeField] Image _gaugeImage;
        [SerializeField] Image _enemyIcon;
        [SerializeField] Sprite _digImage;
        [SerializeField] TMP_Text _wavecountText;
        [SerializeField] Sprite _enemyImage;
        [SerializeField] TMP_Text _text;
        #endregion

        public Color _digIconColor;
        public Color _enemyIconColor;
        public uint _waveCount;
        public float _waveTimer;
        public static bool _bDigmode;
        float _changedTimer;
        const float MAX_FILLAMOUNT = 1.0f;
        const int ALPHA_MAX = 60;
        const float ALPHA_AMOUNT = 50;
        public float _workAlpha;
        bool _bAttenuationAlpha;

        //==============================
        //Unity
        //==============================
        private void Awake()
        {
            _waveCount = 0;
            _parkConditions.waveCount = _waveCount;
            ChangeEnemyIconColor();
        }

        private void Update()
        {
            _waveTimer += Time.deltaTime;
            _text.SetText(((int)(_changedTimer - _waveTimer)).ToString());

            if (_waveTimer >= _changedTimer)
            {
                ModeChange();
                _wavecountText.SetText(_waveCount.ToString());
                _parkConditions.waveCount = _waveCount;
                _bgmManage.SetClip();
            }
            ChangeGaugeFill();
        }

        //==============================
        //public
        //==============================
        public bool IsBattleMode()
        {
            return !_bDigmode;
        }

        //==============================
        //private
        //==============================
        void ModeChange()
        {
            _waveTimer = 0.0f;
            _bDigmode = !_bDigmode;
            _workAlpha = 0.0f;
            ChangeEnemyIconColor();
            if (_bDigmode) { SetDigTimer(); } else { SetBattleTimer(); };
        }

        void SetDigTimer()
        {
            _changedTimer = _waveManageScriptable.digTime;
            _enemyIcon.sprite = _digImage;
            _enemyIcon.color = _digIconColor;

        }
        void SetBattleTimer()
        {
            _changedTimer = _waveManageScriptable.battleTime;
            _enemyIcon.sprite = _enemyImage;
            _enemyIcon.color = _enemyIconColor;
            _waveCount++;
        }

        /// <summary>
        /// ウェーブのタイマーを調べる
        /// </summary>
        void ChangeGaugeFill()
        {
            float gaugeProbability = 0.0f;

            if (_bDigmode)
            {
                gaugeProbability = _waveTimer / _waveManageScriptable.digTime;
            }
            else
            {
                //マックスから徐々に減らしていく
                gaugeProbability = MAX_FILLAMOUNT - _waveTimer / _waveManageScriptable.battleTime;

                //敵のアイコンを点滅させる
                if (!_bAttenuationAlpha)
                {
                    _workAlpha += ALPHA_AMOUNT * Time.deltaTime;
                    if (_workAlpha >= ALPHA_MAX)
                    {
                        _bAttenuationAlpha = !_bAttenuationAlpha;
                        _workAlpha = ALPHA_MAX;
                    }
                }
                else
                {
                    _workAlpha -= ALPHA_AMOUNT * Time.deltaTime;
                    if (_workAlpha <= 0)
                    {
                        _bAttenuationAlpha = !_bAttenuationAlpha;
                        _workAlpha = 0.0f;
                    }
                }
                ChangeEnemyIconColor();
            }
            _gaugeImage.fillAmount = gaugeProbability;
        }
        void ChangeEnemyIconColor()
        {
            Color color = _enemyIcon.color;
            color.a = _workAlpha;
            _enemyIcon.color = color;
        }
    }
}
