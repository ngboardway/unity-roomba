using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
  private int[,] _layout =
  {
    {-1,-1,-1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {-1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {-1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,5,0,1},
    {-1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1},
    {-1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1},
    {1,1,1,1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
  };


  private int _length = 19;
  private int _width = 19;
  private int _totalPotential = 0;
  private List<MapLocation> ObjectsInRoom;
  private TextMeshProUGUI EndText;

  public GameObject WallPrefab;
  public GameObject DirtPrefab;
  public GameObject TilePrefab;
  public GameObject FurniturePrefab;
  public GameObject RugPrefab;
  public GameObject RoombaPrefab;

  public MapLocation CurrentLocation;

  public MapLocation GetObjectForCoordinate(float x, float z)
  {
    return ObjectsInRoom.FirstOrDefault(o => o.X == x && o.Z == z);
  }

  public void EndSimulation()
  {
    double covered = GetPercentCovered();
    string formattedCovered = covered.ToString("N2");
    EndText.text = $"Out of viable moves.\nVisited {formattedCovered}% of potential tiles.";
    EndText.gameObject.SetActive(true);
  }

  void Awake()
  {
    EndText = GameObject.FindGameObjectWithTag(TagConstants.TextEnd).GetComponent<TextMeshProUGUI>();
    EndText.gameObject.SetActive(false);
  }

  void Start()
  {
    ObjectsInRoom = new List<MapLocation>();
    DrawRoom();
  }

  private double GetPercentCovered()
  {
    int numberVisited = ObjectsInRoom.Count(o => o.Visited);
    return (Convert.ToDouble(numberVisited) / _totalPotential) * 100;
  }

  private void DrawRoom()
  {
    for (int i = 0; i < _width; i++)
    {
      for (int j = 0; j < _length; j++)
      {
        int objectCode = _layout[i, j];
        Vector3 position = new Vector3(j, 0, i);
        MapLocation roomObject = DrawObject(objectCode, position);
        ObjectsInRoom.Add(roomObject);
      }
    }
  }

  private MapLocation DrawObject(int objectCode, Vector3 position)
  {
    MapLocation mapLocation = new MapLocation(position.x, position.z);

    if (objectCode >= 0)
    {
      switch (objectCode)
      {
        case 1:
          // draw wall
          Instantiate(WallPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = ObjectType.Wall;
          break;
        case 2:
          // draw dirt and tile (since the dirt is in the air, want to see the ground beneath it too)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);
          Instantiate(DirtPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = ObjectType.Dirt;
          break;
        case 3:
          // draw rug
          Instantiate(RugPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = ObjectType.Rug;
          break;
        case 4:
          // draw furniture
          Instantiate(FurniturePrefab, position, Quaternion.identity);
          mapLocation.ObjectType = ObjectType.Furniture;
          break;
        case 5:
          // draw tank (and tile since the roomba will move)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);

          position.y = 0.15f;
          Instantiate(RoombaPrefab, position, Quaternion.identity);
          CurrentLocation = mapLocation;
          break;
        default:
          // draw tile
          _totalPotential++;
          mapLocation.ObjectType = ObjectType.Tile;
          Instantiate(TilePrefab, position, Quaternion.identity);
          break;
      }
    }

    return mapLocation;
  }

  public class MapLocation
  {
    public float X { get; set; }
    public float Z { get; set; }
    public bool Visited { get; set; }
    public ObjectType ObjectType { get; set; }
    public Vector3 Position { get; set; }

    public MapLocation(float x, float z)
    {
      X = x;
      Z = z;
      ObjectType = ObjectType.None;
    }
  }
}