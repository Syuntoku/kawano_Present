using UnityEngine;

namespace Syuntoku.DigMode.Drop
{
    /// <summary>
    /// �e�N���X
    /// �h���b�v�A�C�e��
    /// </summary>
    public class DropItem : MonoBehaviour
    {
        public virtual void GetItem()
        {
            Destroy(gameObject);
        }
    }
}
