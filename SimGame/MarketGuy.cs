using UnityEngine;
using System.Collections.Generic;

public class MarketGuy : MonoBehaviour ,IInteractable
{
    public GameObject vehicle;  
    public Transform _areaCenter; 
    [SerializeField]
    private Vector3 areaCenter;
    public float areaWidth = 5f; 
    public float areaDepth = 5f;
    public Saleable marketName;
    

    private void Start()
    {
        areaCenter = _areaCenter.position;
    }
    public void CheckVehicleInArea()
    {
        if (vehicle == null) { return; }

        Vector3 pos = vehicle.transform.position;
        if (pos.x >= areaCenter.x - areaWidth / 2 && pos.x <= areaCenter.x + areaWidth / 2 &&
            pos.z >= areaCenter.z - areaDepth / 2 && pos.z <= areaCenter.z + areaDepth / 2)
        {
            if(vehicle.TryGetComponent<Vehicle>(out Vehicle _vehicle))
            {
                if(_vehicle.itemsInVehicle.Count <= 0) { return; }

                    SefaManager.Instance.items.Clear();
                    foreach (Item item in _vehicle.itemsInVehicle)
                    {
                        Debug.Log("Alanýn içindeki aracýn taþýdýðý nesne: " + item.name);
                        SefaManager.Instance.items.Add(item);
                        item.CheckMarket(marketName);
                    }
                    SefaManager.Instance.SetOwnItems(SefaManager.Instance.items);
            }
        }
    }

    public void OnInteract()
    {
        SefaManager.Instance.DeleteOwnItems();
        CheckVehicleInArea();
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


