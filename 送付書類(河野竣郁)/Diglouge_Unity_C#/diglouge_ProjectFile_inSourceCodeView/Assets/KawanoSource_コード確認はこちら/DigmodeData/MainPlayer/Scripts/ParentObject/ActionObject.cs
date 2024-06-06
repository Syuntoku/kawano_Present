using UnityEngine;
using Syuntoku.DigMode.UI;

/// <summary>
/// 親クラス
/// アクションができるオブジェクト
/// </summary>
public class ActionObject : MonoBehaviour
{
    public const string ACTION_OBJECT_TAG_NAME = "ActionObject";

    /// <summary>
    /// アクションを行う
    /// </summary>
    virtual public void OnAction(UIManage uiManage= null)
    {

    }
}
