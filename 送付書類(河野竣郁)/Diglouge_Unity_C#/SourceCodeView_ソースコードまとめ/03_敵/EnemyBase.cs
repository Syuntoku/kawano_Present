using UnityEngine;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;
using Cysharp.Threading.Tasks;

namespace Syuntoku.DigMode.Enemy
{
    /// <summary>
    /// �e�N���X
    /// �G�̃x�[�X����
    /// </summary>
    public class EnemyBase : MonoBehaviour
    {
        [Header("------------------�f�o�b�O�m�F�p-----------------")]
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
        readonly protected static float UPPERRAYLENGTH = 0.8f; �@�@�@�@ //���Ray�������ł���ō�����
        readonly protected static float UNDERRAYLENGTH = 2.0f;�@�@�@�@�@//����Ray�������ł���ō�����
        readonly protected static float UNDERBREAKRAYLENGTH = 0.5f;�@�@ //����Ray�ł̔j�󂪉\�ɂȂ鋗��

        readonly protected static float STACK_TIMER = 5.0f;�@�@�@�@�@�@ //�G���X�^�b�N�����ꍇ�Ƀ��X�|�[�����鎞��
        readonly protected static int   EXTINCTION_TIMER = 3;�@�@�@�@�@ //�G�����S�����ۂɃI�u�W�F�N�g��������x������
        readonly protected static float SWINGROTATIONAMOUNT = 1.0f * Mathf.Rad2Deg;�@//�G���ڂ̑O��Ray���X�C���O����ۂ̊p�x�isin�j
        readonly protected static float MAXROTATE = 360.0f;             //sin�p�x�̃��Z�b�g�Ɏg��

        //�G���U������ۂɑO�̃u���b�N���ז��Ȏ��ɔj�󂷂邽�߂�RAY�̋��̂�RAY�̑傫��
        readonly protected static float CANT_PLAYER_ATTACK_BREAK_BLOCK_RAY_RANGE = 0.5f;
        //�G���U������ۂɑO�̃u���b�N���ז��Ȏ��ɔj�󂷂邽�߂�RAY�̒���
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
        [SerializeField,Tooltip("�|���ꂽ�Ƃ��ɂ����ɏ�����I�u�W�F�N�g")] GameObject[] downEnemyObjectDissolve;
        #endregion

        #region StatusVariable
        [Header("------------------�Q�[���̕ύX�K����X�e�[�^�X *HP�ύX��EnemySetting�ł��肢���܂��B-----------------")]
        [Tooltip("�q�b�g�|�C���g")]
        public float _hp;
        [Tooltip("�q�b�g�|�C���g�̍ő吔")]
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
            SPECIFIC_BEHAVIOR_1 = 0x40, //�p����̓G�̌ŗL�s���P
            SPECIFIC_BEHAVIOR_2 = 0x80,�@//�p����̓G�̌ŗL�s���P
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
            //���̋�Ԕc��
            GroundUpdate(threadTransform);


            //�X�g�b�v���̏���
            if (StateCheck(EnemyState.STOP))
            {
                StayMoveUpdate(threadTransform);
                return;
            }

            //���S���̏���
            if (_bDownEnemy)
            {
                EnemyDownMove(threadTransform);
                return;
            }
            Vector3 workVelosity = _rigidbody.velocity;
            StartRayHitUpdate(threadTransform);

            AddGravityTransform(ref workVelosity);

            EfectUpdate(threadTransform);
            //�X�^�b�N��Ԃ��`�F�b�N
            StackCheck(threadTransform);
            //Ray�̍X�V
            RayUpdate(threadTransform);
            //���݂̃X�e�[�g���玩�g�̍s�������s
            MovementFromEnemyState(threadTransform,ref workVelosity);
            //��莞�Ԃ��ƂɓG�̓�����ύX
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
        /// �X�e�[�g����G�𓮂���
        /// </summary>
        public virtual void MovementFromEnemyState(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            float toPlayer = DifferenceToPlayerY(transform);

            switch (_enemyMoveState)
            {
                //�v���C���[�ɒǏ]����
                case EnemyState.TRACK_PLAYER:
                    TrackPlayer(transform,ref rdVelosity);
                    break;
                //�u���b�N���󂵂Ȃ���O�֐i��
                case EnemyState.FORWARD_BREAK:
                    ForwardBreak(transform,ref rdVelosity);
                    break;
                //�W�����v���Ȃ���K�i��ɏオ��
                case EnemyState.JUMP_UP:
                    JumpUp(transform,toPlayer, ref rdVelosity);
                    break;
                //���Ƀu���b�N���󂵂Ȃ��牺�ɐi��
                case EnemyState.BOTTON_BREAK:
                    Botton(transform, toPlayer, ref rdVelosity);
                    break;
                //�U�����[�V����
                case EnemyState.ATTACK:
                    AttackPlayer(transform);
                    break;
                //�p���p����s���P
                case EnemyState.SPECIFIC_BEHAVIOR_1:
                    SpecificBehavior_1(transform, toPlayer,ref rdVelosity);
                    break;
                //�p���p����s���Q
                case EnemyState.SPECIFIC_BEHAVIOR_2:
                    SpecificBehavior_2(transform, toPlayer, ref rdVelosity);
                    break;
            }
        }

