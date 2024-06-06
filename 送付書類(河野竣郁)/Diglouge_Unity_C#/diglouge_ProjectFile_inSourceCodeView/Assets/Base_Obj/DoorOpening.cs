using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.DigMode.Field
{

    public class DoorOpening : MonoBehaviour
    {
        [SerializeField] GameObject _playerObject;
        public float _openingLength;
        public float _openSpeed;
        Animator _animator;

        //==========================
        //Unity
        //==========================
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetFloat("SpeedMult", _openSpeed);
        }

        private void Update()
        {
            if ((_playerObject.transform.position - transform.position).sqrMagnitude <= _openingLength * _openingLength)
            {
                _animator.SetBool("isOpening", true);
            }
            else
            {
                if (!_animator.GetBool("isOpening")) return;
                _animator.SetBool("isOpening", false);
            }
        }
    }
}
