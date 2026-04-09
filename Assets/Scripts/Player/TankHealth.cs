using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Image healthBar;
    
    private float currentHealth;
    private bool isDead;
    private readonly HashSet<int> processedBulletViewIds = new HashSet<int>();

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        processedBulletViewIds.Clear();
        Debug.Log(photonView.Owner.NickName + " HP: " + currentHealth);
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
        isDead = true;
        Debug.Log(photonView.Owner.NickName + " eliminado.");
        DisableComponents();
    }

    private void DisableComponents()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
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
            stream.SendNext(isDead);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();

            healthBar.fillAmount = currentHealth / maxHealth;

            if (isDead)
                DisableComponents();
        }
    }
}
