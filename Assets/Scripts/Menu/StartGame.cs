using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

///<summary>
/// Eventhandler for StartGameScene
/// Fills Persistent data object to seed Main
///</summary>
public class StartGame : MonoBehaviour 
{
	public List<CanvasGroup> HideablePanels;
	public List<InputField> PlayerNames;
	public List<Toggle> IsCpuToggles;

	private PersistentData persistentData;
	private int numberOfPlayers;

	void Start()
	{
		numberOfPlayers = 6;
	}

	///<summary>
	/// If dropdown value changed -> create or delete player input
	///</summary>
	public void onValueChanged(int index)
	{
		//Debug.Log("Value changed to: " + index);
		numberOfPlayers = index + 3;

		//Hide all panels
		for (int i = index; i < HideablePanels.Count; i++)
		{
			HideablePanels[i].alpha = 0;
		}

		//Show <index> panels
		for (int i = 0; i < index; i++)
		{
			HideablePanels[i].alpha = 1;
		}
	}

	///<summary>
	/// Fill persistentData object with values
	///</summary>
	public void Play()
	{
		//Debug.Log("Play");		
		Player[] players = new Player[numberOfPlayers];

		for (int i = 0; i < numberOfPlayers; i++)
		{
			string playerName =  String.IsNullOrEmpty(PlayerNames[i].text) ? "Player " + (i + 1) : PlayerNames[i].text;

			Color color = Color.black;
			switch(i)
			{
				case 0: color = Color.white;break;
				case 1: color = Color.green;break;
				case 2: color = Color.blue;break;
				case 3: color = Color.yellow;break;
				case 4: color = Color.magenta;break;
				case 5: color = Color.red;break;
			}

			Player player = new Player((i + 1), 
										playerName,
										IsCpuToggles[i].isOn,
										color);	
			players[i] = player;
		}

		GameObject persistentGO = GameObject.Find("PersistentData");
		PersistentData persistentData = persistentGO.GetComponent<PersistentData>();
		persistentData.Players = players;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	///<summary>
	/// Move back to main menu
	///</summary>
	public void Back()
	{
		Debug.Log("Back");
		Destroy(GameObject.Find("PersistentData"));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
}
