using Photon.Pun;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    public event System.Action OnRoom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Conectando a Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("En el Lobby");
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: "My Room");
    }

    public override void OnJoinedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log("Joined Room: " + roomName + ", PlayerCount: " + playerCount);

        OnRoom?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a room random: " + message + " - Creando nueva room...");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Nuevo jugador entro: " + newPlayer.NickName + " | Total: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Jugador salio: " + otherPlayer.NickName + " | Total: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado: " + cause);
    }
}
