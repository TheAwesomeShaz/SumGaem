using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.Rendering;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController playerController)
    {
        // Counter is empty
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
            // Counter already has a kitchen Object
            if (playerController.HasKitchenObject())
            {
                // Player is carrying something
                if(playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is not carrying a plate but carrying summ else (sussy?)
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter has a plate on top of it
                        if (plateKitchenObject.TryAddIngredient(playerController.GetKitchenObject().GetKitchenObjectSO())) ;
                        {
                            playerController.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            }
            else
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(playerController);
            }
        }
    }

}
