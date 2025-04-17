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
    private string supabaseUrl = "https://xvuvlljwjptwnnivwlzl.supabase.co";
    private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inh2dXZsbGp3anB0d25uaXZ3bHpsIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDExMTEzMDEsImV4cCI6MjA1NjY4NzMwMX0.YcPK0pwU1E1IRe9Q_3kLqBadQwmiDIFV7zta7Wabg0g";

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
        string url = $"{supabaseUrl}/rest/v1/{playersTable}?select=*";
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
        string url = $"{supabaseUrl}/rest/v1/{playersTable}";
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

        string url = $"{supabaseUrl}/rest/v1/{objectsTable}";

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
        string url = $"{supabaseUrl}/rest/v1/{objectsTable}?name=eq.{objectName}&select=posX,posY,posZ";

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
        string url = $"{supabaseUrl}/rest/v1/{objectsTable}?name=eq.{objectName}";

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
