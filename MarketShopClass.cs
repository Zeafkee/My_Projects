using UnityEngine;
using System.Collections.Generic;

public class MarketNPC : MonoBehaviour, IInteractable
{
    public List<MarketItem> sellList;

    private void Start()
    {
    }
    public void SellTabOpen()
    {
        SefaManager.Instance.items.Clear();

        foreach (MarketItem item in sellList)
        {
            SefaManager.Instance.marketItems.Add(item);
        }

        SefaManager.Instance.SetMarketItems(SefaManager.Instance.marketItems);


    }

    public void OnInteract()
    {
        SefaManager.Instance.DeleteOwnItems();
        SellTabOpen();
        MapManager.Instance.player.stateMachine.NextState(MapManager.Instance.player.interactState);
    }

    public GameObject ReturnHudObject()
    {

        GameObject _hudObject = ObjectsManager.Instance.SpawnMarketHudObject();
        //if (_hudObject.TryGetComponent<HudSystem>(out HudSystem hudSystem))
        //{
        //    hudSystem.SetInteractableHud("Press E To Get In");
        //}
        return _hudObject;

    }

    public bool HasHud()
    {
        return false;
    }

    public Canvas ReturnObjectCanvas()
    {
        return UIManager.Instance.screenCanvas;
    }
    public void CallBeginningFunction()
    {
        throw new System.NotImplementedException();
    }

    public void CallEndingFunction()
    {
        throw new System.NotImplementedException();
    }
}
