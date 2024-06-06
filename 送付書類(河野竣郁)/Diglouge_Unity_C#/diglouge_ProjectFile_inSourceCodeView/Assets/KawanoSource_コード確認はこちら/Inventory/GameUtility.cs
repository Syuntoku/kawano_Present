using UnityEngine;

public class GameUtility
{
    const int RANDOM_PARSENT = 101;

    /// <summary>
    ///@ƒ‰ƒ“ƒ_ƒ€‚È‚O`‚P‚O‚O‚Ì”’l‚ğæ“¾
    /// </summary>
    public static int GetRandomParsent()
    {
        return Random.Range(0, RANDOM_PARSENT);
    }

    /// <summary>
    /// ƒ‰ƒ“ƒ_ƒ€‚È“‚Æˆø”‚ğ”ä‚×‚é
    /// </summary>
    /// <param name="checkNum"></param>
    /// <returns>true: ‰º@false : ã</returns>
    public static bool CheckUnderParsent(int checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
    /// <summary>
     /// ƒ‰ƒ“ƒ_ƒ€‚È“‚Æˆø”‚ğ”ä‚×‚é
     /// </summary>
     /// <param name="checkNum"></param>
     /// <returns>true: ‰º@false : ã</returns>
    public static bool CheckUnderParsent(float checkNum)
    {
        if (GetRandomParsent() < checkNum)
        {
            return true;
        }
        return false;
    }
}
