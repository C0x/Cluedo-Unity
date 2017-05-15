using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersistentData : MonoBehaviour {

	[HideInInspector]
	public Player[] Players;

	void Awake()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}

	public void SaveSettings(Player[] players)
	{
		this.Players = players;
	}

}
