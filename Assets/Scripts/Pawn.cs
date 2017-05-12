using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

	public int Id;
	public string Name;
	public bool InSpawn;


	public List<GameObject> SearchAvailableMoves(List<GameObject> AccessibleTiles, int maxMoves)
	{
		Vector3 curPos = this.transform.position;
		GameObject curTile = null;

		foreach (var tile in AccessibleTiles) 
		{
			if (tile.transform.position == curPos)
			{
				curTile = tile;
				break;
			}
		}

		if (curTile != null) 
		{
			return Search(curTile, AccessibleTiles, maxMoves);
		} 

		return null;
	}

	public List<GameObject> SearchAvailableMoves(GameObject curTile, List<GameObject> accessibleTiles, int maxMoves)
	{
		return Search(curTile, accessibleTiles, maxMoves);
	}

	private List<GameObject> Search(GameObject curTile, List<GameObject> accessibleTiles, int maxMoves)
	{
		//TODO: path finding

		return null;
	}

}
