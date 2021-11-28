using System;
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
      { -1,-1,-1,-1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      { -1,-1,-1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
    };

    private int _length = 19;
    private int _width = 19;

    public GameObject WallPrefab;
    public GameObject DirtPrefab;
    public GameObject TilePrefab;
    public GameObject FurniturePrefab;
    public GameObject RugPrefab;

    void Start()
    {
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
                DrawObject(objectCode, position);
            }
        }
    }

    private void DrawObject(int objectCode, Vector3 position)
    {
        if (objectCode >= 0)
        {

            switch (objectCode)
            {
                case 1:
                    // draw wall
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = position;

                    break;
                case 2:
                    Instantiate(DirtPrefab, position, Quaternion.identity);
                    break;
                case 3:
                    // draw rug
                    break;
                case 4:
                    // draw furniture
                    break;
                default:
                    // draw tile
                    GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    tile.transform.position = position;
                    break;

            }
        }
    }
}