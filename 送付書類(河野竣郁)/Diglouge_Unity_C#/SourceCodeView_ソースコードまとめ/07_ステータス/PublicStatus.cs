using System.Collections.Generic;

namespace Syuntoku.Status
{

    /// <summary>
    /// �����t���̃X�e�[�^�X�ύX��ݒ�ł���
    /// </summary>
    public class PublicStatus
    {
        List<Action> actions = new List<Action>();

        /// <summary>
        /// �A�N�V�����̎��
        /// </summary>
        public enum KeyAction
        {
            Time = 0x01,
            Break = 0x02,
            Probability = 0x04,
        }

        /// <summary>
        /// ���ʂ���������Ƃ��̏���
        /// </summary>
        public enum ActiveTrigger
        {
            Call,
            Break,
        }


        /// <summary>
        /// ���ԂŕύX����
        /// </summary>
        public void AddTimerChangeAction(ActiveTrigger activeAction, Digmode.StatusKind statusKind, float amount, float enebleTime, bool bTenporary = false, bool inactive = false)
        {
            TimeAction time = new TimeAction();
            time.Setup(KeyAction.Time, activeAction, statusKind, amount, enebleTime, bTenporary, inactive);
            actions.Add(time);
        }
        /// <summary>
        /// �m���ŋ��������X�e�[�^�X�ύX
        /// </summary>
        public void AddPribiltyAction(Digmode.StatusKind statusKind, float amount, int probability, bool bTenporary = false, bool inactive = false)
        {
            PribilityAction plibilityAction = new PribilityAction();
            plibilityAction.Setup(KeyAction.Probability, ActiveTrigger.Call, statusKind, amount, probability, bTenporary, inactive);
            actions.Add(plibilityAction);
        }

        /// <summary>
        /// �o�^����Ă���A�N�V�������󂯎��ƃX�e�[�^�X�ύX���J�n����
        /// </summary>
        /// <param name="statusManage"></param>
        /// <param name="activeAction"></param>
        public void SetActionTrigger(StatusManage statusManage, ActiveTrigger activeAction)
        {
            foreach (Action item in actions)
            {
                item.ActiveTrigger(statusManage, activeAction);
            }
        }

        public void Update(StatusManage statusManage)
        {
            int count = 0;
            foreach (Action item in actions)
            {
                if (!item.IsActive())
                {
                    actions.Remove(item);

                    if (actions.Count == 0) break;

                    continue;
                }

                item.Update(statusManage);
                count++;
            }
        }

        /// <summary>
        /// �w�肵���A�N�V�����̃X�e�[�^�X�ύX�����ɖ߂�
        /// </summary>
        /// <param name="statusManage"></param>
        /// <param name="activeAction"></param>
        public void DisableAction(StatusManage statusManage,KeyAction activeAction)
        {
            foreach (Action item in actions)
            {
                if (item.GetKeyAction() != activeAction) return;

                item.DisableAction(statusManage);

                if (actions.Count == 0) break;

                continue;
            }
        }

        /// <summary>
        /// �ꎞ�I�ɗL���ȃA�N�V�������폜����
        /// </summary>
        public void RemoveTenporaryAction()
        {
            foreach (Action item in actions)
            {
                if (item.IsTemporary())
                {
                    actions.Remove(item);
                    if (actions.Count == 0) break;

                    continue;
                }
            }
        }
    }
}
