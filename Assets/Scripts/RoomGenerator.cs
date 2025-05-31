
using System.Collections.Generic;

using UnityEngine;

[ExecuteInEditMode]
public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// Used to ensure random objects don’t overlap layout defined ones
    /// </summary>
    private HashSet<Vector2Int> _occupiedPositions;

    public RoomLayout layout;

    [Header("Room Size")]
    public int roomWidth = 10;
    public int roomHeight = 8;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject obstaclePrefab;
    public GameObject itemPrefab;

    [Header("Generation Options")]
    public bool generateRandomObstacles = true;
    public int obstacleCount = 5;

    public bool generateRandomItems = true;
    public int  itemCount           = 3;

    private void Start()
    {
        /*Unity runs Start() and Update() in edit mode too,
         this helps to avoid duplicating objects or causing performance issues.*/
        if ( ! Application.isPlaying)
            return;

        if (layout)
        {
            GenerateFromLayout();
        }
        else
        {
            Debug.Log("RoomLayout not assigned to RoomGenerator. Using coded creation Room and its items (if random options are selected.");

            GenerateWalls();

            if (generateRandomObstacles) GenerateObstacles();
            if (generateRandomItems) GenerateItems();
        }
    }

    private void GenerateFromLayout()
    {
        _occupiedPositions = new HashSet<Vector2Int>();

        foreach (RoomObject obj in layout.objects)
        {
            var position = new Vector2Int(obj.position.x, obj.position.y);
            _occupiedPositions.Add(position);

            var prefab = GetPrefabForType(obj.type);
            
            if (prefab)
            {
                Instantiate(prefab, (Vector2)position, Quaternion.identity, transform);
            }
        }
    }

    private GameObject GetPrefabForType(RoomObject.ObjectType type)
    {
        return type switch
        {
             RoomObject.ObjectType.Wall     => wallPrefab
           , RoomObject.ObjectType.Obstacle => obstaclePrefab
           , RoomObject.ObjectType.Item     => itemPrefab
           , _ => null
        };
    }

    private void GenerateWalls()
    {
        for (var x = 0; x < roomWidth; x++)
        {
            Instantiate(wallPrefab, new Vector2(x, 0), Quaternion.identity, transform); // Bottom
            Instantiate(wallPrefab, new Vector2(x, roomHeight - 1), Quaternion.identity, transform); // Top
        }

        for (var y = 1; y < roomHeight - 1; y++)
        {
            Instantiate(wallPrefab, new Vector2(0, y), Quaternion.identity, transform); // Left
            Instantiate(wallPrefab, new Vector2(roomWidth - 1, y), Quaternion.identity, transform); // Right
        }
    }

    private void GenerateObstacles()
    {
        for (var i = 0; i < obstacleCount; i++)
        {
            var pos = GetRandomPositionInside();
            Instantiate(obstaclePrefab, pos, Quaternion.identity, transform);
        }
    }

    private void GenerateItems()
    {
        for (var i = 0; i < itemCount; i++)
        {
            var pos = GetRandomPositionInside();
            Instantiate(itemPrefab, pos, Quaternion.identity, transform);
        }
    }

    private Vector2 GetRandomPositionInside()
    {
        Vector2Int pos;
        
        do
        {
            pos = new Vector2Int(Random.Range(1, roomWidth - 1)
                               , Random.Range(1, roomHeight - 1));

        } while (_occupiedPositions.Contains(pos));

        _occupiedPositions.Add(pos);
        
        return (Vector2)pos;
    }
}