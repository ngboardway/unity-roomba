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

  private Rigidbody Rigidbody;
  private float MovementInputValue;
  private float TurnInputValue;

  private Vector3 Target;
  private bool IsTurning;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtHealthValue = 0.05f;
  private float MovementHealthValue = 0.025f;

  private string CurrentOrientation = "Up";
  private string TurnOrientation = "Left";
  private int TurnCount = 0;

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody>();
    Room = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
  }

  private void OnEnable()
  {
    //Rigidbody.isKinematic = false;
    MovementInputValue = 0f;
    TurnInputValue = 0f;
    CurrentHealth = StartingHealth;
    SetHealthUI();
  }

  private void OnDisable()
  {
    Rigidbody.isKinematic = true;
  }

  private void Start()
  {
    MovementInputValue = 1f;
    Target = transform.position;
  }

  private void Update()
  {
    //Debug.Log("Fixed update");
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

  private void MoveInDirection()
  {
    Debug.Log(CurrentOrientation);

    MapLocation nextLocation = GetNextLocation();
    if (nextLocation != null)
    {
      Debug.Log("Not null");
      Debug.Log(nextLocation.ObjectType);
      if (nextLocation.ObjectType == "Tile" || nextLocation.ObjectType == "Dirt")
      {
        if (CurrentOrientation == "Up")
          MoveUp();
        else if (CurrentOrientation == "Down")
          MoveDown();
        else if (CurrentOrientation == "Left")
          MoveLeft();
        else
          MoveRight();
      }
      else
      {
        Debug.Log("Trying to turn");
        if (CurrentOrientation == "Up")
        {
          MoveLeft();
          TurnCount = 2;
          TurnOrientation = "Left";
          IsTurning = true;
        }

        if(CurrentOrientation == "Down")
        {
          MoveLeft();
          TurnCount = 2;
          TurnOrientation = "Right";
          IsTurning = true;
        }
      }
    }
  }

  private void TurnInDirection()
  {
    if (TurnOrientation == "Left")
    {
      if (CurrentOrientation == "Up")
        CurrentOrientation = "Left";
      else if (CurrentOrientation == "Left")
        CurrentOrientation = "Down";
      else if (CurrentOrientation == "Down")
        CurrentOrientation = "Right";
      else CurrentOrientation = "Up";
    }
    else
    {
      if (CurrentOrientation == "Up")
        CurrentOrientation = "Right";
      else if (CurrentOrientation == "Right")
        CurrentOrientation = "Down";
      else if (CurrentOrientation == "Down")
        CurrentOrientation = "Left";
      else CurrentOrientation = "Up";
    }
  }

  private void Move()
  {
    transform.position = Vector3.MoveTowards(
           transform.position, Target, Speed);
    CurrentHealth -= MovementHealthValue;
  }

  private void Turn()
  {
    // Adjust the rotation of the tank based on the player's input.
    float turn = TurnInputValue * TurnSpeed * Time.fixedDeltaTime;
    Quaternion turnRotation = Quaternion.Euler(0f, turn, 0);
    Rigidbody.MoveRotation(Rigidbody.rotation * turnRotation);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Dirt"))
    {
      other.gameObject.SetActive(false);
      DirtCount++;
      UpdateHealth(DirtHealthValue);
    }
  }

  //private void OnCollisionEnter(Collision collision)
  //{
  //  if (collision.gameObject.CompareTag("Obstacle"))
  //  {
  //    //Debug.Log(collision.collider.name);
  //    //if (CurrentOrientation == "Up")
  //    //  MoveLeft();
  //  }
  //}

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

  private void Redirect(float turnValue, float moveValue)
  {
    MovementInputValue = moveValue;
    Move();

    TurnInputValue = turnValue;
    Turn();
  }

  // have methods for turning all directions
  // have methods for updating rotation in all directions
  // have method for coordinating movement
  // need fields for current direction (orientation)
  private void MoveLeft()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;
    float newX = x - 1f;
    Target = new Vector3(newX, 0, z);
    Room.CurrentLocation = Room.GetObjectForCoordinate(newX, z);
    Move();
  }

  private void MoveRight()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newX = x + 1f;
    Target = new Vector3(newX, 0, z);
    Room.CurrentLocation = Room.GetObjectForCoordinate(newX, z);
    Move();
  }

  private void MoveUp()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newZ = z + 1f;
    Target = new Vector3(x, 0, newZ);
    Room.CurrentLocation = Room.GetObjectForCoordinate(x, newZ);
    Move();
  }

  private void MoveDown()
  {
    float x = Room.CurrentLocation.X;
    float z = Room.CurrentLocation.Z;

    float newZ = z - 1f;
    Target = new Vector3(x, 0, newZ);
    Room.CurrentLocation = Room.GetObjectForCoordinate(x, newZ);
    Move();
  }

  private MapLocation GetNextLocation()
  {
    float currentX = Room.CurrentLocation.X;
    float currentZ = Room.CurrentLocation.Z;

    float nextX;
    float nextZ;

    if (CurrentOrientation == "Up")
    {
      nextX = currentX;
      nextZ = currentZ + 1f;
    }
    else if (CurrentOrientation == "Right")
    {
      nextX = currentX + 1f;
      nextZ = currentZ;
    }
    else if (CurrentOrientation == "Down")
    {
      nextX = currentX;
      nextZ = currentZ - 1f;
    }
    else
    {
      nextX = currentX- 1f;
      nextZ = currentZ ;
    }

    return Room.GetObjectForCoordinate(nextX, nextZ);
  }
}