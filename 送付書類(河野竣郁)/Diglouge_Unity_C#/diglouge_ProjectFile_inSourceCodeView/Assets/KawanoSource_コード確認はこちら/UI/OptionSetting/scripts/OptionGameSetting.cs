using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Syuntoku.DigMode.Settings
{
    public class OptionGameSetting : MonoBehaviour
    {
         Sound.SoundManager _soundManager;
        [SerializeField] TMP_Text _bgmText;
        [SerializeField] Image _bgmNozzleImage;
        [SerializeField] TMP_Text _seText;
        [SerializeField] Image _seNozzleImage;
        [SerializeField] Slider _seSllider;
        [SerializeField] Slider _bgmSllider;
        float saveBgmAmount;
        float saveSeAmount;

        const float AMOUNT_RADIAN = 10.0f;
        const float MIN_VOLUME = -50.0f;
        const int PARSENT = 100;

#if UNITY_EDITOR
        readonly string OPTION_PATH_EDIOTR = "Assets/OptionSettings/OptionSettings.ini";
#else
        //オプションINIファイルのデータ
        string OPTION_PATH;
#endif

        //セクション　WindowSettings
        readonly string WINDOW_SECTION = "WindowSettings";
        readonly string HEIGHT_KEY = "Height";
        readonly string WIDTH_KEY = "Width";
        readonly string FULLSCREEN_KEY = "FullScreen";

        //セクション　Options
        readonly string OPTION_SECTION = "Options";
        readonly string BGM_VOLUME_KEY = "Bgm_Volume";
        readonly string SE_VOLUME_KEY = "Se_Volume";

        private void Awake()
        {
#if !UNITY_EDITOR
            OPTION_PATH = Application.dataPath + "/OptionSettings.ini";
#endif
            LoadSetting();
        }

        private void Start()
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<Sound.SoundManager>();
            LoadSetting();

        }

        public void BGMVolume(float amount)
        {
            Vector3 rotation = Vector3.zero;
            //最低値からの倍率を求め逆の最大値の倍率に変換する
            float pribility = (1.0f - amount / MIN_VOLUME);
            _bgmText.SetText(((int)(pribility* PARSENT)).ToString() + "%");
            rotation.z = AMOUNT_RADIAN * amount;
            _bgmNozzleImage.rectTransform.rotation = Quaternion.Euler(rotation);
            if (_soundManager == null) return;
            _soundManager.SetBGM(amount);

            saveBgmAmount = (int)amount;
        }

        public void SEVolume(float amount)
        {
            Vector3 rotation = Vector3.zero;
            //最低値からの倍率を求め逆の最大値の倍率に変換する
            float pribility = (1.0f - amount / MIN_VOLUME);
            _seText.SetText(((int)(pribility * PARSENT)).ToString() + "%");
            rotation.z = AMOUNT_RADIAN * amount;
            _seNozzleImage.rectTransform.rotation = Quaternion.Euler(rotation);
            if (_soundManager == null) return;
            _soundManager.SetSE(amount);

            saveSeAmount = (int)amount;
        }

        public void Back()
        {
            SaveSetting();
            gameObject.SetActive(false);
        }

        public void ReturnTitle()
        {
            SaveSetting();
            SceneManager.LoadScene("0_Title");
        }

        public void SaveSetting()
        {

            var init = new INIParser();

#if UNITY_EDITOR
            init.Open(OPTION_PATH_EDIOTR);
#else
            init.Open(OPTION_PATH);
#endif
            init.WriteValue(OPTION_SECTION, BGM_VOLUME_KEY,saveBgmAmount);
            init.WriteValue(OPTION_SECTION, SE_VOLUME_KEY, saveSeAmount);

            init.Close();
        }
        public void LoadSetting()
        {
            var init = new INIParser();

#if UNITY_EDITOR
            init.Open(OPTION_PATH_EDIOTR);
#else
            init.Open(OPTION_PATH);
#endif
            var height = init.ReadValue(WINDOW_SECTION, HEIGHT_KEY, 1080);
            var width = init.ReadValue(WINDOW_SECTION, WIDTH_KEY, 1980);
            bool fullScreen = init.ReadValue(WINDOW_SECTION, FULLSCREEN_KEY, true);

            Screen.SetResolution(width, height, fullScreen);

            var bgm_volume = init.ReadValue(OPTION_SECTION, BGM_VOLUME_KEY, 0);
            _bgmSllider.value = bgm_volume;

#if UNITY_EDITOR
            Debug.Log("BGM:" + bgm_volume);
#endif
            var se_volume = init.ReadValue(OPTION_SECTION, SE_VOLUME_KEY, 0);
            _seSllider.value = se_volume;

#if UNITY_EDITOR
            Debug.Log("SE:" + se_volume);
#endif
            BGMVolume(bgm_volume);
            SEVolume(se_volume);

            init.Close();
        }
    }
}
