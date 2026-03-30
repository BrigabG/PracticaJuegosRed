using UnityEngine;

public class ScenarioGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int obstacleCount = 10;
    [SerializeField] private float areaSize = 30f;
    [SerializeField] private float wallThickness = 1f;
    [SerializeField] private float wallHeight = 3f;

    public void Generate(int seed)
    {
        SpawnWalls();
        SpawnObstacles(seed);
    }

    private void SpawnWalls()
    {
        float half = areaSize / 2f;

        SpawnWall(new Vector3(0, wallHeight / 2f, half),  new Vector3(areaSize, wallHeight, wallThickness));
        SpawnWall(new Vector3(0, wallHeight / 2f, -half), new Vector3(areaSize, wallHeight, wallThickness));
        SpawnWall(new Vector3(half, wallHeight / 2f, 0),  new Vector3(wallThickness, wallHeight, areaSize));
        SpawnWall(new Vector3(-half, wallHeight / 2f, 0), new Vector3(wallThickness, wallHeight, areaSize));

        Debug.Log("Paredes generadas.");
    }

    private void SpawnWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.localScale = scale;
    }

    private void SpawnObstacles(int seed)
    {
        Random.InitState(seed);

        float half = areaSize / 2f - 2f;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-half, half),
                0.5f,
                Random.Range(-half, half)
            );

            Instantiate(obstaclePrefab, position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        }

        Debug.Log("Obstaculos generados: " + obstacleCount + " con seed: " + seed);
    }
}
