
namespace Syuntoku.DigMode.Inventory.Juwelry
{
    /// <summary>
    /// ��΂̎g�p�R�X�g���Ǘ�����
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
