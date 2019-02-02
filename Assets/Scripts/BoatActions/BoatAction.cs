#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using EventArgs;
using UnityEngine;

namespace BoatActions
{
    public abstract class BoatAction : MonoBehaviour
    {
        [SerializeField] private float _actionFrequency = 1f;
        [SerializeField] private int _range = 20;
        [SerializeField] private int _quantity = 1;

        private float _lastExecute;

        public event EventHandler<BoatActionArg> NotifyBoatAction;


        protected int Quantity => _quantity;

        /// <summary>Try to continue an action. Called each update</summary>
        /// <returns>Action over ?</returns>
        public bool TryExecute(IBoatActionTarget target, Inventory inventory)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < _range &&
                _lastExecute + _actionFrequency * target.ActionFrequencyMultiplier < Time.timeSinceLevelLoad)
            {
                _lastExecute = Time.timeSinceLevelLoad;
                (bool over, bool succeed) = Execute(target, inventory);
                if (succeed)
                    NotifyBoatAction?.Invoke(this, new BoatActionArg(target.transform.position, _actionFrequency, _lastExecute, _quantity));

                return over;
            }
            return false;
        }

        protected abstract (bool over, bool succeed) Execute(IBoatActionTarget target, Inventory inventory);

        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
