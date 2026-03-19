using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private Animator animator;
    [SerializeField] private Player player;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool(IsWalking, player.IsWalking);
    }

    private void Update() {
        animator.SetBool(IsWalking, player.IsWalking);
    }
}