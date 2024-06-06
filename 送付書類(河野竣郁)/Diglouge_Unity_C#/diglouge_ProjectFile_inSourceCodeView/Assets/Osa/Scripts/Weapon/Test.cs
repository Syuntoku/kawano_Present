using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        for (int i = 0; i < 500; i++)
        {

            Vector2 rand = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1)) * 3;
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = new Vector3(rand.x, 0, rand.y);
            obj.transform.localScale = Vector3.one * 0.3f;
        }
    }
}