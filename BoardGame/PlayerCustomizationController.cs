using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerCustomizationController : NetworkBehaviour
{
    // Head Area
    [SyncVar] public int HeadCostumeValue;

    public List<GameObject> HeadCostumeLists = new List<GameObject>();


    // Face Area
    [SyncVar] public int FaceCostumeValue;

    public List<GameObject> FaceCostumeLists = new List<GameObject>();

    [SyncVar] public int HeadMainRenkDegiskeni;
    [SyncVar] public int FaceMainRenkDegiskeni;
    public List<Material> RenkMaterials = new List<Material>(); 

    // Scriptler
    public PlayerObjectController localplayer;
    public PlayerMechanics localplayermechanics;
    public PlayerCustomizationManager customizationManager;

    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager == null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }
    #region Online Values Update
    #region HeadCostumeValue Update

    [Command(requiresAuthority = false)]
    private void CmdHeadCostumeValueUpdate(int Value)
    {
        HeadCostumeValue = Value;
    }

    public void HeadCostumeValueUpdate(int Value)
    {
        CmdHeadCostumeValueUpdate(Value);
    }


    #endregion
    #region FaceCostumeValue Update
    [Command(requiresAuthority = false)]
    private void CmdFaceCostumeValueUpdate(int Value)
    {
        FaceCostumeValue = Value;
    }

    public void FaceCostumeValueUpdate(int Value)
    {
        CmdFaceCostumeValueUpdate(Value);
    }




    #endregion

    [Command(requiresAuthority = false)]
    private void CmdHeadRenkDegiskeniUpdate(int Value)
    {
        HeadMainRenkDegiskeni = Value;
    }

    public void HeadRenkDegiskeniUpdate(int Value)
    {
        CmdHeadRenkDegiskeniUpdate(Value);
    }


    [Command(requiresAuthority = false)]
    private void CmdFaceMainRenkDegiskeniUpdate(int Value)
    {
        FaceMainRenkDegiskeni = Value;
    }

    public void FaceMainRenkDegiskeniUpdate(int Value)
    {
        CmdFaceMainRenkDegiskeniUpdate(Value);
    }

    #endregion
    #region Kostümleri Atama
    private void Awake()
    {
        manager = CustomNetworkManager.singleton as CustomNetworkManager;
    }

    private void Start()
    {
        if (localplayer.isLocalPlayer)
        {
            StartCoroutine(KostumDegiskenleriAtama());
        }   
    }
    public void SetCostumesToPlayers()
    {        
        PlayerCostumeDefault();       
        if(HeadCostumeValue != 0)
        {
            HeadCostumeLists[HeadCostumeValue].SetActive(true);
            SetMeshForColor(HeadMainRenkDegiskeni, 0);
        }

        if(FaceCostumeValue != 0)
        {
            FaceCostumeLists[FaceCostumeValue].SetActive(true); 
            SetMeshForColor(FaceMainRenkDegiskeni, 1);
        }

    }
    public void SetMeshForColor(int RenkDegiskeni, int Deger)
    {
        if(Deger == 0)
        {
            CostumeMeshes meshofcostumes = HeadCostumeLists[HeadCostumeValue].GetComponent<CostumeMeshes>();
            for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
            {
                Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                Material YeniMalzeme = RenkMaterials[RenkDegiskeni];

                Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                RenkRenderer.materials = yeniMalzemeler;
            }
        }
        else if(Deger == 1)
        {
            CostumeMeshes meshofcostumes = FaceCostumeLists[FaceCostumeValue].GetComponent<CostumeMeshes>();
            for (int i = 0; i < meshofcostumes.meshRenderers.Count; i++)
            {
                Renderer RenkRenderer = meshofcostumes.meshRenderers[i];
                Material YeniMalzeme = RenkMaterials[RenkDegiskeni];

                Material[] yeniMalzemeler = new Material[] { YeniMalzeme };

                RenkRenderer.materials = yeniMalzemeler;
            }
        }
        

    }

    public void PlayerCostumeDefault()
    {
        for (int i = 0; i < HeadCostumeLists.Count; i++)
        {
            HeadCostumeLists[i].SetActive(false);
        }

        for (int i = 0; i < FaceCostumeLists.Count; i++)
        {
            FaceCostumeLists[i].SetActive(false);
        }
    }

    private IEnumerator KostumDegiskenleriAtama()
    {
        while (!NetworkClient.ready)
        {
            yield return new WaitForSeconds(0.5f);
        }
        while(customizationManager == null)
        {
            yield return new WaitForSeconds(0.5f);
            //customizationManager = GameObject.Find("PlayerCustomizationManager").GetComponent<PlayerCustomizationManager>();
            customizationManager = manager.customizationManager;
        }
        customizationManager = manager.customizationManager;
        HeadCostumeValue = customizationManager.HeadCostumeValue;
        FaceCostumeValue = customizationManager.FaceCostumeValue;
        HeadMainRenkDegiskeni = customizationManager.HeadMainRenkDegiskeni;
        FaceMainRenkDegiskeni = customizationManager.FaceMainRenkDegiskeni;
        HeadCostumeValueUpdate(HeadCostumeValue);
        FaceCostumeValueUpdate(FaceCostumeValue);
        HeadRenkDegiskeniUpdate(HeadMainRenkDegiskeni);
        FaceMainRenkDegiskeniUpdate(FaceMainRenkDegiskeni);
    }
    #endregion
}
