using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletPool : MonoBehaviour, IPunPrefabPool
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private string bulletPath = "Prefabs/Bullet";
    [SerializeField] private int initialSize = 10;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.PrefabPool = this;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject bullet = CreateNew();
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }

    }

    private GameObject CreateNew()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPool: bulletPrefab no esta asignado.");
            return null;
        }

        GameObject bullet = UnityEngine.Object.Instantiate(bulletPrefab);
        DontDestroyOnLoad(bullet);
        return bullet;
    }

    public void ReturnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (prefabId != bulletPath)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabId);
            return prefab != null ? UnityEngine.Object.Instantiate(prefab, position, rotation) : null;
        }

        GameObject bullet = pool.Count > 0 ? pool.Dequeue() : CreateNew();
        if (bullet == null) return null;

        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.SetActive(true);
        return bullet;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject != null && gameObject.GetComponent<TankBullet>() != null)
        {
            ReturnToPool(gameObject);
            return;
        }

        if (gameObject != null)
            UnityEngine.Object.Destroy(gameObject);
    }
}
