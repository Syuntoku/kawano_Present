using UnityEngine;

namespace Syuntoku.DigMode.Inventory.Juwelry
{
    [CreateAssetMenu(fileName = "JuwelrySetting",menuName = "Create JuwelrySetting")]
    public class JuwelryScriptable : ScriptableObject
    {
        [Header("スカイライト")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind_SkyLight;
        public GameObject skylightPrf;
        public Sprite skylightIcon;
        public int skylightWeight;
        [SerializeField] AudioClip _getAudioSkylight;

        [Header("アンプリローズ")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind_Ampliroze;
        public GameObject amplirozePrf;
        public Sprite amplirozeIcon;
        public int amplirozeWeight;
        [SerializeField] AudioClip _getAudioAmpliroze;

        [Header("リンクハニー")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind_LinqHonney;
        public GameObject linqhoneyPrf;
        public Sprite linqhoneyIcon;
        public int linqhoneyWeight;
        [SerializeField] AudioClip _getAudioLinqhoney;

        [Header("ヘキサホープ")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind_Hexahope;
        public GameObject hexahopePrf;
        public Sprite hexahopeIcon;
        public int hexahopeWeight;
        [SerializeField] AudioClip _getAudioHexahope;

        [Header("フロラリーフ")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind_FloraReaf;
        public GameObject floraReafPrf;
        public Sprite floraReafIcon;
        public int floraReafWeight;
        [SerializeField] AudioClip _getAudioFloraReaf;

        [Header("シーフォースター")]
        public JuwelryInventory.JUWELRY_KIND juwelryKind;
        public GameObject seeforcestarPrf;
        public Sprite seeforcestarIcon;
        public int seeforcestarWeight;
        [SerializeField] AudioClip _getAudioSeeforcestar;

        public GameObject GetJuwelryPrefab(JuwelryInventory.JUWELRY_KIND juwelryKind)
        {
            switch (juwelryKind)
            {
                case JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                    return skylightPrf;
                case JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                    return amplirozePrf;
                case JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                    return linqhoneyPrf;
                case JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                    return hexahopePrf;
                case JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                    return floraReafPrf;
                case JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                    return seeforcestarPrf;
                case JuwelryInventory.JUWELRY_KIND.RANDOM_DROP:
                    JuwelryInventory.JUWELRY_KIND randomdata = (JuwelryInventory.JUWELRY_KIND)Random.Range((int)JuwelryInventory.JUWELRY_KIND.SKY_LIGHT, (int)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX);
                    return GetJuwelryPrefab(randomdata);
                default:
                    return null;
            }
        }
        public Sprite GetIcon(JuwelryInventory.JUWELRY_KIND juwelryKind)
        {
            switch (juwelryKind)
            {
                case JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                    return skylightIcon;
                case JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                    return amplirozeIcon;
                case JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                    return linqhoneyIcon;
                case JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                    return hexahopeIcon;
                case JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                    return floraReafIcon;
                case JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                    return seeforcestarIcon;
                default:
                    return null;
            }
        }
        public AudioClip GetSE(JuwelryInventory.JUWELRY_KIND juwelryKind)
        {
            switch (juwelryKind)
            {
                case JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                    return _getAudioSkylight;
                case JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                    return _getAudioAmpliroze;
                case JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                    return _getAudioLinqhoney;
                case JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                    return _getAudioHexahope;
                case JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                    return _getAudioFloraReaf;
                case JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                    return _getAudioSeeforcestar;
                default:
                    return null;
            }
        }

        public int JuwelryWeight(JuwelryInventory.JUWELRY_KIND juwelryKind)
        {

            switch (juwelryKind)
            {
                case JuwelryInventory.JUWELRY_KIND.SKY_LIGHT:
                    return skylightWeight;
                case JuwelryInventory.JUWELRY_KIND.AMPLIROZE:
                    return amplirozeWeight;
                case JuwelryInventory.JUWELRY_KIND.LINQHONEY:
                    return linqhoneyWeight;
                case JuwelryInventory.JUWELRY_KIND.HEXAHOPE:
                    return hexahopeWeight;
                case JuwelryInventory.JUWELRY_KIND.FLORAREAF:
                    return floraReafWeight;
                case JuwelryInventory.JUWELRY_KIND.SEEFORCESTER:
                    return seeforcestarWeight;
                default:
                    return 0;
            }
        }
    }
}
