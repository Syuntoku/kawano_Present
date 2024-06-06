using System.Collections.Generic;

namespace Syuntoku.Status
{

    /// <summary>
    /// 条件付きのステータス変更を設定できる
    /// </summary>
    public class PublicStatus
    {
        List<Action> actions = new List<Action>();

        /// <summary>
        /// アクションの種類
        /// </summary>
        public enum KeyAction
        {
            Time = 0x01,
            Break = 0x02,
            Probability = 0x04,
        }

        /// <summary>
        /// 効果が発動するときの条件
        /// </summary>
        public enum ActiveTrigger
        {
            Call,
            Break,
        }


        /// <summary>
        /// 時間で変更する
        /// </summary>
        public void AddTimerChangeAction(ActiveTrigger activeAction, Digmode.StatusKind statusKind, float amount, float enebleTime, bool bTenporary = false, bool inactive = false)
        {
            TimeAction time = new TimeAction();
            time.Setup(KeyAction.Time, activeAction, statusKind, amount, enebleTime, bTenporary, inactive);
            actions.Add(time);
        }
        /// <summary>
        /// 確率で強化されるステータス変更
        /// </summary>
        public void AddPribiltyAction(Digmode.StatusKind statusKind, float amount, int probability, bool bTenporary = false, bool inactive = false)
        {
            PribilityAction plibilityAction = new PribilityAction();
            plibilityAction.Setup(KeyAction.Probability, ActiveTrigger.Call, statusKind, amount, probability, bTenporary, inactive);
            actions.Add(plibilityAction);
        }

        /// <summary>
        /// 登録されているアクションを受け取るとステータス変更を開始する
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
        /// 指定したアクションのステータス変更を元に戻す
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
        /// 一時的に有効なアクションを削除する
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
