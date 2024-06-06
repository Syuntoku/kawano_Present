using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSin : MonoBehaviour
{
    [SerializeField] GameObject player;

    float time;
    Vector3 postion;
    const float ADD_ANGLE = 5.0f * Mathf.Rad2Deg;

    Vector3 holdPosition;


    // Start is called before the first frame update
    void Start()
    {
        holdPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime;

        postion.x += Mathf.Sin(time * Mathf.Rad2Deg);
       // postion.z += Mathf.Cos(time);

        transform.position = postion + holdPosition;
    }
}
