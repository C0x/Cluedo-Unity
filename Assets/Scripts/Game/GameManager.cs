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

	private Player[] players;
	private int numberOfPlayers;


	void Start () {
		LoadPersistentData();
		InitGui();

		currentIndex = 0;
		ToggleCurrentPlayerPanels();
	}

	void Update()
	{
		//debug
		if (Input.GetKeyDown("space"))
		{
			ToggleCurrentPlayerPanels();
		}

	}

	///<summary>
	///	Loads persistent data into local player array
	///</summary>
	private void LoadPersistentData()
	{
		GameObject persistentGO = GameObject.Find("PersistentData");
		players = null;

		if (Debug.isDebugBuild && persistentGO == null)
		{
			players = DebugPlayers();
		}
		else
		{
			persistentData = persistentGO.GetComponent<PersistentData>();
			players = persistentData.Players;
			Destroy(persistentGO, 1f);
		}

		numberOfPlayers = players.Length;
			
		for (int i = 0; i < players.Length; i++)
		{
			Debug.Log(players[i].ToString());
		}
	}

	///<summary>
	///	Loads GUI with values of persistent data
	///</summary>
	private void InitGui()
	{
		//Hide unused images
		for (int i = numberOfPlayers; i < Images.Count; i++)
		{
			Images[i].enabled = false;
		}

		for (int i = 0; i < numberOfPlayers; i++)
		{
			GameObject t1GO = GameObject.Find("PlayerName" + (i + 1));
			t1GO.GetComponent<Text>().text = players[i].Name;
			if (players[i].IsCpu)
			{
				t1GO.GetComponent<Text>().text += "\t(CPU)";
			}

			GameObject t2GO = GameObject.Find("PlayingAs" + (i + 1));

			if (players[i].Color == Color.white)
			{
				t2GO.GetComponent<Text>().text = "Mrs. White";
			} 
			else if (players[i].Color == Color.green)
			{
				t2GO.GetComponent<Text>().text = "Mr. Green";
			} 
			else if (players[i].Color == Color.blue)
			{
				t2GO.GetComponent<Text>().text = "Mrs. Peacock";
			} 
			else if (players[i].Color == Color.yellow)
			{
				t2GO.GetComponent<Text>().text = "Col. Mustard";
			}
			else if (players[i].Color == Color.magenta)
			{
				t2GO.GetComponent<Text>().text = "Prof. Plum";
			}
			else if (players[i].Color == Color.red)
			{
				t2GO.GetComponent<Text>().text = "Mrs. Scarlett";
			}	
		}
	}

	///<summary>
	///	Show panel of current player
	///</summary>
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

	///<summary>
	///	Hides all panels (of current players)
	///</summary>
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
