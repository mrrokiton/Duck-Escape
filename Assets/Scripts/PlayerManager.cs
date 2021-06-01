using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    
    PhotonView PV;
    //GameObject C_controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PV.IsMine)
        {
            CreateController();
        }
    }

    // Update is called once per frame
    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
        
        //C_controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CoinController"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID} );

        //Debug.Log("Instantiated Player Controller");
    }

    /*public void DIE()
    {
        PhotonNetwork.Destroy(C_controller);
        CreateController();
    }*/
}
