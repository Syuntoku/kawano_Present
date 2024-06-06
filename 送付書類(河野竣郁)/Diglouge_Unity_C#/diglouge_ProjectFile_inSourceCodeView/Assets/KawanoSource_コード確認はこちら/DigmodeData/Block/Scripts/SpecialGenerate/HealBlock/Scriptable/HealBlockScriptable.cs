using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Block/HealBlockScriptable")]
public class HealBlockScriptable : ScriptableObject
{
    [Tooltip("‰ñ•œ—Ê")]
    public float healPower;

    [Tooltip("‰ñ•œŒø‰Ê‚ª”­“®‚·‚é”ÍˆÍ")]
    public float healRange;

    public GameObject healobject;
    public LayerMask healSubjectMask;
}
