using System;
using TMPro;
using UnityEngine;
using static Room;

public class TankControl : MonoBehaviour
{
  public float Speed = 12f;
  public float TurnSpeed = 180f;
  public TextMeshProUGUI CountText;
  public TextMeshProUGUI DirtCountText;
  private Room Room;

  private Vector3 Target;
  private bool IsTurning;
  private bool ShouldMove = true;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtHealthValue = 0.05f;
  private float MovementHealthValue = 0.025f;

  private Orientation CurrentOrientation = Orientation.Up;
  private Orientation TurnOrientation = Orientation.Left;
  private int TurnCount = 0;

  private void Awake()
  {
    Room = GameObject.FindGameObjectWithTag(TagConstants.Room).GetComponent<Room>();
    CountText = GameObject.FindGameObjectWithTag(TagConstants.TextHealth).GetComponent<TextMeshProUGUI>();
    DirtCountText = GameObject.FindGameObjectWithTag(TagConstants.TextDirt).GetComponent<TextMeshProUGUI>();
  }

  private void OnEnable()
  {
    CurrentHealth = StartingHealth;
    SetHealthUI();
  }

  private void Start()
  {
    Target = transform.position;
  }

  private void Update()
  {
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
        MoveInDirection();
      }
    }
  }

  private void MoveInDirection()
  {
    MapLocation nextLocation = GetNextLocation(CurrentOrientation);
    if (nextLocation != null)
    {
      if (nextLocation.ObjectType == ObjectType.Tile || nextLocation.ObjectType == ObjectType.Dirt)
      {
        if (CurrentOrientation == Orientation.Up)
          MoveUp();
        else if (CurrentOrientation == Orientation.Down)
          MoveDown();
        else if (CurrentOrientation == Orientation.Left)
          MoveLeft();
        else
          MoveRight();
      }
      else
      {
        MapLocation locationAfterTurn = GetNextLocation(Orientation.Left);
        if (locationAfterTurn != null && (locationAfterTurn.ObjectType == ObjectType.Tile || locationAfterTurn.ObjectType == ObjectType.Dirt))
        {
          if (CurrentOrientation == Orientation.Up)
          {
            MoveLeft();
            TurnCount = 2;
            TurnOrientation = Orientation.Left;
            IsTurning = true;
          }

          if (CurrentOrientation == Orientation.Down)
          {
            MoveLeft();
            TurnCount = 2;
            TurnOrientation = Orientation.Right;
            IsTurning = true;
          }
        }
        else
        {
          ShouldMove = false;
          Room.EndSimulation();
        }
      }
    }
  }

  private void TurnInDirection()
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

  private void Move(float x, float z)
  {
    Target = new Vector3(x, 0, z);
    MapLocation nextLocation = Room.GetObjectForCoordinate(x, z);
    nextLocation.Visited = true;
    Room.CurrentLocation = nextLocation;

    transform.position = Vector3.MoveTowards(
           transform.position, Target, Speed);

    UpdateHealth(MovementHealthValue);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag(TagConstants.Dirt))
    {
      other.gameObject.SetActive(false);
      DirtCount++;
      UpdateHealth(DirtHealthValue);
    }
  }

  private void SetHealthUI()
  {
    CountText.text = $"{Convert.ToInt32(CurrentHealth)}%";
    DirtCountText.text = $"Count: {DirtCount}";
  }

  private void UpdateHealth(float decrement)
  {
    CurrentHealth -= decrement;
    SetHealthUI();
  }

  private void MoveLeft()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;
    float newX = x - 1f;

    Move(newX, z);
  }

  private void MoveRight()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newX = x + 1f;
    Move(newX, z);
  }

  private void MoveUp()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newZ = z + 1f;
    Move(x, newZ);
  }

  private void MoveDown()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newZ = z - 1f;
    Move(x, newZ);
  }

  private MapLocation GetNextLocation(Orientation orientation)
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