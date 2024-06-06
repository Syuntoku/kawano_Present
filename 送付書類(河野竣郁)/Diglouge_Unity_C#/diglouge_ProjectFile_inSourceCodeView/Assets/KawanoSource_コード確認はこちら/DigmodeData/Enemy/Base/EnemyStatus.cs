using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;


namespace Syuntoku.DigMode.Enemy
{
    [System.Serializable]
    public class EnemyStatus
    {
        public string enemyName;

        [Header("------------------Enemyの調整項目-----------------")]
        [Tooltip("敵のスピード")]
        public float speed;
        [Header("--攻撃--")]
        [Header("attackPlayerLength = 攻撃開始\n" +
                "longRangeAttackLength以下の距離　＝　通常攻撃\n" +
                "longRangeAttackLength以上の距離　＝　遠距離攻撃\n")]
        [Tooltip("行動が止まり、攻撃し始める距離")]
        public float attackPlayerLength;
        [Tooltip("遠距離を開始する距離")]
        public float longRangeAttackLength;
        [Header("------")]
        [Tooltip("目線の距離")]
        public float forwordRayLength;
        [Tooltip("背の高さ")]
        public Vector3 tall;
        [Tooltip("敵が地面に乗っていると判断する距離")]
        public float groundStandLength;
        [Tooltip("ジャンプの強さ")]
        public float jumpPower;
        [Tooltip("ジャンプ中の移動速度アップ倍率"),Range(0.0f,1.0f)]
        public float jumpSpeedupMagnification;
        [Tooltip("上に上るときの上昇倍率"), Range(0.0f, 3.0f)]
        public float ClinbPribability;
        [Tooltip("ダメージノックバックを無視する値"), Range(0.0f, 1.0f)]
        public float knockBackIgnore;
        [Tooltip("下のブロックが遠い時に止まる距離")]
        public float stopMoveUnderLength;
        [Tooltip("ブロックに近いと判断する距離")]
        public float nearForwordBlockLength;
        [Tooltip("敵のステートを切り替える時間")]
        public float stateChageTime;
        public LayerMask layerMask;
        public RaycastHit hitObject;
        [Header("------------ステータス----------------")]
        [Tooltip("ヒットポイント")]
        public float hp;
        [Tooltip("ヒットポイントの最大数")]
        public float hpMax;
        [Tooltip("体重"), Range(0.0f, 3.0f)]
        public float bodyWeight;
        [Tooltip("ドロップ設定")]
        public DropSetting dropSetting;

        [Tooltip("攻撃力")]
        public float playerDamage;
        [Tooltip("攻撃インターバル")]
        public float attackCoolTime;
        [Tooltip("近距離攻撃のエフェクトを消す時間")]
        public float attackEfectTime;
        [Tooltip("近距離攻撃のエフェクトサイズ")]
        public Vector3 attackEfectSize;
        [Tooltip("遠距離攻撃を可能にするか")]
        public bool longRangeAttack;
        [Tooltip("定期的な遠距離攻撃をする時間")]
        public float regularlyLongRangeAttackTimer;
        [Tooltip("遠距離攻撃のエフェクトを消す時間")]
        public float longAttackEfectTime;
        [Tooltip("遠距離攻撃のエフェクトサイズ")]
        public Vector3 longAttackEfectSize;
        [Tooltip("弾情報")]
        public BulletStatus bulletStatus;
        [Tooltip("遠距離攻撃インターバル")]
        public float longRangeAttackInterval;
        [Tooltip("遠距離攻撃が出る場所の調整")]
        public Vector3 longRangePositionAjust;
        public float longRangePositionAjustMagnification;
        [Tooltip("ブロックに与えるダメージ倍率"),Range(0.0f,1.0f)]
        public float blockDamageMagnification;
        [Header("---------------------------------------")]

        [Tooltip("消えるときの距離")]
        public float disableLength;
        [Tooltip("ダメージテキストの高さの調整")]
        public float damageTextYAjust;
        [Header("------------------EnemyのRayの調整-----------------")]

        [Tooltip("敵の中心の調整")]
        public Vector3 pivotAjust;
        [Tooltip("上の目線のベクトルを求める")]
        public Vector3 upperAngle;
        [Tooltip("上の目線の調整")]
        public Vector3 upperAngleAjust;
        [Tooltip("下の目線の調整")]
        public Vector3 underRayAjust;
        [Tooltip("目線を調整")]
        public Vector3 forwordRayAjust;
        [Tooltip("目線のRayを左右に揺らすときの振れ幅"),Range(0.0f,1.0f)]
        public float swingWidth;

        [Tooltip("出現時にブロックを破壊する範囲")]
        public float firstBreakRange;

        [Tooltip("ディゾルブが開始される時間")]
        public float delayStartDissolveTime;
        [Tooltip("動いているときのモーション")]
        public string moveMostionName;
        [Tooltip("死んだときのモーション")]
        public string downMositionName;
        [Tooltip("攻撃時のモーション")]
        public string attackMositionName;
        [Tooltip("弾に当たったときのモーション")]
        public string hitByBulletMosionName;
        [Tooltip("遠距離攻撃のモーション")]
        public string longRangeAttackMositonName;

        [Tooltip("下に破壊した時のクールタイム")]
        public float bottonBreakInterval;

        [Header("------------------Enemyの物理挙動-----------------")]

        [Tooltip("プログラムでの重力の適応")]
        public bool bDisableGravity;
        [Tooltip("重力")]
        public float gravityStrength;
    }
}