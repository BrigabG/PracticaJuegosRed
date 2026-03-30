using Photon.Pun;
using UnityEngine;

public class TankBullet : MonoBehaviourPun
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;

    private void Start()
    {
        if (photonView.IsMine)
            Invoke(nameof(DestroyBullet), lifetime);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        TankHealth health = other.GetComponent<TankHealth>();

        if (health == null)
        {
            Debug.Log("Bala impacto en: " + other.name);
            DestroyBullet();
            return;
        }

        if (health.photonView.Owner == photonView.Owner) return;

        Debug.Log("Bala impacto en: " + other.name);
        health.photonView.RPC(nameof(TankHealth.TakeDamage), RpcTarget.All, 30);
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
