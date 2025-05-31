using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Level/RoomLayout")]
public class RoomLayout : ScriptableObject
{
    public int              width;
    public int              height;
    public List<RoomObject> objects;
}

[System.Serializable]
public class RoomObject
{
    public enum ObjectType
    {
        Wall
      , Obstacle
      , Item
    }

    public ObjectType type;
    public Vector2Int position;
}