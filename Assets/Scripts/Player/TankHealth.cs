using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Image healthBar;

    public bool IsDead { get; private set; }
    public string OwnerName => photonView.Owner.NickName;

    private float currentHealth;
    private readonly HashSet<int> processedBulletViewIds = new HashSet<int>();

    private void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;
        processedBulletViewIds.Clear();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (IsDead) return; // guard double-death
        IsDead = true;
        Debug.Log($"[TankHealth] {photonView.Owner.NickName} died. IsMaster: {PhotonNetwork.IsMasterClient}");
        DisableComponents();
        GameManager.Instance?.NotifyPlayerDied();
    }

    private void DisableComponents()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        TankController controller = GetComponent<TankController>();
        if (controller != null) controller.enabled = false;

        TankShooter shooter = GetComponent<TankShooter>();
        if (shooter != null) shooter.enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(IsDead);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            IsDead = (bool)stream.ReceiveNext();

            healthBar.fillAmount = currentHealth / maxHealth;

            if (IsDead) DisableComponents();
        }
    }
}