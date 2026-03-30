using Photon.Pun;
using UnityEngine;

public class TankController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 90f;

    private void Update()
    {
        if (!photonView.IsMine) return;

        float move   = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * rotate * rotateSpeed * Time.deltaTime);
    }
}
