using System.IO;
using Syuntoku.Status;
using Syuntoku.DigMode.ParkData;

namespace Syuntoku.DigMode.ParkData
{

    public class Park_006_Badger : ParkBase
    {

        float damageUpPlibility;
        float downDirectionDamageDownPlibility;
        bool _bDownBreak;

        public override void Loading(StringReader reader)
        {
            base.Loading(reader);

            damageUpPlibility = float.Parse(LoadLine(reader));
            downDirectionDamageDownPlibility = float.Parse(LoadLine(reader));
        }

        public override void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
            if (parkConditionsManage.blockState_Direction_virtical == (uint)ParkConditionsManage.BlockState_Direction.DOWN)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
                _bDownBreak = true;
            }
            else
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(downDirectionDamageDownPlibility);
                _bDownBreak = false;

            }
        }

        public override void EndSwing(StatusManage baseStatus)
        {
            if (_bDownBreak)
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);
            }
            else
            {
                baseStatus.digmodeStatus.AddDigDamagePlibility(-downDirectionDamageDownPlibility);
            }
        }
    }
}
