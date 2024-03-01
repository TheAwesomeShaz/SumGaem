using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] 
    private float moveSpeed = 7f;

    [SerializeField] 
    private InputManager inputManager;

    [SerializeField] 
    private LayerMask countersLayerMask;

    [SerializeField]
    private Transform kitchenObjectHoldPoint;

    private Vector3 mLastInteractDirection;
    private BaseCounter mSelectedCounter;
    private KitchenObject mKitchenObject;

    public bool IsWalking {get;set;}


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than one Player detected");
        }
        Instance = this;
    }
    private void Start()
    {
        inputManager.OnInteractAction += InputManager_OnInteractAction;
        inputManager.OnInteractAlternateAction += InputManager_OnInteractAlternateAction; ;
    }

    private void InputManager_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (mSelectedCounter != null)
        {
            mSelectedCounter.InteractAlternate(this);
        }
    }

    private void InputManager_OnInteractAction(object sender, System.EventArgs e)
    {
        if (mSelectedCounter != null)
        {
            mSelectedCounter.Interact(this);
        }
    }

    private void HandleInteractions()
    {
        float interactDistance = 2f;
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            mLastInteractDirection = moveDir;
        }
        if (Physics.Raycast(transform.position, mLastInteractDirection, out RaycastHit hit, interactDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter counter))
            {
                if(counter != mSelectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);

            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        mSelectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(
            this,
            new OnSelectedCounterChangedEventArgs
            {
                selectedCounter = mSelectedCounter
            }
        );
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        float rotateSpeed = 10f;
        bool canMove;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        Vector2 inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // If A wall nearby

            // Attempt movement on X dir
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = /*moveDir.x != 0 &&*/ !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt movement on Z dir
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = /*moveDir.z != 0 &&*/ !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot Move
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        IsWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
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
