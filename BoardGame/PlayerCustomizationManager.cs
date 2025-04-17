using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerCustomizationManager : MonoBehaviour
{
    public static PlayerCustomizationManager Instance;

    public int HeadCostumeValue;
    public List<GameObject> HeadCostumeLists = new List<GameObject>();

    public int FaceCostumeValue;

    public List<GameObject> FaceCostumeLists = new List<GameObject>();



    public GameObject PlayerPrefab;
    public GameObject Player;


    public MainMenuCharacterColors CharacterColorManager; // In MainMenu

    public int HeadMainRenkDegiskeni;
    public int FaceMainRenkDegiskeni;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadPlayerCustomization();
    }

    public void PreviousOption()
    {
        CharacterColorManager.PreviousOption();
    }

    public void NextOption()
    {
        CharacterColorManager.NextOption();
    }
    public void SpawnPlayerObject(bool Customization)
    {
        GameObject player = Instantiate(PlayerPrefab) as GameObject;
        Quaternion rotate = Quaternion.Euler(0, 180, 0);
        player.transform.rotation = rotate;
        Player = player;    
        CharacterColorManager = player.GetComponent<MainMenuCharacterColors>();
        if (Customization)
        {
            CharacterColorManager.CustomizationActive = true;
        }
        else
        {
            CharacterColorManager.CustomizationActive = false;
        }
    }


    public void DestroyPlayerObject()
    {
        if (Player != null)
        {
            Destroy(Player);
        }
    }
    public void SavePlayerCustomization()
    {
        PlayerPrefs.SetInt("HeadCostumeValue", HeadCostumeValue);
        PlayerPrefs.SetInt("FaceCostumeValue", FaceCostumeValue);
        PlayerPrefs.SetInt("HeadColorValue", HeadMainRenkDegiskeni);
        PlayerPrefs.SetInt("FaceColorValue", FaceMainRenkDegiskeni);
        PlayerPrefs.Save(); // Deðiþiklikleri kaydet     
    }

    // Yükleme fonksiyonu
    public void LoadPlayerCustomization()
    {
        if (PlayerPrefs.HasKey("HeadCostumeValue"))
        {
            HeadCostumeValue = PlayerPrefs.GetInt("HeadCostumeValue");
        }
        if (PlayerPrefs.HasKey("FaceCostumeValue"))
        {
            FaceCostumeValue = PlayerPrefs.GetInt("FaceCostumeValue");
        }
        if (PlayerPrefs.HasKey("HeadColorValue"))
        {
            HeadMainRenkDegiskeni = PlayerPrefs.GetInt("HeadColorValue");
        }
        if (PlayerPrefs.HasKey("FaceColorValue"))
        {
            FaceMainRenkDegiskeni = PlayerPrefs.GetInt("FaceColorValue");
        }
        HeadCostumeButton();
    }

    public void HeadCostumePreview(int Value, int Renk)
    {
        CharacterColorManager.HeadCustomizationButtonPreview(HeadCostumeValue, HeadMainRenkDegiskeni, false);
    }
    public void HeadCostumePreviewForShop(int Value, int Renk)
    {
        CharacterColorManager.HeadCustomizationButtonPreview(Value, Renk, true);
    }
    public void FaceCostumePreview()
    {
        CharacterColorManager.FaceCustomizationButtonPreview(FaceCostumeValue, FaceMainRenkDegiskeni, false);
    }

    public void FaceCostumePreviewForShop(int Value, int Renk) // Atanacak.
    {
        CharacterColorManager.FaceCustomizationButtonPreview(Value, Renk, true);
    }
    public void HeadCostumeButton()
    {
        StartCoroutine(CharacterColorManagerBekletme());
    }
    private IEnumerator CharacterColorManagerBekletme()
    {
        while (CharacterColorManager == null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        HeadCostumePreview(HeadCostumeValue, HeadMainRenkDegiskeni);
        FaceCostumePreview();
    }



}