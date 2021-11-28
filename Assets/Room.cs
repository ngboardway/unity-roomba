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
    private int _totalPotential = 0;

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
                    Instantiate(WallPrefab, position, Quaternion.identity);
                    break;
                case 2:
                    _totalPotential++;
                    Instantiate(TilePrefab, position, Quaternion.identity);
                    Instantiate(DirtPrefab, position, Quaternion.identity);
                    break;
                case 3:
                    // draw rug
                    Instantiate(RugPrefab, position, Quaternion.identity);
                    break;
                case 4:
                    // draw furniture
                    Instantiate(FurniturePrefab, position, Quaternion.identity);
                    break;
                default:
                    // draw tile
                    _totalPotential++;
                    Instantiate(TilePrefab, position, Quaternion.identity);
                    break;

            }
        }
    }
}