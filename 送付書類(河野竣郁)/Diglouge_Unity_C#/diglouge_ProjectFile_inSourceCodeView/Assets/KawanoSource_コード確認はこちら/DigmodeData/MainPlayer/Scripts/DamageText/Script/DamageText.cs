using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode
{
    /// <summary>
    /// uiにダメージテキストを表示する
    /// </summary>
    public class DamageText : MonoBehaviour
    {
        [SerializeField] GameObject _textPrefab;
        [SerializeField] GameObject _textParent;
        public float publicYAjust;

        public void InstanceDamageText(Vector3 position, float damage,float yAjust = 0.0f ,float size = 1)
        {
            Vector3 generatePosition = Vector3.zero;
            generatePosition.y = yAjust + publicYAjust;

            GameObject generateText = Instantiate(_textPrefab, position + generatePosition, Quaternion.identity,_textParent.transform);
            generateText.GetComponent<Damage>().Initialize(damage);
        }
    }
}
