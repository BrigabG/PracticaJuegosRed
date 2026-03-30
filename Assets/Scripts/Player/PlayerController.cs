using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        if (photonView.IsMine)
            Debug.Log("PlayerController: Soy el dueno - ActorNumber: " + photonView.Owner.ActorNumber);
        else
            Debug.Log("PlayerController: Jugador remoto - ActorNumber: " + photonView.Owner.ActorNumber);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

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
}
