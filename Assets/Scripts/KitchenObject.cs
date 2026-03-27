using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSo;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent incomingKitchenObjectParent) {
        if(kitchenObjectParent != null) {
            kitchenObjectParent.ClearKitchenObject();
        }
        
        kitchenObjectParent = incomingKitchenObjectParent;

        if(incomingKitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParent already has a kitchen object!");
        }
        
        incomingKitchenObjectParent.SetKitchenObject(this);

        Transform tr;
        (tr = transform).parent = incomingKitchenObjectParent.GetKitchenObjectFollowTransform();
        tr.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }
    
    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
}