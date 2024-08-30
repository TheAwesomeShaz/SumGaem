using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(PlayerController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            playerController.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
        }
    }
}
