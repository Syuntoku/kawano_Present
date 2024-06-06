using UnityEngine;
using UniRx;
using Battle.Game;
using System;

namespace Battle
{

    public class Player : MonoBehaviour, IDamageApplicable
    {
        #region CreateSingleton
        public static Player Instance;

        private void Awake()
        {
            Instance = this;
        }
        #endregion;

        public PlayerStatus playerStatus;
        public PlayerScriptable playerScriptable;

        public IReadOnlyReactiveProperty<int> CurrentHP => _hp;
        public IReadOnlyReactiveProperty<HealthState> CurrentHealthState => _stateRP;

        public IObservable<Damage> OnDamage => _damageSub;
        private Subject<Damage> _damageSub = new Subject<Damage>();


        public int MaxHP => maxHP;

        private int maxHP = 100;

        private bool _isAllive = true;
        private ReactiveProperty<int> _hp = new ReactiveProperty<int>();
        private ReactiveProperty<HealthState> _stateRP = new ReactiveProperty<HealthState>(HealthState.Healthy);


        public IReadOnlyReactiveProperty<int> CurrentExp => _expRP;
        private ReactiveProperty<int> _expRP = new ReactiveProperty<int>(0);

        public int NeedExp = 5;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            playerStatus = new PlayerStatus(playerScriptable.baseHP);
            _expRP.AddTo(this);
            _damageSub.AddTo(this);
            _stateRP.AddTo(this);
            _hp.AddTo(this);

            _hp.Value = playerStatus.MaxHP;
        }

        public void Init(PlayerScriptable playerScriptable)
        {
            this.playerScriptable = playerScriptable;
            Init();
        }

        public void ApplyDamage(Damage damage)
        {
            _damageSub.OnNext(damage);

            if (!playerStatus.IsAllive()) return;
            playerStatus.HP -= damage.Value;
            _hp.Value = playerStatus.HP;

            _stateRP.Value = playerStatus.HPState;


            if (_hp.Value <= 0) _isAllive = false;
        }

        public void AddExp(int exp)
        {
            playerStatus.Exp += exp;
            _expRP.Value = playerStatus.Exp;
        }
    }

}