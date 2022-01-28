﻿using System;
using AssemblyCSharp;
using AssemblyCSharp.Assets;
using TMPro;
using UnityEngine;
using static Room;
using Random = UnityEngine.Random;

public class TankControl : MonoBehaviour
{
  public float Speed = 6f;
  public float TurnSpeed = 180f;
  public float SimulationTime = 315f;
  public MovementPatterns RoombaPathingType;

  public TextMeshProUGUI CountText;
  public TextMeshProUGUI DirtCountText;
  private Room Room;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtHealthValue = 0.05f;
  private float MovementHealthValue = 0.025f;

  private bool IsSimulationActive = true;

  private Roomba Roomba;

  private void Awake()
  {
    Room = GameObject.FindGameObjectWithTag(TagConstants.Room).GetComponent<Room>();
    CountText = GameObject.FindGameObjectWithTag(TagConstants.TextHealth).GetComponent<TextMeshProUGUI>();
    DirtCountText = GameObject.FindGameObjectWithTag(TagConstants.TextDirt).GetComponent<TextMeshProUGUI>();

    if (RoombaPathingType == MovementPatterns.Lawnmower)
      Roomba = new LawnMowerRoomba(Room, Speed);
    else if (RoombaPathingType == MovementPatterns.Random)
      Roomba = new RandomRoomba(Room, Speed);
  }

  private void Start()
  {
    Random.InitState(54);
  }

  private void OnEnable()
  {
    CurrentHealth = StartingHealth;
    SetHealthUI();
  }

  private void Update()
  {
    if (IsSimulationActive)
    {
      SimulationTime -= 1f;
      if (SimulationTime <= 0f)
      {
        Room.EndSimulation("Time");
        IsSimulationActive = false;
      }
      else
      {
        MoveEndConditions moveEndConditions = Roomba.MoveRoomba(transform);
        float movement = (MovementHealthValue * moveEndConditions.MoveCount);
        UpdateHealth(movement);
        if (CurrentHealth <= 0f)
        {
          Room.EndSimulation("Battery");
          IsSimulationActive = false;
        }

        if (!moveEndConditions.ShouldMove)
        {
          Room.EndSimulation("Stuck");
          IsSimulationActive = false;
        }
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log($"Dirt collected at ({Room.CurrentLocation.X}, {Room.CurrentLocation.Z})");
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
}