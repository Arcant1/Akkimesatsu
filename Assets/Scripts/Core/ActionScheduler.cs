using UnityEngine;
namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currectAction;
        public void StartAction(IAction action)
        {
            if (currectAction == action) return;
            currectAction?.Cancel();
            currectAction = action;
        }
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
