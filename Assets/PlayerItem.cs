using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviour
{
    public Text playerName;

    Image backgroundImage;
    public Color highlightColor;
    public GameObject flechaDerecha;
    public GameObject flechaIzquierda;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
    }

    public void ApplyLocalChanges()
    {
        //backgroundImage.color = highlightColor;
        flechaDerecha.SetActive(true);
        flechaIzquierda.SetActive(true);
    }
}
