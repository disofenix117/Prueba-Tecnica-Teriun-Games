using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    enum playerState
    {
        bussy,
        walking,
        begin
    }
    //Componentes
    public Rigidbody RB;
    public int puntaje;
    int IndexPJ;
    public string nameGO;
    public GameObject[] Personajes;
    CapsuleCollider CC;
    Animator Anim;
    PhotonView PV;

    [Header("Referencias de Objetos")]
    //variables de la Camara
    [SerializeField] Camera Camera;
    [SerializeField] GameObject CameraHolder;

    [SerializeField] GameObject Geometry;
    [SerializeField] GameObject PanelBegin;

    [SerializeField] TextMeshPro NIcknameGO;

    [SerializeField] GameObject PrefabPlayerList;
    [SerializeField] Transform PanelJugadores;


    //Variables Jugador
    bool Grounded;//esta en el suelo?
    [SerializeField] float speedPlayer, walkPlayer, smoothTime, mouseSensivility, verticalLookRot, JumpForce;//entradas y movimientos del jugador
    Vector3 smoothmoveVel;
    public Vector3 moveAmount;
    float MovVert, MovHor;//entradas de posicion
    public bool isPlayerEnabled;//Jugador Habilitado


    private void Awake()
    {
        CC = GetComponent<CapsuleCollider>();
        RB = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        PV = PV == null ? GetComponent<PhotonView>() : PV;
        CC.enabled = false;

        Geometry = transform.GetChild(1).gameObject;

        //Cursor.lockState = CursorLockMode.Locked;//bloquea el mouse para no salirse de la pantalla
    }

    IEnumerator HabColyStuff()
    {
        yield return new WaitForSeconds(1.2f);
        CC.enabled = true;
        PV.RPC("RPC_ASignarNombres", RpcTarget.AllViaServer);
        ListaJugadores();
    }

    void ListaJugadores()
    {

        foreach (Transform hijo in PanelJugadores.transform)
        {
            Destroy(hijo.gameObject);
        }
        PanelJugadores.transform.DetachChildren();

        foreach (Player player in PhotonNetwork.PlayerList)
        {

            GameObject PlayerUser = Instantiate(PrefabPlayerList);
            PlayerUser.transform.SetParent(PanelJugadores.transform, false);
            GameManeger.instance.ListaJugadores.Add(PlayerUser);

            PlayerUser.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.NickName;
            Debug.Log("estanceadi");
            int i = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(Camera);
            Destroy(RB);
            //Destroy(PanelBegin);
        }
        else
        {
            Camera.gameObject.SetActive(true);


            NIcknameGO.gameObject.SetActive(false);
        }
        isPlayerEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(HabColyStuff());

    }

    void look()
    {
        //movimiento de la vista
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensivility);
        verticalLookRot += Input.GetAxisRaw("Mouse Y") * mouseSensivility;
        //limites de movimiento de la vista
        verticalLookRot = Mathf.Clamp(verticalLookRot, -90f, 90f);
        CameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRot;

    }
    void move()
    {
        Vector3 movDir = new Vector3(MovHor, 0, MovVert).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, movDir * (Input.GetKey(KeyCode.LeftShift) ? speedPlayer : walkPlayer), ref smoothmoveVel, smoothTime);

    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            RB.AddForce(transform.up * JumpForce);

        }

    }
    public void GroundedState(bool isGround)
    {
        Grounded = isGround;

    }
    void animations()
    {
        //Corriendo
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Anim.SetBool("isRunning", true);

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Anim.SetBool("isRunning", false);

        }
        //Caminando
        if (MovVert != 0 || MovHor != 0)
        {
            Anim.SetBool("isWalking", true);

        }
        else
        {
            Anim.SetBool("isWalking", false);

        }
        //Saltando
        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Anim.SetTrigger("isJumping");
        }
    }
    void BuscarIndexName()
    {
        for (int i = 0; i < GameManeger.instance.ListaJugadores.Count; i++)
        {
            if (GameManeger.instance.ListaJugadores[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == NIcknameGO.text)
            {
                this.name=NIcknameGO.text;
                
                IndexPJ = i; break;
            }
        }
    }
    private void Update()
    {
        if (!PV.IsMine) return;

        MovVert = Input.GetAxisRaw("Vertical");
        MovHor = Input.GetAxisRaw("Horizontal");

        look();
        move();
        Jump();
        animations();




    }
    private void FixedUpdate()
    {
        if (!PV.IsMine) return;

        if (isPlayerEnabled)
        {
            RB.MovePosition(RB.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        }

    }
   

    #region "funciones RPC"

    [PunRPC]
    void RPC_ListaJugadores()
    {
        ListaJugadores();

    }


    [PunRPC]
    void RPC_ASignarNombres()
    {

        NIcknameGO.text = PV.Owner.NickName;


    }
    [PunRPC]
    void RPC_SumarPuntaje(string who,int Cant)
    {
        for (int i = 0; i < PanelJugadores.childCount; i++)
        {


            if (PanelJugadores.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == who)
            {
                ListaJugadores();
                PanelJugadores.transform.GetChild(0).transform.GetChild(1). GetComponent<TextMeshProUGUI>().text = Cant.ToString();
                Debug.Log("Si esta!");
            }


        }


    }


    #endregion




}
