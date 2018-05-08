using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public const int MainMenu = 0;
    public const int Level1 = 1;
    public const int Level2 = 2;
    public const int Level3 = 3;
    public int currentLevel = 0;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void NextScene()
    {
        if(currentLevel < 3)
        {
            currentLevel++;
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ChangeScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
