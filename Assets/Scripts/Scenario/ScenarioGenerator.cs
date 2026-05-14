using Photon.Pun;
using UnityEngine;

public class ScenarioGenerator : MonoBehaviour
{
    [SerializeField] private string wallPath = "Prefabs/Wall";
    [SerializeField] private string obstaclePath = "Prefabs/Obstacle";
    [SerializeField] private string floorPath = "Prefabs/Floor";
    [SerializeField] private int obstacleCount = 10;
    [SerializeField] private float areaSize = 30f;
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float wallHeight = 3f;

    public void Generate()
    {
        // El método normal asume que estamos conectados a Photon
        SpawnFloor(isOffline: false);
        SpawnWalls(isOffline: false);
        SpawnObstacles(isOffline: false);
    }

    [ContextMenu("Generate")]
    private void GenerateOffline()
    {
        // Este método funciona sin conexión y desde el editor de Unity
        SpawnFloor(isOffline: true);
        SpawnWalls(isOffline: true);
        SpawnObstacles(isOffline: true);
    }

    private void SpawnFloor(bool isOffline)
    {
        GameObject floor;
        if (isOffline)
        {
            GameObject prefab = Resources.Load<GameObject>(floorPath);
            floor = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            floor = PhotonNetwork.InstantiateRoomObject(floorPath, Vector3.zero, Quaternion.identity);
        }

        floor.transform.localScale = new Vector3(areaSize, 0.1f, areaSize);
        Debug.Log("Piso generado.");
    }

    private void SpawnWalls(bool isOffline)
    {
        float half = areaSize / 2f;

        SpawnWall(new Vector3(0, wallHeight / 2f, half), new Vector3(areaSize, wallHeight, wallThickness), isOffline);
        SpawnWall(new Vector3(0, wallHeight / 2f, -half), new Vector3(areaSize, wallHeight, wallThickness), isOffline);
        SpawnWall(new Vector3(half, wallHeight / 2f, 0), new Vector3(wallThickness, wallHeight, areaSize), isOffline);
        SpawnWall(new Vector3(-half, wallHeight / 2f, 0), new Vector3(wallThickness, wallHeight, areaSize), isOffline);

        Debug.Log("Paredes generadas.");
    }

    private void SpawnWall(Vector3 position, Vector3 scale, bool isOffline)
    {
        GameObject wall;
        if (isOffline)
        {
            GameObject prefab = Resources.Load<GameObject>(wallPath);
            wall = Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            wall = PhotonNetwork.InstantiateRoomObject(wallPath, position, Quaternion.identity);
        }

        wall.transform.localScale = scale;
    }

    private void SpawnObstacles(bool isOffline)
    {
        float half = areaSize / 2f - 2f;
        GameObject prefab = isOffline ? Resources.Load<GameObject>(obstaclePath) : null;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-half, half),
                0.5f,
                Random.Range(-half, half)
            );

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            if (isOffline)
            {
                Instantiate(prefab, position, rotation);
            }
            else
            {
                PhotonNetwork.InstantiateRoomObject(obstaclePath, position, rotation);
            }
        }

        Debug.Log("Obstaculos generados: " + obstacleCount);
    }
}
