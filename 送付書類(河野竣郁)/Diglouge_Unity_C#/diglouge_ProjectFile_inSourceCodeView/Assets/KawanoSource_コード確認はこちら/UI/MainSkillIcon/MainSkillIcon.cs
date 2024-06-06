using UnityEngine;
using UnityEngine.UI;

namespace Syuntoku.DigMode.UI
{
    public class MainSkillIcon : MonoBehaviour
    {
        [SerializeField] Player.Player _player;
        [SerializeField] Image _coolTimeImage;

        private void Start()
        {
            _coolTimeImage.fillAmount = 0.0f;
        }

        private void Update()
        {
            if(_player._mainSkillManage.IsActive())
            {
                _coolTimeImage.fillAmount = 0.0f;
                return;
            }
            _coolTimeImage.fillAmount =(1.0f - _player._mainSkillManage.GetnowCoolTime() / _player._mainSkillManage.GetMaxCoolTime());
        }
    }
}
