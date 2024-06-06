using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player.MainSkill
{
    public class MainSkill_Warp : MainSkillBase
    {
        const float DEFAULT_COOL_TIME = 90.0f;
        float timer;
        const float WAIT_TIME = 4.0f;
        GameObject _respornEfect;
        GameObject _generateEfect;
        GameObject _playerObject;
        FirstPerson _firstPerson;

        public override void Initialize(GameObject respornEfect)
        {
            _respornEfect = respornEfect;
        }

        public override bool Active(GameObject playerObject)
        {
            if (IsActive() || !IsUse()) return false;

            _firstPerson = playerObject.GetComponent<Player>()._firstPerson;

            if (!_firstPerson._isGraunded) return false;

            _playerObject = playerObject;
            Actived();
            _firstPerson.StopMove();
            _generateEfect = GameObject.Instantiate(_respornEfect, _playerObject.transform.position, Quaternion.Euler(new Vector3(-90.0f,0.0f,0.0f)));
            return true;
        }

        public override void Update()
        {
            base.Update();

            if (!IsUse() || !IsActive()) return;

            //‚S•b—§‚Â‚ÆƒeƒŒƒ|[ƒg‚·‚é
            timer += Time.deltaTime;
#if UNITY_EDITOR
            Debug.Log("waitTime" + timer);
#endif
            if (timer >= WAIT_TIME)
            {
                timer = 0;
                _firstPerson.Resporn();
                ActiveEnd();

                SetCoolTime(DEFAULT_COOL_TIME);

                GameObject.Destroy(_generateEfect);
            }
        }
    }
}
