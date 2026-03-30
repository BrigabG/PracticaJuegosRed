using Photon.Pun;
using UnityEngine;

public class MasterClientManager : MonoBehaviourPun
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

        int seed = Random.Range(0, 100000);
        Debug.Log("MasterClient generando escenario con seed: " + seed);
        photonView.RPC(nameof(InitScenario), RpcTarget.AllBuffered, seed); //cambiar a un INstanci room obj
    }

    [PunRPC]
    private void InitScenario(int seed)
    {
        Debug.Log("Generando escenario con seed: " + seed);
        generator.Generate(seed);
    }
}
