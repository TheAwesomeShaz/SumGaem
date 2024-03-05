using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    
    public class OnIngredientAddedEventArgs: EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> mKitchenObjectSOList;

    private void Awake()
    {
        mKitchenObjectSOList = new();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid Ingredient
            return false;
        }
        else
        {
            if (mKitchenObjectSOList.Contains(kitchenObjectSO))
            {
                // Already has this added
                return false;
            }
            else
            {
                mKitchenObjectSOList.Add(kitchenObjectSO);
                OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
                {
                    kitchenObjectSO = kitchenObjectSO,
                });
                return true;
            }
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return mKitchenObjectSOList;
    } 
}
