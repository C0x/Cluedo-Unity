using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour {

	public List<CanvasGroup> HideablePanels;

	private PersistentData persistentData;

	public void onValueChanged(int index)
	{
		Debug.Log("Value changed to: " + index);

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

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Back()
	{
		Debug.Log("Back");
		Destroy(GameObject.Find("PersistentData"));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

}
