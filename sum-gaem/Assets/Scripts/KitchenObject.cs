using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent mKitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO; 
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (mKitchenObjectParent != null)
        {
            mKitchenObjectParent.ClearKitchenObject();
        }

        mKitchenObjectParent = kitchenObjectParent;

        if (mKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectParent already has a Kitchen Object");
        }
        else
        {
            kitchenObjectParent.SetKitchenObject(this);
        }


        transform.parent = mKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return mKitchenObjectParent;
    }
}
