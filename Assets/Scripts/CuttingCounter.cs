using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO cutKitchenObjectSo;
    
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
        if(HasKitchenObject()) {
            GetKitchenObject().DestroySelf();
            var kitchenObjectTransform = Instantiate(cutKitchenObjectSo.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
    }
}