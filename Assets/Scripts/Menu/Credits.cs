using UnityEngine.SceneManagement;
using UnityEngine;

///<summary>
/// Eventhandlers for CreditsScene
///</summary>
public class Credits : MonoBehaviour 
{
	public void Back()
	{
		SceneManager.LoadScene(0); //Go back to MainMenu (should be at buildindex 0)
	}
}
