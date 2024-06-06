using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player.MainSkill
{
    public class MainSkill_Magnet : MainSkillBase
    {
        const float DEFAULT_COOL_TIME = 90.0f;
        const float MOVE_END_TIME = 7.0f;
        float timer;

        public override bool Active(GameObject playerObject)
        {
            if (!IsUse() || IsActive()) return false;

            Actived();

            GameObject juwelryParent = GameObject.Find("JuwelryParent");

            foreach (Transform juwelry in juwelryParent.transform)
            {
                Collider collider = juwelry.GetComponent<Collider>();
                Rigidbody rigidbody = juwelry.GetComponent<Rigidbody>();

                collider.enabled = false;
                rigidbody.freezeRotation = true;

                Vector3 length = playerObject.transform.position - juwelryParent.transform.position;
                Vector3 randomPos;

                randomPos.x = Random.Range(-1.0f, 1.0f);
                randomPos.z = Random.Range(-1.0f, 1.0f);
                randomPos.y = 0.1f;

                juwelry.transform.DOMove(playerObject.transform.position + randomPos, Random.Range(1.0f, MOVE_END_TIME)).OnKill(() =>
                {
                    collider.enabled = true;
                    rigidbody.freezeRotation = false;

                });

              
            }
            return true;
        }

        public override void Update()
        {
            base.Update();

            if (IsUse() || IsActive()) return;

            timer += Time.deltaTime;

            if(timer >= MOVE_END_TIME)
            {
                timer = 0.0f;
                SetCoolTime(DEFAULT_COOL_TIME);
                ActiveEnd();
            }
        }
    }
}
