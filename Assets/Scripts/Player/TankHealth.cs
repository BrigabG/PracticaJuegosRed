using Photon.Pun;
using UnityEngine;

public class TankHealth : MonoBehaviourPun
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(photonView.Owner.NickName + " HP: " + currentHealth);
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(photonView.Owner.NickName + " recibio " + damage + " de daño | HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log(photonView.Owner.NickName + " eliminado.");
        gameObject.SetActive(false);
    }
}
