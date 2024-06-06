using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/Create PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("Player�X�e�[�^�X")]
    public string playerName;

    [Header("�ړ����x")]
    public float nomalSpeed;
    [Header("�W�����v�̍���")]
    public float nomalJump;

    [Header("�󒆃W�����v�Ɉړ�����܂ł̒x������  �x�����ԁ@���@���l �~ 0.16�b")]
    public int doubleJumpDeday;

    public float UpPower;

    [Header("�}�b�N�X�̃X�s�[�h�p���[")]
    public Vector3 maxVelosity;

    [Header("�ڐ��̒���")]
    public float sightLength;

    [Header("0�Ń_�b�V������")]
    public float dashSpeed;

    [Header("�e���|�[�g���鎞��")]
    public float RespormTime;

    [Header("�X�|�[���E���X�|�[������I�u�W�F�N�g")]
    public GameObject RespormPoint;

    [Header("�e���|�[�g���鎞�̃G�t�F�N�g")]
    public GameObject teleportEfect;

    [Header("���_����̑��x�W��")]
    public float horizontalSpeed;
    public float verticalSpeed;

}
