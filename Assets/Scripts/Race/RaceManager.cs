using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class RaceManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int playersRequired = 2;
    [SerializeField] private float countdownDuration = 5f;
    private bool timerStarted = false;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient || timerStarted) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount >= playersRequired)
        {
            timerStarted = true;
            double startTime = PhotonNetwork.Time + countdownDuration;

            // Llama al RPC que ahora vive en ESTE mismo script / GameObject (PhotonView 4)
            photonView.RPC("RPC_StartRaceCountdown", RpcTarget.AllBuffered, startTime);
        }
    }

    [PunRPC]
    private void RPC_StartRaceCountdown(double targetNetworkTime)
    {
        StartCoroutine(ExecuteCountdown(targetNetworkTime));
    }

    private IEnumerator ExecuteCountdown(double targetNetworkTime)
    {
        while (PhotonNetwork.Time < targetNetworkTime)
        {
            Debug.Log("Cuenta regresiva: " + (targetNetworkTime - PhotonNetwork.Time).ToString("F2") + " segundos");
            yield return null; // IMPORTANTE: Evita un bucle infinito que congele el juego
        }

        Debug.Log("¡Carrera iniciada!");
        EnableAllTanksMovement();
    }

    private void EnableAllTanksMovement()
    {
        // Busca todos los tanques en la escena local y activa su movimiento
        TankController[] allTanks = FindObjectsOfType<TankController>();
        foreach (TankController tank in allTanks)
        {
            tank.EnableMovement();
        }
    }
}
