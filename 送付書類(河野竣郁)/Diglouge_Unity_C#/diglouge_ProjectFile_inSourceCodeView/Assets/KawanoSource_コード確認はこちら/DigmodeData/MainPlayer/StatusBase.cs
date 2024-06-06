using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syuntoku.Status
{
    public class StatusBase
    {
        public const float MAX_VALUE = float.MaxValue;
        public const float DEFAULT_MAGNIFICATION_VALUE = 1.0f;

        /// <summary>
        /// ”’l‚Ì—LŒø‚È’l‚ÉŠÛ‚ß‚é
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public float ValidValueCheck(float amount)
        {
            if (amount >= MAX_VALUE) return MAX_VALUE;

            return amount;
        }

        

    }
}
