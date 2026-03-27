using System;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    private int cuttingProgress;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs {
        public float ProgressNormalized;
    }

    public event EventHandler OnCut;

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    var cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                        ProgressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax
                    });
                }
            } else {
            }
        } else {
            if(player.HasKitchenObject()) {
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            var cuttingRecipeSo = GetCuttingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                ProgressNormalized = (float)cuttingProgress / cuttingRecipeSo.cuttingProgressMax
            });

            if(cuttingProgress >= cuttingRecipeSo.cuttingProgressMax) {
                var outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo) {
        var cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);

        return cuttingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo) {
        var cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);

        if(cuttingRecipeSo != null) {
            return cuttingRecipeSo.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach(var cuttingRecipeSo in cuttingRecipeSoArray) {
            if(cuttingRecipeSo.input == inputKitchenObjectSo) {
                return cuttingRecipeSo;
            }
        }
        
        return null;
    }
}