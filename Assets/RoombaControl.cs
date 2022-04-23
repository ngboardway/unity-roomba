using System;
using AssemblyCSharp;
using AssemblyCSharp.Assets;
using TMPro;
using UnityEngine;
using static Room;
using Random = UnityEngine.Random;

/* Adapted from https://learn.unity.com/project/tanks-tutorial */

public class RoombaControl : MonoBehaviour
{
  public float Speed = 6f;
  public float TurnSpeed = 180f;
  public float SimulationTimeTotal = 315f;

  public BoxCollider TopCollider;
  public BoxCollider RightCollider;
  public BoxCollider BottomCollider;
  public BoxCollider LeftCollider;

  public TextMeshProUGUI CountText;
  public TextMeshProUGUI DirtCountText;
  private Room Room;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtHealthValue = 0.05f;
  private float MovementHealthValue = 0.0925f;

  private bool IsSimulationActive = true;
  private float SimulationTimeRemaining;

  private Roomba Roomba;
  private Rigidbody rb;

  private void Awake()
  {
    Room = GameObject.FindGameObjectWithTag(TagConstants.Room).GetComponent<Room>();
    CountText = GameObject.FindGameObjectWithTag(TagConstants.TextHealth).GetComponent<TextMeshProUGUI>();
    DirtCountText = GameObject.FindGameObjectWithTag(TagConstants.TextDirt).GetComponent<TextMeshProUGUI>();
    rb = GetComponent<Rigidbody>();

    if (Room.RoombaPathingType == MovementPatterns.Lawnmower)
      Roomba = new LawnMowerRoomba(Room, Speed, TopCollider, RightCollider, BottomCollider, LeftCollider);
    else if (Room.RoombaPathingType == MovementPatterns.Random)
      Roomba = new RandomRoomba(Room, Speed, TopCollider, RightCollider, BottomCollider, LeftCollider);

    SimulationTimeRemaining = SimulationTimeTotal;
  }

  private void OnEnable()
  {
    CurrentHealth = StartingHealth;
    SetHealthUI();
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

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag(TagConstants.Obstacle) || collision.gameObject.CompareTag(TagConstants.InnerObstacle))
    {
      Roomba.HandleCollision(collision, rb);
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

  private void FixedUpdate()
  {
    if (IsSimulationActive)
    {
      SimulationTimeRemaining -= 1f;
      if (SimulationTimeRemaining <= 0f)
      {
        EndSimulation("Time");
        IsSimulationActive = false;
      }
      else
      {
        if (Room.SimulationIsComplete())
        {
          EndSimulation("Complete");
          IsSimulationActive = false;
        }

        MoveEndConditions moveEndConditions = Roomba.MoveRoomba(rb);
        float movement = (MovementHealthValue * moveEndConditions.MoveCount);
        UpdateHealth(movement);

        if (CurrentHealth <= 0f)
        {
          EndSimulation("Battery");
          IsSimulationActive = false;
        }

        if (!moveEndConditions.ShouldMove)
        {
          EndSimulation("Stuck");
          IsSimulationActive = false;
        }
      }
    }
  }

  private void EndSimulation(string reason)
  {
    float timeTaken = SimulationTimeTotal - SimulationTimeRemaining;

    Room.EndSimulation(reason, DirtCount, timeTaken, CurrentHealth);
    IsSimulationActive = false;
  }
}