using Photon.Pun;
using UnityEngine;

public class TankController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 90f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || !rb) return;

        float move   = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        rb.MovePosition(rb.position + transform.forward * move * moveSpeed * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * rotate * rotateSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}