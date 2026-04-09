using System;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private static readonly int IsFlashing = Animator.StringToHash("IsFlashing");

    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        
        animator.SetBool(IsFlashing, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        var burnShowProgressAmount = 0.5f;
        var show = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;

        animator.SetBool(IsFlashing, show);
    }
}