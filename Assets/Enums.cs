public enum Orientation
{
  Left = 1,
  Right = 2,
  Up = 3,
  Down = 4
}

public enum ObjectType
{
  Tile,
  Dirt,
  Rug,
  Furniture,
  Wall,
  None,
  Roomba
}

public enum MovementPatterns
{
  Lawnmower,
  Random
}

public enum RoomType
{
  Preset,
  RandomGeneration
}

public enum PresetRoomOptions
{
  SingleRoom,
  Apartment,
  Square
}

public class TagConstants
{
  public const string Dirt = "Dirt";
  public const string TextDirt = "Text - Dirt";
  public const string TextHealth = "Text - Health";
  public const string TextEnd = "Text - Stuck";
  public const string Obstacle = "Obstacle";
  public const string Room = "Room";
  public const string InnerObstacle = "InnerObstacle";
}