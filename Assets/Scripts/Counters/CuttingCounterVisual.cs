using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {
    private static readonly int Cut = Animator.StringToHash("Cut");
    [SerializeField] private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start() {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, EventArgs e) {
        animator.SetTrigger(Cut);
    }
}
