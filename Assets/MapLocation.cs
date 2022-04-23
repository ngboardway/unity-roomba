using System;
using UnityEngine;

namespace AssemblyCSharp.Assets
{
  public class MapLocation
  {
    public float X { get; set; }
    public float Z { get; set; }
    public bool Visited { get; set; }
    public ObjectType ObjectType { get; set; }
    public Vector3 Position { get; set; }

    public MapLocation(float x, float z)
    {
      X = x;
      Z = z;
      ObjectType = ObjectType.None;
    }

    public bool CanInhabitSpace()
    {
      return ObjectType == ObjectType.Dirt || ObjectType == ObjectType.Tile;
    }
  }
}