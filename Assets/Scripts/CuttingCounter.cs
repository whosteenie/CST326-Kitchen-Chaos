using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;
    
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

    public override void InteractAlternate(Player player) {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            var outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
        }
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach (var cuttingRecipeSo in cuttingRecipeSoArray) {
            if(cuttingRecipeSo.input == inputKitchenObjectSo) {
                return true;
            }
        }

        return false;
    }
    
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach (var cuttingRecipeSo in cuttingRecipeSoArray) {
            if(cuttingRecipeSo.input == inputKitchenObjectSo) {
                return cuttingRecipeSo.output;
            }
        }
        
        Debug.LogError($"No output for {inputKitchenObjectSo.objectName}");
        return null;
    }
}