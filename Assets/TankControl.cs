using System;
using TMPro;
using UnityEngine;

public class TankControl : MonoBehaviour
{
  public int PlayerNumber = 1;
  public float Speed = 12f;
  public float TurnSpeed = 180f;
  public TextMeshProUGUI CountText;
  public TextMeshProUGUI DirtCountText;

  public BoxCollider TopLeftCollider;
  public BoxCollider TopRightCollider;
  //public BoxCollider BottomRightCollider;
  //public BoxCollider BottomLeftCollider;

  private string MovementAxisName;
  private string TurnAxisName;
  private Rigidbody Rigidbody;
  private float MovementInputValue;
  private float TurnInputValue;

  private float StartingHealth = 100f;
  private float CurrentHealth;
  private int DirtCount = 0;

  private float DirtValue = 0.05f;

  private bool TurnLeft;
  private bool TurnRight;
  private bool MoveBack;
  private bool MoveForward;

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
    MovementAxisName = "Vertical" + PlayerNumber;
    TurnAxisName = "Horizontal" + PlayerNumber;
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
      UpdateHealth(DirtValue);
    }
    else if (other.gameObject.CompareTag("Furnishings"))
    {
      Debug.Log("REDIRECT");
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (collision.gameObject.CompareTag("Obstacle"))
    {
      Debug.Log($"Exiting: {collision.collider.name}");

      (float turnValue, float moveValue) = GetMovementValues();
      Debug.Log(turnValue);
      Debug.Log(moveValue);

      Redirect(turnValue, moveValue);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Obstacle"))
    {
      Debug.Log($"Entering: {collision.collider.name}");

      bool hitLeft = TopLeftCollider.bounds.Intersects(collision.collider.bounds);
      bool hitRight = TopRightCollider.bounds.Intersects(collision.collider.bounds);
      //bool hitBottom = BottomRightCollider.bounds.Intersects(collision.collider.bounds);
      //bool hitLeft = BottomLeftCollider.bounds.Intersects(collision.collider.bounds);

      FindDirections(hitLeft, hitRight);
      
      MovementInputValue = -1f;
      Move();
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

  private (float turnValue, float moveValue) GetMovementValues()
  {
    float turnValue = 0f;
    float moveValue = 0f;

    Debug.Log($"RIGHT: {TurnRight}");
    Debug.Log($"BACK: {MoveBack}");
    Debug.Log($"LEFT: {TurnLeft}");
    Debug.Log($"FORWARD: {MoveForward}");

    if (TurnLeft)
      turnValue = 1f;
    else if (TurnRight)
      turnValue = -1f;

    if (MoveBack)
      moveValue = -1f;
    else if (MoveForward)
      moveValue = 1f;

    return (turnValue, moveValue);
  }

  private void FindDirections(bool hitLeft, bool hitRight)
  {
    bool turnLeft = false;
    bool turnRight = false;
    bool moveBack = false;
    bool moveForward = false;

    if (hitRight)
    {
      turnRight = false;
      turnLeft = true;
    }

    if (hitLeft)
    {
      turnLeft = false;
      turnRight = true;
    }

    TurnLeft = turnLeft;
    TurnRight = turnRight;
    MoveBack = moveBack;
    MoveForward = moveForward;
  }
}