using System;
using UnityEngine;

///<summary>
/// Plain c# object to define different TileTypes
/// Needs to be defined in inspector
/// ! ORDER MUST BE THE SAME AS THE ENUM IN TILEMAP.CS !
///</summary>
[Serializable]
public class TileType 
{
	public string Name;
	public GameObject InvisiblePrefab;
	public bool CanBeEntered = false;	
}
