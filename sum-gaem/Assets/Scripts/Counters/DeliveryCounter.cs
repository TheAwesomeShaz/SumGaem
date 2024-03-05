using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(PlayerController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only Accept plates at the delivery counter
                playerController.GetKitchenObject().DestroySelf();
            }
        }
    }
}
