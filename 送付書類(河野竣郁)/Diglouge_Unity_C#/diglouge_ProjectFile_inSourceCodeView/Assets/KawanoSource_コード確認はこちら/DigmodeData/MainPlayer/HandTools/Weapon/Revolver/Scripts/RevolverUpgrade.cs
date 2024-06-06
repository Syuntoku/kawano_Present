using Syuntoku.DigMode.UI;

namespace Syuntoku.DigMode.Tool
{
    public class RevolverUpgrade : WeaponUpgrade
    {

        string[] ToolUpgladeInfoText = {
            "�_���[�W,�_���[�W,�_���[�W,�_���[�W,�_���[�W",
            "�����[�h���x,�����[�h���x,�����[�h���x,�����[�h���x,�����[�h���x",
            "�}�K�W����,�}�K�W����,�}�K�W����,�}�K�W����,�}�K�W����",
        };

        float[] _weaponAmountOfChangeInLevel =
        {
            0.2f,
            0.2f,
            0.2f,
            0.2f,
            0.4f,
        };
        float[] _reroadSpeed_amountOfChangeInLevel =
        {
            0.1f,
            0.1f,
            0.1f,
            0.15f,
            0.15f,
        };
        int[] _magazinCount_amountOfChangeInLevel =
        {
            10,
            10,
            20,
            30,
            40,
        };
        public override void FirstUpGlade()
        {
            base.FirstUpGlade();

            _attackPower += _weaponAmountOfChangeInLevel[_firstUpglade_level];
        }

        public override void SecondUpGlade()
        {
            base.SecondUpGlade();
            _reroadSpeedUpMagnification += _reroadSpeed_amountOfChangeInLevel[_secondUpglade_level];
        }

        public override void ThirdUpGlade()
        {
            base.ThirdUpGlade();
            _reroadSpeedUpMagnification += _magazinCount_amountOfChangeInLevel[_thirdUpglade_level];
        }

        public override string[] GetToolUpgladeInfo()
        {
            return ToolUpgladeInfoText;
        }

        public override string GetUpGladeAmount(int step, int level)
        {
            string data = ToolUpgladeInfoText[step];
            string[] splitData = data.Split(",");
            string amount = "";

            switch (step)
            {
                case (int)Upglade2.Step.UP:
                    amount = (_weaponAmountOfChangeInLevel[level] * PERCENTAGE).ToString() + "%";
                    break;
                case (int)Upglade2.Step.MIDDLE:
                    amount = (_reroadSpeed_amountOfChangeInLevel[level] * PERCENTAGE).ToString() + "%";
                    break;
                case (int)Upglade2.Step.DOWN:
                    amount = (_magazinCount_amountOfChangeInLevel[level]).ToString();
                    break;
            }
            return splitData[level] + "," + amount;
        }

    }
}
