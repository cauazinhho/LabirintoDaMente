using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    private bool hasGeneratedThisScene = false;

    //Area Jogavel
    public int playableWidth = 12; //x 
    public int playableHeight = 8; //y

    //Moldura
    public int WallOffset = 10;

    //Prefabs
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject portalPrefab;
    public GameObject playerPrefab;

    public string nextSceneName = "";

    //Zona de segurança (zona sem obstaculos)
    [Min(0)] public int spawnSafeZoneWidth = 2;
    [Min(1)] public int portalSafeZoneWidthTop = 2;
    [Min(1)] public int portalSafeZoneHeight = 3;


    // controle dos obstaculos

    //densidade dos obstaculos
    [Range(0f, 1f)] public float obstacleChance = 0.8f;

    public int obstacleLayerY = 2;
    public int minObstacleGapX = 2;
    public int maxObstacleGapX = 4;

    public int minNoSpawnX = 1;
    public int maxNoSpawnX = 2;

    public int width => playableWidth;
    public int height => playableHeight;
    public int offset => WallOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (hasGeneratedThisScene) return;

        hasGeneratedThisScene = true;

        GenerateRoom();

    }

    bool IsExcluded(int tileX, int tileY)
    {
        if (tileX < Mathf.Clamp(spawnSafeZoneWidth, 0, playableWidth))
        {
            return true;
        }

        int portalStartX = Mathf.Max(0, playableWidth - portalSafeZoneWidthTop);
        int portalStartY = Mathf.Max(0, playableHeight - portalSafeZoneHeight);

        if (tileX >= portalStartX && tileY >= portalStartY)
        {
            return true;
        }

        return false;
    }

    void GenerateRoom()
    {
        int leftWallX = -WallOffset;
        int rightWallX = playableWidth + WallOffset;


        for (int tileX = leftWallX; tileX <= rightWallX; tileX++)
        {

            for (int tileY = 0; tileY < playableHeight; tileY++)
            {

                Vector2 tileWorldPosition = new Vector2(tileX, tileY);

                if (tileY == 0 && tileX >= 0 && tileX < playableWidth)
                {

                    Instantiate(floorPrefab, tileWorldPosition, Quaternion.identity, transform);

                }

                if (tileX == leftWallX || tileX == rightWallX)
                {

                    Instantiate(wallPrefab, tileWorldPosition, Quaternion.identity, transform);
                }

            }

        }

        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {

            int firstObstacleY = 2;

            int lastObstacleY = Mathf.Max(2, playableHeight - 3);


            for (int tileY = firstObstacleY; tileY <= lastObstacleY; tileY += Mathf.Max(1, obstacleLayerY))
            {

                int tileX = 1;

                while (tileX <= playableWidth - 2)
                {
                    if (IsExcluded(tileX, tileY))
                    {

                        tileX += 1;
                        continue;

                    }

                    if (Random.value < obstacleChance)
                    {
                        int prefabIndex = Random.Range(0, obstaclePrefabs.Length);

                        Instantiate(obstaclePrefabs[prefabIndex], new Vector2(tileX, tileY), Quaternion.identity, transform);

                        tileX += Random.Range(minObstacleGapX, maxObstacleGapX + 1);
                    }
                    else
                    {
                        tileX += Random.Range(minNoSpawnX, maxNoSpawnX + 1);
                    }
                }

            }
        }


        Vector2 portalWorldPosition = new Vector2(playableWidth - 0, playableHeight - 2);
        GameObject portalInstance = Instantiate(portalPrefab, portalWorldPosition, Quaternion.identity, transform);

        int playerSpawnX = WallOffset + 1;
        int playerSpawnY = 1;

        for (int padX = playerSpawnX - 1; padX <= playerSpawnX + 1; padX++)
        {
            Instantiate(floorPrefab, new Vector2(padX, 0), Quaternion.identity, transform);
        }

        Instantiate(playerPrefab, new Vector2(playerSpawnX, playerSpawnY), Quaternion.identity, transform);

    }

}
