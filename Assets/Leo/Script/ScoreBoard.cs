using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public int score, currentScore;
    public TMP_Text P_T, F_T;
    void Start()
    {
        score = Scenemanager.Score;
        StartCoroutine(co());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator co()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(CountPScoreSmooth());
        StartCoroutine(CountFScoreSmooth());
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
}
