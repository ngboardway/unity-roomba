using System;
using AssemblyCSharp.Assets;
using UnityEngine;
using static Room;

namespace AssemblyCSharp
{
  public class LawnMowerRoomba : Roomba
  {
    public LawnMowerRoomba(Room room, float speed,
      BoxCollider topCollider, BoxCollider rightCollider,
      BoxCollider bottomCollider, BoxCollider leftCollider)
      : base(room, speed, topCollider, rightCollider, bottomCollider, leftCollider)
    {
    }


    public override void HandleCollision(Rigidbody rb)
    {
      if (CurrentOrientation == Orientation.Up)
        TurnOrientation = Orientation.Left;
      else
        TurnOrientation = Orientation.Right;

      TurnInDirection();
      MoveInDirection(rb);
      TurnInDirection();
    }
  }
}
