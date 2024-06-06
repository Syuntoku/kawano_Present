using UnityEngine;
using Syuntoku.DigMode;
using Syuntoku.DigMode.Inventory.Juwelry;
using System;

[CreateAssetMenu(menuName = "Scriptable/Create BlockGenerate")]
public class BlockScriptable : ScriptableObject
{
    #region Enum
    public enum BlockKind
    {
        CRACKED_GROUND,
        DIRT_GROUND,
        DIRT_GROUND2,
        DIRT,
        DIRT_GRAVEL,
        FROZEN_PAVERS_AND_ICE,
        FROZEN_PAVERS_AND_SNOW,
        GRASS_AND_FLOWER,
        GRASS_PAVERS,
        GROUND_GRAVEL,
        GROUND_PAVEMENT,
        SAND_AND_ROCK,
        SAND,
        SNOW,
        STONE_AND_BLACK_ROCK,
        STONE_AND_SNOW,
        BED_ROCK,
    }

    public enum Category
    {
        AIR,
        NORMAL,
        JUWELRY,
        SPECIAL,
        PRIVATE_MESH,
        STATIC,
    }

    public enum SpescialCategory
    {
        NONE,
        TRESUREBOX,
        SHOPBLOCK,
        EXPLOSION_BLOCK,
        HEAL_BLOCK, 
    }

    [System.Flags]
    public enum BlockAdvancedSetting
    {
        NONE = 0x00,
        STATIC_BLOCK = 0x01,
        STATIC_MESH = 0x02,
        NOT_BLOCK_DAMAGE_SCALE = 0x04,
    }
    #endregion

    [Header("��`�����N�̃T�C�Y")]
    public Vector3 oneChankSize;
    [Header("�������̃u���b�N�Ԋu")]
    public float block_spaceX;
    public float block_spaceY;
    public float block_spaceZ;

    [Header("�g���m���̌����ʁ@�g���m��/ ?")]
    public int attenuationNum;
    [Header("�u���b�N�����̐���"), TextArea(3,10)]
    public string note;
    [Header("�x�[�X�̃I�u�W�F�N�g��ݒ�")]
    public BaseObjectGenerate[] baseObjectGenerates;
    [Header("�u���b�N���")]
    public BlockInformation[] blockData;
    [Header("�z�Ώ��")]
    public JuwelryInfomation[] jewelryData;
    [Header("���ʂȐ���")]
    public TresureChest tresureChest;
    public ShopBlock shopBlock;
    public ExplosionBlock explosionBlock;
    public HealBlock healBlock;
    [Header("���Ȃ��u���b�N�̐���")]
    public UnBreak unBreaks;

    [Header("�u���b�N��Prefab")]
    public GameObject mainBlockPrefab;
    public float meshPlibability_second;
    public Mesh defaultMesh;
    [Header("�j���̃��b�V��")]
    public Mesh breakMesh_60;
    public Mesh breakMesh_30;

    [Header("���ʂ̕�΃I�u�W�F�N�g")]
    public GameObject JuwelryObject;
    [Header("��΂̃��b�V��_�X�J�C���C�g")]
    public JuwelryMeshData skyBlueMesh;
    [Header("��΂̃��b�V��_�A���v�����[�Y")]
    public JuwelryMeshData amplirozeMesh;
    [Header("��΂̃��b�V��_�����N�n�j�[")]
    public JuwelryMeshData linqhoneyMesh;
    [Header("��΂̃��b�V��_�w�L�T�z�[�v")]
    public JuwelryMeshData hexahopeMesh;
    [Header("��΂̃��b�V��_�t�������[�t")]
    public JuwelryMeshData floraReafMesh;
    [Header("��΂̃��b�V��_�V�[�t�H�[�X�^�[")]
    public JuwelryMeshData seeforcestarMesh;

    /// <summary>
    /// ��΂̃��b�V�����擾
    /// </summary>
    public JuwelryMeshData GetBreakJuwelryMesh(JuwelryInventory.JUWELRY_KIND juwelryKind)
    {
        switch (juwelryKind)
        {
            case JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                return skyBlueMesh;
            case JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                return amplirozeMesh;
            case JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                return linqhoneyMesh;
            case JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                return hexahopeMesh;
            case JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                return floraReafMesh;
            case JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                return seeforcestarMesh;
            default:
                return null;
        }
    }

    /// <summary>
    /// �u���b�N���
    /// </summary>
    [System.Serializable]
    public class BlockInformation
    {
        public string name;
        public int hp;
        public Category category;
        public BlockKind blockKind;
        [Tooltip("�K�w���Ƃ̍d���ݒ�")]
        public Hardness[] blockHardnessMagnification;
        [Header("�K�w���Ƃ̐����ݒ�")]
        public BrockProbabilityData[] brockProbabilityData;
        [Header("�����Ɏg���ݒ�")]
        public BlockGenerateData objectData;
    }

    [System.Serializable]
    public class BaseObjectGenerate
    {
        public string info;
        public string name;
        public int hp;
        public Category category;
        public BlockKind blockKind;
        [Tooltip("�K�w���Ƃ̍d���ݒ�")]
        public Hardness[] blockHardnessMagnification;
        [Tooltip("�����Ɏg���ݒ�")]
        public BlockGenerateData objectData;
    }

