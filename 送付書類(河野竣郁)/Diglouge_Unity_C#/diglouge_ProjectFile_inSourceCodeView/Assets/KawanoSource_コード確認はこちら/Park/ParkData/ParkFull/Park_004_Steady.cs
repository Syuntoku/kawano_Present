using System.IO;
using Syuntoku.Status;

namespace Syuntoku.DigMode.ParkData
{
    public class Park_004_Steady : ParkBase
    {
        float damageUpPlibility;
        float speedDownPlibility;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPlibility = float.Parse(LoadLine(reader));
            speedDownPlibility = float.Parse(LoadLine(reader));
        }

        public override void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
            baseStatus.digmodeStatus.AddSpeedMagnification(speedDownPlibility);
        }

        public override void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
            baseStatus.digmodeStatus.AddSpeedMagnification(-speedDownPlibility);
        }
    }
}
