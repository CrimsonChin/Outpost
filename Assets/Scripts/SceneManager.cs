using UnityEngine;

public class SceneManager : MonoBehaviour 
{
    public void LoadLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }

	public void LoadLevelByIndex(int index)
	{
		Application.LoadLevel(index);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