    /// <summary>
    /// ��Ώ��
    /// </summary>
    [Serializable]
    public class JuwelryInfomation
    {
        public string name;
        public int hp;
        public Category category;
        [Tooltip("�K�w���Ƃ̍d���ݒ�")]
        public Hardness[] blockHardnessMagnification;
        public DropSetting dropSetting = new DropSetting();
        [Header("�K�w���Ƃ̐����ݒ�")]
        public BrockProbabilityData[] brockProbabilityData;
    }

    [Serializable]
    public class TresureChest
    {
        [Header("�L����")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        public GameObject tresureChestPrefab;
        [Range(0,200)]
        public int generateCount;
        [Header("�����p�I�u�W�F�N�g")]
        public BlockGenerateData objectData;
        public DropSetting dropSetting;
    }

    [Serializable]
    public class ShopBlock
    {
        [Header("�L����")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("���[���h�ɐ������鐔 �ŏ��ƍő�")]
        public int WorldInCountMin;
        public int WorldInCountMax;
        [Header("����̃u���b�N")]
        public BlockKind blockKind;
        public GameObject _shopWallPrf;
        [Header("�V���b�v�L�����N�^�[�I�u�W�F�N�g")]
        public GameObject _ShopCharactorPrf;
        [Header("�L�����N�^�[��������E�F�[�u��")]
        public int destoryWeveCount;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class ExplosionBlock
    {
        [Header("�L����")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("���[���h�ɐ������鐔")]
        public int generateCount;
        [Header("�u���b�Nprefab")]
        public GameObject explosionBlockPrf;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class HealBlock
    {
        [Header("�L����")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("���[���h�ɐ������鐔")]
        public int generateCount;
        [Header("�u���b�Nprefab")]
        public GameObject healBlockPrf;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class ExplositionBlock
    {
        [Header("�L����")]
        public bool _bActive;
        public string _name;
        public int hp;
        public string _category;
        public int ID;
        public float explositonRange;
        public float explosionPlayerDamage;
        public float explosionBlockDamage;
        public float explosionEnemyDamage;
        [Header("�����p�I�u�W�F�N�g")]
        public GameObject _brockPrf;
        [Header("�󂵂��Ƃ��̃G�t�F�N�g")]
        public GameObject _BreakEfectPrf;
    }
    [Serializable]
    public class PlayerPowerUp
    {
        [Header("�L����")]
        public bool _bActive;
        public string _name;
        public int hp;
        public string _category;
        public int ID;
        public float upgradeTime;
        [Header("�����p�I�u�W�F�N�g")]
        public GameObject _brockPrf;
        [Header("�󂵂��Ƃ��̃G�t�F�N�g")]
        public GameObject _BreakEfectPrf;
    }

    /// <summary>
    /// �����m��
    /// </summary>
    [Serializable]
    public class JuwelryProbabilityData
    {
        public string name;
        [Range(0, 100)]
        public float probability;
    }

    /// <summary>
    /// �u���b�N�𐶐�����m��
    /// </summary>
    [Serializable]
    public class BrockProbabilityData
    {
        public string name;
        [Header("�S�̉�𐶐����鐔"), Range(1, 100)]
        public int generateNum = 5;      
        [Header("�h�����Đ�������ݒ�")]
        [Header("�����m���@��")]
        public int probility = 20;
        [Header("�����m��������")]
        public int probabilityDecrease = 10;
    }


    /// <summary>
    /// ���A�p�f�[�^
    /// </summary>
    public class CaveData
    {
        [Multiline(4)]
        public string Note;
        public string _name;
        public int _hp;
        [Header("�J�e�S���[")]
        public string _category;
        [Header("�K�w���Ƃ̐����ݒ�")]
        public BrockProbabilityData[] _brockProbabilityData;
    }

    [Serializable]
    public class CaveDataSetting
    {
        [Header("�����m���@���S����")]
        public float probability;
        [Header("���S�𐶐����鐔")]
        public int generatePointNum;
        [Header("���A�̒�����ݒ�@�������e�����ǂꂩ�ɐ�������")]
        public int caveLength;
        [Header("���A�̑傫�� ���~�̔��a")]
        public float createSize;
    }

    /// <summary>
    /// �j�󂳂�Ȃ��u���b�N
    /// </summary>
    [Serializable]
    public class UnBreak
    {
        public int name;
        public int ID;
        public Category category;
        public BlockGenerateData generateData;
    }

    [Serializable]
    public class BlockGenerateData
    {
        public Material[] material;
        public GameObject breakEfectPrefab;
#if UNITY_EDITOR
        [EnumFlags]
#endif
        public BlockAdvancedSetting advancedSetting;

        /// <summary>
        /// �����ݒ�̗L�����`�F�b�N����
        /// </summary>
        /// <returns>true:�L���@false:����</returns>
        public bool CheckFlag(BlockAdvancedSetting check)
        {
            if(((uint)advancedSetting & (uint)check) == (uint)check) return true;
            return false;
        }
    }

    [Serializable]
    public class JuwelryMeshData
    {
        public Mesh meshNormal;
        public Material[] materials;
    }

    [Serializable]
    public struct Hardness
    {
        public string name;
        public float hardPlibability;
    }
}






