using UnityEngine;
using System;

namespace Syuntoku.Status
{
    public class StatusManage : MonoBehaviour
    {
        [Header("採掘モードの共有パラメータ")]
        public Digmode digmodeStatus = new Digmode();
        [Header("戦闘モードの共有パラメータ")]
        public BattleMode battleModeStatus = new BattleMode();

        /// <summary>
        /// ステータスをすべて初期化
        /// </summary>
        public void FullInitialize()
        {
            battleModeStatus.Initialize();
        }

        public void ZeroInitialize()
        {
            battleModeStatus.ZeroInitialize();
        }

        private void Update()
        {
            digmodeStatus.Update(this);
        }
    }
    [Serializable]
    public class Digmode
    {
        public FirstParsonStatus firstParsonStatus = new FirstParsonStatus();
        public DigStatus digStatus = new DigStatus();
        public PublicStatus publicStatus = new PublicStatus();
        public BallStatus ballStatus = new BallStatus();

        public enum StatusKind
        {
            PlayerSpeedMagnification,
            PlayerSpeedPower,
            PlayerGroundSpeedManification,
            PlayerAirSpeedMagnification,
            PlayerHorizontalSpeedMagnification,
            PlayerVirticalSpeedMagnification,
            PlayerSuitPower,
            DigPowerMagnification,
            DigCooltime,
            AddDropCount,
            ImmediatelyBreakManification,
        }

        public void ConnectStatus(FirstParsonStatus firstParsonStatus)
        {
            this.firstParsonStatus = firstParsonStatus;
        }
        public void ConnectStatus(DigStatus digStatus)
        {
            this.digStatus = digStatus;
        }

        public void Update(StatusManage statusManage)
        {
            publicStatus.Update(statusManage);
        }
        /// <summary>
        /// 対応したアクションでのステータス変更を開始させる
        /// </summary>
        /// <param name="activeTrigger"></param>
        public void SetActionTrigger(StatusManage statusManage, PublicStatus.ActiveTrigger activeTrigger)
        {
            publicStatus.SetActionTrigger(statusManage, activeTrigger);
        }

        /// <summary>
        /// 対応したアクションのステータス変更を無効化する
        /// </summary>
        /// <param name="statusManage"></param>
        /// <param name="activeTrigger"></param>
        public void DisableAction(StatusManage statusManage, PublicStatus.KeyAction activeTrigger)
        {
            publicStatus.DisableAction(statusManage, activeTrigger);
        }

        /// <summary>
        /// プレイヤーのスピード倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddSpeedMagnification(float amount)
        {
            firstParsonStatus.AddSpeedMagnification(amount);
        }

        /// <summary>
        /// プレイヤーの移動量を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddSpeedPower(float amount)
        {
            firstParsonStatus.AddSpeedPower(amount);
        }

        /// <summary>
        /// プレイヤーの地面時の移動倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddGroundSpeedMagnification(float amount)
        {
            firstParsonStatus.AddGroundSpeedMagnification(amount);
        }

        /// <summary>
        /// プレイヤーの空中時の移動倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddAirSpeedMagnification(float amount)
        {
            firstParsonStatus.AddAirSpeedMagnification(amount);

        }

        /// <summary>
        /// プレイヤーの横移動の移動倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddHorizontalSpeedMagnification(float amount)
        {
            firstParsonStatus.AddHorizontalSpeedMagnification(amount);
        }

        /// <summary>
        /// プレイヤーの縦の移動倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddVirticalSpeedMagnification(float amount)
        {
            firstParsonStatus.AddVirticalSpeedMagnification(amount);
        }
        /// <summary>
        /// プレイヤーの縦の移動倍率を変更する
        /// </summary>
        /// <param name="amount"></param>
        public void AddSuitPowerMagnification(float amount)
        {
            firstParsonStatus.AddSuitPowerMagnification(amount);
        }

        /// <summary>
        /// 掘るダメージ
        /// </summary>
        public void AddDigDamagePlibility(float amount)
        {
            digStatus.AddDamagePlibility(amount);
        }

        /// <summary>
        /// 追加ドロップの確率
        /// </summary>
        /// <param name="amount"></param>
        public void AddDropPlibility(float amount)
        {
            digStatus.AddDropPlibility(amount);
        }

        public void ReductionToolInvervalMagnification(float amount)
        {
            digStatus.ReductionToolInvervalMagnification(amount);
        }

        /// <summary>
        /// 鉱石のドロップ量
        /// </summary>
        public void AddDropCount(int amount)
        {
            digStatus.AddDropCount(amount);
        }

        /// <summary>
        /// 即破壊確率
        /// </summary>
        /// <param name="amount"></param>
        public void AddImmediatelyBreakManification(int amount)
        {
            digStatus.AddImmediatelyBreakManification(amount);
        }

