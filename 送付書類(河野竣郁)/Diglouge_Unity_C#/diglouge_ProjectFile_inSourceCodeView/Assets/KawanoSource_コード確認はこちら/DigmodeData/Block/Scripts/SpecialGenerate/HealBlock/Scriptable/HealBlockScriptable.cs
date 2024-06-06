using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Block/HealBlockScriptable")]
public class HealBlockScriptable : ScriptableObject
{
    [Tooltip("�񕜗�")]
    public float healPower;

    [Tooltip("�񕜌��ʂ���������͈�")]
    public float healRange;

    public GameObject healobject;
    public LayerMask healSubjectMask;
}
