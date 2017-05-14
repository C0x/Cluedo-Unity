using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	[HideInInspector]
	public int TileX;
	[HideInInspector]
	public int TileY;
	[HideInInspector]
	public TileMap Map;

	void OnMouseUp()
	{
		Debug.Log(TileX + "," + TileY);

		Map.CalculatePathTo(TileX, TileY);
	}

}