        /// <summary>
        /// 効果を選んでステータスを変更できる
        /// </summary>
        /// <param name="statusKind"></param>
        /// <param name="amount"></param>
        public void KindToAddStatus(StatusKind statusKind, float amount)
        {
            switch (statusKind)
            {
                case StatusKind.PlayerSpeedMagnification:
                    firstParsonStatus.AddSpeedMagnification(amount);
                    break;
                case StatusKind.PlayerSpeedPower:
                    firstParsonStatus.AddSpeedPower(amount);
                    break;
                case StatusKind.PlayerGroundSpeedManification:
                    firstParsonStatus.AddGroundSpeedMagnification(amount);
                    break;
                case StatusKind.PlayerAirSpeedMagnification:
                    firstParsonStatus.AddAirSpeedMagnification(amount);
                    break;
                case StatusKind.PlayerHorizontalSpeedMagnification:
                    firstParsonStatus.AddHorizontalSpeedMagnification(amount);
                    break;
                case StatusKind.PlayerVirticalSpeedMagnification:
                    firstParsonStatus.AddVirticalSpeedMagnification(amount);
                    break;
                case StatusKind.PlayerSuitPower:
                    firstParsonStatus.AddSuitPowerMagnification(amount);
                    break;
                case StatusKind.DigPowerMagnification:
                    digStatus.AddDamagePlibility(amount);
                    break;
                case StatusKind.AddDropCount:
                    digStatus.AddDropCount((int)amount);
                    break;
                case StatusKind.ImmediatelyBreakManification:
                    digStatus.AddImmediatelyBreakManification((int)amount);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 秒数で効果がなくなるステータス変更のアクションを追加
        /// </summary>
        /// <param name="activeTrigger">アクションを開始するトリガーを設定</param>
        /// <param name="statusKind">変更するステータス</param>
        /// <param name="amount">ステータスの変化量</param>
        /// <param name="enebleTime">アクティブ時間</param>
        /// <param name="bTemporary">一時的に存在する</param>
        /// <param name="inactive">効果が消えないか</param>
        public void AddTimerChangeAction(PublicStatus.ActiveTrigger activeTrigger, StatusKind statusKind, float amount, float enebleTime, bool bTemporary = false, bool inactive = false)
        {
            publicStatus.AddTimerChangeAction(activeTrigger, statusKind, amount, enebleTime, bTemporary, inactive);
        }

        /// <summary>
        /// 一時的に保存しているアクションを削除
        /// </summary>
        public void RemoveTemporaryAction()
        {
            publicStatus.RemoveTenporaryAction();
        }

        public static Digmode operator +(Digmode mainData, Digmode addData)
        {
            if (addData == null) return mainData;

            mainData.firstParsonStatus += addData.firstParsonStatus;
            return mainData;
        }
    }

    [Serializable]
    public class BattleMode
    {
        public PlayerStatus playerStatus;
        public void ConnectStatus(PlayerStatus playerStatus)
        {
            this.playerStatus = playerStatus;
        }

        public void Initialize()
        {
            playerStatus = PlayerStatus.DefaultPlayerStatus();
            playerStatus.Initialize();
        }

        public void AddReloadSpeedMagnification(float amount) => playerStatus.ReloadSpeedMagnification += amount;
        public float AttackPowerMagnification { get => playerStatus.AttackPowerMagnification; set => playerStatus.AttackPowerMagnification = value; }

        public float ExplosionDamageMagnification { get => playerStatus.ExplosionDamageMagnification; set => playerStatus.ExplosionDamageMagnification = value; }
        public float ExplosionRangeMagnification { get => playerStatus.ExplosionRangeMagnification; set => playerStatus.ExplosionRangeMagnification = value; }
        public float OccurrenceBaseExplosionDamage { get => playerStatus.OccurrenceBaseExplosionDamage; set => playerStatus.OccurrenceBaseExplosionDamage = value; }
        public float BaseExplosionDamage { get => playerStatus.BaseExplosionDamage; set => playerStatus.BaseExplosionDamage = value; }
        public bool bBaseDamageExplosion { get => playerStatus.BBaseDamageExplosion; set => playerStatus.BBaseDamageExplosion = value; }
        public bool bShotExplosionAttack { get => playerStatus.BShotExplosionAttack; set => playerStatus.BShotExplosionAttack = value; }
        public float ShotExplosionDamage { get => playerStatus.ShotExplosionDamage; set => playerStatus.ShotExplosionDamage = value; }
        public float OccurrenceShotExplosionDamage { get => playerStatus.OccurrenceShotExplosionDamage; set => playerStatus.OccurrenceShotExplosionDamage = value; }
        public float HealPowerMagnification { get => playerStatus.HealPowerMagnification; set => playerStatus.HealPowerMagnification = value; }

        /// <summary>
        /// 数値をすべて初期値にする
        /// </summary>
        public void ZeroInitialize()
        {
            playerStatus.ZeroInitialize();
        }

        //public static BattleMode operator +(BattleMode mainData, BattleMode addData)
        //{

        //    if (addData == null) return mainData;

        //    mainData.BaseHp += addData.BaseHp;
        //    mainData.DefencePower += addData.DefencePower;
        //    mainData.HealPower += addData.HealPower;
        //    mainData.HealPowerMagnification += addData.HealPowerMagnification;
        //    mainData.AttackPower += addData.AttackPower;
        //    mainData.AttackPowerMagnification += addData.AttackPowerMagnification;
        //    mainData.AttackRange += addData.AttackRange;
        //    mainData.AttackSpeedMagnification += addData.AttackSpeedMagnification;
        //    mainData.CriticalMagnification += addData.CriticalMagnification;
        //    mainData.ReloadSpeedMagnification = addData.ReloadSpeedMagnification;
        //    mainData.FirstEnemyDamageMagnification = addData.FirstEnemyDamageMagnification;
        //    mainData.OccurrenceBaseExplosionDamage = addData.OccurrenceBaseExplosionDamage;
        //    mainData.ExplosionDamageMagnification = addData.ExplosionDamageMagnification;
        //    mainData.ExplosionRangeMagnification = addData.ExplosionRangeMagnification;
        //    mainData.ShotExplosionDamage = addData.ShotExplosionDamage;

        //    return mainData;
        //}
    }
}

