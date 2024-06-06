using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionSMB : StateMachineBehaviour
{

    [SerializeField] AvatarTarget targetBodyPart = AvatarTarget.Root;
    [SerializeField, Range(0, 1)] float start = 0, end = 1;

    [HeaderAttribute("match target")]
    public Vector3 matchPosition;		// �w��p�[�c�����B���ė~�������W
    public Quaternion matchRotation;	// ���B���ė~������]

    public IMatchTarget target;

    bool isSkip = false;
    bool isInitialized = false;


    [HeaderAttribute("Weights")]
    public Vector3 positionWeight = Vector3.one;        // matchPosition�ɗ^����E�F�C�g�B(1,1,1)�Ŏ��R�A(0,0,0)�ňړ��ł��Ȃ��Be.g. (0,0,1)�őO��̂�
    public float rotationWeight = 0;            // ��]�ɗ^����E�F�C�g�B

    private MatchTargetWeightMask weightMask;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weightMask = new MatchTargetWeightMask(positionWeight, rotationWeight);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("aaa");
        animator.MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, start, end);
    }
}
public interface IMatchTarget
{
    Vector3 TargetPosition { get; }
}