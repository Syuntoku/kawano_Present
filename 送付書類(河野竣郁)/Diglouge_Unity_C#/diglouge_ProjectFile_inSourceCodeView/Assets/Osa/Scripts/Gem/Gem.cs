using System.Collections;
using UnityEngine;

namespace Battle
{

    public class Gem : MonoBehaviour
    {
        [SerializeField] private Player player;

        /// <summary>
        /// �ő哞�B���x
        /// </summary>
        [SerializeField] private float _maxSpeed;
        /// <summary>
        /// �������鎞��
        /// </summary>
        [SerializeField] private float _accelerationDuration;

        /// <summary>
        /// �����
        /// </summary>
        [SerializeField] private AnimationCurve _animationCurve;

        private float _startTime;

        private Rigidbody _rigidbody;

        public float raycastDistance = 1.1f;
        private int _exp;

        private void Start()
        {
            TryGetComponent(out _rigidbody);

            if (!player) player = Player.Instance;
        }

        public void FollowStart(int exp)
        {
            _exp = exp;
            StartCoroutine(Flow());
        }

        private IEnumerator Flow()
        {
            _rigidbody.isKinematic = false;
            while (!Physics.Raycast(transform.position, Vector3.down, out var hit, raycastDistance))
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            _rigidbody.isKinematic = true;

            _startTime = Time.time;
            while (true)
            {
                Follow();
                yield return null;
            }

        }

        private void Follow()
        {
            // �w��b�������čő哞�B���x�܂Ő��`�ɉ������Ă���
            var a = (Time.time - _startTime) / _accelerationDuration;
            var currentSpeed = _animationCurve.Evaluate(a) * _maxSpeed;

            var dir = (player.transform.position - transform.position).normalized;

            transform.position += dir * currentSpeed * Time.deltaTime;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                Destroy(this.gameObject);
                player.AddExp(_exp);
            }
        }
    }

}