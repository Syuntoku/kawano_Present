using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    const float ONE_AMOUNT_RADIAN = 5.0f;
    const float MAX_RADIAN = 360f;
    [SerializeField] float _nowRadian;
    [SerializeField] float _radius;
    [SerializeField] float _cycleMagnification;
    float _y;
    bool _bResolsition;

    void Start()
    {
        _y = transform.position.y;
    }

    void Update()
    {
        if(!_bResolsition)
        {
            _nowRadian += ONE_AMOUNT_RADIAN * _cycleMagnification * Time.deltaTime;
        }
        else
        {
            _nowRadian -= ONE_AMOUNT_RADIAN * _cycleMagnification * Time.deltaTime;
        }

        if (_nowRadian >= MAX_RADIAN || _nowRadian <= 0)
        {
            _bResolsition = !_bResolsition;
        }

        Vector3 pos = transform.position;
        pos.y = _y + Mathf.Sin(_nowRadian) * _radius;
        transform.position = pos;
    }
}
