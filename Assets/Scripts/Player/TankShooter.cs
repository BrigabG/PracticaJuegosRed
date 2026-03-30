using Photon.Pun;
using UnityEngine;

public class TankShooter : MonoBehaviourPun
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private string bulletPath = "Prefabs/TankBullet";
    [SerializeField] private float fireRate = 1f;

    private float nextFireTime;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Disparando desde: " + firePoint.position);
        PhotonNetwork.Instantiate(bulletPath, firePoint.position, firePoint.rotation);
    }
}
