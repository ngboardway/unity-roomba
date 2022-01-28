using System;
using AssemblyCSharp.Assets;
using UnityEngine;
using static Room;

namespace AssemblyCSharp
{
  public class LawnMowerRoomba : Roomba
  {
    public LawnMowerRoomba(Room room, float speed)
      : base(room, speed)
    {
    }

    public override void MoveInDirection(Transform transform)
    {
      MapLocation nextLocation = GetNextLocation(CurrentOrientation);
      if (nextLocation != null)
      {
        if (nextLocation.CanInhabitSpace())
        {
          ContinueInCurrent(transform);
        }
        else
        {
          MapLocation locationAfterTurn = GetNextLocation(Orientation.Left);
          if (locationAfterTurn != null && locationAfterTurn.CanInhabitSpace())
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
