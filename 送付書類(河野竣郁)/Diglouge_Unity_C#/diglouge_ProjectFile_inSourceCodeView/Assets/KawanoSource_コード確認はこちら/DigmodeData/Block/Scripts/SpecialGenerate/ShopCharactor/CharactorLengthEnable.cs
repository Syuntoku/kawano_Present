using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorLengthEnable : MonoBehaviour
{
    [SerializeField]GameObject _playerObject;

    public float ENABLELENGTH = 10.0f;

    int count;
    int maxCount = 3;

    private void Update()
    {
        count++;

        if (count <= maxCount) return;

        LengthSetiing();

        count = 0;
    }

    public void LengthSetiing()
    {
        foreach (Transform item in transform)
        {
            Vector3 length = _playerObject.transform.position - item.position;

            if(length.magnitude <= ENABLELENGTH)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
