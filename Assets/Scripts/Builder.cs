using System.Linq;
using UnityEngine;

public class Builder : BoatAction
{
    /// <summary>Try to continue building. Called each update</summary>
    /// <returns>is building action over ? inventory empty/missmatch or structure finished</returns>
    protected override bool Execute(MonoBehaviour target, Inventory inventory)
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
            return true; //missingItems.Count == 1 && inventoryQuantity >= buildQuantity;
        }
        return false;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        base.OnDrawGizmos();
    }
}
