using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs: EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float mFryingTimer;
    private float mBurningTimer;
    private FryingRecipeSO mFryingRecipeSO;
    private BurningRecipeSO mBurningRecipeSO;
    private State mCurrentState;

    private void Start()
    {
        mCurrentState = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (mCurrentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                        mFryingTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = mFryingTimer / mFryingRecipeSO.fryingTimerMax
                        });

                        if (mFryingTimer > mFryingRecipeSO.fryingTimerMax)
                        {
                            // Fried
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(mFryingRecipeSO.output, this);
                            Debug.Log("ObjectFried");
                            mBurningTimer = 0f;
                            mBurningRecipeSO = GetBruningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                            mCurrentState = State.Fried;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = mCurrentState
                            });
                        }
                    break;
                case State.Fried:
                    mBurningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = mBurningTimer / mBurningRecipeSO.burningTimerMax,
                    });
                    if (mBurningTimer > mBurningRecipeSO.burningTimerMax)
                    {
                        // Burned
                        GetKitchenObject().DestroySelf();
                        Debug.Log("ObjectBurned!");
                        KitchenObject.SpawnKitchenObject(mBurningRecipeSO.output, this);
                        mCurrentState = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = mCurrentState
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f,
                        }) ;
                    }
                    break;
                case State.Burned:
                    break;
                default:
                    break;
            }
        }

        
    }

    public override void Interact(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            // Counter is Empty
            if (playerController.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(playerController.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be cut
                    playerController.GetKitchenObject().SetKitchenObjectParent(this);
                    mFryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    mFryingTimer = 0f;
                    mCurrentState = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = mCurrentState
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = mFryingTimer / mFryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        
        else
        {
            // Counter has a Kitchen Object on it
            if (playerController.HasKitchenObject())
            {
                // Player is carrying something
                if (playerController.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        mCurrentState = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = mCurrentState
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f,
                        });
                    }
                }
            }
            else
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(playerController);
                mCurrentState= State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = mCurrentState
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f,
                });
            }
        }
    }
    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBruningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
