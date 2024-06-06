using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;


namespace Syuntoku.DigMode.Enemy
{
    [System.Serializable]
    public class EnemyStatus
    {
        public string enemyName;

        [Header("------------------Enemy�̒�������-----------------")]
        [Tooltip("�G�̃X�s�[�h")]
        public float speed;
        [Header("--�U��--")]
        [Header("attackPlayerLength = �U���J�n\n" +
                "longRangeAttackLength�ȉ��̋����@���@�ʏ�U��\n" +
                "longRangeAttackLength�ȏ�̋����@���@�������U��\n")]
        [Tooltip("�s�����~�܂�A�U�����n�߂鋗��")]
        public float attackPlayerLength;
        [Tooltip("���������J�n���鋗��")]
        public float longRangeAttackLength;
        [Header("------")]
        [Tooltip("�ڐ��̋���")]
        public float forwordRayLength;
        [Tooltip("�w�̍���")]
        public Vector3 tall;
        [Tooltip("�G���n�ʂɏ���Ă���Ɣ��f���鋗��")]
        public float groundStandLength;
        [Tooltip("�W�����v�̋���")]
        public float jumpPower;
        [Tooltip("�W�����v���̈ړ����x�A�b�v�{��"),Range(0.0f,1.0f)]
        public float jumpSpeedupMagnification;
        [Tooltip("��ɏ��Ƃ��̏㏸�{��"), Range(0.0f, 3.0f)]
        public float ClinbPribability;
        [Tooltip("�_���[�W�m�b�N�o�b�N�𖳎�����l"), Range(0.0f, 1.0f)]
        public float knockBackIgnore;
        [Tooltip("���̃u���b�N���������Ɏ~�܂鋗��")]
        public float stopMoveUnderLength;
        [Tooltip("�u���b�N�ɋ߂��Ɣ��f���鋗��")]
        public float nearForwordBlockLength;
        [Tooltip("�G�̃X�e�[�g��؂�ւ��鎞��")]
        public float stateChageTime;
        public LayerMask layerMask;
        public RaycastHit hitObject;
        [Header("------------�X�e�[�^�X----------------")]
        [Tooltip("�q�b�g�|�C���g")]
        public float hp;
        [Tooltip("�q�b�g�|�C���g�̍ő吔")]
        public float hpMax;
        [Tooltip("�̏d"), Range(0.0f, 3.0f)]
        public float bodyWeight;
        [Tooltip("�h���b�v�ݒ�")]
        public DropSetting dropSetting;

        [Tooltip("�U����")]
        public float playerDamage;
        [Tooltip("�U���C���^�[�o��")]
        public float attackCoolTime;
        [Tooltip("�ߋ����U���̃G�t�F�N�g����������")]
        public float attackEfectTime;
        [Tooltip("�ߋ����U���̃G�t�F�N�g�T�C�Y")]
        public Vector3 attackEfectSize;
        [Tooltip("�������U�����\�ɂ��邩")]
        public bool longRangeAttack;
        [Tooltip("����I�ȉ������U�������鎞��")]
        public float regularlyLongRangeAttackTimer;
        [Tooltip("�������U���̃G�t�F�N�g����������")]
        public float longAttackEfectTime;
        [Tooltip("�������U���̃G�t�F�N�g�T�C�Y")]
        public Vector3 longAttackEfectSize;
        [Tooltip("�e���")]
        public BulletStatus bulletStatus;
        [Tooltip("�������U���C���^�[�o��")]
        public float longRangeAttackInterval;
        [Tooltip("�������U�����o��ꏊ�̒���")]
        public Vector3 longRangePositionAjust;
        public float longRangePositionAjustMagnification;
        [Tooltip("�u���b�N�ɗ^����_���[�W�{��"),Range(0.0f,1.0f)]
        public float blockDamageMagnification;
        [Header("---------------------------------------")]

        [Tooltip("������Ƃ��̋���")]
        public float disableLength;
        [Tooltip("�_���[�W�e�L�X�g�̍����̒���")]
        public float damageTextYAjust;
        [Header("------------------Enemy��Ray�̒���-----------------")]

        [Tooltip("�G�̒��S�̒���")]
        public Vector3 pivotAjust;
        [Tooltip("��̖ڐ��̃x�N�g�������߂�")]
        public Vector3 upperAngle;
        [Tooltip("��̖ڐ��̒���")]
        public Vector3 upperAngleAjust;
        [Tooltip("���̖ڐ��̒���")]
        public Vector3 underRayAjust;
        [Tooltip("�ڐ��𒲐�")]
        public Vector3 forwordRayAjust;
        [Tooltip("�ڐ���Ray�����E�ɗh�炷�Ƃ��̐U�ꕝ"),Range(0.0f,1.0f)]
        public float swingWidth;

        [Tooltip("�o�����Ƀu���b�N��j�󂷂�͈�")]
        public float firstBreakRange;

        [Tooltip("�f�B�]���u���J�n����鎞��")]
        public float delayStartDissolveTime;
        [Tooltip("�����Ă���Ƃ��̃��[�V����")]
        public string moveMostionName;
        [Tooltip("���񂾂Ƃ��̃��[�V����")]
        public string downMositionName;
        [Tooltip("�U�����̃��[�V����")]
        public string attackMositionName;
        [Tooltip("�e�ɓ��������Ƃ��̃��[�V����")]
        public string hitByBulletMosionName;
        [Tooltip("�������U���̃��[�V����")]
        public string longRangeAttackMositonName;

        [Tooltip("���ɔj�󂵂����̃N�[���^�C��")]
        public float bottonBreakInterval;

        [Header("------------------Enemy�̕�������-----------------")]

        [Tooltip("�v���O�����ł̏d�͂̓K��")]
        public bool bDisableGravity;
        [Tooltip("�d��")]
        public float gravityStrength;
    }
}