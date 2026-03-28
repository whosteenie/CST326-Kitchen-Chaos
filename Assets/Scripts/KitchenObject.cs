using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [FormerlySerializedAs("kitchenObjectSO")]
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

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent) {
        var kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
    
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if(this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        
        plateKitchenObject = null;
        return false;
    }
}