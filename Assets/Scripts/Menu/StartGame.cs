using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour {

	public List<CanvasGroup> HideablePanels;
	public List<InputField> PlayerNames;
	public List<Toggle> IsCpuToggles;

	private PersistentData persistentData;
	private int numberOfPlayers;

	void Start()
	{
		numberOfPlayers = 6;
	}

	public void onValueChanged(int index)
	{
		Debug.Log("Value changed to: " + index);
		numberOfPlayers = index + 3;

		for (int i = index; i < HideablePanels.Count; i++)
		{
			HideablePanels[i].alpha = 0;
		}

		for (int i = 0; i < index; i++)
		{
			HideablePanels[i].alpha = 1;
		}
	}

	public void Play()
	{
		Debug.Log("Play");
		
		//TODO: read data & put in persistentData
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
		persistentData.SaveSettings(players);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Back()
	{
		Debug.Log("Back");
		Destroy(GameObject.Find("PersistentData"));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

}
