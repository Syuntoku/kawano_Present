using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyAttackNotificationUI:MonoBehaviour
{
    [SerializeField] Image _label_UP;
    [SerializeField] Image _label_DOWN;
    public float _labelSpeed;
    bool _bAnimationStart;

    [SerializeField] Image[] _dialogs;
    public float _oneDialogTimeDeley;
    public float _comlateDialogAnimationTime;
    readonly Vector3 _dialogResize = new Vector3(0.76f, 0.76f, 0.76f);

    public float START_ANIMATION = 0.2f;
    public float ClOSE_ANIMATION = 0.2f;
    float _blinkingTimer;
    public float _blinkingInterval;
    const float DIALOG_LIT_INTERVAL_DIVISION = 100;
    float _activeTimer;
    public float _activeTime;
    void Start()
    {
        _bAnimationStart = false;
        transform.localScale = Vector3.right;
        transform.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            _bAnimationStart = true;
            //ダイアログのアニメーション
            float deley = 0;
            foreach (var dialog in _dialogs)
            {
                deley += _oneDialogTimeDeley;
                dialog.transform.DOScale(_dialogResize, _comlateDialogAnimationTime).SetDelay(deley);
            }
        });

        foreach (var dialog in _dialogs)
        {
            dialog.transform.localScale = Vector3.right;
        }

    }

    void Update()
    {
        if (!_bAnimationStart) return;
        _label_UP.rectTransform.anchoredPosition += Vector2.left * (_labelSpeed * Time.deltaTime);
        _label_DOWN.rectTransform.anchoredPosition += Vector2.right * (_labelSpeed * Time.deltaTime);

        //定期的にランダムなダイアログを点滅させる
        _blinkingTimer += Time.deltaTime;

        if (_blinkingTimer > _blinkingInterval)
        {
            int random = Random.Range(0, _dialogs.Length);
            _dialogs[random].gameObject.SetActive(false);
            _blinkingTimer = 0;
        }

        if(_blinkingTimer > _blinkingInterval / DIALOG_LIT_INTERVAL_DIVISION)
        {
            int random = Random.Range(0, _dialogs.Length);
            _dialogs[random].gameObject.SetActive(true);
        }
        _activeTimer += Time.deltaTime;

        if(_activeTimer > _activeTime)
        {
            transform.DOScale(Vector3.right, ClOSE_ANIMATION);
            _bAnimationStart = false;
        }
    }
}
