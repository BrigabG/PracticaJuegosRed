using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private float resetDelay = 3f;

    private bool roundOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Called by TankHealth when a tank dies
    public void NotifyPlayerDied()
    {
        if (!PhotonNetwork.IsMasterClient || roundOver) return;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Find all tanks that are still alive
        TankHealth[] allTanks = FindObjectsOfType<TankHealth>();

        int aliveCount = 0;
        TankHealth lastAlive = null;

        foreach (TankHealth tank in allTanks)
        {
            if (!tank.IsDead)
            {
                aliveCount++;
                lastAlive = tank;
            }
        }

        if (aliveCount <= 1)
        {
            roundOver = true;
            int winnerActor = lastAlive != null ? lastAlive.photonView.OwnerActorNr : -1;
            string winnerName = $"Player{winnerActor}";
            photonView.RPC("RPC_AnnounceWinner", RpcTarget.All, winnerName);
        }
    }

    [PunRPC]
    private void RPC_AnnounceWinner(string winnerName)
    {
        Debug.Log($"Ganador: {winnerName}! Reiniciando en {resetDelay}s...");
        UIManager.Instance?.ShowWinner(winnerName);  // add this line
        StartCoroutine(ResetRound());
    }

    private IEnumerator ResetRound()
    {
        yield return new WaitForSeconds(resetDelay);
        UIManager.Instance?.HideWinner();

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RPC_ResetRound", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ResetRound()
    {
        roundOver = false;

        // Destroy all existing tanks
        TankHealth[] allTanks = FindObjectsOfType<TankHealth>();
        foreach (TankHealth tank in allTanks)
        {
            if (tank.photonView.IsMine)
                PhotonNetwork.Destroy(tank.gameObject);
        }

        // Respawn local player
        PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
        if (spawner != null) spawner.SpawnPlayer();
    }
}