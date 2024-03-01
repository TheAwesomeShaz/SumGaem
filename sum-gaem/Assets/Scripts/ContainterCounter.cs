using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainterCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, GetKitchenObjectFollowTransform());
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
