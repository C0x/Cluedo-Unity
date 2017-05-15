using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[HideInInspector]
	public int Round = 0;

	public List<CanvasGroup> CollapsablePanels;
	public List<RawImage> Images;

	private PersistentData persistentData;
	private int currentIndex;
	private int numberOfPlayers;


	void Start () {
		GameObject persistentGO = GameObject.Find("PersistentData");
		Player[] players = null;

		if (Debug.isDebugBuild && persistentGO == null)
		{
			players = DebugPlayers();
		}
		else
		{
			persistentData = persistentGO.GetComponent<PersistentData>();
			players = persistentData.Players;
			//TODO: destroy??
		}

		numberOfPlayers = players.Length;
			
		for (int i = 0; i < players.Length; i++)
		{
			Debug.Log(players[i].ToString());
		}

		currentIndex = 0;
	}

	void Update()
	{

		if (Input.GetKeyDown("space"))
		{
			ToggleCurrentPlayerPanels();
		}

	}

	private void InitGUI()
	{

	}

	private void ToggleCurrentPlayerPanels()
	{
		if (currentIndex == numberOfPlayers)
		{
			currentIndex = 0;
			CollapsablePanels[numberOfPlayers - 1].alpha = 0;
		}
					

		Debug.Log("CurrentIndex: " + currentIndex);

		if (currentIndex > 0)
			CollapseAllPanels();
		CollapsablePanels[currentIndex].alpha = 1;

		for (int i = currentIndex + 1; i < numberOfPlayers; i++) 
		{
			Debug.Log(Images[i].transform.position);
			Images[i].transform.position += new Vector3(185f, 0, 0);
		}

		currentIndex++;
	}

	private void CollapseAllPanels()
	{
		CollapsablePanels.ForEach(p => p.alpha = 0);

		for (int i = currentIndex; i < CollapsablePanels.Count; i++)
		{		
			Images[i].transform.position -= new Vector3(185f, 0, 0);
		}
	}

	///<summary>
	///	Debug methud to create a player array if the scene is directly started without going via gamestart
	///</summary>
	private Player[] DebugPlayers()
	{
		Player[] players = new Player[6]; 

		for (int i = 0; i < players.Length; i++)
		{
			players[i] = new Player((i + 1),
									"Player " + (i + 1),
									Random.value > 0.5f,	//random boolean
									Color.red);
		}


		return players;
	}

}
