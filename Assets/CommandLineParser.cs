using System;
using UnityEngine;

public class CommandLineParser
{
  #region MEMBERS

  private const string pathingTypeArg = "pathingType=";
  private const string roomTypeArg = "roomType=";
  private const string roomLayoutTypeArg = "layoutTypeArg=";
  private const string roomWidthArg = "width=";
  private const string roomLengthArg = "length=";
  private const string seedArg = "seed=";

  [SerializeField]
  private int defaultWidth = 10;

  [SerializeField]
  private int defaultLength = 20;

  [SerializeField]
  private int seed = 2;

  [SerializeField]
  private MovementPatterns pathingType = MovementPatterns.Random;

  [SerializeField]
  private RoomType roomType = RoomType.Preset;

  [SerializeField]
  private PresetRoomOptions roomLayoutType = PresetRoomOptions.Apartment;

  #endregion

  #region PROPERTIES

  public int DefaultLength
  {
    get { return defaultLength; }
  }

  public int DefaultWidth
  {
    get { return defaultWidth; }
  }

  public int Seed
  {
    get { return seed; }
  }

  public MovementPatterns PathingType
  {
    get { return pathingType; }
  }

  public RoomType RoomType
  {
    get { return roomType; }
  }

  public PresetRoomOptions RoomLayoutType
  {
    get { return roomLayoutType; }
  }

  #endregion

  #region FUNCTIONS

  public void ParseCommandLineArguments()
  {
    string[] args = Environment.GetCommandLineArgs();
    string argumentString;


    for (int i = 0; i < args.Length; i++)
    {
      argumentString = "";
      if (args[i].StartsWith(pathingTypeArg))
      {
        argumentString = args[i].Replace(pathingTypeArg, "");
        Enum.TryParse<MovementPatterns>(argumentString, out MovementPatterns movementType);
        pathingType = movementType;
      }
      else if (args[i].StartsWith(roomTypeArg))
      {
        argumentString = args[i].Replace(roomTypeArg, "");
        Enum.TryParse<RoomType>(argumentString, out RoomType rt);
        roomType = rt;
      }
      else if (args[i].StartsWith(roomLayoutTypeArg))
      {
        argumentString = args[i].Replace(roomLayoutTypeArg, "");
        Enum.TryParse<PresetRoomOptions>(argumentString, out PresetRoomOptions layoutType);
        roomLayoutType = layoutType;
      }
      else if (args[i].StartsWith(roomWidthArg))
      {
        argumentString = args[i].Replace(roomWidthArg, "");
        int.TryParse(argumentString, out int width);
        defaultWidth = width;
      }
      else if (args[i].StartsWith(roomLengthArg))
      {
        argumentString = args[i].Replace(roomLengthArg, "");
        int.TryParse(argumentString, out int length);
        defaultLength = length;
      }
      else if (args[i].StartsWith(seedArg))
      {
        argumentString = args[i].Replace(seedArg, "");
        int.TryParse(argumentString, out int seedNum);
        seed = seedNum;
      }
    }

  }

  #endregion
}