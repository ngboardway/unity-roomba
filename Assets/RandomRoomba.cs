using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyCSharp.Assets
{
  public class RandomRoomba : Roomba
  {
    public RandomRoomba(Room room, float speed)
      : base(room, speed)
    {
      CurrentOrientation = PickDirection();
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
          PickNewDirection();
        }
      }
      else
      {
        ShouldMove = false;
      }
    }

    public void PickNewDirection()
    {
      bool hasDirection = false;

      while (!hasDirection)
      {
        Orientation nextOrientation = PickDirection();
        if (nextOrientation != CurrentOrientation)
        {
          MapLocation nextLocation = GetNextLocation(nextOrientation);

          if (nextLocation != null && nextLocation.CanInhabitSpace())
          {
            CurrentOrientation = nextOrientation;
            hasDirection = true;
          }
        }
      }
    }

    private Orientation PickDirection()
    {
      Array allValues = Enum.GetValues(typeof(Orientation));
      int total = allValues.Length;

      int nextDirection = Convert.ToInt32(UnityEngine.Random.value * total);
      if (nextDirection == 0)
        nextDirection = 1;

      return (Orientation)nextDirection;
    }

    public void PickTurnDirection()
    {
      int direction = Convert.ToInt32(UnityEngine.Random.value);
      TurnOrientation = (Orientation)direction;
    }

    public void FindTurnCounts()
    {
      // Left: 0
      // Right: 1
      // Up: 2
      // Down: 3
      if (TurnOrientation == Orientation.Left)
      {
        // L D R U
      }
      else
      {
        // L U R D
      }

      // If current orientation is left and I need to go right next 
    }
  }
}
