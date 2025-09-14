using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public int score, currentScore;
    public TMP_Text P_T, F_T;
    public Image S_Icon,FillImage;
    public Sprite Sad,Smile;
    public Slider slider;
    public float SmileScore = 0.1f;
    public float targetValue;
    public GameObject G_End, B_End,BackBTN;
    public AudioSource sus_a;
    public bool CanOut;
    
    void Start()
    {
        score = Scenemanager.Score;
        StartCoroutine(co());
    }

    // Update is called once per frame
    void Update()
    {
        if (CanOut && Input.anyKeyDown)
        { 
            Scenemanager.Instance.LoadMenuScene();
        }
    }
    IEnumerator co()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(CountPScoreSmooth());
        yield return StartCoroutine(CountFScoreSmooth());
        StartCoroutine(CountSliderScoreSmooth());
        
    }
    void Jugde()
    {
        if (targetValue > SmileScore)
        {
            G_End.SetActive(true);
            StartCoroutine(S());
        }else B_End.SetActive(true);
        //CanOut = true;
        BackBTN.SetActive(true);
        Scenemanager.Instance.BTNGiver(BackBTN.GetComponent<Button>());
    }
    IEnumerator S()
    {
        sus_a.Play();
        yield return new WaitForSeconds(5);
        sus_a.Stop();
    }
    IEnumerator CountPScoreSmooth()
    {
        float duration = 1f; // 花多久時間跑到目標
        float elapsed = 0f;
        int startScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentScore = (int)Mathf.Lerp(startScore, 45 - score, elapsed / duration);
            P_T.text = "X" + currentScore.ToString();
            yield return null;
        }
        currentScore = 45 - score;
        P_T.text = "X"+currentScore.ToString();
        StartCoroutine(CountFScoreSmooth());
    }
    IEnumerator CountFScoreSmooth()
    {
        float duration = 1f; // 花多久時間跑到目標
        float elapsed = 0f;
        int startScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentScore = (int)Mathf.Lerp(startScore, score, elapsed / duration);
            F_T.text = "X" + currentScore.ToString();
            yield return null;
        }
        currentScore = score;
        F_T.text = "X"+ currentScore.ToString();
    }
    IEnumerator CountSliderScoreSmooth()
    {
        float duration = 2.5f; // 花多久時間跑到目標
        float elapsed = 0f;
        int startScore = 0;

        while (elapsed < duration)
        {
            Debug.Log((float)score / 45);
            Debug.Log(score );
            elapsed += Time.deltaTime;
            targetValue = (float)Mathf.Lerp(startScore, (float)score/45, elapsed / duration);
            slider.value = targetValue;
            if (targetValue > SmileScore)
            {
                S_Icon.sprite = Smile;
                FillImage.color = Color.yellow;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        Jugde();
    }
}
