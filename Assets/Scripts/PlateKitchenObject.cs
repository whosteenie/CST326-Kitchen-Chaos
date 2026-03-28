using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo) {
        if(kitchenObjectSOList.Contains(kitchenObjectSo) || !validKitchenObjectSOList.Contains(kitchenObjectSo)) {
            return false;
        }

        kitchenObjectSOList.Add(kitchenObjectSo);
        return true;
    }
}
