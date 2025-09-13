using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenemanager : MonoBehaviour
{
    private static Scenemanager instance;
    public static Scenemanager Instance {  get { return instance; } }
    private void Awake()
    {
 
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
