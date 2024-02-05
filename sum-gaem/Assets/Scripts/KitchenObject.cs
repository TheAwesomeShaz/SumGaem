using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private ClearCounter mParentCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO; 
    }

    public void SetParentCounter(ClearCounter clearCounter)
    {
        if (mParentCounter != null)
        {
            mParentCounter.ClearKitchenObject();
        }

        mParentCounter = clearCounter;

        if (mParentCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a Kitchen Object");
        }
        else
        {
            clearCounter.SetKitchenObject(this);
        }


        transform.parent = mParentCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetParentCounter()
    {
        return mParentCounter;
    }
}
