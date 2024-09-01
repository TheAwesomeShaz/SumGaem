using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 mLastInteractDirection;
    private ClearCounter selectedCounter;
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
    }

    private void InputManager_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
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
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if(clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
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

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(
            this,
            new OnSelectedCounterChangedEventArgs
            {
                selectedCounter = selectedCounter
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
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt movement on Z dir
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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
}
