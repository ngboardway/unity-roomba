using System;
using UnityEngine;
using static Room;

namespace AssemblyCSharp.Assets
{
  public abstract class Roomba
  {
    protected Orientation CurrentOrientation = Orientation.Up;
    protected Orientation TurnOrientation = Orientation.Left;

    protected bool IsTurning;
    protected bool ShouldMove = true;
    protected int TurnCount = 0;
    private float Speed;

    protected int moveCount = 0;
    protected Room Room;

    public Roomba(Room room, float speed)
    {
      Room = room;
      Speed = speed;
    }

    public abstract void MoveInDirection(Transform transform);

    public MoveEndConditions MoveRoomba(Transform transform)
    {
      if (Room.CurrentLocation.ObjectType == ObjectType.Dirt && Room.CurrentLocation.X != 17)
      {
        ShouldMove = false;
      }
      else
      {
        moveCount = 0;
        if (ShouldMove)
        {
          if (IsTurning)
            if (TurnCount > 0)
            {
              TurnInDirection();
              TurnCount--;
            }
            else
            {
              IsTurning = false;
            }
          else
          {
            MoveInDirection(transform);
          }
        }
      }

      return new MoveEndConditions
      {
        MoveCount = moveCount,
        ShouldMove = ShouldMove
      };

    }

    protected void TurnInDirection()
    {
      if (TurnOrientation == Orientation.Left)
      {
        if (CurrentOrientation == Orientation.Up)
          CurrentOrientation = Orientation.Left;
        else if (CurrentOrientation == Orientation.Left)
          CurrentOrientation = Orientation.Down;
        else if (CurrentOrientation == Orientation.Down)
          CurrentOrientation = Orientation.Right;
        else CurrentOrientation = Orientation.Up;
      }
      else
      {
        if (CurrentOrientation == Orientation.Up)
          CurrentOrientation = Orientation.Right;
        else if (CurrentOrientation == Orientation.Right)
          CurrentOrientation = Orientation.Down;
        else if (CurrentOrientation == Orientation.Down)
          CurrentOrientation = Orientation.Left;
        else CurrentOrientation = Orientation.Up;
      }
    }

    protected void Move(float x, float z, Transform transform)
    {
      moveCount++;
      Vector3 Target = new Vector3(x, 0, z);
      MapLocation nextLocation = Room.GetObjectForCoordinate(x, z);
      nextLocation.Visited = true;
      Room.CurrentLocation = nextLocation;

      transform.position = Vector3.MoveTowards(
             transform.position, Target, Speed);

      Debug.Log($"Position: ({nextLocation.X}, {nextLocation.Z})");
    }

    protected void MoveLeft(Transform transform)
    {
      float x = Room.CurrentLocation.X;
      float z = Room.CurrentLocation.Z;
      float newX = x - 1f;

      Move(newX, z, transform);
    }

    protected void MoveRight(Transform transform)
    {
      float x = Room.CurrentLocation.X;
      float z = Room.CurrentLocation.Z;

      float newX = x + 1f;
      Move(newX, z, transform);
    }

    protected void MoveUp(Transform transform)
    {
      float x = Room.CurrentLocation.X;
      float z = Room.CurrentLocation.Z;

      float newZ = z + 1f;
      Move(x, newZ, transform);
    }

    protected void MoveDown(Transform transform)
    {
      float x = Room.CurrentLocation.X;
      float z = Room.CurrentLocation.Z;

      float newZ = z - 1f;
      Move(x, newZ, transform);
    }

    protected MapLocation GetNextLocation(Orientation orientation)
    {
      float currentX = Room.CurrentLocation.X;
      float currentZ = Room.CurrentLocation.Z;

      float nextX;
      float nextZ;

      if (orientation == Orientation.Up)
      {
        nextX = currentX;
        nextZ = currentZ + 1f;
      }
      else if (orientation == Orientation.Right)
      {
        nextX = currentX + 1f;
        nextZ = currentZ;
      }
      else if (orientation == Orientation.Down)
      {
        nextX = currentX;
        nextZ = currentZ - 1f;
      }
      else
      {
        nextX = currentX - 1f;
        nextZ = currentZ;
      }

      return Room.GetObjectForCoordinate(nextX, nextZ);
    }
  }
}
