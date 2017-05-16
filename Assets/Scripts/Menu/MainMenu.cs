using UnityEngine.SceneManagement;
using UnityEngine;

///<summary>
/// Eventhandler for MainMenuScene
///</summary>
public class MainMenu : MonoBehaviour 
{
	public void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
	}

	public void Credits()
	{
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1); //Credits should be last scene in build settings
	}

	public void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
