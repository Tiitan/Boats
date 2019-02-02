using System.Linq;
using UnityEngine;

namespace BoatActions
{
    public class Builder : BoatAction
    {
        /// <summary>Try to continue building. Called each update</summary>
        /// <returns>is building action over ? inventory empty/missmatch or structure finished</returns>
        protected override (bool over, bool succeed) Execute(IBoatActionTarget target, Inventory inventory)
        {
            var blueprint = (Blueprint)target;

            var missingItems = blueprint.MissingItems.ToList();
            var missingItem = missingItems.FirstOrDefault(x => inventory.Items.ContainsKey(x.Type));
            if (missingItem.Quantity > 0)
            {
                int inventoryQuantity = inventory.Items[missingItem.Type].Quantity;
                int buildQuantity = missingItem.Quantity <= inventoryQuantity ? missingItem.Quantity : inventoryQuantity;
                buildQuantity = buildQuantity <= Quantity ? buildQuantity : Quantity;
                inventory.Pick(missingItem.Type, buildQuantity);
                blueprint.Build(missingItem.Type, buildQuantity);
                return (over: missingItems.Count == 1 && missingItem.Quantity == buildQuantity,
                    succeed: true);
            }
            return (over: true,
                succeed: false);
        }

        public override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            base.OnDrawGizmosSelected();
        }
    }
}
