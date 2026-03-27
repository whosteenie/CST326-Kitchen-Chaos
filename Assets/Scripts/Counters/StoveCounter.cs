using System;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State {
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
                    if(fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        
                        burningRecipeSO = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                        burningTimer = 0f;
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    if(burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        
                        state = State.Burnt;
                    }
                    break;
                case State.Burnt:
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
                    fryingTimer = 0f;
                }
            } else {
            }
        } else {
            if(player.HasKitchenObject()) {
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
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
