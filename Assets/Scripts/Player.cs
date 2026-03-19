using UnityEngine;

public class Player : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 7f;
    
    public bool IsWalking { get; private set; }

    private void Update() {
        var inputVector = new Vector2(0,0);

        if(Input.GetKey(KeyCode.W)) {
            inputVector.y += 1;
        }
        if(Input.GetKey(KeyCode.S)) {
            inputVector.y -= 1;
        }
        if(Input.GetKey(KeyCode.A)) {
            inputVector.x -= 1;
        }
        if(Input.GetKey(KeyCode.D)) {
            inputVector.x += 1;
        }
        
        inputVector = inputVector.normalized;

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        var tr = transform;
        tr.position += moveDir * (moveSpeed * Time.deltaTime);
        
        IsWalking = moveDir != Vector3.zero;
        
        const float rotateSpeed = 10f;
        tr.forward = Vector3.Slerp(tr.forward, moveDir, rotateSpeed * Time.deltaTime);
    }
}