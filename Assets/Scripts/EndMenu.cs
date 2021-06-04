using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public static EndMenu Instance;

    public static int level = 1;
    public static int first_level = 1;
    public static int last_level = 2;
    
    [SerializeField] GameObject menu;

    [SerializeField] GameObject menu_button;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;   
    }

    void Update()
    {       
        EnemyController.death = 0;
       
        menu_button.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void RestartLevel()
    {
        PhotonNetwork.LoadLevel(level);
    }

    public void RestartGame()
    {
        level = first_level;
        PhotonNetwork.LoadLevel(level);
    }

    public void NextLevel()
    {
        level++;
        PhotonNetwork.LoadLevel(level);
    }
}
