using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimerMax = 4f;

    private float mSpawnPlateTimer;
    private int mPlatesSpawnedAmount;
    private int mPlatesSpawnedAmountMax = 4;

    private void Update()
    {
        mSpawnPlateTimer += Time.deltaTime;
        if(mSpawnPlateTimer > spawnPlateTimerMax)
        {
            mSpawnPlateTimer = 0;

            if(mPlatesSpawnedAmount < mPlatesSpawnedAmountMax)
            {
                mPlatesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasKitchenObject())
        {
            // player is not carrying anything
            if(mPlatesSpawnedAmount > 0)
            {
                // there is atleast one plate on the counter
                mPlatesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, playerController);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);  
            }
        }

    }

}
