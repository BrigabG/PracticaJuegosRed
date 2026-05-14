using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;

    private bool canMove = false;

    private void Start()
    {
        if (photonView.IsMine)
            Debug.Log("PlayerController: Soy el dueno - ActorNumber: " + photonView.Owner.ActorNumber);
        else
            Debug.Log("PlayerController: Jugador remoto - ActorNumber: " + photonView.Owner.ActorNumber);
    }

    private void Update()
    {
        if (!photonView.IsMine || !canMove) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction.magnitude > 0)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            Debug.Log("Moviendome: " + direction);
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
            Debug.Log("Cuenta regresiva: " + (targetNetworkTime - PhotonNetwork.Time).ToString("F2") + " segundos restantes");
        }

        canMove = true;
        // Aquí puedes iniciar la carrera
        Debug.Log("¡Carrera iniciada!");
        yield return null;
    }
}
