using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
public class Scenemanager : MonoBehaviour
{
    private static Scenemanager instance;
    public static Scenemanager Instance {  get { return instance; } }
    public static int Score;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
            DontDestroyOnLoad(this);
    }
    public void LoadScoreScene()
    {

        SceneManager.LoadSceneAsync(2);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
