using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    public bool IsWalking { get; private set; }

    private void Update() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        var tr = transform;
        tr.position += moveDir * (moveSpeed * Time.deltaTime);

        IsWalking = moveDir != Vector3.zero;

        const float rotateSpeed = 10f;
        tr.forward = Vector3.Slerp(tr.forward, moveDir, rotateSpeed * Time.deltaTime);
    }
}