using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/Create PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("Playerステータス")]
    public string playerName;

    [Header("移動速度")]
    public float nomalSpeed;
    [Header("ジャンプの高さ")]
    public float nomalJump;

    [Header("空中ジャンプに移動するまでの遅延時間  遅延時間　＝　数値 × 0.16秒")]
    public int doubleJumpDeday;

    public float UpPower;

    [Header("マックスのスピードパワー")]
    public Vector3 maxVelosity;

    [Header("目線の長さ")]
    public float sightLength;

    [Header("0でダッシュ無効")]
    public float dashSpeed;

    [Header("テレポートする時間")]
    public float RespormTime;

    [Header("スポーン・リスポーンするオブジェクト")]
    public GameObject RespormPoint;

    [Header("テレポートする時のエフェクト")]
    public GameObject teleportEfect;

    [Header("視点操作の速度係数")]
    public float horizontalSpeed;
    public float verticalSpeed;

}
