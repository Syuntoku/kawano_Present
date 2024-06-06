using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField]
    GameObject lookObj;

    void Update()
    {
        transform.LookAt(lookObj.transform);
    }
}
