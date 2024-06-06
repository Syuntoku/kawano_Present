using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Syuntoku.DigMode.Player
{
    public class IsGraundedCheck : MonoBehaviour
    {
       FirstPerson _firstPerson;

        public void Initialize(FirstPerson firstPerson)
        {
            _firstPerson = firstPerson;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_firstPerson.IsGround())
            {
                _firstPerson.OnGround();
            }
        }

    }
}
