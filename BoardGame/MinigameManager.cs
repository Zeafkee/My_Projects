using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Steamworks;


public class MinigameManager : NetworkBehaviour
{
    [HideInInspector] public PlayerObjectController localplayer;
    [HideInInspector] public PlayerMechanics playerMechanics;
    [SyncVar] public List<PlayerObjectController> PlayersinGame = new List<PlayerObjectController>();
    [HideInInspector] public CustomNetworkManager manager;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public TurnManager turnManager;

    public List<PlayerMechanics> KazananOyuncu = new List<PlayerMechanics>();
    [SyncVar] public int currentPlayerIndex;
    // private int selectionsRemaining = 2; // Local player can select 2 buttons
    [HideInInspector] public List<Button> buttons = new List<Button>();
    [HideInInspector] public GameObject NineSelectorBG;
    public List<GameObject> NineSelectorSecimList = new List<GameObject>();
    [SyncVar] public bool NineSelectorMG = false;


    public GameObject TurnInfoPanel;

    // NineSelectorPrefab Yazdýrma
    public GameObject NSPlayerItemPrefab;
    public GameObject NSPlayerContent;
    public List<GameObject> NSPlayerItemLists = new List<GameObject>();



    public List<GameObject> NSCharacterImages = new List<GameObject>();
    public List<GameObject> NSPlayerPositions = new List<GameObject>();

