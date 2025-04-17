using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharacterColors : MonoBehaviour
{
    public GameObject KarakterKafa;
    public GameObject KarakterBody;

    public List<Material> RenkMaterials;

    public List<Material> CostumeRenkMaterials;
    public int RenkDegiskeni;


    public List<GameObject> HeadCostumeLists = new List<GameObject>();
    public List<GameObject> FaceCostumeLists = new List<GameObject>();

    public int HeadCostumeValue;
    public int FaceCostumeValue;

    public int HeadRenkDegisken;
    public int FaceRenkDegisken;

    public bool CustomizationActive;


    private void Start()
    {
        CharacterRenkleriDuzenleme();
    }
    public void CharacterRenkleriDuzenleme()
    {
        Renderer CharacterBodyRenderer = KarakterBody.gameObject.GetComponent<SkinnedMeshRenderer>();
        Renderer CharacterKafaRenderer = KarakterKafa.gameObject.GetComponent<SkinnedMeshRenderer>();

        Material YeniMalzeme2 = RenkMaterials[RenkDegiskeni];

        Material[] yeniMalzemeler2 = new Material[] { YeniMalzeme2 };

        CharacterBodyRenderer.materials = yeniMalzemeler2;
        CharacterKafaRenderer.materials = yeniMalzemeler2;
    }


    public void PreviousOption()
    {
        if (RenkDegiskeni == 0)
        {
            RenkDegiskeni = RenkMaterials.Count - 1;
        }
        else
        {
            RenkDegiskeni--;
        }
        CharacterRenkleriDuzenleme();
    }


    public void NextOption()
    {
        if (RenkDegiskeni == RenkMaterials.Count - 1)
        {
            RenkDegiskeni = 0;
        }
        else
        {
            RenkDegiskeni++;
        }
        CharacterRenkleriDuzenleme();
    }

    public void HeadCustomizationButtonPreview(int Value, int Renk, bool Shop)
    {
        HeadCostumeValue = Value;
        HeadRenkDegisken = Renk;
        if(CustomizationActive)
        {
            HeadCostumeGiydirme();
        }
        else if(Shop)
        {
            HeadCostumeGiydirmeForShop();
        }    
    }

    public void FaceCustomizationButtonPreview(int Value, int Renk, bool Shop) // Atanacak
    {
        FaceCostumeValue = Value;
        FaceRenkDegisken = Renk;
        if (CustomizationActive)
        {
            FaceCostumeGiydirme();
        }
        else if (Shop)
        {
            FaceCostumeGiydirmeForShop();
        }
    }
    public void HeadCostumeGiydirme()
    {
        if (CustomizationActive)
        {
            for (int i = 0; i < HeadCostumeLists.Count; i++)
            {
                HeadCostumeLists[i].SetActive(false);
            }

            HeadCostumeLists[HeadCostumeValue].SetActive(true);

            if (HeadCostumeValue != 0)
            {
                CostumeMeshes meshofcostumes = HeadCostumeLists[HeadCostumeValue].GetComponent<CostumeMeshes>();
                for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
                {
                    Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                    Material YeniMalzeme = CostumeRenkMaterials[HeadRenkDegisken];

                    Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                    RenkRenderer.materials = yeniMalzemeler;
                }
            }
        }     
    }

    public void HeadCostumeGiydirmeForShop()
    { 
            for (int i = 0; i < HeadCostumeLists.Count; i++)
            {
                HeadCostumeLists[i].SetActive(false);
            }

            HeadCostumeLists[HeadCostumeValue].SetActive(true);

            if (HeadCostumeValue != 0)
            {
                CostumeMeshes meshofcostumes = HeadCostumeLists[HeadCostumeValue].GetComponent<CostumeMeshes>();
                for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
                {
                    Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                    Material YeniMalzeme = CostumeRenkMaterials[HeadRenkDegisken];

                    Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                    RenkRenderer.materials = yeniMalzemeler;
                }
            }        
    }
    public void FaceCostumeGiydirme()
    {
        if (CustomizationActive)
        {
            for (int i = 0; i < FaceCostumeLists.Count; i++)
            {
                FaceCostumeLists[i].SetActive(false);
            }

            FaceCostumeLists[FaceCostumeValue].SetActive(true);

            if (FaceCostumeValue != 0)  
            {
                CostumeMeshes meshofcostumes = FaceCostumeLists[FaceCostumeValue].GetComponent<CostumeMeshes>();
                for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
                {
                    Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                    Material YeniMalzeme = CostumeRenkMaterials[FaceRenkDegisken];

                    Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                    RenkRenderer.materials = yeniMalzemeler;
                }
            }
        }
    }

    private void FaceCostumeGiydirmeForShop()
    {
        for (int i = 0; i < FaceCostumeLists.Count; i++)
        {
            FaceCostumeLists[i].SetActive(false);
        }

        FaceCostumeLists[FaceCostumeValue].SetActive(true);

        if (FaceCostumeValue != 0)
        {
            CostumeMeshes meshofcostumes = FaceCostumeLists[FaceCostumeValue].GetComponent<CostumeMeshes>();
            for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
            {
                Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                Material YeniMalzeme = CostumeRenkMaterials[FaceRenkDegisken];

                Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                RenkRenderer.materials = yeniMalzemeler;
            }
        }
    }
}