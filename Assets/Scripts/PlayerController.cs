using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if(!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        } else {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        
    }

    void Update()
    {
        if (!PV.IsMine) {
            return;
        }

        Look();
        Move();
        Jump();

        /*if(transform.position.y < -10f)
        {
            transform.position.y = 3f;
        }*/
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        float axisHorizontal = Input.GetAxisRaw("Horizontal");
        float axisVertical = Input.GetAxisRaw("Vertical");
        bool isWalking = axisHorizontal != 0 || axisVertical != 0;
        animator.SetBool("Walking", isWalking);

        if (PV.IsMine) {
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable {
                { "Walking", isWalking }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }

        Vector3 moveDir = new Vector3(axisHorizontal, 0, axisVertical).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {
        if (!PV.IsMine && targetPlayer == PV.Owner) {
            animator.SetBool("Walking", (bool) changedProps["Walking"]);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    /*void OnTriggerEnter(Collider collider)
    {
        Death(collider);
    }

    void Death(Collider collider)
    {
       
        if (collider.gameObject.tag == "Enemy")
        {
            
                CoinController.score = 0;
                PhotonNetwork.LoadLevel(3);
                Debug.Log("HEHE BOI");
            
        }

    }*/
}
