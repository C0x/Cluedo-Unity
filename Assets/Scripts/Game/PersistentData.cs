using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {

	

	void Awake()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}

}
