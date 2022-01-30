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

      float roundedX = Mathf.Floor(x);
      float roundedZ = Mathf.Floor(z);

      MapLocation nextLocation = Room.GetObjectForCoordinate(roundedX, roundedZ);
      Debug.Log(nextLocation.X);
      Debug.Log(nextLocation.Z);

      nextLocation.Visited = true;
      Room.SetCurrentLocation(x, z);
      
      transform.position = Vector3.MoveTowards(
             transform.position, Target, Speed);

      Debug.Log($"Goal: ({Target.x}, {Target.z}), Actual: ({transform.position.x}, {transform.position.z})");
    }

    protected void MoveLeft(Transform transform)
    {
      (float x, float z) = Room.GetCurrentLocation();
      float newX = x - 0.5f;

      Move(newX, z, transform);
    }

    protected void MoveRight(Transform transform)
    {
      (float x, float z) = Room.GetCurrentLocation();

      float newX = x + 0.5f;
      Move(newX, z, transform);
    }

    protected void MoveUp(Transform transform)
    {
      (float x, float z) = Room.GetCurrentLocation();

      float newZ = z + 0.5f;
      Move(x, newZ, transform);
    }

    protected void MoveDown(Transform transform)
    {
      (float x, float z) = Room.GetCurrentLocation();

      float newZ = z - 0.5f;
      Move(x, newZ, transform);
    }

    protected MapLocation GetNextLocation(Orientation orientation)
    {
      (float currentX, float currentZ) = Room.GetCurrentLocation();

      float nextX;
      float nextZ;

      if (orientation == Orientation.Up)
      {
        nextX = currentX;
        nextZ = currentZ + 0.5f;
      }
      else if (orientation == Orientation.Right)
      {
        nextX = currentX + 0.5f;
        nextZ = currentZ;
      }
      else if (orientation == Orientation.Down)
      {
        nextX = currentX;
        nextZ = currentZ - 0.5f;
      }
      else
      {
        nextX = currentX - 0.5f;
        nextZ = currentZ;
      }

      float roundedX = Mathf.Floor(nextX);
      float roundedZ = Mathf.Floor(nextZ);
      return Room.GetObjectForCoordinate(roundedX, roundedZ);
    }

    protected void ContinueInCurrent(Transform transform)
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
  }
}
