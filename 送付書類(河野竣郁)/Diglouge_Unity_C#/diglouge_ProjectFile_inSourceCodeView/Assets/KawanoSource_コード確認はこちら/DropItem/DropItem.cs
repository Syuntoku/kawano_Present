using UnityEngine;

namespace Syuntoku.DigMode.Drop
{
    /// <summary>
    /// 親クラス
    /// ドロップアイテム
    /// </summary>
    public class DropItem : MonoBehaviour
    {
        public virtual void GetItem()
        {
            Destroy(gameObject);
        }
    }
}
