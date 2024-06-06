using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    const float ONE_AMOUNT_RADIAN = 5.0f;
    const float MAX_RADIAN = 360f;
    [SerializeField] float _nowRadian;
    [SerializeField] float _radius;
    [SerializeField] float _cycleMagnification;
    float _x;

    void Start()
    {
        _x = transform.position.x;
    }

    void Update()
    {
        _nowRadian += ONE_AMOUNT_RADIAN * _cycleMagnification;
        if (_nowRadian >= MAX_RADIAN)
        {
            _nowRadian = 0.0f;
        }

        Vector3 pos = transform.position;
        pos.x = _x + Mathf.Sin(_nowRadian * Mathf.Rad2Deg) * _radius * Time.deltaTime;
        transform.position = pos;
    }
}
