using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TankController : MonoBehaviourPun
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 90f;

    private Rigidbody rb;
    private bool canMove = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || !rb || !canMove) return;

        float move   = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        rb.MovePosition(rb.position + transform.forward * move * moveSpeed * Time.fixedDeltaTime);

        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * rotate * rotateSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }


    public void EnableMovement()
    {
        canMove = true;
    }
}