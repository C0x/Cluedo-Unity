using UnityEngine.SceneManagement;
using UnityEngine;

public class Credits : MonoBehaviour {

	public void Back()
	{
		SceneManager.LoadScene(0); //Go back to MainMenu (should be at buildindex 0)
	}

}
