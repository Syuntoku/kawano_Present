using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraManage : MonoBehaviour
{
    #region CashVeriable
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _mainUi;
    [SerializeField] GameObject _statusGauge;
    [SerializeField] Sprite DestroyImage;
    [SerializeField] Sprite GaugeImage;
    #endregion
    Image image;
    GameObject drawObject;
    float drawtime;
    public const float DrawMax = 1.0f;

    public void DrawStatusUi(Vector3 blockPos, float nowHp, float maxHp)
    {
        if (drawObject == null)
        {
            drawObject = Instantiate(_statusGauge, Vector3.zero, Quaternion.identity);
            drawObject.GetComponent<RectTransform>().localPosition = _mainUi.transform.position;
            drawObject.transform.SetParent(_mainUi.transform);
            drawObject.GetComponent<RectTransform>().localScale = Vector3.one;
            image = drawObject.transform.GetChild(0).GetComponent<Image>();
            drawtime = 0.0f;
        }
        else
        {
            image.sprite = GaugeImage;
            drawtime = 0.0f;
        }

        image.fillAmount = (1 - nowHp / maxHp);

        if (image.fillAmount >= 1)
        {
            image.sprite = DestroyImage;
        }
    }

    private void Update()
    {
        if (drawObject == null) return;

        drawtime += Time.deltaTime;

        if (drawtime >= DrawMax)
        {
            Destroy(drawObject);
            image.sprite = GaugeImage;
            drawObject = null;
            image = null;
            drawtime = 0.0f;
        }
    }
}