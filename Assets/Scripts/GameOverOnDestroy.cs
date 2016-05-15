using UnityEngine;

public class GameOverOnDestroy : MonoBehaviour
{
    private bool isShuttingDown = false;

    void OnDestroy()
    {
        if (!isShuttingDown && !Application.isLoadingLevel)
        {
            // Terrible to use a string!
            Application.LoadLevel("Title");
        }
    }

    void OnApplicationQuit()
    {
        isShuttingDown = true;
    }
}
