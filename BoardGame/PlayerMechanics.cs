using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices; // event ekleyince system ve unity engine'deki randomlar çakýþýyor
using System.Security.Cryptography;

public class PlayerMechanics : NetworkBehaviour
{
    #region Deðiþkenler

    public string Karakter;
    public Animator Anim;
    public event EventHandler OnCharacterSkill;
    public CharacterSkillAnimationScipt characterSkillAnimationScipt;
    [SyncVar] public bool Bankrupted;
    [SyncVar] public int AtilanZar;
    [SyncVar] public int steps;
    [SyncVar] public int zar1;
    [SyncVar] public int zar2;
    [SyncVar] public int zar3;
    [SyncVar] public int routePosition;
    public int routePositionn;
    [SyncVar] public int ciftzarsayisi = 0;
    [SyncVar] public double Para;
    public bool isMoving = false;
    [SyncVar] public int kodesSayaci = 0;
    [SyncVar] public bool kodes;
    [SyncVar] public int tursayisi = 0;
    [SyncVar] public int harvestorstacks = 0;
    [SyncVar] public int tatilzaratisi;
    [SyncVar] public float karakterozelliksans;
    [SyncVar] public bool karakterozellikaktif = false;
    [SyncVar] public bool karakterozellikpasif = false;
    //[SerializeField][SyncVar] public int OyuncuNumarasi = 0;
    [SyncVar] public bool OyuncuHazir = false;
    [SyncVar] public string Teamname;
    [SyncVar] public bool IflasDurumu = false;
    [SyncVar] public int NorulFlama = 0;
    [SyncVar] public bool Kazanmak = false;
    [SyncVar] public double NorulFlamaUcreti = 0;
    public int ZesuOzellikSayisi = 0;
    public bool ZesuOzellikAktif;
    public bool AnzoKoyu = false;
    [SyncVar] public int TemporsiumMoral = 50;
    public bool TemporsiumZarAtti = false;
    [SyncVar] public double TemporsiumKatsayisi = 1;
    public RawImage Panel029;

    public bool OyulKiraOdememek = false;

    [SyncVar] public int RenkDegiskeni;
    public int zarsayaci = 0;

    public float moveSpeed = 5f;
    public static bool won = false;

    [SyncVar] public bool dicebutton = false;


    public bool CharacterOpak = false;
    public int onemlibirdegerdegil = 0; // Client tarafýnda senkronizesiz denenecek.
    public int evsayisi1; //eðer Senkronize etmezsek client tarafýnda çalýþýr mý sadece denenecek.




    // GameObjects
    public Button satinalmabutonu;
    [SyncVar] public bool satinbutonu = false;

    public Button evalmabutonu;
    [SyncVar] public bool evbutonu = false;

    public Button gerisatinalmabutonu;
    [SyncVar] public bool gerialmabutonu = false;

    public Button evsayisibtn1;
    public bool evsayisibtna = false;

    public Button evsayisibtn2;
    public bool evsayisibtnb = false;

    public Button evsayisibtn3;
    public bool evsayisibtnc = false;

    public Button evsayisibtn4;
    public bool evsayisibtnd = false;

    public Button OtelSatinAlma;
    public Button VazgecmeButonu;

    public Button rollDiceButton;
    public Button kodesciftzarButton;
    public Button kodesparaodeButton;
    private MulkBilgisi _currentMulk;
    DepremMekanigi depremMekanigi;

    public TurnManager turnManager;
    public GameObject TurnManagement;
    public Route currentRoute;
    public GameObject Route;

    public Text Kodeskalantur;
    //public GameObject startPoint;
    public GameObject selectedCharacter; // Seçilen özelleþmiþ karakterin referansý
    public GameObject Tablet;
    public GameObject MulklerimTablet; //Prefab 
    public GameObject MulklerimPanel;
    public GameObject MulklerContent;
    public List<GameObject> TabletMulksItems = new List<GameObject>();
    public bool TabletAcik = false;
    public bool MulkSatmaZorunlulugu = false;

    public bool TeamKiraDuzenleme = false;

    public int Baslangic = 0;

    [SyncVar] public int Oyuncuyagelensirasayisi = 0;
    public bool Kodesici = false;
    public bool SkillPanelMode;
    public GameObject ReachObject;


    [SyncVar] public List<MulkBilgisi> Mulkler; // Mülkler listesini nasýl güncelleyeceðim bilmiyorum. Denenecek.
    [SyncVar] public List<MulkBilgisi> Prefabs = new List<MulkBilgisi>(); // MulkBilgisi listesi 
    [SerializeField] public GameManager gameManager;

    public List<PlayerMechanics> AyniKaredekiPlayerler = new List<PlayerMechanics>();
    public List<PlayerMechanics> KodeseGonderilenler = new List<PlayerMechanics>();

    [SerializeField] public int currentadim;

    public bool SiraBelirlendi = false;

    public bool KodeseGonderildi = false;

    public bool OyunBitti = false;

    public PlayerObjectController PlayerObjectController;

    public int NaidoAdimSayisi;

    [SyncVar] public int NineSelectorDegisken = -1;
    #region Tutulacak Veriler
    public int PlayerToplamAdim;// yapýldý
    public int PlayerToplamSatinAlinanMulkDegeri;
    public int PlayerToplamOdenenKira;
    public int PlayerToplamAlinanKira;
    public int PlayerToplamEldeEdilenPara;
    [SyncVar] public bool Kazandi;

    #endregion

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

