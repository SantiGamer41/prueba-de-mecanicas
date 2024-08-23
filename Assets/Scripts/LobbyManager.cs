using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text nombreRoom;
    public Text nombreBoton;
    public RoomItem roomItemPrefab;

    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float tiempoEntreUpdate = 1.5f;
    float nextUpateTime;

    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.JoinLobby();

        audioManager.Instance.PlaySoundLobby();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCrear()
    {
        if(roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text);
            nombreBoton.text = "Creando Sala...";
        }
        
    }
    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        nombreRoom.text = "Nombre de la sala: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= nextUpateTime)
        {
            UpdateRoomList(roomList);
            nextUpateTime = Time.time + tiempoEntreUpdate;
        }

        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickSalir()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
