using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using WebSocketSharp;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    Player[] ListaPlayers;

    [Header("Login UI Elements")]
    [Header("Seccion Login")]
    [SerializeField] GameObject LoginUIPanel;
    [SerializeField] TMP_InputField PlayerNameInput;
    [SerializeField] Button LoginButton;


    [Header("Loading Panels")]
    [SerializeField] GameObject ConnectingInfoUIPanel;
    [SerializeField] GameObject alertUIPanel;
    [SerializeField] string GM; //Game Mode

    [SerializeField] Slider RoomSize;//Room Size val
    //[Range(1.0f, 6.0f)]
    [SerializeField] int Room_Size = 3;

    [SerializeField] TextMeshProUGUI SizeText; //Room Size text


    public Dictionary<int, GameObject> playerlistGO;

    bool ingame;


    // Start is called before the first frame update
    void Start()
    {
        ingame = false;
        PV=GetComponent<PhotonView>();
        LoginButton.gameObject.SetActive(true);
        RoomSize.onValueChanged.AddListener((SizeChange) =>
        {
            SizeText.text = SizeChange.ToString("0");
            Room_Size = (int)SizeChange;
        });
        SizeText.text = RoomSize.value.ToString("0");
        Room_Size = (int)RoomSize.value;
        ActivatePanel(ConnectingInfoUIPanel.name);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

    }



    #region Local Functions
    void NickNamePlayerPrf()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            if (PlayerPrefs.GetString("NickName").IsNullOrEmpty())
            {
                PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000);

            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");

            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000);
        }

        PlayerNameInput.text = PhotonNetwork.NickName;


    }


    void createRoom()
    {
        Debug.Log("creando sala...");
        int randomRoomNumber = UnityEngine.Random.Range(0, 100);

        RoomOptions roomOpc = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)0
        };


        PhotonNetwork.CreateRoom("Sala " + randomRoomNumber, roomOpc);

    }

    #endregion

    #region UI Callback Methods
    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        alertUIPanel.SetActive(alertUIPanel.name.Equals(panelNameToBeActivated));
    }

    public void QuickStart()
    {
        LoginButton.gameObject.SetActive(false);
        PhotonNetwork.NickName = PlayerNameInput.text;

        PhotonNetwork.JoinRandomRoom();


    }

    public void SetGameMode(string GameMode)//Funcion para asignar el modo de juego
    {
        GM = GameMode;

    }

    public void OnJoinRandomRoomClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    #endregion

    #region photon Callbacks

    public override void OnConnected()
    {

        NickNamePlayerPrf();
        //ActivatePanel(LoginUIPanel.name);
        Debug.Log("Connected to Internet");


    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log(PhotonNetwork.NickName + " is connected to photon");


        ActivatePanel(LoginUIPanel.name);

        LoginButton.gameObject.SetActive(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartGameBtn.gameObject.SetActive(checkPlayersReady());


        }
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " Is created");

    }
    
    public override void OnJoinedRoom()
    {
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        int PlayersInSala = photonPlayers.Length;
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Join to " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel(1);

            /*
        ActivatePanel(InsideRoomUIPanel.name);

        RoomInfo.text = "Nombre de la sala: " + PhotonNetwork.CurrentRoom.Name + " Tamaño de la sala: " + PhotonNetwork.CurrentRoom.MaxPlayers +
    " Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerlistGO == null)
        {

            playerlistGO = new Dictionary<int, GameObject>();

        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject PlayerUser = Instantiate(PrefabPlayerList);
            PlayerUser.transform.SetParent(ListContent);
            // ListContent.transform.localScale = Vector3.one;
            PlayerUser.GetComponent<PlayerListInicializer>().initialize(player.NickName, true, player.ActorNumber);

            
            playerlistGO.Add(player.ActorNumber, PlayerUser);

        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            object GameModeName;//Get Game mode
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gm", out GameModeName))
            {

                Debug.Log(GameModeName.ToString());

            }
            else
            {
                Debug.Log("No existe el modo de juego: " + GameModeName.ToString());
            }
        }
        else
        {
            Debug.Log(PhotonNetwork.CurrentRoom.Name + " Sin modo de juego, de tamaño " + PhotonNetwork.CurrentRoom.MaxPlayers);
        }

        if (PlayersInSala == 3)
        {

            PV.RPC("IniciarMaster", RpcTarget.AllBuffered);

        }
            Debug.Log("jugadores: " + PlayersInSala);
        */
    }
    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("A new player has joined the room. HI! " + newPlayer.NickName);
        // ListContent.transform.localScale = Vector3.one;

       // StartGameBtn.gameObject.SetActive(checkPlayersReady());
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log("CYA " + otherPlayer.NickName);
        //StartGameBtn.gameObject.SetActive(checkPlayersReady());

    }

    public override void OnLeftRoom()
    {
        Debug.Log("salio de la sala");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("failed to join a random room not found");
        createRoom();

    }

    #endregion

    #region RPC Functions
    [PunRPC]
    void RPC_AsignarTagPhoton(string tag)
    {
        
        PhotonNetwork.NickName= tag;
        PhotonNetwork.LocalPlayer.TagObject = tag;

    }


    #endregion
}
