using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class playerController : MonoBehaviourPun
{
    public Player photonPlayer;

   // public List<Unit> units = new List<Unit>();
   // private Unit selectedUnit;

    public static playerController me;
    public static playerController enemy;

    [PunRPC]
    private void Initialize(Player player)
    {
        photonPlayer = player;

        if (player.IsLocal)
        {
            me = this;
        }
        else
        {
            enemy = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
