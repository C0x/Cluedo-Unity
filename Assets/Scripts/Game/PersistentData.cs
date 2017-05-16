using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

///<summary>
/// Object used to store data between StartView & Main
///</summary>
public class PersistentData : MonoBehaviour 
{
	[HideInInspector]
	public Player[] Players;

	void Awake()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}
}
