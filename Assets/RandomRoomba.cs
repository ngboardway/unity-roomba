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

    public void PickNewDirection(Rigidbody rigidbody)
    {
      bool hasDirection = false;

      while (!hasDirection)
      {
        Orientation nextOrientation = PickDirection();
        bool isValid = NextDirectionIsValid(nextOrientation, rigidbody);

        if (nextOrientation != CurrentOrientation && isValid)
        {
          CurrentOrientation = nextOrientation;
          hasDirection = true;
        }
      }
    }

    private bool NextDirectionIsValid(Orientation orientation, Rigidbody rigidbody)
    {
      Vector3 forward = GetForward(orientation);
      Vector3 movement = forward * Speed * Time.fixedDeltaTime;
      Vector3 newPosition = rigidbody.position + movement;

      float x = Mathf.Round(newPosition.x);
      float z = Mathf.Round(newPosition.z);
      
      MapLocation location = Room.GetObjectForCoordinate(x, z);
      return location.CanInhabitSpace();
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

    public override bool HandleCollision(Rigidbody rigidbody)
    {
      PickNewDirection(rigidbody);

      return true;
    }
  }
}
