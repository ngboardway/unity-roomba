public enum Orientation
{
  Left,
  Right,
  Up,
  Down
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

public class TagConstants
{
  public const string Dirt = "Dirt";
  public const string TextDirt = "Text - Dirt";
  public const string TextHealth = "Text - Health";
  public const string TextEnd = "Text - Stuck";
  public const string Obstacle = "Obstacle";
  public const string Room = "Room";
}