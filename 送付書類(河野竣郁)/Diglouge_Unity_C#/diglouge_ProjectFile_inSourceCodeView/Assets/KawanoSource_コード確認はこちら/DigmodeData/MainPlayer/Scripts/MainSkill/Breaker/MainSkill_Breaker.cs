using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Input;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player.MainSkill
{

    public class MainSkill_Breaker : MainSkillBase
    {
        bool _bStandSkill;
        Player _playerData;
        const int RANGE = 5;

        public override void Initialize(GameObject data)
        {
        }

        public override bool Active(GameObject playerObject)
        {
            if (IsUse()) return false;
            _bStandSkill = true;
            _playerData = playerObject.GetComponent<Player>();
            return true;
        }

        public override void Update()
        {
            if (!_bStandSkill) return;

            if (InputData._bAction)
            {
                RaycastHit hit = _playerData.GetFowordRaycastData();
#if UNITY_EDITOR
                if (hit.collider == null)
                {
                    Debug.Log("ブレイカーエラー");
                    return;
                }
#endif
                DamageManager damageManager = new DamageManager();
                damageManager.bBreak = true;
                BlockData blockData = hit.collider.GetComponent<BlockData>();
                //blockData._blockdataInfo.ThisBlockDamageSpreads(RANGE, damageManager);
                UsedSkill();
            }
        }
    }
}