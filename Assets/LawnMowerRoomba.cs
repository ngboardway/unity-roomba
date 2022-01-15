using System;
using AssemblyCSharp.Assets;
using UnityEngine;
using static Room;

namespace AssemblyCSharp
{
  public class LawnMowerRoomba : Roomba
  {
    public LawnMowerRoomba(Room room, float speed) : base(room, speed)
    {
    }

    public override void MoveInDirection(Transform transform)
    {
      MapLocation nextLocation = GetNextLocation(CurrentOrientation);
      if (nextLocation != null)
      {
        if (nextLocation.ObjectType == ObjectType.Tile || nextLocation.ObjectType == ObjectType.Dirt)
        {
          if (CurrentOrientation == Orientation.Up)
            MoveUp(transform);
          else if (CurrentOrientation == Orientation.Down)
            MoveDown(transform);
          else if (CurrentOrientation == Orientation.Left)
            MoveLeft(transform);
          else
            MoveRight(transform);
        }
        else
        {
          MapLocation locationAfterTurn = GetNextLocation(Orientation.Left);
          if (locationAfterTurn != null && (locationAfterTurn.ObjectType == ObjectType.Tile || locationAfterTurn.ObjectType == ObjectType.Dirt))
          {
            if (CurrentOrientation == Orientation.Up)
            {
              MoveLeft(transform);
              TurnCount = 2;
              TurnOrientation = Orientation.Left;
              IsTurning = true;
            }

            if (CurrentOrientation == Orientation.Down)
            {
              MoveLeft(transform);
              TurnCount = 2;
              TurnOrientation = Orientation.Right;
              IsTurning = true;
            }
          }
          else
          {
            ShouldMove = false;
          }
        }
      }
    }
  }
}
