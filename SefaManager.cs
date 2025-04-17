using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SefaManager : MonoBehaviour
{
    public static SefaManager Instance;

    public List<Item> items = new List<Item>();
    public List<MarketItem> marketItems = new List<MarketItem>();

    public GameObject shopItemUI;
    public GameObject marketItemUI;
    public GameObject shopItemContent;
    public GameObject marketItemContent;
    public List<GameObject> spawnedShopItems = new List<GameObject>();
    public List<GameObject> spawnedMarketItems = new List<GameObject>();

    public TextMeshProUGUI fpsText;
    private float deltaTime;

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
    }
    private void Start()
    {
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";

        if(Input.GetKey(KeyCode.Z))
        {
            DeleteOwnItems();
            if (shopItemUI.activeSelf) { shopItemUI.SetActive(false); }
            MapManager.Instance.player.stateMachine.NextState(MapManager.Instance.player.moveState);
        }
    }
    public void SetOwnItems(List<Item> _items)
    {
        if (!shopItemUI.activeSelf) { shopItemUI.SetActive(true); }

        for (int i = 0; i < _items.Count; i++)
        {
            Item itemScript = _items[i];

            GameObject shopItemObject = ObjectsManager.Instance.SpawnItemShopObject();
            if (spawnedShopItems.Contains(shopItemObject)) { return; }
            if (shopItemObject.TryGetComponent<ShopItemUI>(out ShopItemUI shopItemScript))
            {
                shopItemScript.SetValues(itemScript.name, itemScript.MarketPrice,itemScript.ownedCar,itemScript);
            }
            shopItemObject.transform.SetParent(shopItemContent.transform);
            spawnedShopItems.Add(shopItemObject);
        }
    }

    public void SetMarketItems(List<MarketItem> _items)
    {
        if (!marketItemUI.activeSelf) { marketItemUI.SetActive(true); }

        for (int i = 0; i < _items.Count; i++)
        {
            MarketItem itemScript = _items[i];

            GameObject marketItemObject = ObjectsManager.Instance.SpawnItemMarketObject();
            if (spawnedMarketItems.Contains(marketItemObject)) { return; }
            if (marketItemObject.TryGetComponent<MarketItemUI>(out MarketItemUI marketItemScript))
            {
                marketItemScript.SetValues( itemScript);
            }
            marketItemObject.transform.SetParent(marketItemContent.transform);
            spawnedMarketItems.Add(marketItemObject);
        }
    }

    public void DeleteOwnItems()
    {
        if (spawnedShopItems.Count != 0)
        {
            foreach (GameObject item in spawnedShopItems)
            {
                Destroy(item);
            }
            spawnedShopItems.Clear();
        }
    }
}
