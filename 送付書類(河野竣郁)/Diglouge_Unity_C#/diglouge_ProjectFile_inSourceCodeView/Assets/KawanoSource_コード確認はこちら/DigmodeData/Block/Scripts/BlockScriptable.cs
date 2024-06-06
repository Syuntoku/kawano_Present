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

    [Header("一チャンクのサイズ")]
    public Vector3 oneChankSize;
    [Header("生成時のブロック間隔")]
    public float block_spaceX;
    public float block_spaceY;
    public float block_spaceZ;

    [Header("拡張確率の減衰量　拡張確率/ ?")]
    public int attenuationNum;
    [Header("ブロック生成の説明"), TextArea(3,10)]
    public string note;
    [Header("ベースのオブジェクトを設定")]
    public BaseObjectGenerate[] baseObjectGenerates;
    [Header("ブロック情報")]
    public BlockInformation[] blockData;
    [Header("鉱石情報")]
    public JuwelryInfomation[] jewelryData;
    [Header("特別な生成")]
    public TresureChest tresureChest;
    public ShopBlock shopBlock;
    public ExplosionBlock explosionBlock;
    public HealBlock healBlock;
    [Header("壊れないブロックの生成")]
    public UnBreak unBreaks;

    [Header("ブロックのPrefab")]
    public GameObject mainBlockPrefab;
    public float meshPlibability_second;
    public Mesh defaultMesh;
    [Header("破壊後のメッシュ")]
    public Mesh breakMesh_60;
    public Mesh breakMesh_30;

    [Header("共通の宝石オブジェクト")]
    public GameObject JuwelryObject;
    [Header("宝石のメッシュ_スカイライト")]
    public JuwelryMeshData skyBlueMesh;
    [Header("宝石のメッシュ_アンプリローズ")]
    public JuwelryMeshData amplirozeMesh;
    [Header("宝石のメッシュ_リンクハニー")]
    public JuwelryMeshData linqhoneyMesh;
    [Header("宝石のメッシュ_ヘキサホープ")]
    public JuwelryMeshData hexahopeMesh;
    [Header("宝石のメッシュ_フロラリーフ")]
    public JuwelryMeshData floraReafMesh;
    [Header("宝石のメッシュ_シーフォースター")]
    public JuwelryMeshData seeforcestarMesh;

    /// <summary>
    /// 宝石のメッシュを取得
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
    /// ブロック情報
    /// </summary>
    [System.Serializable]
    public class BlockInformation
    {
        public string name;
        public int hp;
        public Category category;
        public BlockKind blockKind;
        [Tooltip("階層ごとの硬さ設定")]
        public Hardness[] blockHardnessMagnification;
        [Header("階層ごとの生成設定")]
        public BrockProbabilityData[] brockProbabilityData;
        [Header("生成に使う設定")]
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
        [Tooltip("階層ごとの硬さ設定")]
        public Hardness[] blockHardnessMagnification;
        [Tooltip("生成に使う設定")]
        public BlockGenerateData objectData;
    }

    /// <summary>
    /// 宝石情報
    /// </summary>
    [Serializable]
    public class JuwelryInfomation
    {
        public string name;
        public int hp;
        public Category category;
        [Tooltip("階層ごとの硬さ設定")]
        public Hardness[] blockHardnessMagnification;
        public DropSetting dropSetting = new DropSetting();
        [Header("階層ごとの生成設定")]
        public BrockProbabilityData[] brockProbabilityData;
    }

    [Serializable]
    public class TresureChest
    {
        [Header("有効か")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        public GameObject tresureChestPrefab;
        [Range(0,200)]
        public int generateCount;
        [Header("生成用オブジェクト")]
        public BlockGenerateData objectData;
        public DropSetting dropSetting;
    }

    [Serializable]
    public class ShopBlock
    {
        [Header("有効か")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("ワールドに生成する数 最少と最大")]
        public int WorldInCountMin;
        public int WorldInCountMax;
        [Header("周りのブロック")]
        public BlockKind blockKind;
        public GameObject _shopWallPrf;
        [Header("ショップキャラクターオブジェクト")]
        public GameObject _ShopCharactorPrf;
        [Header("キャラクターが消えるウェーブ数")]
        public int destoryWeveCount;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class ExplosionBlock
    {
        [Header("有効か")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("ワールドに生成する数")]
        public int generateCount;
        [Header("ブロックprefab")]
        public GameObject explosionBlockPrf;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class HealBlock
    {
        [Header("有効か")]
        public bool bActive;
        public string name;
        public int hp;
        public SpescialCategory specialCategory;
        public Category category;
        [Header("ワールドに生成する数")]
        public int generateCount;
        [Header("ブロックprefab")]
        public GameObject healBlockPrf;
        public BlockGenerateData objectData;
    }

    [Serializable]
    public class ExplositionBlock
    {
        [Header("有効か")]
        public bool _bActive;
        public string _name;
        public int hp;
        public string _category;
        public int ID;
        public float explositonRange;
        public float explosionPlayerDamage;
        public float explosionBlockDamage;
        public float explosionEnemyDamage;
        [Header("生成用オブジェクト")]
        public GameObject _brockPrf;
        [Header("壊したときのエフェクト")]
        public GameObject _BreakEfectPrf;
    }
    [Serializable]
    public class PlayerPowerUp
    {
        [Header("有効か")]
        public bool _bActive;
        public string _name;
        public int hp;
        public string _category;
        public int ID;
        public float upgradeTime;
        [Header("生成用オブジェクト")]
        public GameObject _brockPrf;
        [Header("壊したときのエフェクト")]
        public GameObject _BreakEfectPrf;
    }

    /// <summary>
    /// 生成確率
    /// </summary>
    [Serializable]
    public class JuwelryProbabilityData
    {
        public string name;
        [Range(0, 100)]
        public float probability;
    }

    /// <summary>
    /// ブロックを生成する確率
    /// </summary>
    [Serializable]
    public class BrockProbabilityData
    {
        public string name;
        [Header("４つの塊を生成する数"), Range(1, 100)]
        public int generateNum = 5;      
        [Header("派生して生成する設定")]
        [Header("生成確立　％")]
        public int probility = 20;
        [Header("生成確立減少量")]
        public int probabilityDecrease = 10;
    }


    /// <summary>
    /// 洞窟用データ
    /// </summary>
    public class CaveData
    {
        [Multiline(4)]
        public string Note;
        public string _name;
        public int _hp;
        [Header("カテゴリー")]
        public string _category;
        [Header("階層ごとの生成設定")]
        public BrockProbabilityData[] _brockProbabilityData;
    }

    [Serializable]
    public class CaveDataSetting
    {
        [Header("生成確率　＊百分率")]
        public float probability;
        [Header("中心を生成する数")]
        public int generatePointNum;
        [Header("洞窟の長さを設定　＊個数分各方向どれかに生成する")]
        public int caveLength;
        [Header("洞窟の大きさ ＊円の半径")]
        public float createSize;
    }

    /// <summary>
    /// 破壊されないブロック
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
        /// 生成設定の有効をチェックする
        /// </summary>
        /// <returns>true:有効　false:無効</returns>
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






