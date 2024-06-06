using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Syuntoku.DigMode
{
    public class Damage : MonoBehaviour
    {
            //ダメージ表記はSpriteAssetで行う
        TMP_Text _drawText;
        const float SLIDE_AMOUNT = 0.1f;
        const float COMPLATE_TIME = 0.5f;
        const float DESTROY_TIME = 1.3f;

        const string NormalFontName = "DamageCrt_";
        const string CriticalFontName = "DamageNormal_";

        public void Initialize(float damage,bool bCritical = false, float yAjust = 0.0f, float size = 1.8f)
        {
            _drawText = GetComponent<TMP_Text>();
            gameObject.transform.localScale *= size;
            gameObject.transform.DOMoveY(gameObject.transform.position.y + yAjust + SLIDE_AMOUNT, COMPLATE_TIME);

            //SpriteAssetの呼び出しテキストを設定
            char[] clip = damage.ToString().ToCharArray();
            string work = "";
            for (int i = 0; i < clip.Length; i++)
            {
                string spriteFontName = NormalFontName;
                if (bCritical) spriteFontName = CriticalFontName;
                work += "<sprite=\"" +spriteFontName+ "\""+ " index="+clip[i] +">";
            }
            _drawText.SetText(work);
            Destroy(gameObject, DESTROY_TIME);
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
