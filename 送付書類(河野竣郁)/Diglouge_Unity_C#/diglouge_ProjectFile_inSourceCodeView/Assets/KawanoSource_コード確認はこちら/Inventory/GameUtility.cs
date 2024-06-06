using UnityEngine;

public class GameUtility
{
    const int RANDOM_PARSENT = 101;

    /// <summary>
    ///　ランダムな０〜１００の数値を取得
    /// </summary>
    public static int GetRandomParsent()
    {
        return Random.Range(0, RANDOM_PARSENT);
    }

    /// <summary>
    /// ランダムな％と引数を比べる
    /// </summary>
    /// <param name="checkNum"></param>
    /// <returns>true: 下　false : 上</returns>
    public static bool CheckUnderParsent(int checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
    /// <summary>
     /// ランダムな％と引数を比べる
     /// </summary>
     /// <param name="checkNum"></param>
     /// <returns>true: 下　false : 上</returns>
    public static bool CheckUnderParsent(float checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
}
