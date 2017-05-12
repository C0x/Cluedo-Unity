using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private GameObject gameBoard;
	private List<int> PawnIds;

	void Start()
	{
		DebugSetup();
	}

	void Update()
	{		
	}

	private void DebugSetup()
	{
		//debug
		GameObject gameBoard = GameObject.Find("GameBoard");
		GameboardManager gameboardManager = gameBoard.GetComponent<GameboardManager>();

		PawnIds = new List<int>();
		PawnIds.Add(1);
		PawnIds.Add(2);
		PawnIds.Add(3);
		PawnIds.Add(4);
		PawnIds.Add(5);
		PawnIds.Add(6);

		gameboardManager.SpawnPawns(PawnIds);
	}
}
