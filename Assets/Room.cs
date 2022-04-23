using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using AssemblyCSharp.Assets;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
  private const string LogPath = "RoombaResults.csv";
  private int _totalPotential = 0;
  private List<MapLocation> ObjectsInRoom;
  private TextMeshProUGUI EndText;
  private string FileName;
  private CommandLineParser CommandLineArgs;

  public GameObject WallPrefab;
  public GameObject DirtPrefab;
  public GameObject TilePrefab;
  public GameObject FurniturePrefab;
  public GameObject RugPrefab;
  public GameObject RoombaPrefab;

  public RoomType RoomType;
  public PresetRoomOptions PresetRoomLayout;
  public MovementPatterns RoombaPathingType;

  public int Length = 19;
  public int Width = 19;

  private float CurrentX;
  private float CurrentZ;

  public MapLocation GetObjectForCoordinate(float x, float z)
  {
    return ObjectsInRoom.FirstOrDefault(o => o.X == x && o.Z == z);
  }

  public int GetRoomLength() => Length;
  public int GetRoomWidth() => Width;

  public (float x, float z) GetCurrentLocation()
  {
    return (CurrentX, CurrentZ);
  }

  public void SetCurrentLocation(float x, float z)
  {
    CurrentX = x;
    CurrentZ = z;
  }


  void Awake()
  {
    EndText = GameObject.FindGameObjectWithTag(TagConstants.TextEnd).GetComponent<TextMeshProUGUI>();
    EndText.gameObject.SetActive(false);

    CommandLineParser parser = new CommandLineParser();
    parser.ParseCommandLineArguments();

    CommandLineArgs = parser;

    Width = CommandLineArgs.DefaultWidth;
    Length = CommandLineArgs.DefaultLength;
    RoomType = CommandLineArgs.RoomType;
    RoombaPathingType = CommandLineArgs.PathingType;
    PresetRoomLayout = CommandLineArgs.RoomLayoutType;

    UnityEngine.Random.InitState(CommandLineArgs.Seed);
  }

  void Start()
  {
    UnityEngine.Random.InitState(CommandLineArgs.Seed);
    ObjectsInRoom = new List<MapLocation>();
    if (RoomType == RoomType.Preset)
    {
      if (PresetRoomLayout == PresetRoomOptions.SingleRoom)
        FileName = "Room2.txt";
      else if (PresetRoomLayout == PresetRoomOptions.Square)
        FileName = "Room4.txt";
      else
        FileName = "Room3.txt";
      ReadContents();
    }
    else
      GenerateRoom();
  }

  public void EndSimulation(string reason, int dirtCollected, float timeTaken, float batteryRemaining)
  {
    double covered = GetPercentCovered();
    string formattedCovered = covered.ToString("N2");
    string reasonText;
    if (reason == "Time")
    {
      reasonText = "Out of time.";
    }
    else if (reason == "Battery")
    {
      reasonText = "The battery has died.";
    }
    else if (reason == "Complete")
    {
      reasonText = "Room has been vacuumed";
    }
    else
    {
      reasonText = "Out of viable moves.";
    }

    EndText.text = $"{reasonText}\nVisited {formattedCovered}% of potential tiles.";
    EndText.gameObject.SetActive(true);

    ProcessEndOfSimulation(reason, formattedCovered, dirtCollected, timeTaken,batteryRemaining);
    Application.Quit();
  }

  private void ProcessEndOfSimulation(string reason, string boardCovered, int dirtCollected, float timeTaken, float batteryRemaining)
  {
    string percentDirtCollected = GetDirtCollected(dirtCollected);
    
    string endText = $"{CommandLineArgs.Seed},{CommandLineArgs.RoomType},{CommandLineArgs.RoomLayoutType},{CommandLineArgs.PathingType},{reason},{boardCovered},{percentDirtCollected},{timeTaken},{batteryRemaining.ToString("N2")}";

    File.AppendAllLines(LogPath, new List<string> { endText });
  }

  private string GetDirtCollected(int dirtCollected)
  {
    int totalDirtOnBoard = ObjectsInRoom.Count(o => o.ObjectType == ObjectType.Dirt);
    double percentCollected =  (Convert.ToDouble(dirtCollected) / totalDirtOnBoard) * 100;

    return percentCollected.ToString("N2");
  }

  public bool SimulationIsComplete()
  {
    return GetPercentCovered() >= 100;
  }

  private double GetPercentCovered()
  {
    int numberVisited = ObjectsInRoom.Count(o => o.Visited);
    return (Convert.ToDouble(numberVisited) / _totalPotential) * 100;
  }

  private MapLocation DrawObject(int objectCode, Vector3 position)
  {
    MapLocation mapLocation = new MapLocation(position.x, position.z);

    if (objectCode >= 0)
    {
      switch (objectCode)
      {
        case 1:
          // draw tile
          _totalPotential++;
          mapLocation.ObjectType = ObjectType.Tile;
          Instantiate(TilePrefab, position, Quaternion.identity);
          break;
        case 2:
          // draw dirt and tile (since the dirt is in the air, want to see the ground beneath it too)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);
          Vector3 dirtPosition = new Vector3(position.x, 0.25f, position.z);
          Instantiate(DirtPrefab, dirtPosition, Quaternion.identity);
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
          // draw roomba (and tile since the roomba will move)
          _totalPotential++;
          Instantiate(TilePrefab, position, Quaternion.identity);

          position.y = 0.15f;
          Instantiate(RoombaPrefab, position, Quaternion.identity);
          CurrentX = position.x;
          CurrentZ = position.z;
          mapLocation.ObjectType = ObjectType.Tile;
          break;
        default:
          // draw wall
          Instantiate(WallPrefab, position, Quaternion.identity);
          mapLocation.ObjectType = ObjectType.Wall;
          break;
      }
    }

    return mapLocation;
  }


  public void ReadContents()
  {
    string[] fileContents = File.ReadAllLines(FileName);
    Length = Convert.ToInt32(fileContents[0]);
    Width = Convert.ToInt32(fileContents[1]);

    for (int z = Length - 1; z >= 0; z--)
    {
      string rowText = fileContents[z + 2]; // offset by 2 because the first two rows have the length/ width

      // Even though we are reading the contents in from the end, we want to render them starting from 0
      int coordinateZ = Math.Abs(z - Length) - 1;
      string[] row = rowText.Split(',');
      for (int x = 0; x < Width; x++)
      {
        int objectCode = Convert.ToInt32(row[x]);
        Vector3 position = new Vector3(x, 0, coordinateZ);
        MapLocation roomObject = DrawObject(objectCode, position);
        ObjectsInRoom.Add(roomObject);
      }
    }
  }

  private void GenerateRoom()
  {
    // Find placement for the Roomba so that we only draw it once.
    // We don't want it at 0 or Width/ Length since that is where the walls will go.
    int x = FindRandomNumber(1, Width - 1);
    int z = FindRandomNumber(1, Length - 1);

    DrawObject(x, z, 5); // Draw roomba first
    for (int i = 0; i < Width; i++)
    {
      for (int j = 0; j < Length; j++)
      {
        if (!(x == i && j == z)) // If this isn't the spot chosen for the roomba
        {
          int objectCode = ShouldBeWall(i, j) ? 0 : FindRandomNumber(1, 5);
          DrawObject(i, j, objectCode);
        }
      }
    }
  }

  private void DrawObject(int x, int z, int objectCode)
  {
    Vector3 position = new Vector3(x, 0, z);
    MapLocation roomObject = DrawObject(objectCode, position);
    ObjectsInRoom.Add(roomObject);
  }

  private int FindRandomNumber(int min, int max)
  {
    return UnityEngine.Random.Range(min, max);
  }

  private bool ShouldBeWall(int x, int z)
  {
    return x == 0 || x == Width - 1 || z == 0 || z == Length - 1;
  }
}