using UnityEngine;

namespace BoatActions
{
    public class Harvester : BoatAction
    {
        /// <summary>Try to continue harvesting. Called each update</summary>
        /// <returns>1: is harvesting over ? 2: did harvesting suceed ?</returns>
        protected override (bool over, bool succeed) Execute(MonoBehaviour target, Inventory inventory)
        {
            var resource = (Resource)target;
        
            var availableSpace = inventory.AvailableSpace(resource.Type);
            var quantity = resource.Harvest(availableSpace < Quantity ? availableSpace : Quantity);
            inventory.Add(resource.Type, quantity);

            return (over: availableSpace == quantity || resource.Quantity == 0, 
                succeed: quantity > 0);
        }


        public override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            base.OnDrawGizmosSelected();
        }
    }
}
