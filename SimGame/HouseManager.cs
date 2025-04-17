using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public Transform playerTransform;
    public List<House> houseList;
    private House selectedHouse;
    public float areaWidth = 5f;
    public float areaDepth = 5f;
    public GameObject polePrefab;
    public bool flag;
    public float timeFrequency;
    private float timer;
    public int fame;

    void Start()
    {
        flag = false;
        playerTransform = MapManager.Instance.player.transform;
        timer = timeFrequency;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timeFrequency;
            TrySelectHouse();
        }

        if (selectedHouse != null)
        {
            CheckPlayerInArea();
        }
    }


    void TrySelectHouse()
    {
        float probability = Mathf.Clamp01(0.03f + fame * 0.01f);
        if (Random.value < probability)
        {
            Debug.Log("hit");
            ChooseTradeHouse();
        }
        else
            Debug.Log("missed");
    }

    public void ChooseTradeHouse()
    {
        SelectRandomHouse();
    }

    void SelectRandomHouse()
    {
        if (houseList.Count == 0) return;

        selectedHouse = houseList[Random.Range(0, houseList.Count)];
        SpawnBoundaryPoles();
    }

    void SpawnBoundaryPoles()
    {
        if (polePrefab == null || selectedHouse == null) return;

        Vector3 center = selectedHouse.areaCenter;
        float halfWidth = areaWidth / 2;
        float halfDepth = areaDepth / 2;

        Vector3[] corners = new Vector3[]
        {
            new Vector3(center.x - halfWidth, center.y, center.z - halfDepth),
            new Vector3(center.x - halfWidth, center.y, center.z + halfDepth),
            new Vector3(center.x + halfWidth, center.y, center.z - halfDepth),
            new Vector3(center.x + halfWidth, center.y, center.z + halfDepth)
        };

        foreach (Vector3 corner in corners)
        {
            Instantiate(polePrefab, corner, Quaternion.identity);
        }
    }

    void CheckPlayerInArea()
    {
        if (playerTransform == null || selectedHouse == null) return;

        Vector3 pos = playerTransform.position;
        Vector3 center = selectedHouse.areaCenter;
        if (pos.x >= center.x - areaWidth / 2 && pos.x <= center.x + areaWidth / 2 &&
            pos.z >= center.z - areaDepth / 2 && pos.z <= center.z + areaDepth / 2)
        {
            if (!flag)
            {
                NpcManager.Instance.SefaSpawnSellers(1, new List<Transform> { selectedHouse.npcSpawnPoint });
                flag = true;
            }
        }
    }
}
