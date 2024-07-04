using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//testt
public class GridManager : MonoBehaviour
{
    public GameObject floorTilePrefab;  
    public GameObject destructibleObstaclePrefab; 
    public GameObject indestructibleObstaclePrefab; 
    public float tileSpacing = 1.0f;   
    public int numberOfDestructibleObstacles = 10;  
    public int numberOfIndestructibleObstacles = 5;

    private int gridWidth;  
    private int gridHeight;  
    private List<Vector3> floorTilePositions = new List<Vector3>(); 

    void Start()
    {
        CalculateGridSizeAndCreateGrid();
        GenerateObstacles();  // Ensure that obstacles are generated after the floor is generated
    }

    void CalculateGridSizeAndCreateGrid()
    {
        // Get the width and height of the camera
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Calculate the width and height of the grid
        gridWidth = Mathf.CeilToInt(screenWidth / tileSpacing);
        gridHeight = Mathf.CeilToInt(screenHeight / tileSpacing);

        CreateGrid(gridWidth, gridHeight);
    }

    void CreateGrid(int width, int height)
    {
        // Clear existing child objects to prevent multiple generation
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        floorTilePositions.Clear();  // Empty the previously recorded floor location

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * tileSpacing, y * tileSpacing, 0);
                Instantiate(floorTilePrefab, position, Quaternion.identity, transform);
                floorTilePositions.Add(position);  // Record floor position
                Debug.Log("Added floor tile position: " + position);  // Adding debugging information
            }
        }
        Debug.Log("Total floor tile positions: " + floorTilePositions.Count);  // Adding debugging information
    }

    void GenerateObstacles()
    {
        // Ensure that there are available flooring locations
        if (floorTilePositions.Count == 0)
        {
            Debug.LogWarning("No floor tile positions available.");
            return;
        }

        // Randomly generated destructible obstacles
        int destructibleObstaclesGenerated = 0;
        while (destructibleObstaclesGenerated < numberOfDestructibleObstacles && floorTilePositions.Count > 0)
        {
            Vector3 position = GetRandomFloorTilePosition();
            Instantiate(destructibleObstaclePrefab, position, Quaternion.identity, transform);
            destructibleObstaclesGenerated++;
        }

        // Randomly generated indestructible obstacles
        int indestructibleObstaclesGenerated = 0;
        while (indestructibleObstaclesGenerated < numberOfIndestructibleObstacles && floorTilePositions.Count > 0)
        {
            Vector3 position = GetRandomFloorTilePosition();
            Instantiate(indestructibleObstaclePrefab, position, Quaternion.identity, transform);
            indestructibleObstaclesGenerated++;
        }

        // Output the number of obstacles generated for debugging
        Debug.Log("Generated destructible obstacles: " + destructibleObstaclesGenerated);
        Debug.Log("Generated indestructible obstacles: " + indestructibleObstaclesGenerated);
    }

    Vector3 GetRandomFloorTilePosition()
    {
        int index = Random.Range(0, floorTilePositions.Count);
        Vector3 position = floorTilePositions[index];
        floorTilePositions.RemoveAt(index);  // Ensure that it is not generated repeatedly in the same location
        return position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
