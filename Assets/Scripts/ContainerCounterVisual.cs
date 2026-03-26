using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour {
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");
    [SerializeField] private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;

    private void Start() {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e) {
        animator.SetTrigger(OpenClose);
    }
}
