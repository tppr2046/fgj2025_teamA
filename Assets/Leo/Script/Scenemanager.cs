using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
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
    private void Start()
    {

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
    public void LoadCredicScene()
    { 
        StartCoroutine(loadCredicScene());
    }

    IEnumerator loadCredicScene()
    {
        yield return SceneManager.LoadSceneAsync(3);
        Button BTN = GameObject.Find("ExitBTN").GetComponent<Button>();
        if (BTN != null)
            BTN.onClick.AddListener(LoadMenuScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
