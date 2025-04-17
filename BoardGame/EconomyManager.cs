using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Mozilla;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    public steamIAP steamIAP;
    public int MushiPoint;
    public TextMeshProUGUI mushipointText;
    public Costume costume;
    public int purchasebool;

    public List<Costume> PlayerCostumes = new List<Costume>();
    //public int MushiCoin;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
       StartCoroutine(steamIAPAtama());
        
    }

    private void Start()
    {

    }
    public void BuyCostume()
    {

        if (costume != null && MushiPoint >= costume.MarketValue && costume.Purchased == false)
        {
            MushiPoint -= costume.MarketValue;
            steamIAP.SaveMushiPoint();
            mushipointText.text = MushiPoint.ToString();
            costume.Purchased = true;
            Debug.Log(costume.name + "kostümü " + costume.MarketValue + " parasýna alýndý: ");
            SaveCostumes();
        }
        
    }
    public void SaveCostumes()
    {
        string json = CostumesToJson(PlayerCostumes);
        byte[] encryptedData = EncryptStringToBytes(json);
        bool success = SteamRemoteStorage.FileWrite("Costumes.dat", encryptedData, encryptedData.Length);
        if (success)
        {
            Debug.Log("Costumes saved successfully");
        }
        else
        {
            Debug.LogError("Failed to save costumes");
        }
    }

    public void LoadCostumes()
    {
        if (!SteamRemoteStorage.FileExists("Costumes.dat"))
        {
            Debug.LogWarning("Costumes.dat does not exist");

            foreach (var costume in PlayerCostumes)
            {
                costume.Purchased = false;
            }

            SaveCostumes();
            return;
        }

        int fileSize = SteamRemoteStorage.GetFileSize("Costumes.dat");
        byte[] encryptedData = new byte[fileSize];
        SteamRemoteStorage.FileRead("Costumes.dat", encryptedData, fileSize);
        string json = DecryptStringFromBytes(encryptedData);
        Debug.Log(json);
        SetCostumesFromJson(json);
        Debug.Log("Costumes loaded successfully");
    }

    private string CostumesToJson(List<Costume> costumes)
    {
        List<CostumeData> costumeDataList = new List<CostumeData>();

        foreach (var costume in costumes)
        {
            costumeDataList.Add(new CostumeData
            {
                Type = costume.Type,
                CostumeName = costume.CostumeName,
                Purchased = costume.Purchased,
                index = costume.index
            });
        }

        return JsonUtility.ToJson(new CostumeList { costumes = costumeDataList });
    }

    private void SetCostumesFromJson(string json)
    {
        CostumeList costumeList = JsonUtility.FromJson<CostumeList>(json);

        foreach (var costumeData in costumeList.costumes)
        {
            foreach (var costume in PlayerCostumes)
            {
                if (costume.CostumeName == costumeData.CostumeName)
                {
                    costume.Purchased = costumeData.Purchased;
                    Debug.Log(costume.CostumeName +costume.Purchased);
                }
            }
        }
    }

    // JSON dönüþüm için kullanýlan sýnýflar
    [System.Serializable]
    private class CostumeList
    {
        public List<CostumeData> costumes;
    }

    [System.Serializable]
    private class CostumeData
    {
        public string Type;
        public string CostumeName;
        public int index;
        public bool Purchased;
    }





    private IEnumerator steamIAPAtama()
    {
        if (steamIAP != null)
        {
            Debug.Log("steamIAP atandý ");
            yield break;

        }
        else
        {
            while (steamIAP == null && SceneManager.GetActiveScene().name=="OfflineScene")
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("steamIAP aranýyor");
                steamIAP = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>().steamIAP;
                
            }
            while (mushipointText==null && SceneManager.GetActiveScene().name == "OfflineScene")
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("mushipointtext aranýyor");
                mushipointText= GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>().mushipointtext;
                mushipointText.text = MushiPoint.ToString();
            }

        }
       
        
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private static readonly string encryptionKey = "MushiverseSecretKeySteam"; // 16, 24, or 32 bytes key for AES

    public byte[] EncryptStringToBytes(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // AES block size is 16 bytes

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
    }

    public string DecryptStringFromBytes(byte[] cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // AES block size is 16 bytes

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
