using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        var burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        var playSound = e.state is StoveCounter.State.Frying or StoveCounter.State.Fried;
        if(playSound) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }

    private void Update() {
        if(playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if(warningSoundTimer <= 0f) {
                var warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;
            
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
