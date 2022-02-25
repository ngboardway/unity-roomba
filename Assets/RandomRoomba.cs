using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyCSharp.Assets
{
  public class RandomRoomba : Roomba
  {
    public RandomRoomba(Room room, float speed,
      BoxCollider topCollider, BoxCollider rightCollider,
      BoxCollider bottomCollider, BoxCollider leftCollider)
      : base(room, speed, topCollider, rightCollider, bottomCollider, leftCollider)
    {
      CurrentOrientation = Orientation.Up;
    }

    public void PickNewDirection()
    {
      bool hasDirection = false;

      while (!hasDirection)
      {
        Orientation nextOrientation = PickDirection();
        if (nextOrientation != CurrentOrientation)
        {
          CurrentOrientation = nextOrientation;
          hasDirection = true;
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

    public override void HandleCollision(Rigidbody rigidbody)
    {
      PickNewDirection();
      //MoveRoomba(rigidbody);
    }
  }
}
