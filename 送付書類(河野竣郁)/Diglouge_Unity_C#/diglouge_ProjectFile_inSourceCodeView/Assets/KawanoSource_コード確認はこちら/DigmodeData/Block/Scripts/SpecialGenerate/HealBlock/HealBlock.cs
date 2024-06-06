using UnityEngine;

namespace Syuntoku.DigMode
{
    /// <summary>
    /// �񕜃u���b�N
    /// </summary>
    public class HealBlock : BlockData
    {
        public HealBlockScriptable _healblockScriptable;
        const float RAY_LENGTH = 0.01f;

        public override void BreakAction()
        {
            //�͈͓��̑Ώۂ��񕜂�����
            RaycastHit[] explosionHits = Physics.SphereCastAll(transform.position, _healblockScriptable.healRange, Vector3.up, RAY_LENGTH, _healblockScriptable.healSubjectMask);

            foreach (var hit in explosionHits)
            {
               if(hit.collider.CompareTag(Player.Player.PLAYER_TAG))
                {
                    hit.collider.GetComponent<Player.Player>().HealHp(_healblockScriptable.healPower, _healblockScriptable.healobject);
                }
            }
        }
    }
}
