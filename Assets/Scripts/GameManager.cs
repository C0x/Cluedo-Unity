using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Pawn WhitePrefab;
	public Pawn GreenPrefab;
	public Pawn BluePrefab;
	public Pawn PurplePrefab;
	public Pawn YellowPrefab;
	public Pawn RedPrefab;

	private List<Pawn> PawnsInGame;

	void Start()
	{
		SpawnPawns();
	}

	private void DebugSetup()
	{
		//debug
		PawnsInGame = new List<Pawn>();
		PawnsInGame.Add(WhitePrefab);
		PawnsInGame.Add(GreenPrefab);
		PawnsInGame.Add(BluePrefab);
		PawnsInGame.Add(PurplePrefab);
		PawnsInGame.Add(YellowPrefab);
		PawnsInGame.Add(RedPrefab);
	}

	private void SpawnPawns() 
	{
		DebugSetup();

		foreach (var pawn in PawnsInGame)
		{
			Instantiate(pawn, pawn.transform.position, pawn.transform.rotation);
		}
	}


}
