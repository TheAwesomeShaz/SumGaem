using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] 
    private Transform counterTopPoint;
    private KitchenObject mKitchenObject;

    public virtual void Interact(PlayerController playerController)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(PlayerController playerController)
    {

    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.mKitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return mKitchenObject;
    }

    public void ClearKitchenObject()
    {
        mKitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return mKitchenObject != null;
    }

  
}
