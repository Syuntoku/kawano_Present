using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;



public class TitleScript : MonoBehaviour
{

    [SerializeField]
    RectTransform Title;

    float time;

    void Start()
    {
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time >= 2.0f)
        {
            time = 0.0f;
            SetScale();

        }
    }

    void SetScale()
    {
        /*
        if(bTitleScaleExpansion)
        {
            Title.transform.DOScale(maxScale, 1.9f).SetEase(Ease.InOutQuad);
            bTitleScaleExpansion = false;
        }
        else
        {
            Title.transform.DOScale(Vector3.one, 1.9f).SetEase(Ease.InOutQuad);
            bTitleScaleExpansion = true;
        }*/
    }

    public void SceneMove()
    {
        DOTween.KillAll();
        SceneManager.LoadSceneAsync("Digmode");
    }
}
