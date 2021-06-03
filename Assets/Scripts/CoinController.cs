using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CoinController : MonoBehaviour
{
    public static int score = 0;
    static int max_score = 3;

    PlayerManager playerManager;

    PhotonView PV;

    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip clip;

    
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
        //score++;
        if (collider.gameObject.tag == "Player")
        {
            score++;
            sound.Play();
            if (PhotonNetwork.IsMasterClient)
            {
                //this.gameObject.SetActive(false);
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
                Debug.Log("DESTROYED " + score);
                

                if (score == max_score)
                {
                    score = 0;
                    if (SceneManager.GetActiveScene().buildIndex!=EndMenu.last_level)
                        PhotonNetwork.LoadLevel(4);
                    else
                        PhotonNetwork.LoadLevel(5);
                }
            }
        }
    }

    /*void DIE()
    {
        playerManager.DIE();
    }*/
}
