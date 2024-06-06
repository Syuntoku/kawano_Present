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
        [Header("武器のステータス")]
        public string weaponName;
        [Header("武器説明")]
        public string exlain;
        [Tooltip("手持ち時の移動速度低下倍率")]
        public float HandMovementSpeedDecreased;
        [Tooltip("リロード中の移動速度低下倍率　＊手持ち＋リロード％低下")]
        public float ReloadMovementSpeedDecreased;

        [Header("武器の性能"), Tooltip("一発のダメージ")]
        public float bulletDamage;
        [Tooltip("フルオートモード")]
        public bool bFullOuto;
        [Tooltip("弾薬数")]
        public int magazineSize;
        [Tooltip("弾の発射間隔")]
        public float shotRate;
        [Tooltip("弾の速度")]
        public float shotSpeed;
        [Tooltip("弾の有効距離")]
        public float shotHitLength;
        [Tooltip("リロード時間")]
        public float reloadTime;
        [Tooltip("重量")]
        public int weight;

        [Tooltip("最少の精密"), Range(0.0f, 1.0f)]
        public float minPrecision;
        [Tooltip("銃の最大の精密"), Range(0.0f, 1.0f)]
        public float maxPrecision;
        [Tooltip("銃の追加の精密度"), Range(0.0f, 1.0f)]
        public float addShotPrecision;

        public LayerMask layerMask;

        [Tooltip("弾丸")]
        public GameObject bulletPrf;

        [Tooltip("ヒットエフェクト")]
        public GameObject hitEfect;
        [Tooltip("マズルフラッシュ")]
        public GameObject muzzuleFlash;

        [Tooltip("アイコン")]
        public Sprite icon;
    }
}
