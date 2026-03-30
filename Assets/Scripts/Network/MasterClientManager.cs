using Photon.Pun;
using UnityEngine;

public class MasterClientManager : MonoBehaviour
{
    [SerializeField] private ScenarioGenerator generator;

    private void Start()
    {
        if (PhotonManager.Instance == null)
        {
            Debug.LogError("MasterClientManager: No hay instancia de PhotonManager.");
            return;
        }

        PhotonManager.Instance.OnRoom += OnJoinedRoom;
    }

    private void OnDestroy()
    {
        if (PhotonManager.Instance != null)
            PhotonManager.Instance.OnRoom -= OnJoinedRoom;
    }

    private void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("MasterClient generando escenario.");
        generator.Generate();
    }
}
