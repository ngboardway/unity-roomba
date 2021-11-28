using System;
using TMPro;
using UnityEngine;

public class TankControl : MonoBehaviour
{
  public float Speed = 12f;
  public float TurnSpeed = 180f;
  public TextMeshProUGUI CountText;
  public TextMeshProUGUI DirtCountText;

  private string MovementAxisName;
  private string TurnAxisName;
  private Rigidbody Rigidbody;
  private float MovementInputValue;
  private float TurnInputValue;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtHealthValue = 0.05f;
  private float MovementHealthValue = 0.025f;

  private string CurrentOrientation = "Up";
  private Room Room;

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody>();
  }

  private void OnEnable()
  {
    Rigidbody.isKinematic = false;
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
  }

  private void FixedUpdate()
  {
    // Move and turn the tank.
    Move();
    Turn();
  }

  private void Move()
  {
    // Adjust the position of the tank based on the player's input.
    // Scaled by the amount of input it's receiving
    Vector3 movement = transform.forward * MovementInputValue * Speed * Time.fixedDeltaTime;
    Rigidbody.MovePosition(Rigidbody.position + movement);
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
    Debug.Log(other.name);
    if (other.gameObject.CompareTag("Dirt"))
    {
      other.gameObject.SetActive(false);
      DirtCount++;
      UpdateHealth(DirtHealthValue);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Obstacle"))
    {

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

  }
}