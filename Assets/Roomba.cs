using System;
using UnityEngine;
using static Room;

namespace AssemblyCSharp.Assets
{
  public abstract class Roomba
  {
    protected BoxCollider TopCollider;
    protected BoxCollider RightCollider;
    protected BoxCollider BottomCollider;
    protected BoxCollider LeftCollider;

    protected Orientation CurrentOrientation = Orientation.Up;
    protected Orientation TurnOrientation = Orientation.Left;

    protected bool IsTurning;
    protected bool ShouldMove = true;
    protected int TurnCount = 0;
    protected float Speed;

    protected int MoveCount = 0;
    protected Room Room;

    public Roomba(Room room, float speed,
      BoxCollider topCollider, BoxCollider rightCollider,
      BoxCollider bottomCollider, BoxCollider leftCollider)
    {
      Room = room;
      Speed = speed;

      TopCollider = topCollider;
      RightCollider = rightCollider;
      BottomCollider = bottomCollider;
      LeftCollider = leftCollider;
    }

    public abstract void HandleCollision(Rigidbody rigidbody);

    public MoveEndConditions MoveRoomba(Rigidbody rigidbody)
    {
      MoveCount = 0;
      if (ShouldMove)
      {
        if (IsTurning)
          Turn();
        else
        {
          MoveInDirection(rigidbody);
        }
      }

      return new MoveEndConditions
      {
        MoveCount = MoveCount,
        ShouldMove = ShouldMove
      };

    }

    public void MoveInDirection(Rigidbody rigidbody)
    {
      if (ShouldMove)
      {
        MoveCount++;
        // Adjust the position of the tank based on the player's input.
        // Scaled by the amount of input it's receiving
        Vector3 forward = GetForward();
        Vector3 movement = forward * Speed * Time.fixedDeltaTime;
        Vector3 newPosition = rigidbody.position + movement;

        rigidbody.MovePosition(newPosition);
        float x = Mathf.Round(rigidbody.position.x);
        float z = Mathf.Round(rigidbody.position.z);

        if (x < 0 || x > Room.GetRoomWidth() || z < 0 || z > Room.GetRoomLength())
        {
          Debug.Log("Not valid at (" + x + "," + z + "). " + Room.GetRoomWidth() + " by " + Room.GetRoomLength());
          ShouldMove = false;
        }

        MapLocation location = Room.GetObjectForCoordinate(x, z);
        if (location != null)
          location.Visited = true;
      }
    }

    private Vector3 GetForward()
    {
      float x;
      float z;

      if (CurrentOrientation == Orientation.Left)
      {
        x = -2f;
        z = 0f;
      }
      else if (CurrentOrientation == Orientation.Down)
      {
        x = 0f;
        z = -2f;
      }
      else if (CurrentOrientation == Orientation.Right)
      {
        x = 2f;
        z = 0f;
      }
      else
      {
        x = 0f;
        z = 2f;
      }

      return new Vector3(x, 0f, z);
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



    public void Turn()
    {
      if (IsTurning)
      {
        if (TurnCount > 0)
        {
          TurnInDirection();
          TurnCount--;
        }
        else
        {
          IsTurning = false;
          ShouldMove = true;
        }
      }
    }

    public void HandleCollision(Collision collision, Rigidbody rb)
    {
      //Debug.Log(collision.collider.bounds + " " + collision.collider.name + " " + CurrentOrientation);
      bool handleCollision;

      if (CurrentOrientation == Orientation.Up)
      {
        handleCollision = collision.collider.bounds.Intersects(TopCollider.bounds);
      }
      else if (CurrentOrientation == Orientation.Right)
      {
        handleCollision = collision.collider.bounds.Intersects(RightCollider.bounds);
      }
      else if (CurrentOrientation == Orientation.Down)
      {
        handleCollision = collision.collider.bounds.Intersects(BottomCollider.bounds);
      }
      else
      {
        handleCollision = collision.collider.bounds.Intersects(LeftCollider.bounds);
      }

      if (handleCollision)
      {
        if (collision.gameObject.CompareTag(TagConstants.Obstacle))
          HandleCollision(rb);
        else if (collision.gameObject.CompareTag(TagConstants.InnerObstacle))
          ShouldMove = false;
      } 
    }
  }
}
