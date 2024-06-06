
namespace Syuntoku.DigMode.Inventory.Juwelry
{
    /// <summary>
    /// 宝石の使用コストを管理する
    /// </summary>
    [System.Serializable]
    public class JuwelryCost
    {
       public  Cost[] costs;
    }

    [System.Serializable]
    public class Cost
    {
        public JuwelryInventory.JUWELRY_KIND useKind;
        public int useCount;
    }
}
