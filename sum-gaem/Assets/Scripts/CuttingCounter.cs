using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public override void Interact(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                // Player is carrying something
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            if (playerController.HasKitchenObject())
            {
                // Player is carrying something


            }
            else
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(playerController);
            }
        }
    }
}
