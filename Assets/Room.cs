using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  private int[,] _layout =
  {
      { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
      { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { 1,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,0,2,1 },
      { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { 1,1,1,1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1 },
      { -1,-1,-1,-1,1,0,0,3,3,0,0,4,4,4,4,4,0,0,1 },
      { -1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,5,1 },
      { -1,-1,-1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
    };

  private int _length = 19;
  private int _width = 19;
  private int _totalPotential = 0;
  private List<MapLocation> ObjectsInRoom;

  public GameObject WallPrefab;
  public GameObject DirtPrefab;
  public GameObject TilePrefab;
  public GameObject FurniturePrefab;
  public GameObject RugPrefab;
  public GameObject RoombaPrefab;

  public MapLocation CurrentLocation;


  void Start()
  {
    ObjectsInRoom = new List<MapLocation>();
    DrawRoom();
  }


  private void DrawRoom()
  {
    for (int i = 0; i < _length; i++)
    {
      for (int j = 0; j < _width; j++)
      {
        int objectCode = _layout[i, j];
        Vector3 position = new Vector3(i, 0, j);
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
          mapLocation.ObjectType = "Wall";
          break;
        case 2:
          // draw dirt and tile (since the dirt is in the air, want to see the ground beneath it too)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);
          Instantiate(DirtPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = "Dirt";
          break;
        case 3:
          // draw rug
          Instantiate(RugPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = "Rug";
          break;
        case 4:
          // draw furniture
          Instantiate(FurniturePrefab, position, Quaternion.identity);
          mapLocation.ObjectType = "Furniture";
          break;
        case 5:
          // draw tank (and tile since the roomba will move)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);
          Instantiate(RoombaPrefab, position, Quaternion.identity);
          CurrentLocation = mapLocation;
          break;
        default:
          // draw tile
          _totalPotential++;
          mapLocation.ObjectType = "Tile";
          Instantiate(TilePrefab, position, Quaternion.identity);
          break;
      }
    }

    return mapLocation;
  }

  public class MapLocation
  {
    public float X { get; set; }
    public float Y { get; set; }
    public bool Visited { get; set; }
    public string ObjectType { get; set; }

    public MapLocation(float x, float y)
    {
      X = x;
      Y = y;
      ObjectType = "None";
    }
  }
}