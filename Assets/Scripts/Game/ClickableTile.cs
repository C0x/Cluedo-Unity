using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Tiles that where the player can click to move his pawn
///</summary>
public class ClickableTile : MonoBehaviour 
{
	[HideInInspector]
	public int TileX;
	[HideInInspector]
	public int TileY;
	[HideInInspector]
	public TileMap Map;
	
	public bool Enabled = false;

	void OnMouseUp()
	{
		if (Enabled)
			Map.CalculatePathTo(TileX, TileY);
	}
}