    #endregion
    #region Fixed - Update And Start And Awake
    private void Awake()
    {
        manager = CustomNetworkManager.singleton as CustomNetworkManager;

    }
    public void StartStates()
    {
        //OnCharacterSkill += PlayerMechanics_OnCharacterSkill;
        satinbutonu = false;
        evbutonu = false;
        gerialmabutonu = false;
        evsayisibtna = false;
        evsayisibtnb = false;
        evsayisibtnc = false;
        evsayisibtnd = false;
        kodes = false;
        karakterozellikaktif = false;
        karakterozellikpasif = false;
        NineSelectorDegisken = -1;
        KarakterOzellikAktifUpdate(karakterozellikaktif);
        KarakterOzellikPasifUpdate(karakterozellikpasif);
        KarakterOzelligiYineleme();
        PlayersStartPosition();

        Anim = GetComponent<Animator>();


        Karakter = PlayerObjectController.Karakter;
        RenkDegiskeni = PlayerObjectController.RenkDegiskeni;
        Teamname = PlayerObjectController.Teamname;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(IflasEtmek());
        }
    }
    private void FixedUpdate()
    {
        if (!OyunBitti && SceneManager.GetActiveScene().name == "Mushigame")
        {
            /*if (!SiraBelirlendi && isLocalPlayer && OyuncuNumarasi == 1 && OyuncuHazir)
            {
                turnManager.RastgeleOyuncuylaBaslat();
                SiraBelirlendi = true;
            }*/
            if (!OyuncuHazir && NetworkClient.isConnected)
            {
                Atamalar();
                Teamname = gameObject.GetComponent<PlayerObjectController>().Teamname;
            }
            if (OyuncuHazir == true && isLocalPlayer && turnManager.HerkesHazir && manager.OyuncularBaglandi)
            {

                if (dicebutton && turnManager.currentPlayer == gameObject)
                {
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (!dicebutton)
                {
                    rollDiceButton.gameObject.SetActive(false);
                }
                //Kodestesin();
                if (satinbutonu && turnManager.currentPlayer == gameObject)
                {
                    satinalmabutonu.gameObject.SetActive(true);
                    satinalmabutonu.interactable = true;
                }

                if (!satinbutonu && turnManager.currentPlayer == gameObject)
                {
                    satinalmabutonu.gameObject.SetActive(false);
                }

                if (evbutonu && turnManager.currentPlayer == gameObject)
                {
                    evsayisibtn1.gameObject.SetActive(true);

                    evsayisibtn2.gameObject.SetActive(true);

                    evsayisibtn3.gameObject.SetActive(true);

                    evsayisibtn4.gameObject.SetActive(true);

                    if (_currentMulk.EvOtelSayisi == 0)
                    {
                        if (tursayisi == 1)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = true;
                        }
                        else if (tursayisi == 2)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = true;
                            evsayisibtn1.interactable = true;
                        }
                        else if (tursayisi >= 3)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = true;
                            evsayisibtn2.interactable = true;
                            evsayisibtn1.interactable = true;
                        }
                    }
                    else if (_currentMulk.EvOtelSayisi == 1)
                    {
                        if (tursayisi == 1)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi == 2)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = true;
                            evsayisibtn1.interactable = false;
                        }
                        else if (tursayisi >= 3)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = true;
                            evsayisibtn2.interactable = true;
                            evsayisibtn1.interactable = false;
                        }
                    }
                    else if (_currentMulk.EvOtelSayisi == 2)
                    {
                        if (tursayisi == 1)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi == 2)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi >= 3)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = true;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                        }
                    }
                    else if (_currentMulk.EvOtelSayisi == 3)
                    {
                        if (tursayisi == 1) //burada next turn denenecek.
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi == 2)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi >= 3 && NorulFlama < 2)
                        {
                            evsayisibtn4.interactable = false;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                            evbutonu = false;

                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (tursayisi >= 3 && NorulFlama >= 2)
                        {
                            evsayisibtn4.interactable = true;
                            evsayisibtn3.interactable = false;
                            evsayisibtn2.interactable = false;
                            evsayisibtn1.interactable = false;
                        }
                    }
                    else if (_currentMulk.EvOtelSayisi == 4)
                    {
                        evsayisibtn4.interactable = false;
                        evsayisibtn3.interactable = false;
                        evsayisibtn2.interactable = false;
                        evsayisibtn1.interactable = false;
                        evbutonu = false;

                        evsayisibtn1.gameObject.SetActive(false);

                        evsayisibtn2.gameObject.SetActive(false);

                        evsayisibtn3.gameObject.SetActive(false);

                        evsayisibtn4.gameObject.SetActive(false);

                        //OtelSatinAlma.gameObject.SetActive(true);
                        // þimdilik koydum buraya otel satýn alma konulacak.
                    }

                    evalmabutonu.gameObject.SetActive(true);
                    if (evsayisi1 != 0)
                        evalmabutonu.interactable = true;
                    else
                        evalmabutonu.interactable = false;
                }


                if (evbutonu == false && turnManager.currentPlayer == gameObject)
                {
                    evalmabutonu.gameObject.SetActive(false);

                    evsayisibtn1.gameObject.SetActive(false);
                    evsayisibtn2.gameObject.SetActive(false);
                    evsayisibtn3.gameObject.SetActive(false);
                    evsayisibtn4.gameObject.SetActive(false);// deneme
                }

                if (gerialmabutonu && turnManager.currentPlayer == gameObject)
                {
                    gerisatinalmabutonu.gameObject.SetActive(true);
                    gerisatinalmabutonu.interactable = true;
                }

                if (gerialmabutonu == false && turnManager.currentPlayer == gameObject)
                {
                    gerisatinalmabutonu.gameObject.SetActive(false);
                }

                if (evsayisibtna && evsayisibtnb && evsayisibtnc && evsayisibtnd)
                {
                    evsayisi1 = 4;
                }
                else if ((evsayisibtna && evsayisibtnb && evsayisibtnc) ||
                         (evsayisibtna && evsayisibtnb && evsayisibtnd) ||
                         (evsayisibtna && evsayisibtnc && evsayisibtnd) ||
                         (evsayisibtnb && evsayisibtnc && evsayisibtnd))
                {
                    evsayisi1 = 3;
                }
                else if ((evsayisibtna && evsayisibtnb) ||
                         (evsayisibtna && evsayisibtnc) ||
                         (evsayisibtna && evsayisibtnd) ||
                         (evsayisibtnb && evsayisibtnc) ||
                         (evsayisibtnb && evsayisibtnd) ||
                         (evsayisibtnc && evsayisibtnd))
                {
                    evsayisi1 = 2;
                }
                else if (evsayisibtna || evsayisibtnb || evsayisibtnc || evsayisibtnd)
                {
                    evsayisi1 = 1;
                }
                else if (!evsayisibtna && !evsayisibtnb && !evsayisibtnc && !evsayisibtnc && !evsayisibtnd)
                {
                    evsayisi1 = 0;
                }
            }
        }
    }
    #endregion
    #region ONLINE FONKSIYONLAR    
    #region Para Update -- Atanacak
    [Command(requiresAuthority = false)]
    private void CmdParaUpdate(double NewValue)
    {
        RPCParaUpdate(NewValue);
    }

    public void ParaUpdate(double NewValue)
    {
        if (isServer)
        {
            RPCParaUpdate(NewValue);
        }
        else
        {
            CmdParaUpdate(NewValue);
        }

    }

    [ClientRpc]
    private void RPCParaUpdate(double NewValue)
    {
        Para = NewValue;
    }
    #endregion
    #region kodesSayaci, kodes
    [Command(requiresAuthority = false)]
    private void CmdKodesSayaciUpdate(int NewValue)
    {
        kodesSayaci = NewValue;
        RPCKodesSayaciUpdate(NewValue);
    }

    public void KodesSayaciUpdate(int NewValue)
    {
        CmdKodesSayaciUpdate(NewValue);
    }

    [ClientRpc]
    public void RPCKodesSayaciUpdate(int NewValue)
    {
        kodesSayaci = NewValue;
    }


    [Command(requiresAuthority = false)]
    private void CmdKodesBoolUpdate(bool NewValue)
    {
        kodes = NewValue;
        RPCKodesBoolUpdate(NewValue);
    }

    public void KodesBoolUpdate(bool NewValue)
    {
        CmdKodesBoolUpdate(NewValue);
    }

    [ClientRpc]
    private void RPCKodesBoolUpdate(bool NewValue)
    {
        kodes = NewValue;
    }





    #endregion   
    #region Karakter Özellik Þans ve Aktif Update
    [Command]
    private void CmdKarakterOzellikSansUpdate(float NewValue)
    {
        karakterozelliksans = NewValue;
        RPCKarakterOzellikSansUpdate(NewValue);
    }

    public void KarakterOzellikSansUpdate(float NewValue)
    {
        if (isOwned)
        {
            CmdKarakterOzellikSansUpdate(NewValue);
        }
    }

    [ClientRpc]
    private void RPCKarakterOzellikSansUpdate(float NewValue)
    {
        karakterozelliksans = NewValue;
    }


    [Command]
    private void CmdKarakterOzellikAktifUpdate(bool NewValue)
    {
        karakterozellikaktif = NewValue;
        RPCKarakterOzellikAktifUpdate(NewValue);
    }

    public void KarakterOzellikAktifUpdate(bool NewValue)
    {
        if (isOwned)
        {
            CmdKarakterOzellikAktifUpdate(NewValue);
        }
    }

    [ClientRpc]
    private void RPCKarakterOzellikAktifUpdate(bool NewValue)
    {
        karakterozellikaktif = NewValue;
    }

    [Command(requiresAuthority = false)]
    private void CmdKarakterOzellikPasifUpdate(bool NewValue)
    {
        RPCKarakterOzellikPasifUpdate(NewValue);
    }

    public void KarakterOzellikPasifUpdate(bool NewValue)
    {
        if (isServer)
        {
            RPCKarakterOzellikPasifUpdate(NewValue);
        }
        else
        {
            CmdKarakterOzellikPasifUpdate(NewValue);
        }
    }

    [ClientRpc]
    private void RPCKarakterOzellikPasifUpdate(bool NewValue)
    {
        karakterozellikpasif = NewValue;
        KarakterOzelligiYineleme();
    }
    #endregion 
    #region Oyuncu Hazýr Bool Update
    [Command(requiresAuthority = false)]
    private void CmdOyuncuHazirUpdate(bool NewValue)
    {
        OyuncuHazir = NewValue;
        RPCOyuncuHazirUpdate(NewValue);
    }

    public void OyuncuHazirUpdate(bool NewValue)
    {
        if (isOwned)
        {
            CmdOyuncuHazirUpdate(NewValue);
        }

    }
    [ClientRpc]
    private void RPCOyuncuHazirUpdate(bool NewValue)
    {
        OyuncuHazir = NewValue;
    }


    #endregion    
    #region Mülk Listesi Update
    [Command(requiresAuthority = false)]
    private void CmdMulkListesiUpdate(List<MulkBilgisi> Value)
    {
        Mulkler = Value;
        RPCMulkBilgisiUpdate(Value);
    }

    public void MulkListesiUpdate(List<MulkBilgisi> Value)
    {
        Mulkler = Value;
        CmdMulkListesiUpdate(Value);
    }

    [ClientRpc]
    private void RPCMulkBilgisiUpdate(List<MulkBilgisi> Value)
    {
        Mulkler = Value;
    }



    #endregion
    #region ClientsMove
    [Command]
    private void CmdClientMove(int Value, bool Yon)
    {
        RpcClientMove(Value, Yon);
    }

    public void ClientMove(int Value, bool Yon)
    {
        if (isLocalPlayer && !isServer)
        {
            CmdClientMove(Value, Yon);
        }
        else if (isServer)
        {
            RpcClientMove(Value, Yon);
        }
    }

    [ClientRpc]
    private void RpcClientMove(int Value, bool Yon)
    {
        if (!isLocalPlayer)
        {
            StartCoroutine(ClientBotMove(Value, Yon));
        }
    }
    #endregion
    #region Client Kodese Dus
    [Command(requiresAuthority = false)]
    private void CmdKodeseDusmek()
    {
        RPCKodeseDusmek();
    }

    public void KodeseDusmek()
    {
        KodeseDustun();

        if (isServer)
        {
            RPCKodeseDusmek();
        }
        else
        {
            CmdKodeseDusmek();
        }
    }

    [ClientRpc]
    private void RPCKodeseDusmek()
    {
        Invoke("KodeseDustun", 0.1f);
    }
    #endregion
    #region Flama Sayýsý ve Ucreti Update
    [Command(requiresAuthority = false)]
    private void CmdFlamaUpdate(int Deger, double Deger2)
    {
        RPCFlamaUpdate(Deger, Deger2);
    }

    public void NorulFlamaUpdate(int Sayýsý, double Ucreti)
    {
        if (isServer)
        {
            RPCFlamaUpdate(Sayýsý, Ucreti);
        }
        else
        {
            CmdFlamaUpdate(Sayýsý, Ucreti);
        }
    }

    [ClientRpc]
    private void RPCFlamaUpdate(int Deger, double Deger2)
    {
        NorulFlama = Deger;
        NorulFlamaUcreti = Deger2;
    }

    #endregion
    #region Ýflas Verme
    [Command]
    private void CMDIflasVer()
    {
        RPCIflasVer();
    }

    public void Iflasettin()
    {
        if (isServer)
        {
            RPCIflasVer();
        }
        else
        {
            CMDIflasVer();
        }
    }

    [ClientRpc]
    private void RPCIflasVer()
    {
        IflasVer();
        IflasDurumu = true;
    }

    #endregion 
    #region PlayerVeriUpdate
    [Command]
    private void CmdOyunSonuPlayerVerileriUpdate(int PlayerToplamAdim, int PlayerToplamSatinAlinanMulkDegeri, int PlayerToplamOdenenKira, int PlayerToplamAlinanKira, int PlayerToplamEldeEdilenPara, bool Kazandi)
    {
        RPCOyunSonuVerileriUpdate(PlayerToplamAdim, PlayerToplamSatinAlinanMulkDegeri, PlayerToplamOdenenKira, PlayerToplamAlinanKira, PlayerToplamEldeEdilenPara, Kazandi);
    }

    public void OyunSonuPlayerVerileriUpdate(int PlayerToplamAdim, int PlayerToplamSatinAlinanMulkDegeri, int PlayerToplamOdenenKira, int PlayerToplamAlinanKira, int PlayerToplamEldeEdilenPara, bool Kazandi)
    {
        if (isServer)
        {
            RPCOyunSonuVerileriUpdate(PlayerToplamAdim, PlayerToplamSatinAlinanMulkDegeri, PlayerToplamOdenenKira, PlayerToplamAlinanKira, PlayerToplamEldeEdilenPara, Kazandi);
        }
        else
        {
            CmdOyunSonuPlayerVerileriUpdate(PlayerToplamAdim, PlayerToplamSatinAlinanMulkDegeri, PlayerToplamOdenenKira, PlayerToplamAlinanKira, PlayerToplamEldeEdilenPara, Kazandi);
        }
    }

    [ClientRpc]
    private void RPCOyunSonuVerileriUpdate(int playerToplamAdim, int playerToplamSatinAlinanMulkDegeri, int playerToplamOdenenKira, int playerToplamAlinanKira, int playerToplamEldeEdilenPara, bool kazandi)
    {
        PlayerToplamAdim = playerToplamAdim;
        PlayerToplamSatinAlinanMulkDegeri = playerToplamSatinAlinanMulkDegeri;
        PlayerToplamOdenenKira = playerToplamOdenenKira;
        PlayerToplamAlinanKira = playerToplamAlinanKira;
        PlayerToplamEldeEdilenPara = playerToplamEldeEdilenPara;
        Kazandi = kazandi;
    }

    [Command(requiresAuthority = false)]
    private void CmdToplamAlinanKiraUpdate(int Deger)
    {
        RPCToplamAlinanKiraUpdate(Deger);
    }

    public void ToplamAlinanKiraUpdate(int Deger)
    {
        if (isServer)
        {
            RPCToplamAlinanKiraUpdate(Deger);
        }
        else
        {
            CmdToplamAlinanKiraUpdate(Deger);
        }
    }

    [ClientRpc]
    private void RPCToplamAlinanKiraUpdate(int Deger)
    {
        PlayerToplamAlinanKira = Deger;
    }

    [Command(requiresAuthority = false)]
    private void CmdToplamOdenenKiraUpdate(int Deger)
    {
        RPCToplamOdenenKiraUpdate(Deger);
    }

    public void ToplamOdenenKiraUpdate(int Deger)
    {
        if (isServer)
        {
            RPCToplamOdenenKiraUpdate(Deger);
        }
        else
        {
            CmdToplamOdenenKiraUpdate(Deger);
        }
    }

    private void RPCToplamOdenenKiraUpdate(int Deger)
    {
        PlayerToplamOdenenKira = Deger;
    }


    #endregion
    #region PlayerRol Update
    [Command]
    private void CmdRolUpdate(string Deger)
    {
        RPCRolUpdate(Deger);
    }

    public void RolUpdate(string Deger)
    {
        if (isServer)
        {
            RPCRolUpdate(Deger);
        }
        else
        {
            CmdRolUpdate(Deger);
        }
    }

    [ClientRpc]
    private void RPCRolUpdate(string Deger)
    {
        gameObject.tag = Deger;
        gameObject.GetComponent<PlayerObjectController>().Karakter = Deger;
        this.Karakter = Deger;
    }

    #endregion
    #region Dice Button Update
    [Command(requiresAuthority = false)]
    private void CmdDiceButtonUpdate(bool Deger)
    {
        RPCDiceButtonUpdate(Deger);
        dicebutton = Deger;
    }

    public void DiceButtonUpdate(bool Deger)
    {
        CmdDiceButtonUpdate(Deger);
    }

    [ClientRpc]
    private void RPCDiceButtonUpdate(bool Deger)
    {
        dicebutton = Deger;
    }
    #endregion
    #region Temporsium Moral ve Temporsium Katsayisi Update
    [Command(requiresAuthority = false)]
    private void CmdTemporsiumMoral(int Moral)
    {
        RPCTemporsiumMoral(Moral);
    }

    public void TemporsiumMoralUpdate(int Moral)
    {
        if (isServer)
        {
            RPCTemporsiumMoral(Moral);
        }
        else
        {
            CmdTemporsiumMoral(Moral);
        }
    }

    [ClientRpc]
    private void RPCTemporsiumMoral(int Moral)
    {
        TemporsiumMoral = Moral;
    }

    [Command(requiresAuthority = false)]
    private void CmdTemporsiumKatsayisi(double Katsayisi)
    {
        RPCTemporsiumKatsayisi(Katsayisi);
    }

    public void TemporsiumKatsayisiUpdate(double Katsayisi)
    {
        if (isServer)
        {
            RPCTemporsiumKatsayisi(Katsayisi);
        }
        else
        {
            CmdTemporsiumKatsayisi(Katsayisi);
        }
    }

    [ClientRpc]
    private void RPCTemporsiumKatsayisi(double Katsayisi)
    {
        TemporsiumKatsayisi = Katsayisi;
    }
    #endregion
    #region Oyuncuya Gelen Sýra Sayýsý Update
    [Command(requiresAuthority = false)]
    private void CmdOyuncuSirasi(int Deger)
    {
        RPCOyuncuSirasi(Deger);
    }

    public void OyuncuSirasi(int Deger)
    {
        if (isServer)
        {
            RPCOyuncuSirasi(Deger);
        }
        if (isClient)
        {
            CmdOyuncuSirasi(Deger);
        }
    }

    [ClientRpc]
    private void RPCOyuncuSirasi(int Deger)
    {
        Oyuncuyagelensirasayisi = Deger;
    }

    #endregion
    #region Kodes Fonksiyonu
    [Command(requiresAuthority = false)]
    private void CmdKodesFonksiyonu()
    {
        RPCKodesFonksiyonu();
    }

    private void KodesFonksiyonu()
    {
        if (isServer)
        {
            RPCKodesFonksiyonu();
        }
        else
        {
            CmdKodesFonksiyonu();
        }
    }

    [ClientRpc]
    private void RPCKodesFonksiyonu()
    {
        Kodestesin();
        Debug.Log("OyuncuKodesiCalisti");
    }
    #endregion
    #region Girilen Þehir Kartý
    [Command(requiresAuthority = false)]
    private void CmdCityCardOpen(string SahibiName, int CityEvFiyati, double SatinAlmaFiyati, MulkBilgisi mulk, PlayerMechanics player)
    {
        RPCCityCardOpen(SahibiName, CityEvFiyati, SatinAlmaFiyati, mulk, player);
    }

    public void CityCardOpen(string SahibiName, int CityEvFiyati, double SatinAlmaFiyati, MulkBilgisi mulk, PlayerMechanics player)
    {
        if (isServer)
        {
            RPCCityCardOpen(SahibiName, CityEvFiyati, SatinAlmaFiyati, mulk, player);
        }
        else
        {
            CmdCityCardOpen(SahibiName, CityEvFiyati, SatinAlmaFiyati, mulk, player);
        }
    }

    [ClientRpc]
    private void RPCCityCardOpen(string SahibiName, int CityEvFiyati, double SatinAlmaFiyati, MulkBilgisi mulk, PlayerMechanics player)
    {
        StartCoroutine(gameManager.CityCardOpen(SahibiName, CityEvFiyati, SatinAlmaFiyati, mulk, player));
    }

    #endregion
    #region RoutePosition Update
    [Command(requiresAuthority = false)]
    private void CmdRoutePositionUpdate(int Value)
    {
        routePositionn = Value;
        routePosition = Value;
        RPCRoutePositionUpdate(Value);
    }

    public void RoutePositionUpdate(int Value)
    {
        if (isServer)
        {
            RPCRoutePositionUpdate(Value);
        }
        else
        {
            CmdRoutePositionUpdate(Value);
        }
    }

    private void RPCRoutePositionUpdate(int Value)
    {
        routePositionn = Value;
        routePosition = Value;
    }


    #endregion
    #region NineSelector Deger Update
    [Command(requiresAuthority = false)]
    private void CmdNineSelectorUpdate(int Deger)
    {
        NineSelectorDegisken = Deger;
        RPCNineSelectorUpdate(Deger);
    }

    public void NineSelectorUpdate(int Deger)
    {
        if (isServer)
        {
            RPCNineSelectorUpdate(Deger);
        }
        else
        {
            CmdNineSelectorUpdate(Deger);
        }
    }

    [ClientRpc]
    private void RPCNineSelectorUpdate(int Deger)
    {
        NineSelectorDegisken = Deger;
    }


    #endregion
    #region Varýþ Noktasý 
    public void VarisNoktasiUpdate(int Deger)
    {
        if (isServer)
        {
            RPCVarisNoktasiUpdate(Deger);
        }
        else
        {
            CmdVarisNoktasiUpdate(Deger);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdVarisNoktasiUpdate(int Deger)
    {
        RPCVarisNoktasiUpdate(Deger);
    }

    [ClientRpc]
    private void RPCVarisNoktasiUpdate(int Deger)
    {
        Debug.Log("Online Deger" + Deger);
        VarisNoktasiVerme(Deger);
    }


    public void VarisNoktasiKapatma()
    {
        if (isServer)
        {
            RPCVarisNoktasiKapatma();
        }
        else
        {
            CmdVarisNoktasiKapatma();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdVarisNoktasiKapatma()
    {
        RPCVarisNoktasiKapatma();
    }

    [ClientRpc]
    private void RPCVarisNoktasiKapatma()
    {
        ReachObjectClose();
    }


    #endregion
    #region Kazanma Bool Update
    [Command(requiresAuthority = false)]
    private void CmdKazanmaBoolUpdate(bool Value)
    {
        Kazanmak = Value;
    }

    public void KazanmaBoolUpdate(bool Value)
    {
        Kazanmak = Value;
        CmdKazanmaBoolUpdate(Value);
    }
    #endregion
    #region Effects Update
    [Command(requiresAuthority = false)]
    private void CmdKodesCornerEffectsUpdate()
    {
        RPCKodesCornerEffectsUpdate();
    }

    public void KodesCornerEffectsUpdate()
    {
        if (isServer)
        {
            RPCKodesCornerEffectsUpdate();
        }
        else
        {
            CmdKodesCornerEffectsUpdate();
        }
    }

    [ClientRpc]
    private void RPCKodesCornerEffectsUpdate()
    {
        gameManager.KodesCornerEffects();
    }


    [Command(requiresAuthority = false)]
    private void CmdChangeRouteCornerEffectsUpdate()
    {
        RPCChangeRouteCornerEffectsUpdate();
    }

    public void ChangeRouteCornerEffectsUpdate()
    {
        if (isServer)
        {
            RPCChangeRouteCornerEffectsUpdate();
        }
        else
        {
            CmdChangeRouteCornerEffectsUpdate();
        }
    }

    [ClientRpc]
    private void RPCChangeRouteCornerEffectsUpdate()
    {
        gameManager.ChangeRouteCornerEffects();
    }
    #endregion
    #region TurnItem Update
    [Command(requiresAuthority = false)]
    private void CmdTurnItemUpdate(int PlayerIdNumber, bool Value)
    {
        RPCTurnItemUpdate(PlayerIdNumber, Value);
    }

    public void TurnItemUpdate(int PlayerIdNumber, bool Value)
    {
        if (isServer)
        {
            RPCTurnItemUpdate(PlayerIdNumber, Value);
        }
        else
        {
            CmdTurnItemUpdate(PlayerIdNumber, Value);
        }
    }

    [ClientRpc]
    private void RPCTurnItemUpdate(int PlayerIdNumber, bool Value)
    {
        if (gameManager == null)
        {
            StartCoroutine(WaitGameManager(PlayerIdNumber, Value));
        }
        else
        {
            gameManager.PlayerVeriTurnItem(PlayerIdNumber, Value);
        }

    }

    private IEnumerator WaitGameManager(int PlayerIdNumber, bool Value)
    {
        while (gameManager == null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        gameManager.PlayerVeriTurnItem(PlayerIdNumber, Value);
    }

    #endregion
    #region Ýflas Update
    [Command(requiresAuthority = false)]
    private void CmdBankruptedUpdate(bool Value)
    {
        Bankrupted = Value;
        RPCBankruptedUpdate(Value);
    }

    public void BankruptedUpdate(bool Value)
    {
        Bankrupted = Value;
        if (isServer)
        {
            RPCBankruptedUpdate(Value);
        }
        else
        {
            CmdBankruptedUpdate(Value);
        }
    }

    [ClientRpc]
    private void RPCBankruptedUpdate(bool Value)
    {
        Bankrupted = Value;
    }
    #endregion
    #region Iflas Edince Outmap
    [Command(requiresAuthority = false)]
    private void CmdPlayerToOutMap()
    {
        RPCOutMap();
    }

    public void PlayerToOutMapUpdate()
    {
        if (isServer)
        {
            RPCOutMap();
        }
        else
        {
            CmdPlayerToOutMap();
        }
    }

    [ClientRpc]
    public void RPCOutMap()
    {
        PlayerToOutMap();
    }


    #endregion
    #region Atamalar Update

    [Command(requiresAuthority = false)]
    private void CmdAtamalar()
    {
        RPCAtamalar();
    }
    public void Atamalar()
    {
        if (isOwned)
        {
            if (gameManager != null)
            {
                currentRoute = gameManager.Route;
                turnManager = gameManager.TurnManager;
                turnManager.FindLocalPlayer(gameObject);
                satinalmabutonu = gameManager.satinalmabutonu;
                evalmabutonu = gameManager.evalmabutonu;
                gerisatinalmabutonu = gameManager.gerisatinalmabutonu;
                evsayisibtn1 = gameManager.evsayisibtn1;
                evsayisibtn2 = gameManager.evsayisibtn2;
                evsayisibtn3 = gameManager.evsayisibtn3;
                evsayisibtn4 = gameManager.evsayisibtn4;
                kodesciftzarButton = gameManager.kodesciftzarButton;
                kodesparaodeButton = gameManager.kodesparaodeButton;
                OtelSatinAlma = gameManager.OtelSatinAlma;
                rollDiceButton = gameManager.rollDiceButton;
                VazgecmeButonu = gameManager.VazgecmeButonu;

                satinalmabutonu.gameObject.SetActive(false);
                evalmabutonu.gameObject.SetActive(false);
                gerisatinalmabutonu.gameObject.SetActive(false);
                evsayisibtn1.gameObject.SetActive(false);
                evsayisibtn2.gameObject.SetActive(false);
                evsayisibtn3.gameObject.SetActive(false);
                evsayisibtn4.gameObject.SetActive(false);
                kodesciftzarButton.gameObject.SetActive(false);
                kodesparaodeButton.gameObject.SetActive(false);
                OtelSatinAlma.gameObject.SetActive(false);
                VazgecmeButonu.gameObject.SetActive(false);
                Kodeskalantur = gameManager.Kodeskalantur;
                Kodeskalantur.gameObject.SetActive(false);
                depremMekanigi = gameManager.depremMekanigi;
                Tablet = gameManager.Tablet;
                MulklerimPanel = gameManager.MulklerimPanel;
                MulklerContent = gameManager.MulklerContent;
                MulklerimPanel.gameObject.SetActive(false);
                Tablet.gameObject.SetActive(false);
                ReachObject = gameManager.ReachObject;
                //TabletManager.Instance.OyuncuCagirma(this);
                Tablet.GetComponent<TabletManager>().OyuncuCagirma(this);

                if (turnManager.HerkesHazir && this.isActiveAndEnabled)
                {
                    /*rollDiceButton.onClick.AddListener(RollDice);
                    satinalmabutonu.onClick.AddListener(SatýnAlmaIslemýniBaslat);
                    evalmabutonu.onClick.AddListener(EvAl);
                    gerisatinalmabutonu.onClick.AddListener(RakipArsayiSatýnAl);
                    evsayisibtn1.onClick.AddListener(btnaktif1);
                    evsayisibtn2.onClick.AddListener(btnaktif2);
                    evsayisibtn3.onClick.AddListener(btnaktif3);
                    evsayisibtn4.onClick.AddListener(btnaktif4);
                    kodesciftzarButton.onClick.AddListener(KodesCiftDeneme);
                    kodesparaodeButton.onClick.AddListener(kodesParaOde);
                    //OtelSatinAlma.onClick.AddListener(OtelSatinAl);
                    VazgecmeButonu.onClick.AddListener(IslemlerdenVazgec);*/

                    if (isServer)
                    {
                        DepremSehriSec();
                        if (manager.AsikSehirler)
                        {
                            gameManager.LovingCitySecim();
                        }
                    }
                    StartStates();
                    OyuncuHazir = true;
                    OyuncuHazirUpdate(true);
                    CmdAtamalar();
                }
            }
        }
    }
    [ClientRpc]
    private void RPCAtamalar()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (TurnManagement == null)
        {
            turnManager = gameManager.TurnManager;
            turnManager.FindLocalPlayer(gameObject);
            ReachObject = gameManager.ReachObject;
        }
        if (currentRoute == null)
        {
            currentRoute = gameManager.Route;
        }

        StartStates();
        SetPosition();
    }
    #endregion
    #endregion
    #region FONKSIYONLAR
    #region Pozisyon Atama
    public void SetPosition()
    {
        if (manager.GamePlayers.Count == 4)
        {
            if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 1)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 2)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 3)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 4)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 5.012f);
            }
        }
        else if (manager.GamePlayers.Count == 5)
        {
            if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 1)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 2)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 3)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 4)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 5)
            {
                transform.position = new Vector3(-3.67f, 12.312f, 5.65f);
            }
        }
        else if (manager.GamePlayers.Count == 6)
        {
            if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 1)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 2)
            {
                transform.position = new Vector3(-3.089f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 3)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 6.05f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 4)
            {
                transform.position = new Vector3(-4.2f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 5)
            {
                transform.position = new Vector3(-3.67f, 12.312f, 5.012f);
            }
            else if (gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber == 6)
            {
                transform.position = new Vector3(-3.67f, 12.312f, 6.26f);
            }
        }
        else if (manager.GamePlayers.Count == 1)
        {
            transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
        }
        else if (manager.GamePlayers.Count == 2)
        {
            transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
        }
        else if (manager.GamePlayers.Count == 3)
        {
            transform.position = new Vector3(-3.089f, 12.312f, 5.012f);
        }
    }

    public void PlayersStartPosition()
    {
        transform.position = new Vector3(20.39f, 12.312f, -2.98f);
    }

    #endregion
    #region OnTriggerEnter Fonksiyonu
    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer && OyuncuHazir)
        {
            #region Tatil Yeri
            if (other.CompareTag("Tatil") && zarsayaci == 0) // tatili otomatik satýn alsýn.
            {
                gameManager.NineSelectionStart();
                /*_currentMulk = other.gameObject.GetComponent<MulkBilgisi>();
                if (_currentMulk.Sahibi == null && Para >= _currentMulk.SatinAlmaFiyati)
                {
                    Para -= _currentMulk.SatinAlmaFiyati;
                    ParaUpdate(Para);
                    _currentMulk.ArsaSahibiUpdate(this);
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralArtti(5);
                    }
                    Debug.Log($"{_currentMulk.Isim} tatil yeri {_currentMulk.SatinAlmaFiyati} fiyatýna alýndý.");

                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }
                else if (_currentMulk.Sahibi == null && Para < _currentMulk.SatinAlmaFiyati)
                {
                    Debug.Log("Tatil yeri satýn alýnamadý. Yeterli paranýz yok!.");


                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }
                else if (_currentMulk.Sahibi != this && _currentMulk != null && Teamname != _currentMulk.Sahibi.Teamname)
                {
                    if (Teamname == "Oyul" && !OyulKiraOdememek)
                    {
                        tatilzaratisi = Random.Range(2, 13);
                        Debug.Log($"{tatilzaratisi} atýldý.");
                        int AnlikSahipOlunanEvSayisi = 0;
                        foreach (MulkBilgisi mulk in _currentMulk.Sahibi.Mulkler)
                        {
                            AnlikSahipOlunanEvSayisi += mulk.EvOtelSayisi;
                        }
                        int tatilodeme = 100 * tatilzaratisi * AnlikSahipOlunanEvSayisi;
                        if (Para > tatilodeme)
                        {
                            Para -= tatilodeme;
                            ParaUpdate(Para);
                            PlayerToplamOdenenKira += tatilodeme;
                            ToplamOdenenKiraUpdate(PlayerToplamOdenenKira);
                            _currentMulk.Sahibi.Para += tatilodeme;
                            _currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);
                            _currentMulk.Sahibi.PlayerToplamAlinanKira += tatilodeme;
                            _currentMulk.Sahibi.ToplamAlinanKiraUpdate(_currentMulk.Sahibi.PlayerToplamAlinanKira);
                            Debug.Log($"{_currentMulk.Isim} tatil yeri için {tatilodeme} ödendi.");
                            if (Karakter == "Temporsium")
                            {
                                TemporsiumMoralAzaldi(5);
                            }
                            if (_currentMulk.Sahibi.Karakter == "Temporsium")
                            {
                                _currentMulk.Sahibi.TemporsiumMoralArtti(5);
                            }
                            if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                                StartCoroutine(WaitAndNextTurn());
                        }
                        else if (Para < tatilodeme)
                        {
                            KodeseDusmek();
                        }
                    }
                    else if (Teamname == "Oyul" && OyulKiraOdememek)
                    {
                        if (Karakter == "Temporsium")
                        {
                            TemporsiumMoralArtti(5);
                        }
                        if (_currentMulk.Sahibi.Karakter == "Temporsium")
                        {
                            _currentMulk.Sahibi.TemporsiumMoralAzaldi(5);
                        }
                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());
                    }
                    else
                    {
                        tatilzaratisi = Random.Range(2, 13);
                        Debug.Log($"{tatilzaratisi} atýldý.");
                        int AnlikSahipOlunanEvSayisi = 0;
                        foreach (MulkBilgisi mulk in _currentMulk.Sahibi.Mulkler)
                        {
                            AnlikSahipOlunanEvSayisi += mulk.EvOtelSayisi;
                        }
                        int tatilodeme = 100 * tatilzaratisi * AnlikSahipOlunanEvSayisi;
                        if (Para > tatilodeme)
                        {
                            Para -= tatilodeme;
                            ParaUpdate(Para);
                            PlayerToplamOdenenKira += tatilodeme;
                            ToplamOdenenKiraUpdate(PlayerToplamOdenenKira);
                            _currentMulk.Sahibi.Para += tatilodeme;
                            _currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);
                            _currentMulk.Sahibi.PlayerToplamAlinanKira += tatilodeme;
                            _currentMulk.Sahibi.ToplamAlinanKiraUpdate(_currentMulk.Sahibi.PlayerToplamAlinanKira);
                            Debug.Log($"{_currentMulk.Isim} tatil yeri için {tatilodeme} ödendi.");
                            if (Karakter == "Temporsium")
                            {
                                TemporsiumMoralAzaldi(5);
                            }
                            if (_currentMulk.Sahibi.Karakter == "Temporsium")
                            {
                                _currentMulk.Sahibi.TemporsiumMoralArtti(5);
                            }
                            if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                                StartCoroutine(WaitAndNextTurn());
                        }
                        else if (Para < tatilodeme)
                        {
                            KodeseDusmek();
                        }
                    }

                }
                else if (_currentMulk.Sahibi != this && _currentMulk != null && Teamname == _currentMulk.Sahibi.Teamname)
                {
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }
                else if (_currentMulk.Sahibi = this)
                {
                    harvestorstacks += 3; // tatil yerinden 3 stack alýr.
                    if (harvestorstacks >= 7)
                    {
                        HasatToplama();
                    }

                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }*/
            }
            #endregion
            #region Kodes Yeri
            else if (other.CompareTag("Kodes") && zarsayaci == 0)
            {
                if (kodes == false)
                {
                    KodesTriggerAktif();
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralAzaldi(10);
                    }
                }
            }
            #endregion
            #region Arsa Yeri
            else if (other.CompareTag("Arsa") && zarsayaci == 0)
            {
                StartCoroutine(TriggerArsa(other));
            }
            #endregion
            #region StartPoint Yeri
            else if (other.CompareTag("StartPoint"))
            {
                if (tursayisi != 0)
                {
                    Para += 5000;
                    PlayerToplamEldeEdilenPara += 5000;
                }
                tursayisi += 1;
                if (zarsayaci == 0)
                {
                    //Para += 7500;
                    if (tursayisi != 1)
                    {
                        gameManager.ParaDusurme(7500, this.gameObject.transform, gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber, gameObject.GetComponent<PlayerObjectController>().RenkDegiskeni, this);
                    }
                    PlayerToplamEldeEdilenPara += 7500;
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralArtti(5);
                    }
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }
                ParaUpdate(Para);
            }
            #endregion
            #region Kervan Yeri
            else if (other.CompareTag("Kervan") && zarsayaci == 0)
            {
                if (this.Teamname == "Kiya")
                {
                    KiyaKoyuParaToplama();
                }
                else if (this.Teamname == "Naido") //bir kiþi seçilecek karakterþans 0 olacak.
                {
                    gameManager.RolPanelYazdirma(2);
                    StartCoroutine(koybildirimopen(gameManager.naidotext));
                }
                else if (this.Teamname == "Hyva") // bir þehir seçilecek kirasý x çarpan olacak.
                {
                    if (Mulkler.Count != 0)
                    {
                        gameManager.MulkleriHyvaYazdirma();
                        StartCoroutine(koybildirimopen(gameManager.hyvatext));
                    }
                    else
                    {
                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                            {
                                gameManager.DiceReset();
                            }
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());
                    }

                }
                else if (this.Teamname == "Anzo") // Seçilen bir kiþinin tagýný çalacak.
                {
                    gameManager.RolPanelYazdirma(1);
                }
                else if (this.Teamname == "Naido")
                {
                    gameManager.RolPanelYazdirma(2);
                }

                else if (this.Teamname == "Zesu")
                {
                    if (ZesuOzellikSayisi < 2)
                    {
                        double Deger = gameManager.NorulFlamPrices[NorulFlama] * 0.8f;
                        if (Para >= Deger)
                        {
                            ZesuOzellikAktif = true;
                            gameManager.FlamaTriggerAcma(NorulFlama, this);
                        }
                        else
                        {
                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                    }
                    else
                    {
                        if (Para >= gameManager.NorulFlamPrices[NorulFlama])
                        {
                            gameManager.FlamaTriggerAcma(NorulFlama, this);
                        }
                        else
                        {
                            if (zar1 == zar2)
                            {
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else
                            {
                                StartCoroutine(WaitAndNextTurn());
                            }
                        }
                    }
                }
                else if (this.Teamname == "Oyul")
                {
                    OyulKoyuKiraOdememe();
                }
            }
            #endregion
            #region ChangeRoute Yeri
            else if (other.CompareTag("ChangeRoute") && zarsayaci == 0)
            {
                ChangeRouteCornerEffectsUpdate();
                Invoke("RouteYonDegistir", 2f);
            }
            #endregion
            #region NorulFlama Yeri
            else if (other.CompareTag("NorulFlama") && zarsayaci == 0)
            {
                if (PlayerToplamAdim > 32)
                {
                    if (Para >= gameManager.NorulFlamPrices[NorulFlama])
                    {
                        gameManager.FlamaTriggerAcma(NorulFlama, this);
                    }
                    else
                    {
                        NotificationOpen(0);
                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                            {
                                gameManager.DiceReset();
                            }
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());
                    }
                }
                else
                {
                    NotificationOpen(1);
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }

            }
            #endregion
        }
    }

    #region OnTrigger Arsa Giriþ
    private IEnumerator TriggerArsa(Collider other)
    {
        while (true)
        {
            if (zarsayaci != -1)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                break;
            }
        }
        _currentMulk = other.gameObject.GetComponent<MulkBilgisi>();
        _currentMulk.ArsayaGirildi(this);
        double originalKiraBedel = _currentMulk.KiraBedeliHesapla(_currentMulk.EvOtelSayisi);
        int yenievfiyati = _currentMulk.RengeGoreEvSatinAlmaFiyati();
        while (true)
        {
            if (Karakter == "Sheriff")
            {
                if (AyniKaredekiPlayerler.Count > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            if (KodeseGonderildi)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                break;
            }
        }
        if (_currentMulk.Sahibi == null)
        {
            CityCardOpen(" ", yenievfiyati, _currentMulk.SatinAlmaFiyati, _currentMulk, this);
        }
        else
        {
            CityCardOpen(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerName, yenievfiyati, _currentMulk.GeriSatinAlmaFiyati, _currentMulk, this);
        }
        if (turnManager.currentPlayer == gameObject)
        {
            ResetEvSelection();
            if (_currentMulk != null)
            {
                if (_currentMulk.Sahibi == null && Para >= _currentMulk.SatinAlmaFiyati)
                {
                    gameManager.BosArsaPanelAcma(_currentMulk.KartMaterialBos.mainTexture, _currentMulk.SatinAlmaFiyati);
                    satinbutonu = true;
                    VazgecmeButonu.gameObject.SetActive(true);
                }
                else if (_currentMulk.Sahibi == null && Para < _currentMulk.SatinAlmaFiyati)
                {
                    NotificationOpen(0);
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        yield return new WaitForSeconds(0.5f);
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());

                }
                else if (_currentMulk != null && _currentMulk.Sahibi != this && this.Teamname != _currentMulk.Sahibi.Teamname)
                {
                    double originalKiraBedeli = _currentMulk.KiraBedeliHesapla(_currentMulk.EvOtelSayisi);
                    double kiraBedeli;
                    if (Teamname == "Oyul" && !OyulKiraOdememek)
                    {
                        Debug.Log("ArsaDurum1");
                        if (Karakter == "Wallector")
                        {
                            kiraBedeli = CalculateRent(originalKiraBedeli, true);
                            karakterozellikaktif = false; //wallector için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                            KarakterOzellikAktifUpdate(karakterozellikaktif);
                            KarakterOzelligiYineleme();
                        }
                        else
                        {
                            kiraBedeli = originalKiraBedeli;
                        }

                        if (Para >= kiraBedeli)
                        {
                            Para -= kiraBedeli;
                            ParaUpdate(Para);
                            PlayerToplamOdenenKira += (int)kiraBedeli;
                            ToplamOdenenKiraUpdate(PlayerToplamOdenenKira);

                            //_currentMulk.Sahibi.Para += kiraBedeli;
                            //_currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);
                            //_currentMulk.Sahibi.PlayerToplamAlinanKira += (int)kiraBedeli;
                            //_currentMulk.Sahibi.ToplamAlinanKiraUpdate(PlayerToplamAlinanKira);
                            //_currentMulk.Sahibi.PlayerToplamEldeEdilenPara += (int)kiraBedeli;

                            int Deger = (int)kiraBedeli;
                            gameManager.ParaDusurme(Deger, this.gameObject.transform, _currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber, gameObject.GetComponent<PlayerObjectController>().RenkDegiskeni, _currentMulk.Sahibi);
                            gameManager.RentActionVerme(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber, kiraBedeli, gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);

                            if (Karakter == "Temporsium")
                            {
                                TemporsiumMoralAzaldi(10);
                            }
                            if (_currentMulk.Sahibi.Karakter == "Temporsium")
                            {
                                _currentMulk.Sahibi.TemporsiumMoralArtti(5);
                            }
                            if (_currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi <= 4 && turnManager.currentPlayer == gameObject && Para >= _currentMulk.GeriSatinAlmaFiyati)
                            {
                                yield return new WaitForSeconds(3.0f);
                                gameManager.RakipArsaPanelAcma(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerName, originalKiraBedeli, _currentMulk.GeriSatinAlmaFiyati, _currentMulk.EvOtelSayisi, _currentMulk.KartMaterialDolu.mainTexture);
                                gerialmabutonu = true; // buton burada çalýþýyor.
                                VazgecmeButonu.gameObject.SetActive(true);
                            }
                            else if (_currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi <= 4 && turnManager.currentPlayer == gameObject && Para < _currentMulk.GeriSatinAlmaFiyati)
                            {
                                NotificationOpen(0);
                            }
                            else
                            {
                                Debug.Log($"{_currentMulk.Isim} arsasýnda otel bulunduðu için satýn alýnamadý.");

                                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                                {
                                    yield return new WaitForSeconds(1.0f);
                                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                                    {
                                        gameManager.DiceReset();
                                    }
                                    dicebutton = true;
                                    rollDiceButton.gameObject.SetActive(true);
                                }
                                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                                    StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (Para < kiraBedeli)
                        {
                            NotificationOpen(0);
                            if (Mulkler.Count != 0)
                            {
                                gameManager.IflasDurumuMulkYazdirma(kiraBedeli, this, _currentMulk.Sahibi);
                            }
                            else
                            {
                                Invoke("KodeseDustun", 2f);
                            }
                        }
                    }
                    else if (Teamname == "Oyul" && OyulKiraOdememek)
                    {
                        Debug.Log("ArsaDurum2");
                        if (Karakter == "Wallector")
                        {
                            kiraBedeli = CalculateRent(originalKiraBedeli, false);
                            karakterozellikaktif = false; //wallector için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                            KarakterOzellikAktifUpdate(karakterozellikaktif);
                            KarakterOzelligiYineleme();
                        }
                        else
                        {
                            kiraBedeli = originalKiraBedeli;
                        }

                        if (Karakter == "Temporsium")
                        {
                            TemporsiumMoralArtti(10);
                        }
                        if (_currentMulk.Sahibi.Karakter == "Temporsium")
                        {
                            _currentMulk.Sahibi.TemporsiumMoralAzaldi(5);
                        }

                        if (_currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi <= 4 && turnManager.currentPlayer == gameObject && Para >= _currentMulk.GeriSatinAlmaFiyati)
                        {
                            yield return new WaitForSeconds(3.0f);
                            gameManager.RakipArsaPanelAcma(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerName, originalKiraBedeli, _currentMulk.GeriSatinAlmaFiyati, _currentMulk.EvOtelSayisi, _currentMulk.KartMaterialDolu.mainTexture);
                            gerialmabutonu = true; // buton burada çalýþýyor.
                            VazgecmeButonu.gameObject.SetActive(true);
                        }
                        else if (_currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi <= 4 && turnManager.currentPlayer == gameObject && Para < _currentMulk.GeriSatinAlmaFiyati)
                        {
                            NotificationOpen(0);
                        }
                        else
                        {
                            Debug.Log($"{_currentMulk.Isim} arsasýnda otel bulunduðu için satýn alýnamadý.");

                            if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                            {
                                yield return new WaitForSeconds(0.5f);
                                if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                                {
                                    gameManager.DiceReset();
                                }
                                dicebutton = true;
                                rollDiceButton.gameObject.SetActive(true);
                            }
                            else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                                StartCoroutine(WaitAndNextTurn());
                        }
                    }
                    else
                    {
                        Debug.Log("ArsaDurum3");
                        if (Karakter == "Wallector")
                        {
                            kiraBedeli = CalculateRent(originalKiraBedeli, true);
                            karakterozellikaktif = false; //wallector için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                            KarakterOzellikAktifUpdate(karakterozellikaktif);
                            KarakterOzelligiYineleme();
                        }
                        else
                        {
                            kiraBedeli = originalKiraBedeli;
                        }

                        if (Para >= kiraBedeli)
                        {
                            Para -= kiraBedeli;
                            ParaUpdate(Para);
                            PlayerToplamOdenenKira += (int)kiraBedeli;
                            ToplamOdenenKiraUpdate(PlayerToplamOdenenKira);

                            /*_currentMulk.Sahibi.Para += kiraBedeli;
                            _currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);
                            _currentMulk.Sahibi.PlayerToplamAlinanKira += (int)kiraBedeli;
                            _currentMulk.Sahibi.ToplamAlinanKiraUpdate(PlayerToplamAlinanKira);
                            _currentMulk.Sahibi.PlayerToplamEldeEdilenPara += (int)kiraBedeli;*/
                            int Deger = (int)kiraBedeli;
                            gameManager.ParaDusurme(Deger, this.gameObject.transform, _currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber, gameObject.GetComponent<PlayerObjectController>().RenkDegiskeni, _currentMulk.Sahibi);
                            gameManager.RentActionVerme(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber, kiraBedeli, gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                            if (Karakter == "Temporsium")
                            {
                                TemporsiumMoralAzaldi(10);
                            }
                            if (_currentMulk.Sahibi.Karakter == "Temporsium")
                            {
                                _currentMulk.Sahibi.TemporsiumMoralArtti(5);
                            }
                            if (_currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi <= 4 && turnManager.currentPlayer == gameObject && Para >= _currentMulk.GeriSatinAlmaFiyati)
                            {
                                yield return new WaitForSeconds(3.0f);
                                gameManager.RakipArsaPanelAcma(_currentMulk.Sahibi.gameObject.GetComponent<PlayerObjectController>().PlayerName, originalKiraBedeli, _currentMulk.GeriSatinAlmaFiyati, _currentMulk.EvOtelSayisi, _currentMulk.KartMaterialDolu.mainTexture);
                                gerialmabutonu = true; // buton burada çalýþýyor.
                                VazgecmeButonu.gameObject.SetActive(true);
                            }
                            else
                            {
                                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                                {
                                    yield return new WaitForSeconds(0.5f);
                                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                                    {
                                        gameManager.DiceReset();
                                    }
                                    dicebutton = true;
                                    rollDiceButton.gameObject.SetActive(true);
                                }
                                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                                    StartCoroutine(WaitAndNextTurn());
                            }
                        }
                        else if (Para < kiraBedeli)
                        {
                            NotificationOpen(0);
                            if (Mulkler.Count != 0)
                            {
                                gameManager.IflasDurumuMulkYazdirma(kiraBedeli, this, _currentMulk.Sahibi);
                            }
                            else
                            {
                                Invoke("KodeseDustun", 2f);
                            }

                        }
                    }
                }
                else if (_currentMulk != null && _currentMulk.Sahibi != this && Teamname == _currentMulk.Sahibi.Teamname)
                {
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralArtti(5);
                    }
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        yield return new WaitForSeconds(0.5f);
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());
                }
                else if (_currentMulk != null && _currentMulk.Sahibi == this)
                {
                    if (tursayisi == 1 && _currentMulk.EvOtelSayisi == 1)
                    {
                        if (Karakter == "Harvestor")
                        {
                            harvestorstacks++;
                            if (harvestorstacks >= 7)
                            {
                                HasatToplama();
                            }
                        }
                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            yield return new WaitForSeconds(0.5f);
                            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                            {
                                gameManager.DiceReset();
                            }
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());
                    }
                    else if (tursayisi == 1 && _currentMulk.EvOtelSayisi == 0)
                    {
                        if (Karakter == "Harvestor")
                        {
                            harvestorstacks++;
                            if (harvestorstacks >= 7)
                            {
                                HasatToplama();
                            }
                        }
                        CheckConditionsForCurrentMulk();
                    }
                    else if (tursayisi == 2 && _currentMulk.EvOtelSayisi == 3)
                    {
                        if (Karakter == "Harvestor")
                        {
                            harvestorstacks++;
                            if (harvestorstacks >= 7)
                            {
                                HasatToplama();
                            }
                        }
                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            yield return new WaitForSeconds(0.5f);
                            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                            {
                                gameManager.DiceReset();
                            }
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());
                    }
                    else if (tursayisi == 2 && _currentMulk.EvOtelSayisi < 3)
                    {
                        if (Karakter == "Harvestor")
                        {
                            harvestorstacks++;
                            if (harvestorstacks >= 7)
                            {
                                HasatToplama();
                            }
                        }
                        CheckConditionsForCurrentMulk();
                    }
                    else if (tursayisi > 2)
                    {
                        if (Karakter == "Harvestor")
                        {
                            harvestorstacks++;
                            if (harvestorstacks >= 7)
                            {
                                HasatToplama();
                            }
                        }
                        CheckConditionsForCurrentMulk();
                    }
                }
            }
            else
                Debug.Log("mulk tanýmlý deil");

        }



    }



    #endregion

    #endregion
    #region Initialize Stats
    public void InitializeStats()
    {
        OyunBitti = true;
        AtilanZar = 0;
        steps = 0;
        zar1 = 0;
        zar2 = 0;
        routePosition = 0;
        routePositionn = 0;
        ciftzarsayisi = 0;
        Para = 22500;
        isMoving = false;
        kodesSayaci = 0;
        kodes = false;
        tursayisi = 0;
        harvestorstacks = 0;
        tatilzaratisi = 0;
        karakterozelliksans = 0;
        karakterozellikaktif = false;
        OyuncuHazir = false;
        Teamname = "";
        IflasDurumu = false;
        NorulFlama = 0;
        Kazanmak = false;
        NorulFlamaUcreti = 0;
        ZesuOzellikSayisi = 0;
        AnzoKoyu = false;
        TemporsiumMoral = 50;
        TemporsiumZarAtti = false;
        TemporsiumKatsayisi = 0;
        OyulKiraOdememek = false;
        RenkDegiskeni = 0;
        zarsayaci = 0;
        moveSpeed = 5;
        dicebutton = false;
        CharacterOpak = false;
        onemlibirdegerdegil = 0;
        evsayisi1 = 0;
        satinbutonu = false;
        evbutonu = false;
        gerialmabutonu = false;
        evsayisibtna = false;
        evsayisibtnb = false;
        evsayisibtnc = false;
        evsayisibtnd = false;
        TabletMulksItems.Clear();
        TabletAcik = false;
        MulkSatmaZorunlulugu = false;
        TeamKiraDuzenleme = false;
        Baslangic = 0;
        Oyuncuyagelensirasayisi = 0;
        Kodesici = false;
        Mulkler.Clear();
        Prefabs.Clear();
        AyniKaredekiPlayerler.Clear();
        KodeseGonderilenler.Clear();
        currentadim = 0;
        SiraBelirlendi = false;
        KodeseGonderildi = false;
        NineSelectorDegisken = -1;
        PlayerToplamAdim = 0;
        PlayerToplamAlinanKira = 0;
        PlayerToplamEldeEdilenPara = 0;
        PlayerToplamOdenenKira = 0;
        PlayerToplamSatinAlinanMulkDegeri = 0;
        Kazandi = false;
        ZesuOzellikAktif = false;
        Bankrupted = false;
        gameObject.GetComponent<PlayerObjectController>().Ready = false;
        gameObject.GetComponent<PlayerObjectController>().OyuncuHazir = false;
        this.enabled = false;

    }

    #endregion
    #region Mulk Arsadaki list silme
    private void ArsadakiBeniSil()
    {
        if (isLocalPlayer)
        {
            if (_currentMulk != null)
            {
                _currentMulk.ArsadanCikildi(this);
            }
        }
    }


    #endregion
    #region Zar Atma
    public void RollDice()
    {
        dicebutton = false;
        rollDiceButton.gameObject.SetActive(false);
        float x1, y1, z1;
        x1 = Random.Range(0, 500);
        y1 = Random.Range(0, 500);
        z1 = Random.Range(0, 500);

        float x2, y2, z2;
        x2 = Random.Range(0, 500);
        y2 = Random.Range(0, 500);
        z2 = Random.Range(0, 500);

        gameManager.RollDiceUpdate(x1, y1, z1, x2, y2, z2);
        StartCoroutine(WaitForValues());
        //rolldice2();
    }
    public IEnumerator WaitForValues()
    {
        while (true)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                zar1 = gameManager.dice1.diceValue;
                zar2 = gameManager.dice2.diceValue;
                RollDiceS();
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

        }

    }
    public void RollDiceS()
    {
        if (isLocalPlayer && turnManager.currentPlayer == gameObject)
        {
            /*if (Baslangic == 0)
            {
                zar1 = gameManager.dice1.diceValue;
                zar2 = gameManager.dice2.diceValue;
                Baslangic += 1;
            }
            else
            {

                zar1 = gameManager.dice1.diceValue;
                zar2 = gameManager.dice2.diceValue;
            }*/
            int PlayerIdNumber = gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber;
            //gameManager.ZarActionVerme(zar1, zar2, PlayerIdNumber);
            Debug.Log("Zar Gönderildi");
            steps = zar1 + zar2;
            PlayerToplamAdim += steps;

            if (zar1 == zar2)
            {
                ciftzarsayisi += 1;
            }
            else
            {
                ciftzarsayisi = 0;
            }
            int ClientZar = steps;
            rollDiceButton.gameObject.SetActive(false);
            dicebutton = false;
            if (Karakter == "Temporsium" && !TemporsiumZarAtti && TemporsiumMoral > 30)
            {
                TemporsiumPanelAc();
            }
            else
            {
                StartCoroutine(Move());
                ClientMove(ClientZar, gameManager.RouteYon);
                TemporsiumZarAtti = false;
            }
            ReachObject.transform.position = currentRoute.childNodeList[routePosition].transform.position;
            ReachObject.SetActive(isMoving);
        }
    }


    #endregion
    #region Zar Sonucu Hareket Etme
    IEnumerator Move()
    {
        zarsayaci = steps;
        ArsadakiBeniSil();
        if (isMoving)
        {
            yield break;
        }
        //yield return new WaitForSeconds(0.5f);
        isMoving = true;
        Anim.SetBool("Moving", true);
        ReachObjectOpen();
        while (steps > 0)
        {
            //KaraterYonBelirleme();
            if (gameManager.RouteYon)
            {
                KaraterYonBelirlemePozitif();
                if (routePosition == 32)
                {
                    routePosition = 0;
                }
                routePosition++;
            }
            else if (!gameManager.RouteYon)
            {
                if (routePosition == 0)
                {
                    routePosition = 32; // 32 yapýlacak.
                }
                routePosition--;
                KaraterYonBelirlemeNegatif();
            }
            routePosition %= currentRoute.childNodeList.Count;
            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            nextPos.y = transform.position.y; // Oyuncunun yüksekliðini sabit tut             

            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }
            //yield return new WaitForSeconds(0.1f);
            steps--;
            zarsayaci = steps - 1;
        }
        Anim.SetBool("Moving", false);
        gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        isMoving = false;
        Invoke("RoutepositionGonderme", 2f);
        NaidoAdimSayisiCheck();
        VarisNoktasiKapatma();
        OyulCheckAdimSayisi();
        AyniKaredekiOyuncular();
    }
    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime));
    }

    private void RoutepositionGonderme()
    {
        RoutePositionUpdate(routePosition);
    }

    public void KaraterYonBelirlemePozitif()
    {
        if (routePosition >= 0 && routePosition < 8)
        {
            Quaternion hedefDonus = Quaternion.Euler(0, 90, 0);
            if (gameObject.transform.rotation != hedefDonus)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (routePosition >= 8 && routePosition < 16)
        {
            Quaternion hedefDonus1 = Quaternion.Euler(0, 180, 0);
            if (gameObject.transform.rotation != hedefDonus1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (routePosition >= 16 && routePosition < 24)
        {
            Quaternion hedefDonus2 = Quaternion.Euler(0, -90, 0);
            if (gameObject.transform.rotation != hedefDonus2)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if (routePosition >= 24 && routePosition < 32)
        {
            Quaternion hedefDonus3 = Quaternion.Euler(0, 0, 0);
            if (gameObject.transform.rotation != hedefDonus3)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void KaraterYonBelirlemeNegatif()
    {
        if (routePosition >= 0 && routePosition < 8)
        {
            Quaternion hedefDonus4 = Quaternion.Euler(0, -90, 0);
            if (gameObject.transform.rotation != hedefDonus4)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if (routePosition >= 8 && routePosition < 16)
        {
            Quaternion hedefDonus5 = Quaternion.Euler(0, 0, 0);
            if (gameObject.transform.rotation != hedefDonus5)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (routePosition >= 16 && routePosition < 24)
        {
            Quaternion hedefDonus6 = Quaternion.Euler(0, 90, 0);
            if (gameObject.transform.rotation != hedefDonus6)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (routePosition >= 24 && routePosition < 32)
        {
            Quaternion hedefDonus7 = Quaternion.Euler(0, 180, 0);
            if (gameObject.transform.rotation != hedefDonus7)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

    }
    #endregion
    #region Client Botu Hareket Ettirme
    IEnumerator ClientBotMove(int Value, bool Yon)
    {
        //yield return new WaitForSeconds(0.5f);
        Anim.SetBool("Moving", true);
        while (Value > 0)
        {
            if (Yon)
            {
                BotKaraterYonBelirlemePozitif();
                if (routePositionn == 32)
                {
                    routePositionn = 0;
                }
                routePositionn++;
            }
            else if (!Yon)
            {
                if (routePositionn == 0)
                {
                    routePositionn = 32;
                }
                routePositionn--;
                BotKaraterYonBelirlemeNegatif();
            }
            routePositionn %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePositionn].position;
            nextPos.y = transform.position.y; // Oyuncunun yüksekliðini sabit tut        
            while (MoveToNextNodeBot(nextPos))
            {
                yield return null;
            }
            //yield return new WaitForSeconds(0.1f);
            Value--;
        }
        routePositionn = routePosition;
        Anim.SetBool("Moving", false);
        gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    bool MoveToNextNodeBot(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime));
    }

    public void BotKaraterYonBelirlemePozitif()
    {
        if (routePositionn >= 0 && routePositionn < 8)
        {
            Quaternion hedefDonus = Quaternion.Euler(0, 90, 0);
            if (gameObject.transform.rotation != hedefDonus)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (routePositionn >= 8 && routePositionn < 16)
        {
            Quaternion hedefDonus1 = Quaternion.Euler(0, 180, 0);
            if (gameObject.transform.rotation != hedefDonus1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (routePositionn >= 16 && routePositionn < 24)
        {
            Quaternion hedefDonus2 = Quaternion.Euler(0, -90, 0);
            if (gameObject.transform.rotation != hedefDonus2)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if (routePositionn >= 24 && routePositionn < 32)
        {
            Quaternion hedefDonus3 = Quaternion.Euler(0, 0, 0);
            if (gameObject.transform.rotation != hedefDonus3)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void BotKaraterYonBelirlemeNegatif()
    {
        if (routePositionn >= 0 && routePositionn < 8)
        {
            Quaternion hedefDonus4 = Quaternion.Euler(0, -90, 0);
            if (gameObject.transform.rotation != hedefDonus4)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if (routePositionn >= 8 && routePositionn < 16)
        {
            Quaternion hedefDonus5 = Quaternion.Euler(0, 0, 0);
            if (gameObject.transform.rotation != hedefDonus5)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (routePositionn >= 16 && routePositionn < 24)
        {
            Quaternion hedefDonus6 = Quaternion.Euler(0, 90, 0);
            if (gameObject.transform.rotation != hedefDonus6)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (routePositionn >= 24 && routePositionn < 32)
        {
            Quaternion hedefDonus7 = Quaternion.Euler(0, 180, 0);
            if (gameObject.transform.rotation != hedefDonus7)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
    #region Boþ Arsa Satýn Alma
    public void SatýnAlmaIslemýniBaslat()
    {
        if (isLocalPlayer && turnManager.currentPlayer == gameObject)
        {
            gameManager.BosArsaPanelKapatma();
            if (_currentMulk.LovingCity)
            {
                if (Para >= _currentMulk.SatinAlmaFiyati)
                {
                    foreach (MulkBilgisi asiksehirler in gameManager.AsikSehirler)
                    {
                        asiksehirler.ArsaSahibiUpdate(this);
                        Mulkler.Add(asiksehirler);
                    }
                    Para -= _currentMulk.SatinAlmaFiyati;
                    PlayerToplamSatinAlinanMulkDegeri += 2 * (int)(_currentMulk.SatinAlmaFiyati);
                    ParaUpdate(Para);
                    MulkListesiUpdate(Mulkler);
                    satinbutonu = false;
                    VazgecmeButonu.gameObject.SetActive(false);
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralArtti(10);
                    }
                    CheckConditionsForCurrentMulk();
                }
                else
                {
                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralAzaldi(5);
                    }
                    satinbutonu = false;

                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());

                    VazgecmeButonu.gameObject.SetActive(false);
                }
            }
            else if (!_currentMulk.LovingCity)
            {
                if (_currentMulk.Sahibi == null)
                {
                    if (Para >= _currentMulk.SatinAlmaFiyati)
                    {
                        Para -= _currentMulk.SatinAlmaFiyati;
                        PlayerToplamSatinAlinanMulkDegeri += (int)(_currentMulk.SatinAlmaFiyati);

                        ParaUpdate(Para);
                        Mulkler.Add(_currentMulk);
                        MulkListesiUpdate(Mulkler);
                        _currentMulk.ArsaSahibiUpdate(this);
                        satinbutonu = false;
                        VazgecmeButonu.gameObject.SetActive(false);

                        if (Karakter == "Temporsium")
                        {
                            TemporsiumMoralArtti(5);
                        }
                        CheckConditionsForCurrentMulk();
                    }
                    else
                    {
                        satinbutonu = false;

                        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                        {
                            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                            {
                                gameManager.DiceReset();
                            }
                            dicebutton = true;
                            rollDiceButton.gameObject.SetActive(true);
                        }
                        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                            StartCoroutine(WaitAndNextTurn());

                        VazgecmeButonu.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    #endregion
    #region Ev Koþullarý Kontrol Etme
    private void CheckConditionsForCurrentMulk()
    {
        // Diðer koþullarý kontrol et
        if (_currentMulk.Sahibi == this && _currentMulk.EvOtelSayisi < 4 && isLocalPlayer)
        {
            if (tursayisi == 1 && _currentMulk.EvOtelSayisi >= 1)
            {
                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
            else if (tursayisi == 2 && _currentMulk.EvOtelSayisi >= 2)
            {
                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
            else if (tursayisi == 3 && _currentMulk.EvOtelSayisi >= 3)
            {
                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
            else if (tursayisi > 3 && _currentMulk.EvOtelSayisi == 3 && NorulFlama < 2)
            {
                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
            else if (_currentMulk.EvOtelSayisi == 4)
            {
                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
            else
            {
                gameManager.EvSatinAlmaPanelAcma(_currentMulk.KartMaterialDolu.mainTexture, _currentMulk.EvOtelSayisi);
                evbutonu = true;
                VazgecmeButonu.gameObject.SetActive(true);
            }
        }
        else if (_currentMulk.Sahibi == this && _currentMulk.EvOtelSayisi == 4 && isLocalPlayer)
        {
            if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
            {
                if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                {
                    gameManager.DiceReset();
                }
                dicebutton = true;
                rollDiceButton.gameObject.SetActive(true);
            }
            else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                StartCoroutine(WaitAndNextTurn());
        }
    }
    #endregion
    #region Ev Alma Fonksiyonu
    public void EvAl()
    {
        if (isLocalPlayer && turnManager.currentPlayer == gameObject)
        {
            gameManager.EvSatinAlmaPanelKapatma();
            int fiyat = Evfiyati();
            int evSayisi = evsayisi1;

            if (_currentMulk != null && _currentMulk.Sahibi == this && _currentMulk.EvOtelSayisi >= 0 && _currentMulk.EvOtelSayisi < 5 && Para >= evSayisi * fiyat && evSayisi != 0)
            {
                if (_currentMulk.LovingCity)
                {
                    foreach (MulkBilgisi asiksehirler in gameManager.AsikSehirler)
                    {
                        asiksehirler.EvOtelSayisi += evSayisi;
                        asiksehirler.EvOtelSayisiUpdate(asiksehirler.EvOtelSayisi);
                    }
                    Para -= evSayisi * fiyat; // Her ev için renge göre satýn alma fiyatýný kullan       
                    ParaUpdate(Para);
                    evbutonu = false;

                    if (selectedCharacter != null && selectedCharacter.CompareTag("Builder"))
                    {
                        karakterozellikaktif = false; //Builder için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                        KarakterOzelligiYineleme();
                    }
                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());


                    VazgecmeButonu.gameObject.SetActive(false);
                    PlayerToplamSatinAlinanMulkDegeri += 2 * (int)(evSayisi * fiyat);
                }
                else
                {
                    // Sahip olduðunuz arsa ve ev sayýsý 1 ile 4 arasýnda ise
                    // Belirtilen sayýda ev ekleyin
                    _currentMulk.EvOtelSayisi += evSayisi;
                    _currentMulk.EvOtelSayisiUpdate(_currentMulk.EvOtelSayisi);
                    Para -= evSayisi * fiyat; // Her ev için renge göre satýn alma fiyatýný kullan       
                    ParaUpdate(Para);
                    evbutonu = false;

                    if (selectedCharacter != null && selectedCharacter.CompareTag("Builder"))
                    {
                        karakterozellikaktif = false; //Builder için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                        KarakterOzelligiYineleme();
                    }

                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());


                    VazgecmeButonu.gameObject.SetActive(false);
                    PlayerToplamSatinAlinanMulkDegeri += (int)(evSayisi * fiyat);
                }
                if (Karakter == "Temporsium")
                {
                    TemporsiumMoralArtti(evSayisi);
                }
            }
            else if (Para < evSayisi * fiyat)
            {
                evbutonu = false;

                if (Para >= fiyat)
                {
                    CheckConditionsForCurrentMulk();
                }
                else if (Para >= 2 * fiyat)
                {
                    CheckConditionsForCurrentMulk();
                }
                else if (Para >= 3 * fiyat)
                {
                    CheckConditionsForCurrentMulk();
                }
                else
                {


                    if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                    {
                        if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                        {
                            gameManager.DiceReset();
                        }
                        dicebutton = true;
                        rollDiceButton.gameObject.SetActive(true);
                    }
                    else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                        StartCoroutine(WaitAndNextTurn());


                    VazgecmeButonu.gameObject.SetActive(false);
                }
            }
            else if (evSayisi == 0)
            {
                evbutonu = false;
                VazgecmeButonu.gameObject.SetActive(false);

                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
        }
    }
    #endregion
    #region OtelSatinAlma
    public void OtelSatinAl()
    {
        int fiyat = Evfiyati();
        if (_currentMulk != null && _currentMulk.Sahibi == this && _currentMulk.EvOtelSayisi == 4)
        {
            _currentMulk.EvOtelSayisi = 5;
            _currentMulk.EvOtelSayisiUpdate(_currentMulk.EvOtelSayisi);
            Para -= 2 * fiyat;
            ParaUpdate(Para);

            if (selectedCharacter != null && selectedCharacter.CompareTag("Builder"))
            {
                karakterozellikaktif = false; //Builder için hakkýný 1 kez kullandýðýnda sýfýrlanmasýný saðlayan kod.
                KarakterOzelligiYineleme();
            }
            if (zar1 == zar2)
            {
                dicebutton = true;
                rollDiceButton.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(WaitAndNextTurn());
            }
        }
        OtelSatinAlma.gameObject.SetActive(false);
        VazgecmeButonu.gameObject.SetActive(false);
    }
    #endregion
    #region Ýþlemlerden Vazgeçme
    public void IslemlerdenVazgec()
    {
        gameManager.BosArsaPanelKapatma();
        gameManager.EvSatinAlmaPanelKapatma();
        gameManager.RakipArsaPanelKapama();
        satinalmabutonu.gameObject.SetActive(false);
        satinbutonu = false;
        evalmabutonu.gameObject.SetActive(false);
        evsayisibtn1.gameObject.SetActive(false);
        evsayisibtn2.gameObject.SetActive(false);
        evsayisibtn3.gameObject.SetActive(false);
        evsayisibtn4.gameObject.SetActive(false);
        evbutonu = false;
        gerisatinalmabutonu.gameObject.SetActive(false);
        gerialmabutonu = false;
        kodesciftzarButton.gameObject.SetActive(false);
        kodesparaodeButton.gameObject.SetActive(false);
        OtelSatinAlma.gameObject.SetActive(false);
        VazgecmeButonu.gameObject.SetActive(false);

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }
    #endregion
    #region Þans Kartlarý
    public void SansKartiKullan()
    {
        // Rastgele bir þans kartý seçmek için 0 ile 2 arasýnda bir sayý seçelim.
        int randomKart = Random.Range(0, 2);

        // Seçilen þans kartýna göre ilgili fonksiyonu çaðýralým.
        switch (randomKart)
        {
            case 0:
                Kart1000Ode();
                Debug.Log("1000 ödedin");
                if (zar1 == zar2)
                {
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else
                {
                    StartCoroutine(WaitAndNextTurn());
                }
                break;
            case 1:
                Kart1000Al();
                Debug.Log("1000 aldýn");
                if (zar1 == zar2)
                {
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else
                {
                    StartCoroutine(WaitAndNextTurn());
                }

                break;
            case 2:
                KartStartPointeGit();
                Debug.Log("baþlangýca gittin");
                break;
            case 3:
                StartCoroutine(KodeseGit());
                Debug.Log("Kodese düþtün.");
                break;
            case 4:
                KarakterSans();
                Debug.Log("Karakter özelliðin açýldý.");
                break;
            default:
                Debug.LogError("Geçersiz þans kartý indeksi!");
                break;
        }
    }

    private void Kart1000Ode()
    {
        // Oyuncudan 1000 birim ödemesini iste
        Para -= 1000;
        ParaUpdate(Para);
    }

    private void Kart1000Al()
    {
        // Oyuncuya 1000 birim ekle
        Para += 1000;
        ParaUpdate(Para);
    }
    private void KartStartPointeGit()
    {
        StartCoroutine(StartPointeGit());
    }

    private IEnumerator KodeseGit()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(KodeseDus());
    }
    #endregion
    #region Rakip Arsayý Satýn Alma
    public void RakipArsayiSatýnAl()
    {
        gameManager.RakipArsaPanelKapama();
        if (VazgecmeButonu.isActiveAndEnabled)
        {
            VazgecmeButonu.gameObject.SetActive(false);
        }

        if (_currentMulk != null && _currentMulk.Sahibi != this && isLocalPlayer)
        {
            double toplamDeger = _currentMulk.getGeriSatinAlmaFiyati();


            // Eðer oyuncunun parasý arsanýn toplam deðerine yetiyorsa arsayý satýn al
            if (Para >= toplamDeger)
            {
                if (_currentMulk.LovingCity)
                {
                    Para -= toplamDeger;
                    ParaUpdate(Para);
                    _currentMulk.Sahibi.Para += toplamDeger;
                    _currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);

                    if (Karakter == "Temporsium")
                    {
                        TemporsiumMoralArtti(10);
                    }
                    if (_currentMulk.Sahibi.Karakter == "Temporsium")
                    {
                        TemporsiumMoralAzaldi(10);
                    }
                    foreach (MulkBilgisi asiksehirler in gameManager.AsikSehirler)
                    {
                        int Evsayýsý = asiksehirler.EvOtelSayisi;
                        asiksehirler.EvOtelSayisi = 0;
                        asiksehirler.EvOtelSayisiUpdate(asiksehirler.EvOtelSayisi);

                        asiksehirler.Sahibi.Mulkler.Remove(asiksehirler); //incelenecek.
                        asiksehirler.Sahibi.MulkListesiUpdate(asiksehirler.Sahibi.Mulkler);
                        Mulkler.Add(asiksehirler);
                        MulkListesiUpdate(Mulkler);
                        asiksehirler.ArsaSahibiUpdate(this);
                        asiksehirler.setGeriSatinAlmaFiyati(_currentMulk.EvOtelSayisi * _currentMulk.RengeGoreEvSatinAlmaFiyati());

                        asiksehirler.EvOtelSayisi = Evsayýsý;
                        asiksehirler.EvOtelSayisiUpdate(asiksehirler.EvOtelSayisi);
                    }
                    gerialmabutonu = false;
                    CheckConditionsForCurrentMulk();
                    PlayerToplamSatinAlinanMulkDegeri += 2 * (int)toplamDeger;
                }
                else
                {
                    int Evsayýsý = _currentMulk.EvOtelSayisi;
                    _currentMulk.EvOtelSayisi = 0;
                    _currentMulk.EvOtelSayisiUpdate(_currentMulk.EvOtelSayisi);
                    Para -= toplamDeger;
                    ParaUpdate(Para);
                    _currentMulk.Sahibi.Para += toplamDeger;
                    _currentMulk.Sahibi.ParaUpdate(_currentMulk.Sahibi.Para);
                    _currentMulk.Sahibi.Mulkler.Remove(_currentMulk); //incelenecek.
                    _currentMulk.Sahibi.MulkListesiUpdate(_currentMulk.Sahibi.Mulkler);
                    Mulkler.Add(_currentMulk);
                    _currentMulk.ArsaSahibiUpdate(this);
                    _currentMulk.setGeriSatinAlmaFiyati(_currentMulk.EvOtelSayisi * _currentMulk.RengeGoreEvSatinAlmaFiyati());
                    gerialmabutonu = false;

                    _currentMulk.EvOtelSayisi = Evsayýsý;
                    _currentMulk.EvOtelSayisiUpdate(_currentMulk.EvOtelSayisi);
                    CheckConditionsForCurrentMulk();
                    PlayerToplamSatinAlinanMulkDegeri += (int)toplamDeger;
                }

            }
            else
            {
                gerialmabutonu = false;
                VazgecmeButonu.gameObject.SetActive(false);

                if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
                {
                    if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                    {
                        gameManager.DiceReset();
                    }
                    dicebutton = true;
                    rollDiceButton.gameObject.SetActive(true);
                }
                else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                    StartCoroutine(WaitAndNextTurn());
            }
        }
    }
    #endregion
    #region Ýflas Eþiði
    public void IflasDurumuSehirSatma(MulkBilgisi mulk, double CityPrice)
    {
        Para += CityPrice;
        ParaUpdate(Para);
        Mulkler.Remove(mulk);
        MulkListesiUpdate(Mulkler);
        mulk.SetMulkDefaultUpdate();
    }

    public IEnumerator MulkSatisKontrolu(double OdenecekTutar, PlayerMechanics OdemeYapacakOyuncu, PlayerMechanics OdemeAlacakOyuncu)
    {
        while (Para < OdenecekTutar)
        {
            yield return new WaitForSeconds(0.1f);
            double Borc = OdenecekTutar - Para;
            gameManager.IflasEsigiBorcText.text = Borc.ToString();
            if (Mulkler.Count == 0)
            {
                gameManager.IflasDurumPanelKapatma();
                StartCoroutine(IflasEtmek());
                break;
                //OdemeYapacakOyuncu.KodeseDusmek();
                //OdemeAlacakOyuncu.Para += OdenecekTutar;
                //OdemeAlacakOyuncu.ParaUpdate(OdemeAlacakOyuncu.Para);
                /*while (!kodes)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                break;*/
            }
        }
        if (!Bankrupted)
        {
            gameManager.IflasDurumPanelKapatma();

            OdemeAlacakOyuncu.Para += OdenecekTutar;
            OdemeAlacakOyuncu.ParaUpdate(OdemeAlacakOyuncu.Para);

            OdemeYapacakOyuncu.Para -= OdenecekTutar;
            OdemeYapacakOyuncu.ParaUpdate(OdemeYapacakOyuncu.Para);
        }   
        if (zar1 == zar2)
        {
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(WaitAndNextTurn());
        }

    }
    #endregion
    #region Kazanma -- Bu incelenecek --
    /*public void Kazanma()
    {
        if (turnManager.players.Count == 1)
        {
            won = true;
            Debug.Log($"{turnManager.players[0]} kazandý!");
            transform.position = winnerplatform.transform.position;
            // Kazananý ilan etmek için gerekli iþlemleri yapabilirsiniz
        }
        else if (turnManager.players.Count == 0)
        {
            Debug.Log("Oyun bitti, kazanan yok!");
            // Oyunun sonu için gerekli iþlemleri yapabilirsiniz
        }
    }*/
    #endregion
    #region Ev Alma Butonlarý
    public void btnaktif1()
    {
        evsayisibtna = !evsayisibtna; // Durumu tersine çevir

        // Butonu týklanabilir hale getir
        evsayisibtn1.interactable = true;

        // Butonun rengini ve metnini deðiþtir (isteðe baðlý)
        if (evsayisibtna)
        {
            // Buton aktifken rengi deðiþtir
            evsayisibtn1.GetComponent<Image>().color = Color.green;
            evsayisibtn1.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
        else
        {
            // Buton pasifken rengi deðiþtir
            evsayisibtn1.GetComponent<Image>().color = Color.red;
            evsayisibtn1.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
    }

    public void btnaktif2()
    {
        evsayisibtnb = !evsayisibtnb; // Durumu tersine çevir

        // Butonu týklanabilir hale getir
        evsayisibtn2.interactable = true;

        // Butonun rengini ve metnini deðiþtir (isteðe baðlý)
        if (evsayisibtnb)
        {
            // Buton aktifken rengi deðiþtir
            evsayisibtn2.GetComponent<Image>().color = Color.green;
            evsayisibtn2.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
        else
        {
            // Buton pasifken rengi deðiþtir
            evsayisibtn2.GetComponent<Image>().color = Color.red;
            evsayisibtn2.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
    }

    public void btnaktif3()
    {
        evsayisibtnc = !evsayisibtnc; // Durumu tersine çevir

        // Butonu týklanabilir hale getir
        evsayisibtn3.interactable = true;

        // Butonun rengini ve metnini deðiþtir (isteðe baðlý)
        if (evsayisibtnc)
        {
            // Buton aktifken rengi deðiþtir
            evsayisibtn3.GetComponent<Image>().color = Color.green;
            evsayisibtn3.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
        else
        {
            // Buton pasifken rengi deðiþtir
            evsayisibtn3.GetComponent<Image>().color = Color.red;
            evsayisibtn3.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
    }

    public void btnaktif4()
    {
        evsayisibtnd = !evsayisibtnd; // Durumu tersine çevir

        // Butonu týklanabilir hale getir
        evsayisibtn4.interactable = true;

        // Butonun rengini ve metnini deðiþtir (isteðe baðlý)
        if (evsayisibtnd)
        {
            // Buton aktifken rengi deðiþtir
            evsayisibtn4.GetComponent<Image>().color = Color.green;
            evsayisibtn4.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
        else
        {
            // Buton pasifken rengi deðiþtir
            evsayisibtn4.GetComponent<Image>().color = Color.red;
            evsayisibtn4.GetComponentInChildren<Text>().text = $"{_currentMulk.RengeGoreEvSatinAlmaFiyati()}";
        }
    }
    public void ResetEvSelection()
    {
        // Butonlarýn durumunu sýfýrla
        evsayisibtna = false;
        evsayisibtnb = false;
        evsayisibtnc = false;
        evsayisibtnd = false;

        // Butonlarý etkileþim dýþý býrak
        evsayisibtn1.interactable = false;
        evsayisibtn2.interactable = false;
        evsayisibtn3.interactable = false;
        evsayisibtn4.interactable = false;

        // Butonlarýn rengini ve metnini sýfýrla
        evsayisibtn1.GetComponent<Image>().color = Color.white;
        evsayisibtn2.GetComponent<Image>().color = Color.white;
        evsayisibtn3.GetComponent<Image>().color = Color.white;
        evsayisibtn4.GetComponent<Image>().color = Color.white;
        evsayisibtn1.GetComponentInChildren<Text>().text = " ";
        evsayisibtn2.GetComponentInChildren<Text>().text = " ";
        evsayisibtn3.GetComponentInChildren<Text>().text = " ";
        evsayisibtn4.GetComponentInChildren<Text>().text = " ";

        // Ev sayýsýný sýfýrla
        evsayisi1 = 0;
    }
    #endregion
    #region WaitAndNextTurn
    public IEnumerator WaitAndNextTurn()
    {
        if (turnManager.currentPlayer == gameObject && isLocalPlayer)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }

            if (KodeseGonderildi)
            {
                while (KodeseGonderildi)
                {
                    yield return new WaitForSeconds(2.0f);
                }
                yield return new WaitForSeconds(2f); // 2 saniye bekleme
                if (SceneManager.GetActiveScene().name == "Mushigame")
                {
                    StartCoroutine(turnManager.NextTurn());
                    Debug.Log("Þerifkodesegönderdi ve next turn atýldý");
                }

            }
            else
            {
                yield return new WaitForSeconds(2f); // 2 saniye bekleme
                if (SceneManager.GetActiveScene().name == "Mushigame")
                {
                    StartCoroutine(turnManager.NextTurn());
                }
            }
        }
    }
    #endregion
    #region StartPointe Git
    public IEnumerator StartPointeGit()
    {
        steps = 33 - routePosition;

        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            nextPos.y = transform.position.y; // Oyuncunun yüksekliðini sabit tut
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
        }
        isMoving = false;

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }
    #endregion
    #region KodeseDus
    public IEnumerator KodeseDus()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        Anim.SetBool("Moving", true);
        while (routePosition != 8)
        {
            if (routePosition >= 0 && routePosition <= 7)
            {
                KaraterYonBelirlemePozitif();
                routePosition++;
                Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
                nextPos.y = transform.position.y; // Oyuncunun yüksekliðini sabit tut
                while (MoveToNextNode(nextPos))
                {
                    yield return null;
                }
            }
            else if (routePosition > 8)
            {
                routePosition--;
                KaraterYonBelirlemeNegatif();
                Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
                nextPos.y = transform.position.y; // Oyuncunun yüksekliðini sabit tut

                while (MoveToNextNode(nextPos))
                {
                    yield return null;
                }
            }
        }
        Anim.SetBool("Moving", false);
        gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        isMoving = false;
        ciftzarsayisi = 0;
        steps = 0;
        KodesTriggerAktif();
    }


    public void KodeseDustun()
    {
        StartCoroutine(KodeseDus());
    }
    #endregion
    #region Kodes Trigger Fonksiyonu
    private void KodesTriggerAktif()
    {
        KodesCornerEffectsUpdate();
        onemlibirdegerdegil = Oyuncuyagelensirasayisi;
        if (isLocalPlayer)
        {
            kodes = true;
            kodesSayaci = 4;
        }
        Invoke("Kodestesin", 1f);
    }

    private void Kodestesin()
    {
        routePosition = 8;
        routePositionn = 8;
        RoutePositionUpdate(routePosition); // denenecek
        if (isLocalPlayer)
        {
            if (kodesSayaci == 4)
            {
                kodesSayaci--;
                KodesSayaciUpdate(kodesSayaci);
                if (turnManager.currentPlayer == gameObject)
                {
                    StartCoroutine(WaitAndNextTurn());
                }
            }
            else
            {
                if (Oyuncuyagelensirasayisi - onemlibirdegerdegil == 1) // Kodes Sayacý 3
                {
                    KodesButonlarAcilmasi();
                }
                else if (Oyuncuyagelensirasayisi - onemlibirdegerdegil == 2) //Kodes Sayacý 2
                {
                    KodesButonlarAcilmasi();
                }
                else if (Oyuncuyagelensirasayisi - onemlibirdegerdegil == 3) //Kodes Sayacý 1
                {
                    KodesButonlarAcilmasi();
                }
                else if (Oyuncuyagelensirasayisi - onemlibirdegerdegil == 4) //Kodes Sayacý 0
                {
                    kodes = false;
                    StartMyTurn();
                }
            }
        }
    }

    private void KodesButonlarAcilmasi()
    {
        if (!gameManager.KodesPanel.activeSelf)
        {
            gameManager.KodesPanel.SetActive(true);
        }

        if (Para < 5000)
        {
            kodesciftzarButton.gameObject.SetActive(true);
            kodesparaodeButton.gameObject.SetActive(false);
        }
        else
        {
            kodesparaodeButton.gameObject.SetActive(true);
            kodesciftzarButton.gameObject.SetActive(true);
        }
    }




    #endregion
    #region KodesCiftDeneme -- Kodese Özel çift zar integer baðlanýp senkronize edilip gösterilecek. -- 
    public void KodesCiftDeneme()
    {
        zar1 = Random.Range(1, 7);
        zar2 = Random.Range(1, 7);

        gameManager.ZarActionVerme(zar1, zar2, this.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);

        if (zar1 == zar2)
        {
            steps = zar1 + zar2;
            Debug.Log($"{steps} atýldý ve kodesten çýkýldý.");
            kodes = false;
            kodesSayaci = 0;
            KodesSayaciUpdate(kodesSayaci);
            KodesBoolUpdate(kodes);

            if (gameManager.KodesPanel.activeSelf)
            {
                gameManager.KodesPanel.SetActive(false);
            }
            kodesciftzarButton.gameObject.SetActive(false);
            kodesparaodeButton.gameObject.SetActive(false);
            Kodeskalantur.gameObject.SetActive(false);
            int stepsClient = steps;
            StartCoroutine(Move());
            ClientMove(stepsClient, gameManager.RouteYon);
        }
        else
        {
            if (gameManager.KodesPanel.activeSelf)
            {
                gameManager.KodesPanel.SetActive(false);
            }
            Kodeskalantur.gameObject.SetActive(false);
            kodesciftzarButton.gameObject.SetActive(false);
            kodesparaodeButton.gameObject.SetActive(false);
            kodesSayaci -= 1;
            KodesSayaciUpdate(kodesSayaci);
            Kodeskalantur.text = "Kodes Kalan Tur            :  " + kodesSayaci;
            StartCoroutine(WaitAndNextTurn());
        }
    }
    #endregion
    #region Kodes Para Ode
    public void kodesParaOde()
    {
        if (gameManager.KodesPanel.activeSelf)
        {
            gameManager.KodesPanel.SetActive(false);
        }
        kodes = false;
        KodesBoolUpdate(kodes);
        Kodeskalantur.gameObject.SetActive(false);
        kodesciftzarButton.gameObject.SetActive(false);
        kodesparaodeButton.gameObject.SetActive(false);
        Debug.Log("5k ödendi ve kodesten çýkýlýyor.");

        Para -= 5000;
        ParaUpdate(Para);
        dicebutton = true;
    }
    #endregion
    #region Start And End My Turn ------ incelenecek.

    public void StartMyTurn()
    {
        if (!kodes)
        {
            dicebutton = true;
            DiceButtonUpdate(true);
        }
        else
        {
            KodesFonksiyonu();
        }
    }

    // Sýranýn oyuncuda olmadýðýnda çaðrýlacak fonksiyon

    public void EndMyTurn()
    {
        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
        // Karakter hareketlerini etkinleþtir veya diðer gerekli iþlemleri yap  
    }

    public void BaslangictaHerkesinZarKapatma()
    {
        dicebutton = false;
        DiceButtonUpdate(false);
    }
    #endregion 
    #region Deprem Þehir Seçme tetikleme
    private void DepremSehriSec()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(depremMekanigi.DepremSehriSecme());
        }
    }

    #endregion
    #region Mulklerimi Tablete Yazdýrma
    public void MulkleriYazdir()
    {
        if (!Tablet.gameObject.activeSelf)
        {
            Tablet.gameObject.SetActive(true);
        }
        MulklerimPanel.gameObject.SetActive(true);
        TabletManager.Instance.MulklerimButton.interactable = false;
        for (int i = 0; i < Mulkler.Count; i++)
        {
            MulkBilgisi mulk = Mulkler[i];
            GameObject NewPlayerItem = Instantiate(MulklerimTablet) as GameObject;
            MulklerimTabletItem mulklerimTabletItem = NewPlayerItem.GetComponent<MulklerimTabletItem>();
            mulklerimTabletItem.Isim = mulk.Isim;

            mulklerimTabletItem.SetMulkValues();
            NewPlayerItem.transform.SetParent(MulklerContent.transform);
            TabletMulksItems.Add(NewPlayerItem);
        }
    }
    public void MulklerimiYazdir()
    {
        if (!Tablet.gameObject.activeSelf)
        {
            Tablet.gameObject.SetActive(true);
        }
        MulklerimPanel.gameObject.SetActive(true);
        TabletManager.Instance.MulklerimButton.interactable = false;

        if (Mulkler.Count != 0)
        {
            foreach (MulkBilgisi mulk in Mulkler)
            {
                GameObject NewPlayerItem = Instantiate(MulklerimTablet) as GameObject;
                MulklerimTabletItem mulklerimTabletItem = NewPlayerItem.GetComponent<MulklerimTabletItem>();

                int toplamDeger;
                if (mulk.LovingCity)
                {
                    if (mulk.EvOtelSayisi != 0)
                    {
                        toplamDeger = mulk.EvOtelSayisi * mulk.SatinAlmaFiyati;
                    }
                    else
                    {
                        toplamDeger = mulk.SatinAlmaFiyati;
                    }

                }
                else
                {
                    if (mulk.EvOtelSayisi == 0)
                    {
                        toplamDeger = (mulk.SatinAlmaFiyati) * 2;
                    }
                    else
                    {
                        toplamDeger = (mulk.SatinAlmaFiyati) * 3 / 2;
                        toplamDeger += mulk.RengeGoreEvSatinAlmaFiyati() * mulk.EvOtelSayisi;
                    }
                }

                double Kirabedeli = mulk.KiraBedeliHesapla(mulk.EvOtelSayisi);

                mulklerimTabletItem.Isim = mulk.Isim;
                mulklerimTabletItem.EvSayisi = mulk.EvOtelSayisi;
                mulklerimTabletItem.KiraBedeli = Kirabedeli;
                mulklerimTabletItem.SatisFiyati = toplamDeger;
                mulklerimTabletItem.CityTexture = mulk.KartMaterialDolu.mainTexture;
                mulklerimTabletItem.SetMulkValues();

                NewPlayerItem.transform.SetParent(MulklerContent.transform);
                TabletMulksItems.Add(NewPlayerItem);

                /*if (MulkSatmaZorunlulugu == true)
                {
                    mulklerimTabletItem.ArsaSatisButonu.gameObject.SetActive(true);
                    mulklerimTabletItem.ArsaSatisButonu.onClick.AddListener(() => ArsaSat(mulk, toplamDeger, NewPlayerItem));

                }
                else
                {
                    mulklerimTabletItem.ArsaSatisButonu.gameObject.SetActive(false);
                }*/

                /*if (TeamKiraDuzenleme)
                {
                    if (Mulkler.Count != 0)
                    {
                        TabletManager.Instance.SettingsButonu.interactable = false;
                        TabletManager.Instance.KapatmaButonu.interactable = false;
                        TabletManager.Instance.MulklerimButton.interactable = false;
                        TabletManager.Instance.Iflasislembitir.gameObject.SetActive(false);
                        TabletManager.Instance.OdenecekParaText.gameObject.SetActive(false);
                        MulklerimPanel.gameObject.SetActive(true);        
                        mulklerimTabletItem.ArsaKiraBedelButonu.gameObject.SetActive(true);
                        mulklerimTabletItem.ArsaKiraBedelButonu.onClick.AddListener(() => ArsaKiraBedelBelirle(mulk, 1.15));             
                    }
                }*/

                /*if (mulk.TatilYeri)
                {
                    mulklerimTabletItem.KiraText.gameObject.SetActive(false);
                    mulklerimTabletItem.EvsayisiText.gameObject.SetActive(false);
                    mulklerimTabletItem.SatisFiyatiText.gameObject.SetActive(false);
                }*/
            }
        }
    }
    public void TabletClose()
    {
        if (MulkSatmaZorunlulugu)
        {
            MulkSatmaZorunlulugu = false;
        }
        if (TeamKiraDuzenleme)
        {
            TeamKiraDuzenleme = false;
        }
        if (MulklerimPanel.activeSelf)
        {
            MulklerimPanel.gameObject.SetActive(false);
        }
        TabletManager.Instance.SettingsButonu.interactable = true;
        TabletManager.Instance.KapatmaButonu.interactable = true;
        TabletManager.Instance.MulklerimButton.interactable = true;
        TabletManager.Instance.Iflasislembitir.gameObject.SetActive(false);
        TabletManager.Instance.OdenecekParaText.gameObject.SetActive(false);
        TabletManager.Instance.CityInfoCard.SetActive(false);
        MulklerimPanel.gameObject.SetActive(false);
        Tablet.gameObject.SetActive(false);
        TabletAcik = false;
        foreach (var items in TabletMulksItems)
        {
            Destroy(items);
        }
        TabletMulksItems.Clear();
    }

    private void MulklerimAcikUpdate()
    {
        foreach (MulkBilgisi mulk in Mulkler)
        {
            foreach (GameObject items in TabletMulksItems)
            {
                MulklerimTabletItem mulklerimTabletItem = items.GetComponent<MulklerimTabletItem>();

                if (mulklerimTabletItem.Isim == mulk.Isim)
                {
                    int toplamDeger;

                    if (_currentMulk.EvOtelSayisi == 0)
                    {
                        toplamDeger = (mulk.SatinAlmaFiyati) * 2;
                    }
                    else
                    {
                        toplamDeger = (mulk.SatinAlmaFiyati) * 3 / 2;
                        toplamDeger += mulk.RengeGoreEvSatinAlmaFiyati() * mulk.EvOtelSayisi;
                    }
                    double Kirabedeli = mulk.KiraBedeliHesapla(mulk.EvOtelSayisi);

                    mulklerimTabletItem.Isim = mulk.Isim;
                    mulklerimTabletItem.EvSayisi = mulk.EvOtelSayisi;
                    mulklerimTabletItem.KiraBedeli = Kirabedeli;
                    mulklerimTabletItem.SatisFiyati = toplamDeger;
                    mulklerimTabletItem.SetMulkValues();
                }
            }
        }
    }

    #endregion
    #region Arsa Sat
    public void ArsaSat(MulkBilgisi mulk, int SatisFiyati, GameObject Mulk)
    {
        if (mulk.LovingCity)
        {
            foreach (MulkBilgisi asiksehirler in gameManager.AsikSehirler)
            {
                asiksehirler.Sahibi = null;
                asiksehirler.ArsaSahibiUpdate(null);
                asiksehirler.EvOtelSayisi = 0;
                asiksehirler.EvOtelSayisiUpdate(mulk.EvOtelSayisi);
                asiksehirler.EvPrefab = null;

                Mulkler.Remove(asiksehirler);
                MulkListesiUpdate(Mulkler);
                Destroy(asiksehirler);

                Para += SatisFiyati;
                ParaUpdate(Para);
            }
        }
        else
        {
            mulk.Sahibi = null;
            mulk.ArsaSahibiUpdate(null);
            mulk.EvOtelSayisi = 0;
            mulk.EvOtelSayisiUpdate(mulk.EvOtelSayisi);
            mulk.EvPrefab = null;

            Mulkler.Remove(mulk);
            MulkListesiUpdate(Mulkler);
            Destroy(Mulk);

            Para += SatisFiyati;
            ParaUpdate(Para);
        }
    }

    #endregion
    #region Odemek Ýçin MulkYazdýrma
    public void MulklerimiOdemekicinYazdir(double OdenecekTutar, MulkBilgisi durum)
    {
        if (Tablet.activeSelf)
        {
            TabletClose();
        }
        TabletAcik = true;
        Tablet.gameObject.SetActive(true);
        TabletManager.Instance.SettingsButonu.interactable = false;
        TabletManager.Instance.KapatmaButonu.interactable = false;
        TabletManager.Instance.MulklerimButton.interactable = false;
        MulklerimPanel.gameObject.SetActive(true);
        TabletManager.Instance.Iflasislembitir.gameObject.SetActive(true);
        TabletManager.Instance.OdenecekParaText.gameObject.SetActive(true);
        TabletManager.Instance.OdenecekTutar = OdenecekTutar;
        TabletManager.Instance.mulk = durum;

        foreach (MulkBilgisi mulk in Mulkler)
        {
            GameObject NewPlayerItem = Instantiate(MulklerimTablet) as GameObject;
            MulklerimTabletItem mulklerimTabletItem = NewPlayerItem.GetComponent<MulklerimTabletItem>();


            int toplamDeger;

            if (_currentMulk.EvOtelSayisi == 0)
            {
                toplamDeger = (mulk.SatinAlmaFiyati) * 7 / 10;
            }
            else
            {
                toplamDeger = (mulk.SatinAlmaFiyati) * 7 / 10;
                toplamDeger += (mulk.RengeGoreEvSatinAlmaFiyati() * mulk.EvOtelSayisi) * 7 / 10;
            }
            double Kirabedeli = mulk.KiraBedeliHesapla(mulk.EvOtelSayisi);

            mulklerimTabletItem.Isim = mulk.Isim;
            mulklerimTabletItem.EvSayisi = mulk.EvOtelSayisi;
            mulklerimTabletItem.KiraBedeli = Kirabedeli;
            mulklerimTabletItem.SatisFiyati = toplamDeger;
            mulklerimTabletItem.SetMulkValues();


            NewPlayerItem.transform.SetParent(MulklerContent.transform);
            TabletMulksItems.Add(NewPlayerItem);

            if (MulkSatmaZorunlulugu == true)
            {
                mulklerimTabletItem.ArsaSatisButonu.gameObject.SetActive(true);
                mulklerimTabletItem.ArsaSatisButonu.onClick.AddListener(() => ArsaSat(mulk, toplamDeger, NewPlayerItem));

            }
            else
            {
                mulklerimTabletItem.ArsaSatisButonu.gameObject.SetActive(false);
            }
            if (mulk.TatilYeri)
            {
                mulklerimTabletItem.KiraText.gameObject.SetActive(false);
                mulklerimTabletItem.EvsayisiText.gameObject.SetActive(false);
            }
        }
    }


    #endregion
    #region SatisHesaplamasý
    public void ZorunluArsaSatisi(double OdenecekTutar, MulkBilgisi mulk)
    {
        if (Para < OdenecekTutar)
        {
            //Iflasettin(); // Online
            Invoke("KodeseDustun", 2f);
            KodeseDusmek();
            mulk.Sahibi.Para += OdenecekTutar;
            mulk.Sahibi.ParaUpdate(mulk.Sahibi.Para);
        }
        else if (Para >= OdenecekTutar)
        {
            Para -= OdenecekTutar;
            ParaUpdate(Para);
            mulk.Sahibi.Para += OdenecekTutar;
            mulk.Sahibi.ParaUpdate(mulk.Sahibi.Para);
        }
        TabletManager.Instance.OdenecekTutar = 0;
        TabletManager.Instance.mulk = null;
        TabletAcik = false;
        MulklerimPanel.gameObject.SetActive(false);
        Tablet.gameObject.SetActive(false);
        TabletManager.Instance.SettingsButonu.interactable = true;
        TabletManager.Instance.KapatmaButonu.interactable = true;
        TabletManager.Instance.MulklerimButton.interactable = true;
        MulkSatmaZorunlulugu = false;

        foreach (var items in TabletMulksItems)
        {
            Destroy(items);
        }
        TabletMulksItems.Clear();
        TabletClose();

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());

    }
    #endregion
    #region Mulk Satis Ýslemi Bitir
    public void IslemiBitir(double OdenecekTutar, MulkBilgisi mulk)
    {
        ZorunluArsaSatisi(OdenecekTutar, mulk);
    }
    #endregion
    #region Ýflas Ver
    public void IflasVer()
    {
        turnManager.playersToRemove.Add(gameObject);
        this.Route = null;
        Tablet = null;
        if (Mulkler.Count != 0)
        {
            foreach (MulkBilgisi mulk in Mulkler)
            {
                mulk.Sahibi = null;
                mulk.ArsaSahibiUpdate(null);
                mulk.EvOtelSayisi = 0;
                mulk.EvOtelSayisiUpdate(mulk.EvOtelSayisi);
                mulk.EvPrefab = null;

                Mulkler.Remove(mulk);
                MulkListesiUpdate(Mulkler);
            }
        }
        IflasDurumu = true;
        // itemler vs her þey silinecek.
    }

    #endregion
    #region Route Yön Deðiþtir
    private void RouteYonDegistir()
    {
        gameManager.RouteYon = !gameManager.RouteYon;
        gameManager.RouteYonUpdate(gameManager.RouteYon);


        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }



    #endregion
    #region FlamaSatýnAlma
    public void FlamaSatinAl()
    {
        if (ZesuOzellikAktif)
        {
            double Deger = gameManager.NorulFlamPrices[NorulFlama] * 0.8f;
            Para -= Deger;
            ZesuOzellikAktif = false;
            ZesuOzellikSayisi += 1;

            if (Mulkler.Count != 0)
            {
                foreach (MulkBilgisi mulk in Mulkler)
                {
                    mulk.ArsaKiraBedelKatsayisi *= 0.8;
                    mulk.ArsaKatSayisiUpdate(mulk.ArsaKiraBedelKatsayisi);
                }
            }
        }
        else
        {
            Para -= gameManager.NorulFlamPrices[NorulFlama];
        }
        NorulFlama += 1;
        ParaUpdate(Para);
        NorulFlamaUcreti = gameManager.NorulFlamPrices[NorulFlama];
        NorulFlamaUpdate(NorulFlama, NorulFlamaUcreti);
        gameManager.MiniGameKazananAction(true, gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);


        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }

    #endregion
    #region ArsaKiraBedeliBelirle
    public void ArsaKiraBedelBelirle(MulkBilgisi mulk, double Katsayi)
    {
        if (isLocalPlayer)
        {
            mulk.ArsaKiraBedelKatsayisi *= Katsayi;
            mulk.ArsaKatSayisiUpdate(mulk.ArsaKiraBedelKatsayisi);


            if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
            {
                if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
                {
                    gameManager.DiceReset();
                }
                dicebutton = true;
                rollDiceButton.gameObject.SetActive(true);
            }
            else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
                StartCoroutine(WaitAndNextTurn());
        }
    }


    #endregion
    #region Arsa Farmlarý Toplamak
    public void ArsaFarmlariToplamak()
    {
        if (isLocalPlayer)
        {
            if (Mulkler.Count != 0)
            {
                foreach (MulkBilgisi mulk in Mulkler)
                {
                    mulk.ArsaFarmlama();
                }
            }
        }
    }
    #endregion 
    #region Anzo Köyü Rakip Rolü Çalma Fonksiyonu 

    public void RoluCal(PlayerMechanics player, string KarakterName, string TeamName)
    {
        player.gameObject.GetComponent<PlayerObjectController>().ChooseTeam(this.Teamname);
        gameObject.GetComponent<PlayerObjectController>().ChooseTeam(TeamName);
        player.gameObject.GetComponent<PlayerObjectController>().KarakterSecim(this.Karakter);
        gameObject.GetComponent<PlayerObjectController>().KarakterSecim(KarakterName);
        gameObject.tag = KarakterName;
        RolUpdate(gameObject.tag);

        //AnzoKoyu = true;

        gameManager.RolPanelKapatma();

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
        //Þans oraný %100 ayarlanacak. Sýfýrlarken de anzokoyu false olacak. Eklenecek.
    }
    #endregion
    #region Naido Köyü Rakip Birisinin Rolünü Engelleme
    public void RakipKarakterRolKisitla()
    {
        StartCoroutine(koybildirimopen(gameManager.naidotext));
        /*gameManager.OyuncuVerileriniYazdirma();
        gameManager.KarakterVeriOpenButton.gameObject.SetActive(false);
        gameManager.KarakterVeriCloseButton.gameObject.SetActive(false);

        foreach (PlayerVeriItem playerverileri in gameManager.PlayerVerileri)
        {
            if (playerverileri.playerrakip != this)
            {  
                if(playerverileri.btnAnzoRolCal.isActiveAndEnabled)
                {
                    playerverileri.btnAnzoRolCal.gameObject.SetActive(false);
                }
                playerverileri.btnNaidoRolKisitla.gameObject.SetActive(true);
            }
            playerverileri.playersahip = this;
        }*/
    }

    public void RolKisitla(PlayerMechanics player)
    {
        if (player.karakterozellikaktif)
        {
            player.karakterozellikaktif = false;
            player.KarakterOzellikAktifUpdate(player.karakterozellikaktif);
        }
        player.karakterozellikpasif = true;
        player.KarakterOzellikPasifUpdate(player.karakterozellikpasif);
        gameManager.RolPanelKapatma();

        player.CmdNaidoToplamAdimGonderme();

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }

    [Command(requiresAuthority = false)]
    public void CmdNaidoToplamAdimGonderme()
    {
        RPCNaidoToplamAdimGonderme();
    }

    [ClientRpc]
    private void RPCNaidoToplamAdimGonderme()
    {
        AdimSayisiniTutma();
    }
    public void AdimSayisiniTutma()
    {
        if (isLocalPlayer)
        {
            NaidoAdimSayisi = PlayerToplamAdim;
        }
    }

    public void NaidoAdimSayisiCheck()
    {
        if (NaidoAdimSayisi != 0)
        {
            if (PlayerToplamAdim - NaidoAdimSayisi >= 32)
            {
                karakterozellikpasif = false;
                NaidoAdimSayisi = 0;
                KarakterOzellikPasifUpdate(karakterozellikpasif);
                KarakterOzelligiYineleme();
            }
        }
    }


    #endregion
    #region Kiya Köyü Para Toplama
    private void KiyaKoyuParaToplama()
    {
        StartCoroutine(koybildirimopen(gameManager.kiyatext));
        int ToplanacakPara = 0;
        int Turagorepara = tursayisi * 150;
        if (gameManager.Hyva.Count != 0)
        {
            for (int i = 0; i < gameManager.Hyva.Count; i++)
            {
                PlayerMechanics player = gameManager.Hyva[i];
                int dice1 = Random.Range(1, 7);
                int dice2 = Random.Range(1, 7);
                gameManager.ZarAction(dice1, dice2, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                int deger = dice1 + dice2;
                int oyuncudanpara = deger * Turagorepara;


                if (player.Para < oyuncudanpara)
                {
                    player.KodeseDusmek();
                }
                else
                {
                    player.Para -= oyuncudanpara;
                    player.ParaUpdate(player.Para);
                    gameManager.KiyaKoyuOdeyen(oyuncudanpara, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                    ToplanacakPara += oyuncudanpara;
                }
            }
        }
        if (gameManager.Oyul.Count != 0)
        {
            for (int i = 0; i < gameManager.Oyul.Count; i++)
            {
                PlayerMechanics player = gameManager.Oyul[i];
                int dice1 = Random.Range(1, 7);
                int dice2 = Random.Range(1, 7);
                gameManager.ZarAction(dice1, dice2, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                int deger = dice1 + dice2;
                int oyuncudanpara = deger * Turagorepara;
                if (player.Para < oyuncudanpara)
                {
                    player.KodeseDusmek();
                }
                else
                {
                    player.Para -= oyuncudanpara;
                    player.ParaUpdate(player.Para);
                    gameManager.KiyaKoyuOdeyen(oyuncudanpara, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                    ToplanacakPara += oyuncudanpara;
                }
            }
        }
        if (gameManager.Anzo.Count != 0)
        {
            for (int i = 0; i < gameManager.Anzo.Count; i++)
            {
                PlayerMechanics player = gameManager.Anzo[i];
                int dice1 = Random.Range(1, 7);
                int dice2 = Random.Range(1, 7);
                gameManager.ZarAction(dice1, dice2, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                int deger = dice1 + dice2;
                int oyuncudanpara = deger * Turagorepara;
                if (player.Para < oyuncudanpara)
                {
                    player.KodeseDusmek();
                }
                else
                {
                    player.Para -= oyuncudanpara;
                    player.ParaUpdate(player.Para);
                    gameManager.KiyaKoyuOdeyen(oyuncudanpara, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                    ToplanacakPara += oyuncudanpara;
                }
            }
        }
        if (gameManager.Naido.Count != 0)
        {
            for (int i = 0; i < gameManager.Naido.Count; i++)
            {
                PlayerMechanics player = gameManager.Naido[i];
                int dice1 = Random.Range(1, 7);
                int dice2 = Random.Range(1, 7);
                gameManager.ZarAction(dice1, dice2, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                int deger = dice1 + dice2;
                int oyuncudanpara = deger * Turagorepara;
                if (player.Para < oyuncudanpara)
                {
                    player.KodeseDusmek();
                }
                else
                {
                    player.Para -= oyuncudanpara;
                    player.ParaUpdate(player.Para);
                    gameManager.KiyaKoyuOdeyen(oyuncudanpara, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                    ToplanacakPara += oyuncudanpara;
                }
            }
        }
        if (gameManager.Zesu.Count != 0)
        {
            for (int i = 0; i < gameManager.Zesu.Count; i++)
            {
                PlayerMechanics player = gameManager.Zesu[i];
                int dice1 = Random.Range(1, 7);
                int dice2 = Random.Range(1, 7);
                gameManager.ZarAction(dice1, dice2, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                int deger = dice1 + dice2;
                int oyuncudanpara = deger * Turagorepara;
                if (player.Para < oyuncudanpara)
                {
                    player.KodeseDusmek();
                }
                else
                {
                    player.Para -= oyuncudanpara;
                    player.ParaUpdate(player.Para);
                    gameManager.KiyaKoyuOdeyen(oyuncudanpara, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                    ToplanacakPara += oyuncudanpara;
                }
            }
        }
        if (ToplanacakPara != 0)
        {
            int paraperperson = ToplanacakPara / gameManager.Kiya.Count;
            foreach (PlayerMechanics player in gameManager.Kiya)
            {
                player.Para += paraperperson;
                player.ParaUpdate(player.Para);
                gameManager.KiyaKoyuOdenen(paraperperson, player.gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
            }
        }
        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }

    #endregion
    #region Oyul Köyü Kira Ödememe Fonksiyonu
    private void OyulKoyuKiraOdememe()
    {
        StartCoroutine(koybildirimopen(gameManager.oyulText));
        currentadim = PlayerToplamAdim;
        OyulKiraOdememek = true;

        if (zar1 == zar2 && turnManager.currentPlayer == gameObject)
        {
            if (gameManager.dice1.diceValue != 0 && gameManager.dice2.diceValue != 0)
            {
                gameManager.DiceReset();
            }
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else if (zar1 != zar2 && turnManager.currentPlayer == gameObject)
            StartCoroutine(WaitAndNextTurn());
    }

    private void OyulCheckAdimSayisi()
    {
        if (PlayerToplamAdim - currentadim >= 32)
        {
            OyulKiraOdememek = false;
        }
    }


    #endregion
    #endregion
    #region Karakter Fonksiyonlarý
    #region KarakterÝhtimalOranlarý
    public void KarakterSans()
    {
        karakterozellikaktif = true;
        KarakterOzellikAktifUpdate(karakterozellikaktif);

        if (Karakter == "Wallector")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 0.3f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Builder")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 0.5f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Sheriff")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }

        if (zar1 == zar2)
        {
            dicebutton = true;
            rollDiceButton.gameObject.SetActive(true);
        }
        else
            StartCoroutine(WaitAndNextTurn());
    }

    public void KarakterOzelligiYineleme()
    {
        if (Karakter == "Wallector")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 0.3f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Builder")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 0.5f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Sheriff")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Haydut")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Temporsium")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Farmer")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        else if (Karakter == "Anzorien")
        {
            if (karakterozellikaktif)
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else if (karakterozellikpasif)
            {
                karakterozelliksans = 0f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
            else
            {
                karakterozelliksans = 1f;
                KarakterOzellikSansUpdate(karakterozelliksans);
            }
        }
        AnzoKoyu = false;
    }
    #endregion
    #region Ayný Karedeki Oyuncularý Tutma
    public void AyniKaredekiOyuncular()
    {
        AyniKaredekiPlayerler.Clear(); // Listeyi temizleyip tekrar eklemeler yapýlýp iþlem yapýlýyor.
        for (int i = 0; i < manager.GamePlayers.Count; i++)
        {
            PlayerMechanics player = manager.GamePlayers[i].gameObject.GetComponent<PlayerMechanics>();
            PlayerObjectController oyuncu = manager.GamePlayers[i];
            if (this.routePosition == player.routePosition && gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber != oyuncu.PlayerIDNumber && this.Teamname != player.Teamname)
            {
                Debug.Log("OyuncuListeyeEklendi");
                Debug.Log(PlayerObjectController.PlayerName);
                AyniKaredekiPlayerler.Add(player);
            }
        }
        PasifRollerinCagrilmasi();
    }

    #endregion
    #region Taglara Göre Pasif Rollerin Çaðrýlmasý
    private void PasifRollerinCagrilmasi()
    {
        KarakterOzelligiYineleme();
        if (Karakter == "Sheriff")
        {
            SheriffKodeseGonderir();
        }
        else if (Karakter == "Haydut")
        {
            HaydutParaCalar();
        }
    }
    #endregion
    #region Temporsium Fonksiyonu

    private void TemporsiumPanelAc()
    {
        gameManager.TemporsiumZarPanel.SetActive(true);
        gameManager.TemporsiumMoralText.text = TemporsiumMoral.ToString();
        if (TemporsiumMoral > 30)
        {
            gameManager.TemporsiumTekrarZarButton.interactable = true;
            gameManager.TemporsiumTekrarZarButtonText.text = "Roll Again";
        }

    }


    private void TemporsiumPanelKapat()
    {
        gameManager.TemporsiumZarPanel.SetActive(false);
    }
    public void TekrardanZarAT()
    {
        TemporsiumPanelKapat();
        TemporsiumMoral -= 20;
        TemporsiumZarAtti = true;
        RollDice();
        gameManager.ActionCharacterSkillsVerme("Temporsium", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
    }

    public void ZarAtmaktanVazgec()
    {
        TemporsiumPanelKapat();
        int ClientZar = steps;
        //CameraChanger(1);
        StartCoroutine(Move());
        ClientMove(ClientZar, gameManager.RouteYon);
    }

    public void TemporsiumKiraDengelemesi()
    {
        if (TemporsiumMoral >= 30 && TemporsiumMoral <= 70)
        {
            TemporsiumKatsayisi = 1;
        }
        else if (TemporsiumMoral < 30)
        {
            TemporsiumKatsayisi = 0.5;
        }
        else if (TemporsiumMoral > 70)
        {
            TemporsiumKatsayisi = 2;
        }
        TemporsiumKatsayisiUpdate(TemporsiumKatsayisi);
    }

    public void TemporsiumMoralArtti(int Deger)
    {
        TemporsiumMoral += Deger;
        TemporsiumMoralUpdate(TemporsiumMoral);
    }

    public void TemporsiumMoralAzaldi(int Deger)
    {
        TemporsiumMoral -= Deger;
        TemporsiumMoralUpdate(TemporsiumMoral);
    }

    #endregion
    #region Sheriff Fonksiyonu
    public void SheriffKodeseGonderir()
    {
        if (Random.Range(0.0f, 1.0f) < karakterozelliksans)
        {
            if (AyniKaredekiPlayerler.Count != 0)
            {
                for (int i = 0; i < AyniKaredekiPlayerler.Count; i++)
                {
                    PlayerMechanics player = AyniKaredekiPlayerler[i];

                    player.KodeseDusmek();
                    AyniKaredekiPlayerler.Remove(player);
                    KodeseGonderilenler.Add(player);
                    Debug.Log("ÞerifKodeseGönderdi");
                }
                KodeseGonderildi = true;
                StartCoroutine(KodeseGonderilenPlayers());
                gameManager.ActionCharacterSkillsVerme("Sheriff", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
            }
        }
    }


    private IEnumerator KodeseGonderilenPlayers()
    {
        while (KodeseGonderildi)
        {
            bool KodesVarildi = false;
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < KodeseGonderilenler.Count; i++)
            {
                PlayerMechanics player = KodeseGonderilenler[i];
                if (player.kodes)
                {
                    KodesVarildi = true;
                }
                else
                {
                    KodesVarildi = false;
                    break;
                }
            }
            if (KodesVarildi)
            {
                for (int i = 0; i < KodeseGonderilenler.Count; i++)
                {
                    PlayerMechanics player = KodeseGonderilenler[i];
                    KodeseGonderilenler.Remove(player);
                }
                KodeseGonderildi = false;
            }
        }
    }
    #endregion
    #region Haydut Fonksiyonu
    public void HaydutParaCalar()
    {
        if (Random.Range(0.0f, 1.0f) < karakterozelliksans)
        {
            if (AyniKaredekiPlayerler.Count != 0)
            {
                for (int i = 0; i < AyniKaredekiPlayerler.Count; i++)
                {
                    PlayerMechanics player = AyniKaredekiPlayerler[i].gameObject.GetComponent<PlayerMechanics>();

                    double CalinacakPara = player.Para / 10;

                    player.Para -= CalinacakPara;
                    player.ParaUpdate(player.Para);

                    this.Para += CalinacakPara;
                    ParaUpdate(Para);


                    AyniKaredekiPlayerler.Remove(player);
                }
                gameManager.ActionCharacterSkillsVerme("Haydut", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                AyniKaredekiPlayerler.Clear();
            }
        }
    }



    #endregion
    #region Builder Fonksiyonu
    public int Evfiyati()
    {

        int yenievfiyati = _currentMulk.RengeGoreEvSatinAlmaFiyati();

        if (Karakter == "Builder")
        {
            if (Random.Range(0.0f, 1.0f) < karakterozelliksans)
            {
                Debug.Log($"Builder evleri birim baþýna {yenievfiyati / 2} fiyatýyla yarý yarýya ödedi.");
                gameManager.ActionCharacterSkillsVerme("Builder", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                return yenievfiyati / 2;
            }
        }
        return yenievfiyati;

    }
    #endregion
    #region Wallector Fonksiyonu
    public double CalculateRent(double originalKiraBedeli, bool hesaplatmak)
    {

        if (Karakter == "Wallector" && hesaplatmak)
        {

            if (Random.Range(0.0f, 1.0f) < karakterozelliksans)
            {
                //ChracterSkillPanelOnline();
                Debug.Log("WallectorCalisti");
                gameManager.ActionCharacterSkillsVerme("Wallector", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
                return originalKiraBedeli / 2;

            }
        }
        return originalKiraBedeli * TemporsiumKatsayisi;
    }
    #endregion
    #region Harvestor Fonksiyonu

    public void HasatToplama()
    {
        if (Karakter == "Harvestor")
        {
            NorulFlama++;
            gameManager.FlamaDeger = gameManager.FlamaDeger + (gameManager.FlamaDeger * 1 / 2);
            NorulFlamaUcreti = gameManager.FlamaDeger;
            NorulFlamaUpdate(NorulFlama, NorulFlamaUcreti);
            gameManager.ActionMinigameKazanan(true, gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
            harvestorstacks = 0;
            //ChracterSkillPanelOnline();
            gameManager.ActionCharacterSkillsVerme("Harvestor", gameObject.GetComponent<PlayerObjectController>().PlayerIDNumber);
        }
    }

    #endregion
    #endregion
    #region Karakter Panel
    private void PlayerMechanics_OnCharacterSkill(object sender, EventArgs e)
    {
        SkillPanel();

    }
    public void ChracterSkillPanelOnline()
    {
        if (isServer)
        {
            RPCSkillPanel();

        }
        else
        {
            CMDSkillPanel();
        }

    }
    [Command(requiresAuthority = false)]
    private void CMDSkillPanel()
    {
        RPCSkillPanel();
    }
    [ClientRpc]
    private void RPCSkillPanel()
    {
        OnCharacterSkill?.Invoke(this, EventArgs.Empty);
    }
    public void SkillPanel()
    {

        string myTag = gameObject.tag;
        switch (myTag)
        {
            case "Anzorien":
                Panel029.texture = gameManager.CharacterTexture[0];
                break;
            case "Builder":
                Panel029.texture = gameManager.CharacterTexture[1];
                break;
            case "Farmer":
                Panel029.texture = gameManager.CharacterTexture[2];
                break;
            case "Harvester":
                Panel029.texture = gameManager.CharacterTexture[3];
                break;
            case "Haydut":
                Panel029.texture = gameManager.CharacterTexture[4];
                break;
            case "Sheriff":
                Panel029.texture = gameManager.CharacterTexture[5];
                break;
            case "Temporsium":
                Panel029.texture = gameManager.CharacterTexture[6];
                break;
            case "Wallector":
                Panel029.texture = gameManager.CharacterTexture[7];
                break;
            default:
                Debug.Log("failed texture change");
                break;
        }
        SkillPanelMode = true;
        StartCoroutine(Abc());
    }

    public IEnumerator Abc()
    {
        yield return new WaitForSeconds(2f);
        SkillPanelMode = false;
    }
    #endregion
    #region Reach Object 
    public void ReachObjectOpen()
    {
        int Deger;
        if (gameManager.RouteYon)
        {
            if (routePosition + steps >= 32)
            {
                Deger = (routePosition + steps) % 32;
            }
            else
            {
                Deger = routePosition + steps;
            }
        }
        else
        {
            if (routePosition - steps < 0)
            {
                Deger = 32 - (steps - routePosition);
            }
            else
            {
                Deger = routePosition - steps;
            }
        }

        Debug.Log("Verilen Deger" + Deger);
        VarisNoktasiUpdate(Deger);
    }
    public void ReachObjectClose()
    {
        ReachObject.SetActive(false);
    }

    public void VarisNoktasiVerme(int Deger)
    {
        ReachObject.transform.SetParent(gameManager.MulklerForReachObject[Deger].transform);
        ReachObject.transform.position = currentRoute.childNodeList[Deger].transform.position;
        //Vector3 vector = new Vector3(ReachObject.transform.position.x, ReachObject.transform.position.y + 2f, ReachObject.transform.position.z);
        //ReachObject.transform.position = vector;
        //ReachObject.SetActive(true);
        Invoke("ReachObjectAcma", 0.1f);
    }
    private void ReachObjectAcma()
    {
        ReachObject.SetActive(true);
    }
    #endregion
    #region Player To Out Map
    public void PlayerToOutMap()
    {
        transform.position = gameManager.OutMapPlace.position;
    }

    #endregion
    public IEnumerator koybildirimopen(GameObject panel)
    {
        /*gameManager.koybildirimtext.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameManager.koybildirimtext.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);*/
        yield return new WaitForSeconds(0.1f);
    }
    #region Notifications Open

    public void Notifications(int Value)
    {
        if (Value == 0) //0 = Yetersiz Para
        {
            StartCoroutine(NotificationManager.instance.OpenYetersizPara());
        }
        else if (Value == 1) //32 adým atmadan flama alýnamaz
        {
            StartCoroutine(NotificationManager.instance.StepsForFlama());
        }
        else if (Value == 2)
        {
            StartCoroutine(NotificationManager.instance.IflasEtti());
        }
    }
    public void NotificationOpen(int Value)
    {
        if (isServer)
        {
            RPCNotificationOpen(Value);
        }
        else
        {
            CmdNotificationOpen(Value);
        }
    }

    [ClientRpc]
    private void RPCNotificationOpen(int Value)
    {
        Notifications(Value);
    }

    [Command(requiresAuthority = false)]
    private void CmdNotificationOpen(int Value)
    {
        RPCNotificationOpen(Value);
    }

    #endregion
    #region Ýflas Etme Fonksiyonu
    public IEnumerator IflasEtmek()
    {
        BankruptedUpdate(true);
        NotificationOpen(2);
        PlayerToOutMapUpdate();
        if (Mulkler.Count > 0)
        { 
            for (int i = 0; i < Mulkler.Count; i++)
            {
                    MulkBilgisi mulk = Mulkler[i];
                    mulk.SetMulkDefaultUpdate();               
            }
            Mulkler.Clear();
        }
        yield return new WaitForSeconds(0.2f);
        turnManager.IflasEdenOyuncuyuEkleme(gameObject);
    }



    #endregion
}



