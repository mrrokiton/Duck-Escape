using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoinController : MonoBehaviour
{
    static int score = 0;

    PlayerManager playerManager;

    PhotonView PV;

    void Awake()
    {
        //PV.GetComponent<PhotonView>();

        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Score(other);    
    }

    void Score(Collider collider)
    {
        score++;
        if (PhotonNetwork.IsMasterClient)
        {
            if (collider.gameObject.tag == "Player")
            {
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
                Debug.Log("DESTROYED " + score);
            }
        }
    }

    /*void DIE()
    {
        playerManager.DIE();
    }*/
}
