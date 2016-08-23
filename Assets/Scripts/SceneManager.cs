using UnityEngine;

public class SceneManager : MonoBehaviour 
{
    public void LoadLevel(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

	public void LoadLevelByIndex(int index)
	{
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
