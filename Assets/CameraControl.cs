using UnityEngine;

/* Mostly from https://learn.unity.com/project/tanks-tutorial */

public class CameraControl : MonoBehaviour
{
  public float m_DampTime = 0.2f;
  public float m_ScreenEdgeBuffer = 4f;
  public float m_MinSize = 6.5f;
  /*[HideInInspector]*/
public Transform[] m_Targets; // targets


  private Camera m_Camera; // needed for controlling the size                       
  private float m_ZoomSpeed;
  private Vector3 m_MoveVelocity;
  private Vector3 m_DesiredPosition;


  private void Awake()
  {
    m_Camera = GetComponentInChildren<Camera>();
  }


  private void FixedUpdate()
  {
    // Using this because the tanks are moving on fixed update, even though the camera
    // doesn't require the use of any physics. Keeping things in sync.
    Move();
    Zoom();
  }


  private void Move()
  {
    FindAveragePosition();

    transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
  }

  /// <summary>
  /// Find the position in the middle of all of the active targets so that we
  /// can see all of them 
  /// </summary>
  private void FindAveragePosition()
  {
    Vector3 averagePos = new Vector3();
    int numTargets = 0;

    for (int i = 0; i < m_Targets.Length; i++)
    {
      // has the tank died? Don't want to zoom in on a dead tank
      if (!m_Targets[i].gameObject.activeSelf)
        continue; // skip the rest of the body and continue on to the next iteration

      averagePos += m_Targets[i].position;
      numTargets++;
    }

    if (numTargets > 0)
      averagePos /= numTargets;

    averagePos.y = transform.position.y;

    m_DesiredPosition = averagePos;
  }


  private void Zoom()
  {
    float requiredSize = FindRequiredSize();
    m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
  }

  /// <summary>
  /// Go through all of the targets and find what size it could be to accommodate each, and
  /// ultimately pick whichever is largest.
  /// </summary>
  /// <returns></returns>
  private float FindRequiredSize()
  {
    // Finding the size based on the desired position of the camera, not the current position
    Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

    float size = 0f;

    for (int i = 0; i < m_Targets.Length; i++)
    {
      if (!m_Targets[i].gameObject.activeSelf)
        continue;

      Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

      Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

      size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

      size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
    }

    size += m_ScreenEdgeBuffer;

    size = Mathf.Max(size, m_MinSize);

    return size;
  }


  public void SetStartPositionAndSize()
  {
    FindAveragePosition();

    transform.position = m_DesiredPosition;

    m_Camera.orthographicSize = FindRequiredSize();
  }
}