using UnityEngine;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode.Input;
using Syuntoku.Status;
using DG.Tweening;

namespace Syuntoku.DigMode.Player
{
    [System.Serializable]
    public class FirstPerson
    {
        #region VARIABLES
        public FirstParsonStatus _firstParsonStatus = new FirstParsonStatus();
        [SerializeField] PlayerData _playerData;
        GameObject _respormPoint;
        StatusManage _statusManage;
        GameObject _mainCamera;
        GameObject _playerTrans;
        GameSetting _gameSetting;

        //Player�̈ړ����������������
        public Vector3 _moveDirection;
        public Rigidbody _rb;

        bool _bStopMove;
        public float _jumpPushTime;
        public bool _isGraunded;
        public float _angle;

        Vector3 _velosity;
        const float AIR_RESISTANCE = 0.1f;
        const float LIMIT_ADS_ANGLE = 85.0f;
        readonly Vector3 AJUST_RESPORN_POINT = new Vector3(0.0f, 3.0f, 0.0f);
        const float RESPORN_MOTION_POMPLATE_TIME = 2.0f;
        Vector3 _junpPower;
        Vector3 _normalJunpPower;
        Vector3 _defaultMaxSpeed;
        Animator _playerAnimator;

        LayerMask _layerMask;

        public enum PlayerState
        {
            WORK,
            DASH,
        }

        PlayerState _playerState;
        #endregion

        //======================================
        //public
        //======================================
        public void Initalize(GameObject playerObject, Animator playerAnimator, StatusManage statusManage)
        {
            _rb = playerObject.GetComponent<Rigidbody>();
            _respormPoint = GameObject.Find("RespornPoint");
            _gameSetting = GameObject.Find("GameSetting").GetComponent<GameSetting>();
            _mainCamera = GameObject.Find("Camera");
            _playerTrans = playerObject;
            _statusManage = statusManage;
            //�v���C���[�̃|�W�V���������X�|�[���|�C���g��
            playerObject.transform.position = _respormPoint.transform.position;

            _playerAnimator = playerAnimator;

            _velosity = _rb.velocity;
            _mainCamera.transform.parent = playerObject.transform;
            //�J�����̌��������̃I�u�W�F�N�g�Ɠ����ɂ���
            _mainCamera.transform.rotation = playerObject.transform.rotation;

            _junpPower = new Vector3(0, _playerData.UpPower, 0);
            _normalJunpPower = new Vector3(0, _playerData.nomalJump, 0);

            _moveDirection = Vector3.zero;
            _bStopMove = false;
            _defaultMaxSpeed = _playerData.maxVelosity;

            _firstParsonStatus.SetStatusAndDefaultSet();
            _firstParsonStatus.SetDefaultSpeed(_playerData.nomalSpeed, _playerData.dashSpeed);
            _statusManage.digmodeStatus.ConnectStatus(_firstParsonStatus);

            _layerMask = LayerMask.GetMask("Block");
            _layerMask += LayerMask.GetMask("Field");
        }

