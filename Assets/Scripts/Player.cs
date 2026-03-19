using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    public bool IsWalking { get; private set; }

    private void Update() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        var moveDistance = moveSpeed * Time.deltaTime;
        var tr = transform;
        var position = tr.position;
        const float playerRadius = 0.7f;
        const float playerHeight = 2f;
        var canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if(!canMove) {
            var moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove) {
                moveDir = moveDirX;
            } else {
                var moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

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
}