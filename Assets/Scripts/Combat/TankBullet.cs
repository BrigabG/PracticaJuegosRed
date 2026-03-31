using Photon.Pun;
using UnityEngine;

public class TankBullet : MonoBehaviourPun
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float tankHitDelay = 1f;

    private Renderer bulletRenderer;
    private Collider bulletCollider;
    private bool muted;

    private void Awake()
    {
        bulletRenderer = GetComponentInChildren<Renderer>();
        bulletCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        muted = false;
        if (bulletRenderer != null) bulletRenderer.enabled = true;
        if (bulletCollider != null) bulletCollider.enabled = true;

        if (photonView.IsMine)
            Invoke(nameof(ReturnBullet), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (!photonView.IsMine || muted)
            return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (muted) return;

        TankHealth health = other.GetComponentInParent<TankHealth>();

        if (health != null)
        {
            if (health.photonView.Owner == photonView.Owner)
                return;

            MuteBulletLocally();
            if (photonView.IsMine)
            {
                Invoke(nameof(ReturnBullet), tankHitDelay);
            }
            return;
        }

        if (!photonView.IsMine) return;
        Debug.Log("Bala impacto en: " + other.name);
        ReturnBullet();
    }

    private void MuteBulletLocally()
    {
        muted = true;
        if (bulletRenderer != null) bulletRenderer.enabled = false;
        if (bulletCollider != null) bulletCollider.enabled = false;
        Debug.Log("Bala silenciada localmente.");
    }

    private void ReturnBullet()
    {
        if (!photonView.IsMine) return;
        PhotonNetwork.Destroy(gameObject);
    }
}
