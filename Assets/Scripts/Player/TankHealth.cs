using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TankHealth : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float maxHealth = 100;

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
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

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
            currentHealth = Convert.ToInt32(stream.ReceiveNext());
            isDead = (bool)stream.ReceiveNext();

            if (isDead)
                DisableComponents();
        }
    }
}
