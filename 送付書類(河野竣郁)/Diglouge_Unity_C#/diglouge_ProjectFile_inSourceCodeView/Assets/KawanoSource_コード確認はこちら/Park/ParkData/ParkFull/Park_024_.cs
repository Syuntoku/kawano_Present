using System.Collections;
using System.Collections.Generic;
using System.IO;
using Syuntoku.Status;
using UnityEngine;
using UniRx;
using System;

namespace Syuntoku.DigMode.ParkData
{

    /// <summary>
    /// 24
    /// 宙に浮いているとき、リロード速度が(10/20/30)%UPする。
    /// </summary>
    public class Park_024_ : ParkBase
    {
        float reloadMultiplier;
        IDisposable disposable;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);
            reloadMultiplier = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            disposable = parkConditionsManage.JumpReactiveProperty.SkipWhile(x => !x).Subscribe(isJump =>
                {
                    if (isJump)
                    {
                        baseStatus.battleModeStatus.AddReloadSpeedMagnification(reloadMultiplier);
                    }
                    else
                    {
                        baseStatus.battleModeStatus.AddReloadSpeedMagnification(-reloadMultiplier);
                    }
                });

        }
        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            disposable.Dispose();
        }

    }
}
