using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    
    public enum State {
        Idle,
        Frying,
        Fried,
        Burnt
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    
    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if(HasKitchenObject()) {
            switch(state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        ProgressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    
                    if(fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        ProgressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    
                    if(burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        
                        state = State.Burnt;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Burnt:
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        ProgressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    
                    fryingRecipeSO = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                    
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        ProgressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    
                    fryingTimer = 0f;
                }
            } else {
            }
        } else {
            if(player.HasKitchenObject()) {
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    ProgressNormalized = 0f
                });
            }
        }
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo) {
        var fryRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);

        return fryRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo) {
        var fryRecipeSo = GetFryingRecipeSoWithInput(inputKitchenObjectSo);

        if(fryRecipeSo != null) {
            return fryRecipeSo.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach(var fryRecipeSo in fryingRecipeSOArray) {
            if(fryRecipeSo.input == inputKitchenObjectSo) {
                return fryRecipeSo;
            }
        }
        
        return null;
    }
    
    private BurningRecipeSO GetBurningRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach(var burnRecipeSo in burningRecipeSOArray) {
            if(burnRecipeSo.input == inputKitchenObjectSo) {
                return burnRecipeSo;
            }
        }
        
        return null;
    }
}
