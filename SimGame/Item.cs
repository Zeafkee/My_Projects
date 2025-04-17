using UnityEngine;

public class Item : MonoBehaviour, IInteractable, IHoldable
{
    #region Methods
    [SerializeField] private ItemSO m_Item;
    
    [SerializeField] private float absolutePrice; //npc buna göre hareket edecek.
    [SerializeField] private float marketPrice; // markete satýþ fiyatý.
    
    [SerializeField] private Status itemStatus;
    
    
    
    
    [SerializeField] private Canvas itemCanvas;
    public int ItemId => m_Item.itemId;
    

    public string ItemName => m_Item.itemName;

    public float BasePrice => m_Item.basePrice;
    public float AbsolutePrice => (float)absolutePrice;
    public int MarketPrice => (int)marketPrice;
    public Status ItemStatus => itemStatus;

    public Saleable ItemSaleable => m_Item.itemSaleable;
    public ItemRarity ItemRarity => m_Item.itemRarity; 
    #endregion
    public bool isInCar;
    public Vehicle ownedCar;

    public bool canHold;

    private void Start()
    {
        CalculatePrice(); // bu silinecek
    }
    public void CalculatePrice()
    {
        float rarityMultiplier = m_Item.itemRarity.GetRarityAttribute().ValuePercent;
        float statusMultiplier = GetStatusMultiplier();
        absolutePrice = m_Item.basePrice * rarityMultiplier * statusMultiplier;
    }

    private float GetStatusMultiplier()
    {
        switch (itemStatus)
        {
            case Status.Pristine: return 1.0f;
            case Status.BarelyUsed: return 0.9f;
            case Status.SlightlyDamaged: return 0.75f;
            case Status.Damaged: return 0.5f;
            default: return 1.0f;
        }
    }

    public void CheckMarket(Saleable marketName)
    {
        float priceMultiplier = marketName == m_Item.itemSaleable ? 1.0f : 0.7f;
        int finalPrice = Mathf.RoundToInt(absolutePrice * priceMultiplier);
        marketPrice = finalPrice ;
    }
    public void OnInteract()
    {
        Debug.Log(m_Item.itemRarity.GetRarityAttribute().Color.ToString());
        throw new System.NotImplementedException();
    }

    public GameObject ReturnHudObject()
    {
        Debug.Log("Hud Döndürüldü" + this.name);
        CalculatePrice();
        UIManager.Instance.RefreshItemUI(this);
        return UIManager.Instance.itemUI;
    }


    public bool HasHud()
    {
        return true;
    }
    public Canvas ReturnObjectCanvas()
    {
        return itemCanvas;
    }

    public void OnHold()
    {
        if(isInCar) 
        {
            ownedCar?.itemsInVehicle.Remove(this); 

            isInCar = false;
            ownedCar = null;
        }
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    public bool CanHold()
    {
        return canHold;
    }

    public void CallBeginningFunction()
    {
        UIManager.Instance.OpenItemUI(this);
    }

    public void CallEndingFunction()
    {
        UIManager.Instance.CloseItemUI();
    }
}
