using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersistentData : MonoBehaviour {

	public Dropdown Dropdown;
	public List<InputField> PlayerNameFields;
	public List<Toggle> IsCpuToggles;

	[HideInInspector]
	public List<string> PlayerNames;

	void Awake()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}

	public int GetNumberOfPlayers()
	{
		return PlayerNames.Count;
	}

}
