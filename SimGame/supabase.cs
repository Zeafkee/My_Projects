using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;



[System.Serializable]
class PositionData
{
    public string name;
    public float posX;
    public float posY;
    public float posZ;

    public PositionData(string name, Vector3 position)
    {
        this.name = name;
        this.posX = position.x;
        this.posY = position.y;
        this.posZ = position.z;
    }
}

public class supabase : MonoBehaviour
{
    private string supabaseUrl = "..";
    private string supabaseKey = "...";

    private string playersTable = "players";
    private string objectsTable = "objects";

    public string objectName = "Cube1"; // Objeyi tanýmlamak için
    public GameObject monkey;

    void Start()
    {
        StartCoroutine(LoadPosition());
    }

    IEnumerator GetPlayers()
    {
        string url = $"{supa...";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Oyuncular: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }
    }

    IEnumerator AddPlayer(string name, int score)
    {
        string url = $"{sup...le}";
        string json = $"{{\"name\": \"{name}\", \"score\": {score}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Oyuncu eklendi: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }
    }

    public void SavePosition()
    {
        SavePositionAndDeleteOld();
    }

    IEnumerator SaveObjectPosition(Transform objTransform)
    {
        PositionData posData = new PositionData(objectName, objTransform.position);
        string json = JsonUtility.ToJson(posData);

        string url = $"{su...}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Konum kaydedildi: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }
    }

    IEnumerator LoadPosition()
    {
        string url = $"{sup....}";

        Debug.Log("1");

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        Debug.Log("2");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("3");
            string jsonResult = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(jsonResult) && jsonResult.Length > 2)
            {
                Debug.Log("4");
                jsonResult = jsonResult.Trim('[', ']'); // Supabase dizisini temizle

                PositionData posData = JsonUtility.FromJson<PositionData>(jsonResult);

                Debug.Log("5");

                if (posData != null)
                {
                    Vector3 loadedPosition = new Vector3(posData.posX, posData.posY, posData.posZ);
                    monkey.transform.position = loadedPosition;

                    Debug.Log("Konum yüklendi: " + loadedPosition);
                }
                else
                {
                    Debug.LogError("Konum verisi çözümlenemedi.");
                }
            }
            else
            {
                Debug.Log("Kayýtlý konum bulunamadý.");
            }
        }
        else
        {
            Debug.LogError("Hata: " + request.error);
        }
    }

    IEnumerator DeleteObjectPosition()
    {
        string url = $"{....}";

        UnityWebRequest request = new UnityWebRequest(url, "DELETE");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Veritabanýndan {objectName} kaydý baþarýyla silindi.");
        }
        else
        {
            Debug.LogError("Silme hatasý: " + request.error);
        }
    }

    public void SavePositionAndDeleteOld()
    {
        StartCoroutine(DeleteAndSavePosition());  
    }

    IEnumerator DeleteAndSavePosition()
    {
        yield return StartCoroutine(DeleteObjectPosition());

        yield return StartCoroutine(SaveObjectPosition(monkey.transform));
    }

}
