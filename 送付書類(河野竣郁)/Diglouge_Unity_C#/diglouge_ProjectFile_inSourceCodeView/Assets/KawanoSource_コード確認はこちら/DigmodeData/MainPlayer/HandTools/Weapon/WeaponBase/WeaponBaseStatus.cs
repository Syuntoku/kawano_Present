using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    [System.Serializable]
    public class WeaponBaseStatus
    {
        public ToolGenerater.WeaponName weaponKind;
        [Header("����̃X�e�[�^�X")]
        public string weaponName;
        [Header("�������")]
        public string exlain;
        [Tooltip("�莝�����̈ړ����x�ቺ�{��")]
        public float HandMovementSpeedDecreased;
        [Tooltip("�����[�h���̈ړ����x�ቺ�{���@���莝���{�����[�h���ቺ")]
        public float ReloadMovementSpeedDecreased;

        [Header("����̐��\"), Tooltip("�ꔭ�̃_���[�W")]
        public float bulletDamage;
        [Tooltip("�t���I�[�g���[�h")]
        public bool bFullOuto;
        [Tooltip("�e��")]
        public int magazineSize;
        [Tooltip("�e�̔��ˊԊu")]
        public float shotRate;
        [Tooltip("�e�̑��x")]
        public float shotSpeed;
        [Tooltip("�e�̗L������")]
        public float shotHitLength;
        [Tooltip("�����[�h����")]
        public float reloadTime;
        [Tooltip("�d��")]
        public int weight;

        [Tooltip("�ŏ��̐���"), Range(0.0f, 1.0f)]
        public float minPrecision;
        [Tooltip("�e�̍ő�̐���"), Range(0.0f, 1.0f)]
        public float maxPrecision;
        [Tooltip("�e�̒ǉ��̐����x"), Range(0.0f, 1.0f)]
        public float addShotPrecision;

        public LayerMask layerMask;

        [Tooltip("�e��")]
        public GameObject bulletPrf;

        [Tooltip("�q�b�g�G�t�F�N�g")]
        public GameObject hitEfect;
        [Tooltip("�}�Y���t���b�V��")]
        public GameObject muzzuleFlash;

        [Tooltip("�A�C�R��")]
        public Sprite icon;
    }
}
