using System.IO;
using Syuntoku.Status;
using Syuntoku.DigMode.ParkData;

public class Park_005_Innocent : ParkBase
{
    float damageUpPlibility;
    float downDirectionDamageDownPlibility;
    bool _bUpBreak;
    bool bUpStatus;

    public override void Loading(StringReader reader)
    {
        base.Loading(reader);

        damageUpPlibility = float.Parse(LoadLine(reader));
        downDirectionDamageDownPlibility = float.Parse(LoadLine(reader));
    }

    public override void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
    {
        if(parkConditionsManage.blockState_Direction_virtical ==(uint)ParkConditionsManage.BlockState_Direction.UP)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(damageUpPlibility);
            _bUpBreak = true;
        }
        else 
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(downDirectionDamageDownPlibility);
            _bUpBreak = false;

        }
    }
    public override void EndSwing(StatusManage baseStatus)
    {
        if(_bUpBreak)
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-damageUpPlibility);

        }
        else
        {
            baseStatus.digmodeStatus.AddDigDamagePlibility(-downDirectionDamageDownPlibility);
        }
    }
}
