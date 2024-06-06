using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Player;

namespace Syuntoku.Status
{
    [System.Serializable]
    public class FirstParsonStatus : StatusBase
    {

        public FirstParsonStatus()
        {
            ZeroReset();
        }

        private FirstParsonStatus defaultStatus;

        [field: SerializeField]
        public float normalSpeed { get; private set; }

        [field: SerializeField]
        public float dashSpeed { get; private set; }

        [field: SerializeField]
        public float speedMagnification { get; private set; }

        [field: SerializeField]
        public float speedPower { get; private set; }

        [field: SerializeField]
        public float groundSpeedMagnification { get; private set; }

        [field: SerializeField]
        public float airSpeedMagnification { get; private set; }

        [field: SerializeField]
        public float horizontalSpeedMagnification { get; private set; }

        [field: SerializeField]
        public float virticalSpeedMagnification { get; private set; }

        [field: SerializeField]
        public float suitLightPower { get; private set; }

        public void SetStatusAndDefaultSet(float speedMagnification = DEFAULT_MAGNIFICATION_VALUE, float speedPower = DEFAULT_MAGNIFICATION_VALUE
                        , float groundspeed = DEFAULT_MAGNIFICATION_VALUE, float airSpeed = DEFAULT_MAGNIFICATION_VALUE, float horizontalSpeed = DEFAULT_MAGNIFICATION_VALUE, float virticalSpeed = DEFAULT_MAGNIFICATION_VALUE, float suitPower = DEFAULT_MAGNIFICATION_VALUE)
        {
            defaultStatus = new FirstParsonStatus();

            defaultStatus.speedMagnification = speedMagnification;
            defaultStatus.speedPower = speedPower;
            defaultStatus.groundSpeedMagnification = groundspeed;
            defaultStatus.airSpeedMagnification = airSpeed;
            defaultStatus.horizontalSpeedMagnification = horizontalSpeed;
            defaultStatus.virticalSpeedMagnification = virticalSpeed;
            defaultStatus.suitLightPower = suitPower;

            Reset();
        }

        public void SetStatus(FirstParsonStatus data)
        {
            speedMagnification = data.speedMagnification;
            speedPower = data.speedPower;
            groundSpeedMagnification = data.groundSpeedMagnification;
            airSpeedMagnification = data.airSpeedMagnification;
            horizontalSpeedMagnification = data.horizontalSpeedMagnification;
            virticalSpeedMagnification = data.virticalSpeedMagnification;
            suitLightPower = data.suitLightPower;
        }

        public void Reset()
        {
            SetStatus(defaultStatus);
        }

        public void ZeroReset()
        {
            speedMagnification = 0.0f;
            speedPower = 0.0f;
            groundSpeedMagnification = 0.0f;
            airSpeedMagnification = 0.0f;
            horizontalSpeedMagnification = 0.0f;
            virticalSpeedMagnification = 0.0f;
            suitLightPower = 0.0f;
        }

        public void SetDefaultSpeed(float normalSpeed, float dashSpeed)
        {
           this.normalSpeed = normalSpeed;
           this.dashSpeed = dashSpeed;
        }

        public void AddSpeedMagnification(float amount)
        {
            speedMagnification = ValidValueCheck(speedMagnification + amount);
        }

        public void AddSpeedPower(float amount)
        {
            speedPower = ValidValueCheck(speedPower + amount);

        }

        public void AddGroundSpeedMagnification(float amount)
        {
            groundSpeedMagnification = ValidValueCheck(groundSpeedMagnification + amount);
        }

        public void AddAirSpeedMagnification(float amount)
        {
            airSpeedMagnification = ValidValueCheck(airSpeedMagnification + amount);

        }

        public void AddHorizontalSpeedMagnification(float amount)
        {
            horizontalSpeedMagnification = ValidValueCheck(horizontalSpeedMagnification + amount);

        }

        public void AddVirticalSpeedMagnification(float amount)
        {
            virticalSpeedMagnification = ValidValueCheck(virticalSpeedMagnification + amount);

        }

        public void AddSuitPowerMagnification(float amout)
        {
            suitLightPower = ValidValueCheck(suitLightPower + amout);
        }

        /// <summary>
        /// プレイヤーの加速倍率
        /// </summary>
        /// <returns></returns>
        public float PlayerMagnification(bool isGround)
        {
            float speed = 0.0f;

            //地面の時
            if (isGround)
            {
                speed = groundSpeedMagnification * horizontalSpeedMagnification * speedMagnification;
            }
            //空中の時
            else
            {
                speed = airSpeedMagnification * virticalSpeedMagnification * speedMagnification;
            }

            return speed;
        }

        public float GetPlayerSpeed(FirstPerson.PlayerState playerState, bool isGround)
        {
            float speed = 0;
            if (playerState == FirstPerson.PlayerState.WORK)
            {
                speed = normalSpeed;
            }

            if (playerState == FirstPerson.PlayerState.DASH)
            {
                speed = dashSpeed;
            }

            return speed * PlayerMagnification(isGround);
        }

        /// <summary>
        /// プレイヤーがジャンプしたときの倍率
        /// </summary>
        /// <returns></returns>
        public float JumpPowerMagnification()
        {
            return airSpeedMagnification * virticalSpeedMagnification;
        }

        /// <summary>
        /// 演算子オーバーロード　＋
        /// </summary>
        public static FirstParsonStatus operator +(FirstParsonStatus data, FirstParsonStatus data1)
        {
            data.speedMagnification += data1.speedMagnification;
            data.speedPower += data1.speedMagnification;
            data.groundSpeedMagnification += data1.groundSpeedMagnification;
            data.airSpeedMagnification += data1.airSpeedMagnification;
            data.horizontalSpeedMagnification += data1.horizontalSpeedMagnification;
            data.virticalSpeedMagnification += data1.virticalSpeedMagnification;
            data.suitLightPower += data1.suitLightPower;

            return data;
        }
    }
}