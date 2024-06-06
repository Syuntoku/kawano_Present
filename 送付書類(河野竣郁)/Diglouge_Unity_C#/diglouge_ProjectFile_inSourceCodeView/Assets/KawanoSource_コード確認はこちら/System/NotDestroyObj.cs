using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyObj : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
