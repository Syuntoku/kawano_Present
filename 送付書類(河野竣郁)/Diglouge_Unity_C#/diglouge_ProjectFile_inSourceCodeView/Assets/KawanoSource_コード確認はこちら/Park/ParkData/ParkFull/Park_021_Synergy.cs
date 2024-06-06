using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 21:相乗
    /// 3m移動するごとにメインスキルのクールタイムが(1/2/3)%減少する。
    /// </summary>
    public class Park_021_Synergy : ParkBase
    {
        const float MOVETHRESHOLD = 3f;
        float cooldownReductionMultiplier;

        float _moveAmount;
        Vector3 _prePos;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            cooldownReductionMultiplier = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            var playerPos = Vector3.zero;

            _prePos = playerPos;
        }

        public override void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {
            var playerPos = Vector3.zero;
            _moveAmount += Vector3.Distance(_prePos, playerPos);
            _prePos = playerPos;

            if (_moveAmount >= MOVETHRESHOLD)
            {
                _moveAmount = 0;
                // クールタイムを減少
            }
        }

    }
}
