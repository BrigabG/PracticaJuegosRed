using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int playersRequired = 2;
    [SerializeField] private float countdownDuration = 5f;

    private bool timerStarted = false;

    // Detecta cuando un jugador se une a la sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Solo el Master Client gestiona el inicio del temporizador
        if (!PhotonNetwork.IsMasterClient || timerStarted) return;

        // Verifica si se alcanzó la cantidad mínima de jugadores
        if (PhotonNetwork.CurrentRoom.PlayerCount >= playersRequired)
        {
            timerStarted = true;
            double startTime = PhotonNetwork.Time + countdownDuration;
            
            // Envía el tiempo exacto de inicio a todos los clientes
            photonView.RPC("RPC_StartRaceCountdown", RpcTarget.AllBuffered, startTime);
        }
    }
}