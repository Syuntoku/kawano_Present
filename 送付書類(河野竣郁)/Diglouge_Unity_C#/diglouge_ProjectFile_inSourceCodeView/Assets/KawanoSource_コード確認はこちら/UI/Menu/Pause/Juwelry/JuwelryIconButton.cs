using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JuwelryIconButton : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _countText;
    [SerializeField] TMP_Text _resultWeight;

    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    public void SetTextCount(int count,int weight)
    {
        _countText.SetText(count.ToString());
        _resultWeight.SetText((count * weight).ToString());
    }
}
