using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string resourcesPath = "Prefabs/Player";

    private void Start()
    {
        if (PhotonManager.Instance == null)
        {
            Debug.LogError("PlayerSpawner: No hay instancia de PhotonManager en escena.");
            return;
        }

        PhotonManager.Instance.OnRoom += SpawnPlayer;
        Debug.Log("PlayerSpawner: suscripto al evento OnRoom.");
    }

    private void OnDestroy()
    {
        if (PhotonManager.Instance != null)
            PhotonManager.Instance.OnRoom -= SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerSpawner: playerPrefab no asignado en el Inspector.");
            return;
        }

        Debug.Log("SpawnPlayer llamado - path: " + resourcesPath + " | pos: " + transform.position);
        PhotonNetwork.Instantiate(resourcesPath, transform.position, Quaternion.identity);
    }
}
