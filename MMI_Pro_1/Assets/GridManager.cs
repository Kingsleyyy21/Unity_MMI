using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject floorTilePrefab;  // 设置为你的 FloorTile 预制件
    public GameObject destructibleObstaclePrefab;  // 可破坏障碍物预制件
    public GameObject indestructibleObstaclePrefab;  // 不可破坏障碍物预制件
    public float tileSpacing = 1.0f;    // 瓷砖间距
    public int numberOfDestructibleObstacles = 10;  // 可破坏障碍物的数量
    public int numberOfIndestructibleObstacles = 5;  // 不可破坏障碍物的数量

    private int gridWidth;  // 网格的宽度
    private int gridHeight;  // 网格的高度
    private List<Vector3> floorTilePositions = new List<Vector3>();  // 记录每个地板位置

    void Start()
    {
        CalculateGridSizeAndCreateGrid();
        GenerateObstacles();  // 确保在地板生成之后生成障碍物
    }

    void CalculateGridSizeAndCreateGrid()
    {
        // 获取相机的宽度和高度
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // 计算网格的宽度和高度
        gridWidth = Mathf.CeilToInt(screenWidth / tileSpacing);
        gridHeight = Mathf.CeilToInt(screenHeight / tileSpacing);

        CreateGrid(gridWidth, gridHeight);
    }

    void CreateGrid(int width, int height)
    {
        // 清除已有的子对象，防止多次生成
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        floorTilePositions.Clear();  // 清空之前记录的地板位置

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * tileSpacing, y * tileSpacing, 0);
                Instantiate(floorTilePrefab, position, Quaternion.identity, transform);
                floorTilePositions.Add(position);  // 记录地板位置
                Debug.Log("Added floor tile position: " + position);  // 添加调试信息
            }
        }
        Debug.Log("Total floor tile positions: " + floorTilePositions.Count);  // 添加调试信息
    }

    void GenerateObstacles()
    {
        // 确保有可用的地板位置
        if (floorTilePositions.Count == 0)
        {
            Debug.LogWarning("No floor tile positions available.");
            return;
        }

        // 随机生成可破坏障碍物
        int destructibleObstaclesGenerated = 0;
        while (destructibleObstaclesGenerated < numberOfDestructibleObstacles && floorTilePositions.Count > 0)
        {
            Vector3 position = GetRandomFloorTilePosition();
            Instantiate(destructibleObstaclePrefab, position, Quaternion.identity, transform);
            destructibleObstaclesGenerated++;
        }

        // 随机生成不可破坏障碍物
        int indestructibleObstaclesGenerated = 0;
        while (indestructibleObstaclesGenerated < numberOfIndestructibleObstacles && floorTilePositions.Count > 0)
        {
            Vector3 position = GetRandomFloorTilePosition();
            Instantiate(indestructibleObstaclePrefab, position, Quaternion.identity, transform);
            indestructibleObstaclesGenerated++;
        }

        // 输出生成的障碍物数量以便调试
        Debug.Log("Generated destructible obstacles: " + destructibleObstaclesGenerated);
        Debug.Log("Generated indestructible obstacles: " + indestructibleObstaclesGenerated);
    }

    Vector3 GetRandomFloorTilePosition()
    {
        int index = Random.Range(0, floorTilePositions.Count);
        Vector3 position = floorTilePositions[index];
        floorTilePositions.RemoveAt(index);  // 确保不会重复生成在同一个位置
        return position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
