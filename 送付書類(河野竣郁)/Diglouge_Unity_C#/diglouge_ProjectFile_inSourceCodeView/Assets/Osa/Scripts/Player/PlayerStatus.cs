using UnityEngine;
using System;

public enum HealthState { Healthy, Dying, Dead }

[Serializable]
public class PlayerStatus
{
    private const float Dyingthreshold = 0.2f;

    [SerializeField] private int maxHP;
    [SerializeField] private int hp;
    [SerializeField] private int exp;
    [SerializeField] private int _defencePower;
    [SerializeField] private float _healPower;
    [SerializeField] private float healPowerMagnification;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackPowerMagnification;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeedMagnification;
    [SerializeField] private float reloadSpeedMagnification;
    [SerializeField] private float firstEnemyDamageMagnification;
    [SerializeField] private bool bShotExplosionAttack;
    [SerializeField] private float shotExplosionDamage;
    [SerializeField] private float occurrenceShotExplosionDamage;
    [SerializeField] private bool bBaseDamageExplosion;
    [SerializeField] private float occurrenceBaseExplosionDamage;
    [SerializeField] private float baseExplosionDamage;
    [SerializeField] private float explosionDamageMagnification;
    [SerializeField] private float explosionRangeMagnification;
    [SerializeField] private float criticalMagnification;

    public HealthState HPState { get; private set; } = HealthState.Healthy;
    public bool IsAllive() => HPState != HealthState.Dead;

    public int MaxHP { get; set; }
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
            HPState = CheckState(hp);
        }
    }
    public int Exp
    {
        get => exp;
        set => exp = value;
    }
    /// <summary> 
    /// 防御力
    /// </summary> 
    public int DefencePower => _defencePower;
    /// <summary>
    /// ベース回復力
    /// </summary>
    public float HealPower => _healPower;
    /// <summary>
    /// 回復力倍率
    /// </summary>
    public float HealPowerMagnification { get => healPowerMagnification; set => healPowerMagnification = value; }

    /// <summary>
    /// ベースの攻撃力倍率
    /// </summary>
    public float AttackPower { get => attackPower; set => attackPower = value; }
    /// <summary>
    /// 攻撃力倍率
    /// </summary>
    public float AttackPowerMagnification { get => attackPowerMagnification; set => attackPowerMagnification = value; }

    /// <summary>
    /// 攻撃範囲倍率
    /// </summary>
    public float AttackRange { get => attackRange; set => attackRange = value; }
    /// <summary>
    /// 攻撃スピード倍率
    /// </summary>
    public float AttackSpeedMagnification { get => attackSpeedMagnification; set => attackSpeedMagnification = value; }
    /// <summary>
    /// 武器のリロードスピード倍率
    /// </summary>
    public float ReloadSpeedMagnification { get => reloadSpeedMagnification; set => reloadSpeedMagnification = value; }
    /// <summary>
    /// 最初に攻撃した敵の攻撃倍率
    /// </summary>
    public float FirstEnemyDamageMagnification { get => firstEnemyDamageMagnification; set => firstEnemyDamageMagnification = value; }
    /// <summary>
    /// 攻撃したときに爆発するか
    /// </summary>
    public bool BShotExplosionAttack { get => bShotExplosionAttack; set => bShotExplosionAttack = value; }
    /// <summary>
    /// 攻撃の爆発ダメージ
    /// </summary>
    public float ShotExplosionDamage { get => shotExplosionDamage; set => shotExplosionDamage = value; }

    /// <summary>
    /// 爆発の確率
    /// </summary>
    public float OccurrenceShotExplosionDamage { get => occurrenceShotExplosionDamage; set => occurrenceShotExplosionDamage = value; }

    /// <summary>
    /// 拠点がダメージを受けたときに爆発を起こすか
    /// </summary>
    public bool BBaseDamageExplosion { get => bBaseDamageExplosion; set => bBaseDamageExplosion = value; }
    /// <summary>
    /// 拠点爆発時のダメージ
    /// </summary>
    public float OccurrenceBaseExplosionDamage { get => occurrenceBaseExplosionDamage; set => occurrenceBaseExplosionDamage = value; }
    /// <summary>
    /// 爆発ダメージ
    /// </summary>
    public float BaseExplosionDamage { get => baseExplosionDamage; set => baseExplosionDamage = value; }
    /// <summary>
    /// 爆発の攻撃倍率
    /// </summary>
    public float ExplosionDamageMagnification { get => explosionDamageMagnification; set => explosionDamageMagnification = value; }
    /// <summary>
    /// 爆発範囲倍率
    /// </summary>
    public float ExplosionRangeMagnification { get => explosionRangeMagnification; set => explosionRangeMagnification = value; }
    /// <summary>
    /// クリティカル倍率
    /// </summary>
    public float CriticalMagnification { get => criticalMagnification; set => criticalMagnification = value; }

    public PlayerStatus(int maxHP)
    {
        MaxHP = maxHP;
        hp = maxHP;
        exp = 0;
    }

    /// <summary>
    /// 初期ステータスに初期化
    /// </summary>
    public void Initialize()
    {
        hp = 100;
        _defencePower = 0;
        _healPower = 10.0f;
        AttackPower = 1.0f;
        AttackPowerMagnification = 1.0f;
        AttackRange = 1.0f;
        AttackSpeedMagnification = 1.0f;
        ReloadSpeedMagnification = 1.0f;
        FirstEnemyDamageMagnification = 1.0f;
        OccurrenceBaseExplosionDamage = 0.0f;
        ExplosionDamageMagnification = 1.0f;
        ExplosionRangeMagnification = 1.0f;
        ShotExplosionDamage = 0.0f;
        CriticalMagnification = 1.0f;
    }

    /// <summary>
    /// 数値をすべて初期値にする
    /// </summary>
    public void ZeroInitialize()
    {
        hp = 0;
        _defencePower = 0;
        _healPower = 0;
        AttackPower = 0;
        AttackPowerMagnification = 0;
        AttackRange = 0;
        AttackSpeedMagnification = 0;
        ReloadSpeedMagnification = 0.0f;
        CriticalMagnification = 0;
        OccurrenceBaseExplosionDamage = 0.0f;
        ExplosionDamageMagnification = 0.0f;
        ExplosionRangeMagnification = 0.0f;
        ShotExplosionDamage = 0.0f;
    }

    private static HealthState CheckState(int hp)
    {
        var threshold = hp * Dyingthreshold;

        if (hp >= threshold)
            return HealthState.Healthy;
        else if (hp > 0)
            return HealthState.Dying;
        else
            return HealthState.Dead;
    }

    public static PlayerStatus DefaultPlayerStatus()
    {
        var playerStatus = new PlayerStatus(100);
        playerStatus.Initialize();
        return playerStatus;
    }

}