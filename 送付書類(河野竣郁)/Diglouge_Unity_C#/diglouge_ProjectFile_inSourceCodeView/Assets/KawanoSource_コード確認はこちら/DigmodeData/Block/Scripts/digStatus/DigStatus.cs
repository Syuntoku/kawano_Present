using Unity.Collections;
using UnityEngine;

namespace Syuntoku.Status
{
    [System.Serializable]
    public class DigStatus : StatusBase
    {
        [SerializeField, ReadOnly] private float damagePlibility;
        [SerializeField, ReadOnly] private float toolIntervalMagnification;
        [SerializeField, ReadOnly] private float addDropPibility;
        [SerializeField, ReadOnly] private int dropCount;
        [SerializeField, ReadOnly] private int immediatelyBreakMagnification;
        [SerializeField, ReadOnly] private float suitLightPowerMagnification;

        public float DamagePlibility => damagePlibility;
        public float ToolIntervalMagnification => toolIntervalMagnification;
        public float AddDropPibility => addDropPibility;
        public int DropCount => dropCount;
        public int ImmediatelyBreakMagnification => immediatelyBreakMagnification;
        public float SuitLightPowerMagnification => suitLightPowerMagnification;

        public DigStatus()
        {
            Initialize();
        }

        public void Initialize()
        {
            damagePlibility = 1.0f;
            addDropPibility = 1.0f;
            dropCount = 1;
            immediatelyBreakMagnification = 0;
            suitLightPowerMagnification = 1.0f;
            toolIntervalMagnification = 1.0f;
        }

        public void ZeroInitialize()
        {
            damagePlibility = 0.0f;
            addDropPibility = 0.0f;
            dropCount = 0;
            immediatelyBreakMagnification = 0;
            suitLightPowerMagnification = 0.0f;
        }

        public void AddDamagePlibility(float amount)
        {
            damagePlibility += ValidValueCheck(amount);
        }

        public void ReductionToolInvervalMagnification(float amount)
        {
            toolIntervalMagnification -= ValidValueCheck(amount);
        }

        public void AddDropPlibility(float amount)
        {
            addDropPibility += ValidValueCheck(amount);
        }

        public void AddDropCount(int amount)
        {
            dropCount += amount;
        }

        public void AddImmediatelyBreakManification(int amount)
        {
            immediatelyBreakMagnification += amount;
        }

        public void AddSuitLightPower(int amount)
        {
            suitLightPowerMagnification += amount;
        }

        /// <summary>
        /// 演算子オーバーロード　＋
        /// </summary>
        public static DigStatus operator +(DigStatus main, DigStatus data)
        {
            main.damagePlibility += data.DamagePlibility;
            main.addDropPibility += data.AddDropPibility;
            main.dropCount += data.DropCount;
            main.immediatelyBreakMagnification += data.ImmediatelyBreakMagnification;

            return main;
        }
    }
}
