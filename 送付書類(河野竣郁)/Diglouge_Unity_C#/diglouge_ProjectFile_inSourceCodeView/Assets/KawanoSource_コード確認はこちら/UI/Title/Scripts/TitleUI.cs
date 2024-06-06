using System.Collections;
using System.Collections.Generic;
using Syuntoku.DigMode.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    int _selectNum;
    public GameObject _selectCanvas;
    [SerializeField] public RectTransform _selectObjectTrans;
    [SerializeField] GameObject _selectObject;
    [SerializeField] GameObject _settingUI;
    [SerializeField] OptionGameSetting _optionGameSetting;

    enum Select
    { 
        START,
        SETTING,
        EXIT,
    }

    private void Awake()
    {
        _optionGameSetting.LoadSetting();
    }

    private void Start()
    {
        _selectObject.gameObject.SetActive(false);
    }

    public void SetTransform(RectTransform rectTransform,int num)
    {
        if(num == int.MaxValue)
        {
            _selectObject.gameObject.SetActive(false);
            return;
        }

        if(!_selectObject.gameObject.activeSelf)
        {
            _selectObject.gameObject.SetActive(true);
        }
        _selectNum = num;
        _selectObjectTrans.anchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnClick()
    {
        if (_selectNum == int.MaxValue) return;
        switch (_selectNum)
        {
            case (int)Select.START:
                SceneManager.LoadSceneAsync("1_Loading");
                break;
                case (int)Select.SETTING:
                _settingUI.SetActive(true);
                break;
            case (int)Select.EXIT:

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
                break;
        }
    }

    public void DisableCanvas()
    {
        _selectCanvas.SetActive(false);
    }
}