        /// <summary>
        /// ��莞�ԃX�^�b�N�������ɌĂ΂��
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
        /// �G���|���ꂽ�Ƃ��ɌĂ΂��
        /// </summary>
        protected virtual void EnemyDownMove(ThreadTransform transform)
        {
            //�d�͂Ɠ����蔻������Ȃ�����
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
                //�f�B�]���u������
                if (_downCount >= _dissolveCount)
                {
                    _dissolveCount += transform.deltaTime / DISOLVE_POWER;
                    _dissolve.SetFloat("_ClipTime", _dissolveCount);
                    Color color = Color.white;
                    //�ŏI�������l�𒲐�����
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
        /// �G�̃X�e�[�g��ύX
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void ChangeState(ThreadTransform transform, float toPlayer)
        {
#if UNITY_EDITOR
            if (_debugMode)
            {
                Debug.Log("�v���C���[�̋���" + toPlayer);
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

                //�v���C���[����ɂ���Ƃ�
            if (toPlayer > (int)HieralkeyState.DOWN_BREAK)
            {
                _enemyMoveState = EnemyState.JUMP_UP;
                _stateChangeTimer = 0.0f;
                return;
            }

            //���ɂ���Ƃ�
            if (toPlayer <= (int)HieralkeyState.DOWN_BREAK)
            {
                _enemyMoveState = EnemyState.BOTTON_BREAK;
            }

            _stateChangeTimer = 0.0f;
        }

        /// <summary>
        /// �v���C���[�ɒǏ]����
        /// </summary>
        protected void TrackPlayer(ThreadTransform transform, ref Vector3 rdVelosity)
        {
            if (DifferenceToPlayer(transform) <= _enemyStatus.attackPlayerLength * _enemyStatus.attackPlayerLength) return;

            float jumpAddSpeed = 1.0f;

            //�W�����v���œ�������������
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
        /// �O�ɐi�݂Ȃ���ڂ̑O�̃u���b�N����
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
        /// �W�����v���Ȃ����ֈړ�����
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
        /// ���Ɍ@��
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void Botton(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {
            //�v���C���[��艺�ɂ���ꍇ�͑O�ɐi��
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
        /// ���ʂȍs���P
        /// �p�����ɓG�ŗL�̓���������΂���
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void SpecificBehavior_1(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {

        }

        /// <summary>
        /// ���ʂȍs���Q
        /// �p�����ɓG�ŗL�̓���������΂���
        /// </summary>
        /// <param name="toPlayer"></param>
        protected virtual void SpecificBehavior_2(ThreadTransform transform, float toPlayer, ref Vector3 rdVelosity)
        {

        }

        /// <summary>
        /// �v���C���[�Ƀ_���[�W��^���鎞�ɌĂ΂��
        /// </summary>
        protected void SendPlayerDamage(ThreadTransform transform, float toPlayerLengthY)
        {
            _bAttack = true;

            if (_enemyStatus.longRangeAttack)
            {
                //�������U�����s��
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
        /// ����̃u���b�N��j�󂷂�iSphereRay�j
        /// </summary>
        /// <param name="range">���͈̔�</param>
        /// <param name="raylength">RAY�̒���</param>
        /// <param name="bBreakUnder">������艺���󂳂Ȃ�</param>
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
        /// �������~����
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
        /// �_���[�W��^����
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="knockBackPower"></param>
        /// <param name="addKnockBackVec"></param>
        public void SendDamage(float damage, float knockBackPower, Vector3 addKnockBackVec)
        {;
            if (_bDownEnemy) return;
            _hp -= damage;

            //�m�b�N�o�b�N���v�Z
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
        /// �X�e�[�g�X�V���Ԃ��v�Z
        /// </summary>
        void ChangeActionState(ThreadTransform transform)
        {
            if (StateCheck(EnemyState.STOP)) return;
            _stateChangeTimer += transform.deltaTime;
            _regularlyLongRangeAttackTime += transform.deltaTime;

            //��莞�Ԃ��ƂɃA�N�V������ύX����
            if (_stateChangeTimer >= _enemyStatus.stateChageTime)
            {
                float toPlayer = DifferenceToPlayerY(transform);
                ChangeState(transform, toPlayer);
            }

            //����I�ɉ������U��������
            if (_regularlyLongRangeAttackTime >= _enemyStatus.regularlyLongRangeAttackTimer)
            {
                _bAttack = true;
                LongRangeAttack(transform);
                _regularlyLongRangeAttackTime = 0.0f;
            }
        }

        /// <summary>
        /// �g�������B��Ă��邩�̃`�F�b�N
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
        /// �U���G�t�F�N�g�̎��Ԑݒ�
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
        //����
        //=======================================================

        //Ray�̍X�V
        void RayUpdate(ThreadTransform transform)
        {
            Quaternion workRotate = transform.quaternion;

            //�O������Ray
            _bStop = false;

            Vector3 work;
           
            work = GetPivot(transform);
            _ray.origin = work + (workRotate * _enemyStatus.forwordRayAjust);
            work = transform.forward;
            //work = workRotate * Vector3.forward;
            work.z += _swingAmount;
            _ray.direction = work;

            //�������Ray
            work = GetPivot(transform);
            _worldUpperRay.origin = work + (workRotate * _enemyStatus.upperAngleAjust);
            work = workRotate * _enemyStatus.upperAngle;
            _worldUpperRay.direction = work;

            //��������Ray
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
        /// Ray�����E�ɓ�����
        /// </summary>
        void RaySwingUpdate(ThreadTransform transform)
        {
            _swingTime += SWINGROTATIONAMOUNT * transform.deltaTime;
            _swingAmount = Mathf.Sin(_swingTime) * _enemyStatus.swingWidth;
            if (_swingTime >= MAXROTATE) { _swingTime = 0.0f; }
        }

        //�v���C���[�̕���������
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
        //����̏󋵂��擾����@�\
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
        /// �n�ʂɂ��Ă��邩�̃`�F�b�N
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
        /// �v���C���[�Ƃ̋��������߂�
        /// </summary>
        /// <returns></returns>
        protected float DifferenceToPlayer(ThreadTransform transform)
        {
            return (transform.playerPosition - GetPivot(transform)).sqrMagnitude;
        }

        /// <summary>
        /// �v���C���[�Ƃ�Y���W�̋��������߂�
        /// </summary>
        /// <returns>�ׂ��������͐؂�̂�</returns>
        protected float DifferenceToPlayerY(ThreadTransform transform)
        {
            return transform.playerPosition.y - GetPivot(transform).y;
        }
        /// <summary>
        /// �v���C���[�Ƃ̕��ʂ̋��������߂�
        /// </summary>
        /// <returns>�ׂ��������͐؂�̂�</returns>
        protected float DifferenceToPlayerHorizontalSqrMag(ThreadTransform transform)
        {
            Vector3 length = transform.playerPosition - GetPivot(transform);
            length.y = 0;
            return length.sqrMagnitude;
        }

        /// <summary>
        /// �E�F�[�u�ɐݒ肳�ꂽ�U���{��
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
                    //�A�j���[�V�����𓮂���
                    if (_enemyStatus.moveMostionName != "")
                    {
                        if (_animation.GetBool(_enemyStatus.moveMostionName)) return;
                        _animation.SetBool(_enemyStatus.moveMostionName, true);
                    }
                    break;
                case EnemyState.FORWARD_BREAK:
                    //�A�j���[�V�����𓮂���
                    if (_enemyStatus.moveMostionName != "")
                    {
                        if (_animation.GetBool(_enemyStatus.moveMostionName)) return;
                        _animation.SetBool(_enemyStatus.moveMostionName, true);
                    }
                    break;
            }
        }

        //=======================================================
        //�j��֘A
        //=======================================================

        /// <summary>
        /// �u���b�N�Ƀ_���[�W��^����
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
        /// �u���b�N�Ƀ_���[�W��^����
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
        /// �u���b�N���󂵂���Ɏ~�܂�
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
                //�f�o�b�O�F�j���Ɉ�b�҂�
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
        /// �u���b�N�Ƀ_���[�W��^����
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
        //�t���O�`�F�b�N��X�e�[�g���Ǘ�����@�\
        //=======================================================

        /// <summary>
        /// �n�ʂɂ��Ă��邩
        /// </summary>
        /// <returns></returns>
        protected bool IsGround()
        {
            return _bGround;
        }
        /// <summary>
        /// �X�e�[�g��Ԃ��`�F�b�N
        /// </summary>
        /// <param name="enemyState"></param>
        /// <returns></returns>
        protected bool StateCheck(EnemyState enemyState)
        {
            return (_enemyMoveState & enemyState) != 0;
        }

        /// <summary>
        /// �s�����I���������̃f�t�H���g�X�e�[�g��Ԃ��Z�b�g����
        /// </summary>
        /// <param name="enemyState"></param>
        protected void SetEndStopToTransitionState(EnemyState enemyState)
        {
            _endStopToDefaultChangeState = enemyState;
        }

        /// <summary>
        /// �G�̒�����̒��S���擾
        /// </summary>
        protected Vector3 GetPivot(ThreadTransform transform)
        {
            return transform.position + _enemyStatus.pivotAjust;
        }
        /// <summary>
        /// �X�g�b�v��Ԃ̓G�̃A�b�v�f�[�g
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
        /// �ʃX���b�h�Ŏg�p�����Ɨp�f�[�^
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