    private void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
    }
    #region Nine Selector
    public void CreateListPlayers()
    {
        playerMechanics = localplayer.gameObject.GetComponent<PlayerMechanics>();
        if (!PlayersinGame.Contains(localplayer))
        {
            PlayersinGame.Add(localplayer);
        }
        int Deger = localplayer.PlayerIDNumber;

        if (Deger == manager.GamePlayers.Count)
        {
            Deger = 0;
        }
        else
        {
            Deger = localplayer.PlayerIDNumber;
        }

        for (int k = Deger; k < manager.GamePlayers.Count + 1; k++)
        {
            if (k == manager.GamePlayers.Count)
            {
                k = 0;
            }
            PlayerObjectController player = manager.GamePlayers[k];
            if (!PlayersinGame.Contains(player))
            {
                 PlayersinGame.Add(player);                    
            }
            else
            {
                break;
            }
        }
        NineSelectorGameStartOnline(PlayersinGame);
    }
    [Command(requiresAuthority = false)]
    private void CmdListPlayerUpdate(List<PlayerObjectController> players)
    {
        PlayersinGame = players;
    }
    [Command(requiresAuthority = false)]
    private void CmdNineSelectorGameStart(List<PlayerObjectController> players)
    {
        PlayersinGame = players;
        RPCNineSelectorGameStart(players);
    }
    [ClientRpc]
    private void RPCNineSelectorGameStart(List<PlayerObjectController> players)
    {
        PlayersinGame = players;
        StartCoroutine(NineSelectorGameStart());
    }

    private void NineSelectorGameStartOnline(List<PlayerObjectController> players)
    {
        if (isServer)
        {
            RPCNineSelectorGameStart(players);
        }
        else
        {
            CmdNineSelectorGameStart(players);
        }
    }

    private IEnumerator NineSelectorGameStart()
    {
        NineSelectorMG = true;
        NineSelectorBG.SetActive(true);
        PrintPlayersForNineSelector();
        Debug.Log("Minigame Basladi");
        for (int a = 0; a < buttons.Count; a++)
        {
            GameObject buttongameobject = buttons[a].gameObject;
            buttongameobject.SetActive(true);

            if (turnManager.currentPlayer == localplayer.gameObject)
            {
                buttons[a].interactable = true;
            }
            else
            {
                buttons[a].interactable = false;
            }
        }
        while (NineSelectorMG)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < PlayersinGame.Count; i++)
            {
                PlayerMechanics players = PlayersinGame[i].gameObject.GetComponent<PlayerMechanics>();
                if (players.NineSelectorDegisken != -1)
                {
                    if (buttons[players.NineSelectorDegisken].gameObject.activeSelf)
                    {
                        buttons[players.NineSelectorDegisken].gameObject.SetActive(false);
                    }
                    if (!NSCharacterImages[players.NineSelectorDegisken].activeSelf)
                    {
                        NSCharacterImages[players.NineSelectorDegisken].SetActive(true);

                        for (int a = 0; a < NSPlayerItemLists.Count; a++)
                        {
                            NineSelectorPlayersItem NineSelectorScript = NSPlayerItemLists[a].GetComponent<NineSelectorPlayersItem>();
                            if (NineSelectorScript.player == PlayersinGame[i])
                            {
                                NineSelectorScript.gameObject.transform.SetParent(NSPlayerPositions[players.NineSelectorDegisken].transform);
                                RectTransform rectTransform = NineSelectorScript.gameObject.GetComponent<RectTransform>();
                                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                                rectTransform.anchoredPosition = new Vector2(0f, 0f);
                            }
                        }
                    }
                }
            }
            bool HerkesSecti = false;
            for (int x = 0; x < PlayersinGame.Count; x++)
            {
                PlayerMechanics oyuncu = PlayersinGame[x].gameObject.GetComponent<PlayerMechanics>();
                if (oyuncu.NineSelectorDegisken != -1)
                {
                    HerkesSecti = true;
                }
                else
                {
                    HerkesSecti = false;
                }
            }

            if (HerkesSecti)
            {
                if (PlayersinGame[0].isLocalPlayer)
                {
                    Debug.Log("Oyuncu Seçiliyor");
                    NineSelectorMG = false;
                    KazananOyuncuSecim();
                    NineSelectorMGUpdate(NineSelectorMG);
                }
            }
            if (!HerkesSecti)
            {
                for (int a = 0; a < buttons.Count; a++)
                {
                    if (PlayersinGame.Count != 0)
                    {
                        if (PlayersinGame[currentPlayerIndex].isLocalPlayer)
                        {
                            buttons[a].interactable = true;
                            if (!TurnInfoPanel.activeSelf)
                            {
                                TurnInfoPanel.SetActive(true);
                            }
                        }
                        else
                        {
                            buttons[a].interactable = false;
                            if (TurnInfoPanel.activeSelf)
                            {
                                TurnInfoPanel.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    public void NineSelectorSayiSec(int Deger)
    {
        for (int x = 0; x < PlayersinGame.Count; x++)
        {
            PlayerMechanics player = PlayersinGame[x].gameObject.GetComponent<PlayerMechanics>();
            if (player.isLocalPlayer)
            {
                player.NineSelectorDegisken = Deger;
                player.NineSelectorUpdate(Deger);
            }
        }

        if (currentPlayerIndex != PlayersinGame.Count - 1)
        {
            OyuncuTurnGonderme(currentPlayerIndex);
            Debug.Log("Turn gönderildi");
        }
    }


    private void OyuncuTurnGonderme(int Value)
    {
        currentPlayerIndex++;
        if (isServer)
        {
            RPCOyuncuTurnGonderme(currentPlayerIndex);
        }
        else
        {
            CmdOyuncuTurnGonderme(currentPlayerIndex);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdOyuncuTurnGonderme(int Value)
    {
        currentPlayerIndex = Value;
        RPCOyuncuTurnGonderme(Value);
    }

    [ClientRpc]
    private void RPCOyuncuTurnGonderme(int Value)
    {
        currentPlayerIndex = Value;
        Debug.Log(currentPlayerIndex);
    }

    public void KazanmaGeriSayimVerme(int KazananDeger)
    {
        if (isServer)
        {
            RPCKazanmaGeriSayimVerme(KazananDeger);
        }
        else
        {
            CmdKazanmaGeriSayimVerme(KazananDeger);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdKazanmaGeriSayimVerme(int KazananDeger)
    {
        RPCKazanmaGeriSayimVerme(KazananDeger);
    }

    [ClientRpc]
    private void RPCKazanmaGeriSayimVerme(int KazananDeger)
    {
        StartCoroutine(KazanmaGeriSayim(KazananDeger));
    }
    public IEnumerator KazanmaGeriSayim(int KazananDeger)
    {
        if (TurnInfoPanel.activeSelf)
        {
            TurnInfoPanel.SetActive(false);
        }

        bool Secim = true;
        bool Durmak = false;
        float secimhizi = 0.01f;
        int SecilenDeger = 0;
        while (Secim)
        {
            yield return new WaitForSeconds(secimhizi);
            for (int i = 0; i < NineSelectorSecimList.Count; i++)
            {
                GameObject Secilen = NineSelectorSecimList[i];
                if (Secilen.activeSelf)
                {
                    Secilen.SetActive(false);
                }
            }
            GameObject Secilen2 = NineSelectorSecimList[SecilenDeger];
            Secilen2.SetActive(true);
            if (secimhizi <= 0.2f)
            {
                SecilenDeger++;
            }

            if (SecilenDeger == NineSelectorSecimList.Count)
            {
                SecilenDeger = 0;
            }
            if (secimhizi <= 0.2f)
            {
                secimhizi += 0.01f;
            }
            else
            {
                Secim = false;
                Durmak = true;
            }
        }

        while (Durmak)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < NineSelectorSecimList.Count; i++)
            {
                GameObject Secilen = NineSelectorSecimList[i];
                if (Secilen.activeSelf)
                {
                    Secilen.SetActive(false);
                }
            }
            if (SecilenDeger == KazananDeger)
            {
                Durmak = false;
            }
            else
            {
                SecilenDeger++;
            }
            if (SecilenDeger == NineSelectorSecimList.Count)
            {
                SecilenDeger = 0;
            }
            NineSelectorSecimList[SecilenDeger].SetActive(true);
        }
        if(PlayersinGame.Count > 0)
        {
            if (PlayersinGame[0].isLocalPlayer)
            {
                if (KazananOyuncu.Count != 0)
                {
                    KazananOyuncuVerileriCheck(KazananOyuncu[0]);
                }
                else
                {
                    Debug.Log("Kimse Kazanamadý");
                }
            }
        }
        else
        {
            Debug.Log("Minigamede bir hata ver Playersingame count 0 olmamalý");
        }     
        Invoke("ListedekileriSilme", 2f);
    }

    private void KazananOyuncuSecim()
    {
        int x = Random.Range(0, 9);
        Debug.Log("SecilenSayi" + x);

        for (int i = 0; i < PlayersinGame.Count; i++)
        {
            PlayerMechanics players = PlayersinGame[i].gameObject.GetComponent<PlayerMechanics>();

            if (x == players.NineSelectorDegisken)
            {
                KazananOyuncu.Add(players);
                Debug.Log(players + " Kazandý");
            }
        }   
        KazanmaGeriSayimVerme(x);
    }

    private void KazananOyuncuVerileriCheck(PlayerMechanics player)
    {
        if (player.NorulFlama < 2)
        {
            player.NorulFlama++;
            gameManager.FlamaDeger = gameManager.FlamaDeger + (gameManager.FlamaDeger * 1 / 2);
            player.NorulFlamaUcreti = gameManager.FlamaDeger;
            player.NorulFlamaUpdate(player.NorulFlama, player.NorulFlamaUcreti);
            gameManager.ActionMinigameKazanan(true,player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
        }
        else
        {
            player.Para += 10000;
            player.ParaUpdate(player.Para);
            gameManager.ActionMinigameKazanan(false, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdNineSelectorMGUpdate(bool Value)
    {
        NineSelectorMG = Value;
        RPCNineSelectorMGUpdate(Value);
    }

    private void NineSelectorMGUpdate(bool Value) // Minigame Sonu
    {
        if (isServer)
        {
            RPCNineSelectorMGUpdate(Value);
        }
        else
        {
            CmdNineSelectorMGUpdate(Value);
        }
    }

    [ClientRpc]
    private void RPCNineSelectorMGUpdate(bool Value)
    {
        NineSelectorMG = Value;
    }

    private void ListedekileriSilme()
    {
        if (PlayersinGame.Count > 0)
        {
            //if (PlayersinGame[0].isLocalPlayer)
            //{
            for (int i = 0; i < PlayersinGame.Count; i++)
            {
                PlayerMechanics player = PlayersinGame[i].gameObject.GetComponent<PlayerMechanics>();

                player.NineSelectorDegisken = -1;
                player.NineSelectorUpdate(player.NineSelectorDegisken);
            }
            //}
        }

        if (NSPlayerItemLists.Count != 0)
        {
            for (int i = 0; i < NSPlayerItemLists.Count; i++)
            {
                GameObject SilinecekObjeler = NSPlayerItemLists[i];
                Destroy(SilinecekObjeler);
            }
            NSPlayerItemLists.Clear();
        }

        for (int i = 0; i < NSCharacterImages.Count; i++)
        {
            if (NSCharacterImages[i].activeSelf)
            {
                NSCharacterImages[i].SetActive(false);
            }
        }
        currentPlayerIndex = 0;
        for (int i = 0; i < NineSelectorSecimList.Count; i++)
        {
            GameObject Secilen = NineSelectorSecimList[i];
            if (Secilen.activeSelf)
            {
                Secilen.SetActive(false);
            }
        }
        if (NineSelectorBG.activeSelf)
        {
            NineSelectorBG.SetActive(false);
        }
        if (PlayersinGame[0].isLocalPlayer)
        {
            gameManager.NextTurnAtma();
        }
        PlayersinGame.Clear();
        //CmdListPlayerUpdate(PlayersinGame);
        KazananOyuncu.Clear();
    }
    private void PrintPlayersForNineSelector()
    {
        for (int i = 0; i < PlayersinGame.Count; i++)
        {
            GameObject NineSelectorPlayerItem = Instantiate(NSPlayerItemPrefab) as GameObject;
            NineSelectorPlayersItem NineSelectorScript = NineSelectorPlayerItem.GetComponent<NineSelectorPlayersItem>();
            PlayerObjectController oyuncu = PlayersinGame[i];

            NineSelectorScript.PlayerName = oyuncu.PlayerName;
            NineSelectorScript.minigameManager = this;
            NineSelectorScript.player = oyuncu;
            NineSelectorScript.SetItemValues();

            NineSelectorPlayerItem.transform.SetParent(NSPlayerContent.transform);
            NSPlayerItemLists.Add(NineSelectorPlayerItem);
        }
    }


    public void AppearCharacterImage(int Value)
    {
        if (PlayersinGame.Count > 0 && NineSelectorMG)
        {
            if (PlayersinGame[currentPlayerIndex] == localplayer)
            {
                for (int i = 0; i < NSCharacterImages.Count; i++)
                {
                    GameObject Obje = NSCharacterImages[i];

                    if (Obje.activeSelf)
                    {
                        Obje.SetActive(false);
                    }
                }
                NSCharacterImages[Value].SetActive(true);

                for (int i = 0; i < NSPlayerItemLists.Count; i++)
                {
                    NineSelectorPlayersItem NineSelectorScript = NSPlayerItemLists[i].GetComponent<NineSelectorPlayersItem>();

                    if (NineSelectorScript.player.isLocalPlayer)
                    {
                        NineSelectorScript.gameObject.transform.SetParent(NSPlayerPositions[Value].transform);
                        RectTransform rectTransform = NineSelectorScript.gameObject.GetComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        rectTransform.anchoredPosition = new Vector2(0f, 0f);
                        //NineSelectorScript.gameObject.transform.localPosition = Vector3.zero;

                    }
                }
            }
        }
    }
    #endregion
}