        /// <summary>
        /// �v���C���[�̓����𐧌䂷��
        /// </summary>
        public void TranfromUpdate()
        {
            if (_gameSetting.bStopGameAction || _bStopMove) return;

            RotateCamera();

            //XZ���̈ړ��ƌ�����������
            //WASD,�㉺���E�L�[
            _moveDirection.y = 0.0f;
            _moveDirection.x = InputData._InputMoveVec2.x;
            _moveDirection.z = InputData._InputMoveVec2.y;

            //�A�j���[�V�����𓮂���
            _playerAnimator.SetBool("Dash", _moveDirection == Vector3.zero ? true : false);

            //�v���C���[���_�b�V�����ł���Ή����X�s�[�h���グ��
            _playerState = InputData._bdash ? PlayerState.DASH : PlayerState.WORK;

            //�󒆂ɂ���ꍇ�̓_�b�V���𖳌��ɂ���
            if (!_isGraunded)
            {
                _playerState = PlayerState.WORK;
            }

            //�ړ��x�N�g�������[���h���W�ɕύX
            _moveDirection = _playerTrans.transform.TransformDirection(_moveDirection);

            float speedMagnification = _firstParsonStatus.PlayerMagnification(_isGraunded);

            Jump();

            Debug.DrawRay(_playerTrans.transform.position, _moveDirection.normalized * 0.5f);

            if(Physics.Raycast(_playerTrans.transform.position,_moveDirection.normalized,0.5f, _layerMask))
            {
                return;
            }

            //�x���V�e�B�[�𐧌�����
            if (Mathf.Abs(_rb.velocity.x) < _playerData.maxVelosity.x * speedMagnification && Mathf.Abs(_rb.velocity.z) < _playerData.maxVelosity.z * speedMagnification)
            {
                _velosity += (_moveDirection).normalized * _firstParsonStatus.GetPlayerSpeed(_playerState, _isGraunded) * Time.deltaTime;
            }

            Vector3 airResistance = -AIR_RESISTANCE * _velosity;
            _velosity += airResistance;
            _velosity.y = _rb.velocity.y;
            _rb.velocity = _velosity;

        }

        public void SetMaxSpeedPower()
        {
            _playerData.maxVelosity = _defaultMaxSpeed * _firstParsonStatus.speedMagnification;
        }

        /// <summary>
        /// ���X�|�[������
        /// </summary>
        public void Resporn()
        {
            _playerTrans.transform.position = _respormPoint.transform.position + AJUST_RESPORN_POINT;
            _playerTrans.transform.DOMove(_respormPoint.transform.position, RESPORN_MOTION_POMPLATE_TIME).SetEase(Ease.InOutExpo);
            _bStopMove = false;
        }

        /// <summary>
        /// �������~�߂�
        /// �����̊֐����Ăяo�����ꍇ�K��ActiveMove���ĂԂ���
        /// </summary>
        public void StopMove()
        {
            _bStopMove = true;
            _rb.velocity = Vector3.zero;
        }

        /// <summary>
        /// �~�߂Ă����������ĊJ����
        /// </summary>
        public void ActiveMove()
        {
            _bStopMove = false;
        }


        public void OnGround()
        {
            _isGraunded = true;
        }

        public bool IsGround()
        {
            return _isGraunded;
        }


        //======================================
        //private
        //======================================
        void RotateCamera()
        {
            //�}�E�X�ŃJ�����̌�����Player�̉��̌�����ς���
            float h = _playerData.horizontalSpeed * InputData._InputMouseDelta.x;
            float v = _playerData.verticalSpeed * InputData._InputMouseDelta.y;

            _playerTrans.transform.Rotate(0, h, 0);
            _angle = _mainCamera.transform.eulerAngles.x;
            //360�̊p�x��180�`-180�x�̊p�x�ɕύX����
            _angle = Mathf.Repeat(_angle + 180, 360) - 180;
            //��Βl85�x�܂ł̊p�x���������Ȃ�
            if (Mathf.Abs(_angle + v) <= LIMIT_ADS_ANGLE)
            {
                _mainCamera.transform.Rotate(v, 0.0f, 0.0f);
            }
        }

        void Jump()
        {
            //�n�ʂɂ���Β������̏�Ԃ�������
            if (_isGraunded)
            {
                _jumpPushTime = 0;
            }

            // Y�������ɃW�����v������
            if (InputData._bJump)
            {
                if (_isGraunded)
                {
                    _rb.velocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);
                    _rb.AddForce(_normalJunpPower * _firstParsonStatus.JumpPowerMagnification(), ForceMode.Impulse);
                    _isGraunded = false;
                }

                _jumpPushTime++;
                //�_�u���W�����v
                if (_jumpPushTime >= _playerData.doubleJumpDeday)
                {
                    if (_rb.velocity.y <= _playerData.maxVelosity.y * _firstParsonStatus.JumpPowerMagnification())
                    {
                        _rb.AddForce(_junpPower * _firstParsonStatus.JumpPowerMagnification(), ForceMode.Acceleration);
                    }
                }
            }
        }
    }
}