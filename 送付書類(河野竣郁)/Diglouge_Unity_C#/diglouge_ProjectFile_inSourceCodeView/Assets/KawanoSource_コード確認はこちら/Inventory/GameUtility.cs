using UnityEngine;

public class GameUtility
{
    const int RANDOM_PARSENT = 101;

    /// <summary>
    ///�@�����_���ȂO�`�P�O�O�̐��l���擾
    /// </summary>
    public static int GetRandomParsent()
    {
        return Random.Range(0, RANDOM_PARSENT);
    }

    /// <summary>
    /// �����_���ȁ��ƈ������ׂ�
    /// </summary>
    /// <param name="checkNum"></param>
    /// <returns>true: ���@false : ��</returns>
    public static bool CheckUnderParsent(int checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
    /// <summary>
     /// �����_���ȁ��ƈ������ׂ�
     /// </summary>
     /// <param name="checkNum"></param>
     /// <returns>true: ���@false : ��</returns>
    public static bool CheckUnderParsent(float checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
}
