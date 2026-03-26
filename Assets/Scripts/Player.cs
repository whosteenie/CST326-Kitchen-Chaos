using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter SelectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public bool IsWalking { get; private set; }
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    
    private KitchenObject kitchenObject;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        var moveDistance = moveSpeed * Time.deltaTime;
        var tr = transform;
        var position = tr.position;
        const float playerRadius = 0.7f;
        const float playerHeight = 2f;
        var canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDir,
            moveDistance);

        if(!canMove) {
            var moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirX,
                moveDistance);

            if(canMove) {
                moveDir = moveDirX;
            } else {
                var moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirZ,
                    moveDistance);

                if(canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        if(canMove) {
            tr.position += moveDir * moveDistance;
        }

        IsWalking = moveDir != Vector3.zero;

        const float rotateSpeed = 10f;
        tr.forward = Vector3.Slerp(tr.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    private void HandleInteractions() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        const float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out var hit, interactDistance, countersLayerMask)) {
            if(hit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if(baseCounter != null && baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter sCounter) {
        selectedCounter = sCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            SelectedCounter = sCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject incomingKitchenObject) {
        kitchenObject = incomingKitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}