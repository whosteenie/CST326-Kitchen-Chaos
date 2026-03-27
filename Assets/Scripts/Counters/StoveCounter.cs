using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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
}
