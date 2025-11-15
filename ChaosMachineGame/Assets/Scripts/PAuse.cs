using UnityEngine;

public class PAuse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
