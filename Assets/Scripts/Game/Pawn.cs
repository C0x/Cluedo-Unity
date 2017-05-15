using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

	public int Id;
	public string Name;
	
	public int TileX;
	public int TileY;

	[HideInInspector]
	public TileMap Map;
	
	public List<Node> CurrentPath = null;


	void Update()
	{
		DrawLine();

		//If pawn is close to destination, move to next tile
		if (Vector3.Distance(this.transform.position, Map.TileCoordToWorldCoord(TileX, TileY)) < .1f)
		{
			MoveToNextTile();
		}
	
		//Smooth animation	
		this.transform.position = Vector3.Lerp(this.transform.position, Map.TileCoordToWorldCoord(TileX, TileY), 5f * Time.deltaTime);		
	}

	public void MoveToNextTile()
	{
		if (CurrentPath == null)
			return;

		this.transform.position = Map.TileCoordToWorldCoord(TileX, TileY);

		//Assign next tile
		TileX = CurrentPath[1].X;
		TileY = CurrentPath[1].Y;

		//remove current tile from pathfinding list
		CurrentPath.RemoveAt(0);

		if (CurrentPath.Count == 1)
		{
			CurrentPath = null;
		}
	}

	///<summary>
	///	Draws debugline of found path
	///</summary>
	private void DrawLine()
	{
		if (CurrentPath != null)
		{
			int curNode = 0;

			while (curNode < CurrentPath.Count - 1)
			{
				Vector3 start = Map.TileCoordToWorldCoord( CurrentPath[curNode].X, CurrentPath[curNode].Y);
				Vector3 end = Map.TileCoordToWorldCoord( CurrentPath[curNode + 1].X, CurrentPath[curNode + 1].Y);
				Debug.DrawLine(start, end, Color.blue);

				curNode++;
			}
		}
	}
	
}
