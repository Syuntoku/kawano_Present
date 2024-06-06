using UnityEngine;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;
using Cysharp.Threading.Tasks;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// 親クラス
    /// 敵のベース処理
    /// </summary>
    public class EnemyBase : MonoBehaviour
    {
        [Header("------------------デバッグ確認用-----------------")]
        #region Variable
        protected GameObject _playerTrans;
        protected Rigidbody _rigidbody;
        protected EnemyStatus _enemyStatus;
        protected DamageText _damageText;
        protected DropManager _dropManager;
        protected EnemyGenerator _enemyGenerater;
        protected BlockManage _blockManage;
        protected EnemyBulletManager _enemyBulletManager;
        [SerializeField] EnemyScriptable enemyScriptable;

        [SerializeField] bool _debugMode;

        #region MoveVariable
        protected Ray _worldUpperRay;
        protected Ray _worldUnderRay;
        protected Ray _ray;

        float _stateChangeTimer;
        [SerializeField] Vector3 _gravityVelosity;

        protected bool _bJump;
        protected bool _bStop;
        protected bool _bGround;
        protected bool _bDownEnemy;
        protected bool _bFirstDownMosion;
        protected float _downCount;
        protected float _dissolveCount;

        protected float _swingTime;
        protected float _swingAmount;
        WaveEnemyChangeStatus _waveEnemyStatus;

        protected Material _dissolve;
        protected Animator _animation;
        protected bool _bAttack;
        protected float _bAttackTimer;
        float _staytimer;
        float _endStay;
        Vector3 _holdPosition;
        float _stackTime;
        float _regularlyLongRangeAttackTime;

        public static string ENEMY_TAG = "Enemy";
        readonly protected static float DISOLVE_POWER = 0.5f;
        readonly protected static float UPPERRAYLENGTH = 0.8f; 　　　　 //上のRayが反応できる最高距離
        readonly protected static float UNDERRAYLENGTH = 2.0f;　　　　　//下のRayが反応できる最高距離
        readonly protected static float UNDERBREAKRAYLENGTH = 0.5f;　　 //下のRayでの破壊が可能になる距離

        readonly protected static float STACK_TIMER = 5.0f;　　　　　　 //敵がスタックした場合にリスポーンする時間
        readonly protected static int   EXTINCTION_TIMER = 3;　　　　　 //敵が死亡した際にオブジェクトが消える遅延時間
        readonly protected static float SWINGROTATIONAMOUNT = 1.0f * Mathf.Rad2Deg;　//敵が目の前のRayをスイングする際の角度（sin）
        readonly protected static float MAXROTATE = 360.0f;             //sin角度のリセットに使う

        //敵が攻撃する際に前のブロックが邪魔な時に破壊するためのRAYの球体のRAYの大きさ
        readonly protected static float CANT_PLAYER_ATTACK_BREAK_BLOCK_RAY_RANGE = 0.5f;
        //敵が攻撃する際に前のブロックが邪魔な時に破壊するためのRAYの長さ
        readonly protected static float CANT_PLAYER_ATTACK_BREAK_BLOCK_RAY_LENGTH = 0.1f;

        protected LayerMask _blockMask;

        public GameObject _attackBoneEfect;
        float _efectime;
        float _nextDeliteEfectTime;


#if UNITY_EDITOR
        readonly static float DEBUG_DAMAGE = 5;
        readonly static float DEBUG_BREAK_WAIT_TIME = 1.0f;
        public int bindId;
#endif
        [SerializeField,Tooltip("倒されたときにすぐに消えるオブジェクト")] GameObject[] downEnemyObjectDissolve;
        #endregion

        #region StatusVariable
        [Header("------------------ゲームの変更適応後ステータス *HP変更はEnemySettingでお願いします。-----------------")]
        [Tooltip("ヒットポイント")]
        public float _hp;
        [Tooltip("ヒットポイントの最大数")]
        public float _hpMax;

        #endregion
        EnemyState _endStopToDefaultChangeState;
        [SerializeField]protected EnemyState _enemyMoveState;
        protected enum EnemyState
        {
            TRACK_PLAYER = 0x01,
            STOP = 0x02,
            FORWARD_BREAK = 0x04,
            JUMP_UP = 0x08,
            BOTTON_BREAK = 0x10,
            ATTACK = 0x20,
            SPECIFIC_BEHAVIOR_1 = 0x40, //継承先の敵の固有行動１
            SPECIFIC_BEHAVIOR_2 = 0x80,　//継承先の敵の固有行動１
        }

        protected enum HieralkeyState
        {
            FORWORD = -1,
            JUMP = 3,
            DOWN_BREAK = -2,
        }

        #endregion

        //=======================================
        //Unity
        //=======================================
        private void Start()
        {
            Initialize();
        }

        protected void FixedUpdate()
        {
            ThreadTransform threadTransform = new ThreadTransform(transform,_playerTrans.transform.position);
            //下の空間把握
            GroundUpdate(threadTransform);


            //ストップ中の処理
            if (StateCheck(EnemyState.STOP))
            {
                StayMoveUpdate(threadTransform);
                return;
            }

            //死亡時の処理
            if (_bDownEnemy)
            {
                EnemyDownMove(threadTransform);
                return;
            }
            Vector3 workVelosity = _rigidbody.velocity;
            StartRayHitUpdate(threadTransform);

            AddGravityTransform(ref workVelosity);

            EfectUpdate(threadTransform);
            //スタック状態をチェック
            StackCheck(threadTransform);
            //Rayの更新
            RayUpdate(threadTransform);
            //現在のステートから自身の行動を実行
            MovementFromEnemyState(threadTransform,ref workVelosity);
            //一定時間ごとに敵の動きを変更
            ChangeActionState(threadTransform);

            _rigidbody.velocity = workVelosity;
            transform.position = threadTransform.position;
            transform.rotation = threadTransform.quaternion;

            StateFromChangeAnimation();
        }
        //=======================================
        //Initialize
        //=======================================
        protected void Initialize()
        {
            _dissolve = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
            _animation = GetComponent<Animator>();
            _blockMask = LayerMask.GetMask("Block");
            _enemyBulletManager = GameObject.Find("EnemyBulletManager").GetComponent<EnemyBulletManager>();
            SetEndStopToTransitionState(EnemyState.TRACK_PLAYER);
#if UNITY_EDITOR
            if (_debugMode)
            {
                _enemyStatus = enemyScriptable.GetEnemySetting(bindId);
                _ray = new Ray();
                _worldUpperRay = new Ray(transform.position + _enemyStatus.pivotAjust, (Vector3.up + transform.forward).normalized);
                _rigidbody = GetComponent<Rigidbody>();
                _playerTrans = GameObject.Find("Player");
                _bGround = true;
            }
            else
            {
                _enemyGenerater = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
                _dropManager = GameObject.Find("DropManager").GetComponent<DropManager>();
                _blockManage = GameObject.Find("BlockManage").GetComponent<BlockManage>();
            }
#else
                _enemyGenerater = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
                _dropManager = GameObject.Find("DropManager").GetComponent<DropManager>();
                _blockManage = GameObject.Find("BlockManage").GetComponent<BlockManage>();
#endif

            _attackBoneEfect.SetActive(false);
            _attackBoneEfect.transform.localScale = _enemyStatus.attackEfectSize;
        }

        public void Initialize(GameObject playerObject, WaveEnemyChangeStatus waveEnemyChangeStatus, EnemyStatus enemyStatus, DamageText damageText)
        {
            _ray = new Ray();
            _enemyStatus = enemyStatus;
            _worldUpperRay = new Ray(transform.position + _enemyStatus.pivotAjust, (Vector3.up + transform.forward).normalized);
            Initialize();
            _rigidbody = GetComponent<Rigidbody>();
            _playerTrans = playerObject;
            _bGround = true;
            _damageText = damageText;

            _hp = _enemyStatus.hp * waveEnemyChangeStatus.hpPribability;
            _hpMax = _hp;
            _waveEnemyStatus = waveEnemyChangeStatus;

            RangeDestruction(new ThreadTransform(transform, _playerTrans.transform.position),_enemyStatus.firstBreakRange);
        }

        //=======================================
        //vertual
        //=======================================

        /// <summary>
        /// ステートから敵を動かす
        /// </summary>
        public virtual void MovementFromEnemyState(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            float toPlayer = DifferenceToPlayerY(transform);

            switch (_enemyMoveState)
            {
                //プレイヤーに追従する
                case EnemyState.TRACK_PLAYER:
                    TrackPlayer(transform,ref rdVelosity);
                    break;
                //ブロックを壊しながら前へ進む
                case EnemyState.FORWARD_BREAK:
                    ForwardBreak(transform,ref rdVelosity);
                    break;
                //ジャンプしながら階段状に上がる
                case EnemyState.JUMP_UP:
                    JumpUp(transform,toPlayer, ref rdVelosity);
                    break;
                //下にブロックを壊しながら下に進む
                case EnemyState.BOTTON_BREAK:
                    Botton(transform, toPlayer, ref rdVelosity);
                    break;
                //攻撃モーション
                case EnemyState.ATTACK:
                    AttackPlayer(transform);
                    break;
                //継承用特殊行動１
                case EnemyState.SPECIFIC_BEHAVIOR_1:
                    SpecificBehavior_1(transform, toPlayer,ref rdVelosity);
                    break;
                //継承用特殊行動２
                case EnemyState.SPECIFIC_BEHAVIOR_2:
                    SpecificBehavior_2(transform, toPlayer, ref rdVelosity);
                    break;
            }
        }

        /// <summary>
        /// 一定時間スタックした時に呼ばれる
        /// </summary>
        protected void Resporn()
        {
#if UNITY_EDITOR
            if (_debugMode)
            {
                _stackTime = 0.0f;
                return;
            }
#endif
            Vector3 position = transform.position;
            _enemyGenerater.ReSpornEnemy(out position);
            transform.position = position;
            _stackTime = 0.0f;

        }

        /// <summary>
        /// 敵が倒されたときに呼ばれる
        /// </summary>
        protected virtual void EnemyDownMove(ThreadTransform transform)
        {
            //重力と当たり判定をしなくする
            GroundUpdate(transform);
            if (_bGround)
            {
                if (!_bFirstDownMosion)
                {
                    Destroy(GetComponent<Rigidbody>());
                    Destroy(GetComponent<Collider>());
                    _bFirstDownMosion = true;
                }
            }

            _bGround = false;
            _downCount += transform.deltaTime;
            if (_downCount >= _enemyStatus.delayStartDissolveTime)
            {
                if (_dissolve == null) return;
                //ディゾルブさせる
                if (_downCount >= _dissolveCount)
                {
                    _dissolveCount += transform.deltaTime / DISOLVE_POWER;
                    _dissolve.SetFloat("_ClipTime", _dissolveCount);
                    Color color = Color.white;
                    //最終調整α値を調整する
                    color.a = 1.0f - _downCount - _enemyStatus.delayStartDissolveTime;
                    foreach (GameObject item in downEnemyObjectDissolve)
                    {
                        item.SetActive(false);
                    }
                }
            }

            if (_downCount >= EXTINCTION_TIMER)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 敵のステートを変更
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void ChangeState(ThreadTransform transform, float toPlayer)
        {
#if UNITY_EDITOR
            if (_debugMode)
            {
                Debug.Log("プレイヤーの距離" + toPlayer);
            }
#endif
            
            if(DifferenceToPlayerHorizontalSqrMag(transform) <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength)
            {
                _enemyMoveState = EnemyState.ATTACK;
                _stateChangeTimer = 0.0f;
                return;
            }

            if(toPlayer == 0 || toPlayer == (int)HieralkeyState.FORWORD)
            {
                _enemyMoveState = EnemyState.FORWARD_BREAK;
                _stateChangeTimer = 0.0f;
                return;
            }

                //プレイヤーが上にいるとき
            if (toPlayer > (int)HieralkeyState.DOWN_BREAK)
            {
                _enemyMoveState = EnemyState.JUMP_UP;
                _stateChangeTimer = 0.0f;
                return;
            }

            //下にいるとき
            if (toPlayer <= (int)HieralkeyState.DOWN_BREAK)
            {
                _enemyMoveState = EnemyState.BOTTON_BREAK;
            }

            _stateChangeTimer = 0.0f;
        }

        /// <summary>
        /// プレイヤーに追従する
        /// </summary>
        protected void TrackPlayer(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            if (DifferenceToPlayer(transform) <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength) return;

            float jumpAddSpeed = 1.0f;

            //ジャンプ中で動きを加速する
            if (!IsGround())
            {
                jumpAddSpeed += _enemyStatus.jumpSpeedupMagnification;
            }

            Vector3 work = transform.forward * _enemyStatus.speed * jumpAddSpeed;
            work.y = 0.0f;
            rdVelosity = rdVelosity + work;

            Vector3 toPlayer = transform.playerPosition - GetPivot(transform);
            toPlayer.y = 0.0f;
            LookPlayer(transform, toPlayer);
        }

        /// <summary>
        /// 前に進みながら目の前のブロックを壊す
        /// </summary>
        protected virtual void ForwardBreak(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            if (DifferenceToPlayer(transform) <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength) return;
            if (!BlockManage.IsBlock(transform.forwardHit))
            {
                TrackPlayer(transform, ref rdVelosity);
            }
            SetBlockBreak(transform.forwardHit);
            SetBlockBreak(transform.upRayHit);
        }
        /// <summary>
        /// ジャンプしながら上へ移動する
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void JumpUp(ThreadTransform transform,float toPlayer,ref Vector3 rdVelosity)
        {
            if (DifferenceToPlayer(transform) <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength) return;

            if (!BlockManage.IsBlock(transform.forwardHit))
            {
                TrackPlayer(transform,ref rdVelosity);
                return;
            }

            SetBlockBreak(transform.upRayHit);

            if (IsGround())
            {
                Jump();
            }
        }

        /// <summary>
        /// 下に掘る
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void Botton(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {
            //プレイヤーより下にいる場合は前に進む
            if (toPlayer >= 0)
            {
                _enemyMoveState = EnemyState.FORWARD_BREAK;
                return;
            }

            if (transform.downRayHit.collider != null && transform.downRayHit.distance <= UNDERBREAKRAYLENGTH)
            {
                SetBlockBreak(transform.downRayHit, _enemyStatus.bottonBreakInterval);
            }
        }

        /// <summary>
        /// 特別な行動１
        /// 継承時に敵固有の動きがあればつかう
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void SpecificBehavior_1(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {

        }

        /// <summary>
        /// 特別な行動２
        /// 継承時に敵固有の動きがあればつかう
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void SpecificBehavior_2(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {

        }

        /// <summary>
        /// プレイヤーにダメージを与える時に呼ばれる
        /// </summary>
        protected void SendPlayerDamage(ThreadTransform transform, float toPlayerLengthY)
        {
            _bAttack = true;

            if (_enemyStatus.longRangeAttack)
            {
                //遠距離攻撃を行う
                if (toPlayerLengthY >= _enemyStatus.longRangeAttackLength)
                {
                    LongRangeAttack(transform);
                    return;
                }
            }
            MeleeAttack();
        }

        protected void MeleeAttack()
        {
            
            _animation.SetTrigger(_enemyStatus.attackMositionName);
            _attackBoneEfect.SetActive(true);
            _nextDeliteEfectTime = _enemyStatus.longAttackEfectTime;
            StayEnemy(_enemyStatus.attackCoolTime);
            if (_debugMode) return;
            _playerTrans.GetComponent<Player.Player>().SendDamage(_enemyStatus.playerDamage * GetWaveAttackPowerMagnification(), transform);
        }

        protected void LongRangeAttack(ThreadTransform transform)
        {
            Vector3 startPos = (GetPivot(transform) + _enemyStatus.pivotAjust + _enemyStatus.longRangePositionAjust) + transform.forward * _enemyStatus.longRangePositionAjustMagnification;
            _enemyBulletManager.InstanceBullet(new BulletStatus(_enemyStatus.bulletStatus), startPos, (transform.playerPosition - startPos).normalized);
            StayEnemy(_enemyStatus.longRangeAttackInterval);
            _animation.SetTrigger(_enemyStatus.longRangeAttackMositonName);
            _nextDeliteEfectTime = _enemyStatus.attackEfectTime;
        }

        /// <summary>
        /// 周りのブロックを破壊する（SphereRay）
        /// </summary>
        /// <param name="range">球体範囲</param>
        /// <param name="raylength">RAYの長さ</param>
        /// <param name="bBreakUnder">自分より下を壊さない</param>
        protected void RangeDestruction(ThreadTransform transform ,float range = 0.5f,float raylength = 0.01f,bool bBreakUnder = true)
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetPivot(transform) + _enemyStatus.tall, range, (Vector3.up + transform.forward).normalized * raylength, _blockMask) ;

            foreach (RaycastHit item in hits)
            {
                if (!BlockManage.IsBlock(item)) continue;
                if (item.collider == null) continue;

                if (!bBreakUnder)
                {
                    float height = item.collider.transform.position.y - GetPivot(transform).y;
                    if (height < 0) continue;
                } 
                SetBlockBreak(item);
            }
        }

        /// <summary>
        /// 動きを停止する
        /// </summary>
        /// <param name="time"></param>
        protected void StayEnemy(float time)
        {
            _enemyMoveState = EnemyState.STOP;
            _endStay = time;

            if(_enemyStatus.moveMostionName != "")
            {
                _animation.SetBool(_enemyStatus.moveMostionName, false);
            }
        }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockBackPower"></param>
        /// <param name="addKnockBackVec"></param>
        public void SendDamage(float damage, float knockBackPower, Vector3 addKnockBackVec)
        {;
            if (_bDownEnemy) return;
            _hp -= damage;

            //ノックバックを計算
            Vector3 work = addKnockBackVec;
            work += Vector3.up;

            float knokBack = knockBackPower - _enemyStatus.bodyWeight;
            if (knockBackPower < 0) knokBack = 0;
            work *= knokBack;
            _rigidbody.AddForce(work);

            if (!_debugMode)
            {
                _damageText.InstanceDamageText(GetPivot(new ThreadTransform(transform, _playerTrans.transform.position)), damage, yAjust: _enemyStatus.damageTextYAjust);
            }
            _animation.SetTrigger(_enemyStatus.hitByBulletMosionName);

            if (_hp <= 0)
            {
                if (_enemyStatus.downMositionName != "")
                {
                    _animation.SetTrigger(_enemyStatus.downMositionName);
                }

                if (!_debugMode)
                {
                    _dropManager.InstanceJuwelry(_enemyStatus.dropSetting, GetPivot(new ThreadTransform(transform, _playerTrans.transform.position)));
                }

                _bDownEnemy = true;
                DownEnemy();
                _rigidbody.useGravity = true;
            }
        }

        //=======================================
        //private
        //=======================================
        protected async void AttackPlayer(ThreadTransform transform)
        {
            await UniTask.SwitchToMainThread();
            Vector3 toPlayerDir = _playerTrans.transform.position- (GetPivot(transform) + _enemyStatus.forwordRayAjust);
            float toPlayerLength = toPlayerDir.magnitude;

            RaycastHit hit;
#if UNITY_EDITOR
            Debug.DrawRay(GetPivot(transform) + _enemyStatus.forwordRayAjust + _enemyStatus.tall, toPlayerDir,Color.gray);
#endif
            if(Physics.Raycast(GetPivot(transform) + _enemyStatus.forwordRayAjust + _enemyStatus.tall, toPlayerDir.normalized,out hit,toPlayerLength))
            {
                if (BlockManage.IsBlock(hit))
                {
                    RangeDestruction(transform, CANT_PLAYER_ATTACK_BREAK_BLOCK_RAY_RANGE, CANT_PLAYER_ATTACK_BREAK_BLOCK_RAY_LENGTH,bBreakUnder:false);
                    return;
                }
            }

            SendPlayerDamage(transform,DifferenceToPlayerY(transform));

            await UniTask.SwitchToThreadPool();
        }

        /// <summary>
        /// ステート更新時間を計算
        /// </summary>
        void ChangeActionState(ThreadTransform transform)
        {
            if (StateCheck(EnemyState.STOP)) return;
            _stateChangeTimer += transform.deltaTime;
            _regularlyLongRangeAttackTime += transform.deltaTime;

            //一定時間ごとにアクションを変更する
            if (_stateChangeTimer >= _enemyStatus.stateChageTime)
            {
                float toPlayer = DifferenceToPlayerY(transform);
                ChangeState(transform, toPlayer);
            }

            //定期的に遠距離攻撃をする
            if (_regularlyLongRangeAttackTime >= _enemyStatus.regularlyLongRangeAttackTimer)
            {
                _bAttack = true;
                LongRangeAttack(transform);
                _regularlyLongRangeAttackTime = 0.0f;
            }
        }

        /// <summary>
        /// 身動きが撮れているかのチェック
        /// </summary>
        void StackCheck(ThreadTransform transform)
        {
            if (_holdPosition == GetPivot(transform))
            {
                _stackTime += transform.deltaTime;

                if (_stackTime >= STACK_TIMER)
                {
                    Resporn();
                }
            }
            else
            {
                _holdPosition = GetPivot(transform);
                _stackTime = 0.0f;
            }
        }

        /// <summary>
        /// 攻撃エフェクトの時間設定
        /// </summary>
        void EfectUpdate(ThreadTransform transform)
        {
            if (!_bAttack) return;
            _efectime += transform.deltaTime;

            if(_efectime >= _nextDeliteEfectTime)
            {
                _attackBoneEfect.SetActive(false);
                _efectime = 0.0f;
            }
        }

        //=======================================================
        //動き
        //=======================================================

        //Rayの更新
        void RayUpdate(ThreadTransform transform)
        {
            Quaternion workRotate = transform.quaternion;

            //前方向のRay
            _bStop = false;

            Vector3 work;
           
            work = GetPivot(transform);
            _ray.origin = work + (workRotate * _enemyStatus.forwordRayAjust);
            work = transform.forward;
            //work = workRotate * Vector3.forward;
            work.z += _swingAmount;
            _ray.direction = work;

            //上方向のRay
            work = GetPivot(transform);
            _worldUpperRay.origin = work + (workRotate * _enemyStatus.upperAngleAjust);
            work = workRotate * _enemyStatus.upperAngle;
            _worldUpperRay.direction = work;

            //下方向のRay
            work = GetPivot(transform);
            _worldUnderRay.origin = work + _enemyStatus.underRayAjust;

            work = (workRotate * Vector3.down).normalized;
            _worldUnderRay.direction = work;
#if UNITY_EDITOR
            if (_debugMode)
            {
                Debug.DrawRay(_ray.origin, _ray.direction * _enemyStatus.forwordRayLength, Color.red);
                Debug.DrawRay(_worldUpperRay.origin, _worldUpperRay.direction * UPPERRAYLENGTH, Color.blue);
                Debug.DrawRay(_worldUnderRay.origin, _worldUnderRay.direction * UNDERRAYLENGTH, Color.yellow);
            }
#endif
            RaySwingUpdate(transform);
        }

        /// <summary>
        /// Rayを左右に動かす
        /// </summary>
        void RaySwingUpdate(ThreadTransform transform)
        {
            _swingTime += SWINGROTATIONAMOUNT * transform.deltaTime;
            _swingAmount = Mathf.Sin(_swingTime) * _enemyStatus.swingWidth;
            if (_swingTime >= MAXROTATE) { _swingTime = 0.0f; }
        }

        //プレイヤーの方向を向く
        protected void LookPlayer(ThreadTransform transform, Vector3 length)
        {
            transform.quaternion = Quaternion.LookRotation(length, Vector3.up);
        }

        void AddGravityTransform(ref Vector3 velosity)
        {
            if (_enemyStatus.bDisableGravity) return;
            if (_rigidbody == null) return;
            if(!IsGround())
            {        
                _gravityVelosity.y = velosity.y -_enemyStatus.gravityStrength;
            }
            velosity = _gravityVelosity;
        }

        protected void Jump()
        {
            if(_rigidbody == null) return;
            _rigidbody.velocity += Vector3.up * _enemyStatus.jumpPower;
        }

        //=================================
        //周りの状況を取得する機能
        //=================================
         protected virtual RaycastHit LineOfSight()
        {
            RaycastHit hit;
            Physics.Raycast(_ray, out hit, _enemyStatus.forwordRayLength, _enemyStatus.layerMask);
            return hit;
        }

        protected virtual RaycastHit CheckUpperBlock()
        {
            RaycastHit hitObj;
            Physics.Raycast(_worldUpperRay, out hitObj, UPPERRAYLENGTH, _enemyStatus.layerMask);
            return hitObj;
        }

        protected virtual RaycastHit UnderRayCheck()
        {
            RaycastHit hit;
            Physics.Raycast(_worldUnderRay, out hit, UNDERRAYLENGTH, _enemyStatus.layerMask);
            return hit;
        }

        /// <summary>
        /// 地面についているかのチェック
        /// </summary>
        void GroundUpdate(ThreadTransform transform)
        {
            if (transform.downRayHit.collider == null)
            {
                _bGround = false;
                return;
            }

            if (transform.downRayHit.distance <= _enemyStatus.groundStandLength)
            {
                _bGround = true;
                _gravityVelosity = Vector3.zero;
                return;
            }
            
        }

        /// <summary>
        /// プレイヤーとの距離を求める
        /// </summary>
        /// <returns></returns>
        protected float DifferenceToPlayer(ThreadTransform transform)
        {
            return (transform.playerPosition - GetPivot(transform)).sqrMagnitude;
        }

        /// <summary>
        /// プレイヤーとのY座標の距離を求める
        /// </summary>
        /// <returns>細かい距離は切り捨て</returns>
        protected float DifferenceToPlayerY(ThreadTransform transform)
        {
            return transform.playerPosition.y - GetPivot(transform).y;
        }
        /// <summary>
        /// プレイヤーとの平面の距離を求める
        /// </summary>
        /// <returns>細かい距離は切り捨て</returns>
        protected float DifferenceToPlayerHorizontalSqrMag(ThreadTransform transform)
        {
            Vector3 length = transform.playerPosition - GetPivot(transform);
            length.y = 0;
            return length.sqrMagnitude;
        }

        /// <summary>
        /// ウェーブに設定された攻撃倍率
        /// </summary>
        /// <returns></returns>
        protected float GetWaveAttackPowerMagnification()
        {
            return _waveEnemyStatus.attackPribability;
        }

        public void StateFromChangeAnimation()
        {
            switch(_enemyMoveState)
            {
                case EnemyState.TRACK_PLAYER:
                    //アニメーションを動かす
                    if (_enemyStatus.moveMostionName != "")
                    {
                        if (_animation.GetBool(_enemyStatus.moveMostionName)) return;
                        _animation.SetBool(_enemyStatus.moveMostionName, true);
                    }
                    break;
                case EnemyState.FORWARD_BREAK:
                    //アニメーションを動かす
                    if (_enemyStatus.moveMostionName != "")
                    {
                        if (_animation.GetBool(_enemyStatus.moveMostionName)) return;
                        _animation.SetBool(_enemyStatus.moveMostionName, true);
                    }
                    break;
            }
        }

        //=======================================================
        //破壊関連
        //=======================================================

        /// <summary>
        /// ブロックにダメージを与える
        /// </summary>
        protected void SetBlockBreak(RaycastHit hitObject)
        {
            if (hitObject.collider == null) return;
            if (!BlockManage.IsBlock(hitObject)) return;
#if UNITY_EDITOR
            if (_debugMode)
            {
                DamageManager damageManager = new DamageManager();
                damageManager.damage = DEBUG_DAMAGE;
                hitObject.collider.GetComponent<BlockData>().DebugBreak();

                return;
            }
#endif
            BlockData blockData = hitObject.collider.GetComponent<BlockData>();
            if (blockData == null) return;

            _blockManage.SendFixedMagnificationDamage(blockData, _enemyStatus.blockDamageMagnification);
        }
        /// <summary>
        /// ブロックにダメージを与える
        /// </summary>
        protected void SetBlockBreak(RaycastHit[] hitObject)
        {
            foreach (RaycastHit hit in hitObject)
            {
                if (hit.collider == null) return;
                if (!BlockManage.IsBlock(hit)) return;
#if UNITY_EDITOR
                if (_debugMode)
                {
                    DamageManager damageManager = new DamageManager();
                    damageManager.damage = DEBUG_DAMAGE;
                    hit.collider.GetComponent<BlockData>().DebugBreak();
                    return;
                }
#endif
                if (hit.collider == null) return;
                BlockData blockData = hit.collider.GetComponent<BlockData>();
                if (blockData == null) return;

                _blockManage.SendFixedMagnificationDamage(blockData, _enemyStatus.blockDamageMagnification);
            }
        }
        /// <summary>
        /// ブロックを壊した後に止まる
        /// </summary>
        protected void SetBlockBreak(RaycastHit hitObject, float bStaytime)
        {
            if (hitObject.collider == null) return;
            if (!BlockManage.IsBlock(hitObject)) return;
#if UNITY_EDITOR
            if (_debugMode)
            {
                DamageManager damageManager = new DamageManager();
                damageManager.damage = DEBUG_DAMAGE;
                hitObject.collider.GetComponent<BlockData>().DebugBreak();
                //デバッグ：破壊後に一秒待つ
                StayEnemy(DEBUG_BREAK_WAIT_TIME);
                return;
            }
#endif
            if (hitObject.collider == null) return;
            BlockData blockData = hitObject.collider.GetComponent<BlockData>();
            if (blockData == null) return;

            _blockManage.SendFixedMagnificationDamage(blockData, _enemyStatus.blockDamageMagnification);

            StayEnemy(bStaytime);
        }
        /// <summary>
        /// ブロックにダメージを与える
        /// </summary>
        protected void SetBlockBreak(RaycastHit[] hitObject, float deleyTime)
        {
            bool bBreak = false;
            foreach (RaycastHit hit in hitObject)
            {
                bBreak = true;
                if (hit.collider == null) continue;
                if (!BlockManage.IsBlock(hit)) continue;
#if UNITY_EDITOR
                if (_debugMode)
                {
                    DamageManager damageManager = new DamageManager();
                    damageManager.damage = DEBUG_DAMAGE;
                    hit.collider.GetComponent<BlockData>().DebugBreak();

                    continue;
                }
#endif
                if (hit.collider == null) continue;
                BlockData blockData = hit.collider.GetComponent<BlockData>();
                if (blockData == null) continue;

                _blockManage.SendFixedMagnificationDamage(blockData, _enemyStatus.blockDamageMagnification);
            }

            if (!bBreak) return;
            StayEnemy(deleyTime);
        }

        public virtual void WorkEnemy(ThreadTransform transform)
        {

        }

        public virtual void DownEnemy()
        {

        }

        public virtual void StartRayHitUpdate(ThreadTransform transform)
        {
            transform.upRayHit = CheckUpperBlock();
            transform.downRayHit = UnderRayCheck();
            transform.forwardHit = LineOfSight();
        }

        //=======================================================
        //フラグチェックやステートを管理する機能
        //=======================================================

        /// <summary>
        /// 地面についているか
        /// </summary>
        /// <returns></returns>
        protected bool IsGround()
        {
            return _bGround;
        }
        /// <summary>
        /// ステート状態をチェック
        /// </summary>
        /// <param name="enemyState"></param>
        /// <returns></returns>
        protected bool StateCheck(EnemyState enemyState)
        {
            return (_enemyMoveState & enemyState) != 0;
        }

        /// <summary>
        /// 行動が終了した時のデフォルトステート状態をセットする
        /// </summary>
        /// <param name="enemyState"></param>
        protected void SetEndStopToTransitionState(EnemyState enemyState)
        {
            _endStopToDefaultChangeState = enemyState;
        }

        /// <summary>
        /// 敵の調整後の中心を取得
        /// </summary>
        protected Vector3 GetPivot(ThreadTransform transform)
        {
            return transform.position + _enemyStatus.pivotAjust;
        }
        /// <summary>
        /// ストップ状態の敵のアップデート
        /// </summary>
        protected void StayMoveUpdate(ThreadTransform transform)
        {
            _staytimer += transform.deltaTime;
            
            if(_staytimer >= _endStay)
            {
                _staytimer = 0.0f;
                _enemyMoveState = _endStopToDefaultChangeState;
            }
        }

        /// <summary>
        /// 別スレッドで使用する作業用データ
        /// </summary>
        public class ThreadTransform
        {
            public Vector3 position;
            public Quaternion quaternion;
            public Vector3 scale;
            public bool active;
            public Vector3 forward;
            public float deltaTime;
            public bool bEfectActive;

            public Vector3 playerPosition;

            public RaycastHit upRayHit;
            public RaycastHit downRayHit;
            public RaycastHit forwardHit;

            public RaycastHit[] upRayHits;
            public RaycastHit[] downRayHits;
            public RaycastHit[] forwardHits;

            public ThreadTransform(Transform transform, Vector3 playerPos)
            {
                position = transform.position;
                quaternion = transform.rotation;
                scale = transform.localScale;
                forward = transform.forward;
                playerPosition  = playerPos;
                active = true;
                deltaTime = Time.deltaTime;

            }
        }
    }
}
