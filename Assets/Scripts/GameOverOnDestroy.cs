using UnityEngine;

public class GameOverOnDestroy : MonoBehaviour
{
    private bool _isShuttingDown = false;

    void OnDestroy()
    {
        if (!_isShuttingDown && !Application.isLoadingLevel)
        {
            // Terrible to use a string!
            Application.LoadLevel("Title");
        }
    }

    void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }
}
