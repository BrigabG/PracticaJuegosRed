using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string resourcesPath = "Prefabs/Player";
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        if (PhotonManager.Instance == null)
        {
            Debug.LogError("PlayerSpawner: No hay instancia de PhotonManager en escena.");
            return;
        }

        PhotonManager.Instance.OnRoom += SpawnPlayer;
    }

    private void OnDestroy()
    {
        if (PhotonManager.Instance != null)
            PhotonManager.Instance.OnRoom -= SpawnPlayer;
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerSpawner: playerPrefab no asignado.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length < 3)
        {
            Debug.LogError("PlayerSpawner: Asigna 3 spawnPoints en el Inspector.");
            return;
        }

        // ActorNumber is 1-based, so subtract 1 for the index
        int index = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawnPoints.Length;
        Transform spawnPoint = spawnPoints[index];

        Debug.Log($"Spawning player {PhotonNetwork.LocalPlayer.ActorNumber} at spawn {index}");
        PhotonNetwork.Instantiate(resourcesPath, spawnPoint.position, spawnPoint.rotation);
    }
}