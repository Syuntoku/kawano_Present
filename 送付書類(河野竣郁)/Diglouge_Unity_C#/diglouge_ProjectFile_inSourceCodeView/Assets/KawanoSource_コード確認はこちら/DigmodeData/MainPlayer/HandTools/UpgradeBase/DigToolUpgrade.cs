using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Tool;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    public class DigToolUpgrade : ToolUpgrade
    {
        public float _toolSpeedMagnification;

        public float _damageUpMagnification;

        public float _toolInvervalMagnification;

        public virtual void PulusStatus(StatusManage statusManage, ToolInfo toolInfo)
        {

        }

        public virtual void DisableStatus(StatusManage baseStatus, ToolInfo toolinfo)
        {

        }
    }
}
