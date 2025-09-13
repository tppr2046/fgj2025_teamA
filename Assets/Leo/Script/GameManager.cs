
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Character")]
    public List<People> people;
    public List<People> Friend;
    public boss101 Boss;
    [Header("Level")]
    public List<ContentLevel> levels = new List<ContentLevel>();
    public ContentLevel CurrentLevel;
    public int CurrentLevelCount;
    public int CurrentLevelAnswerIndex;
    public string CurrentLevelAnswer;
    [Header("Option")]
    public int StartFriendCount;
    public int TalkLimit;
    public int FriendLimit;
    public int WinFriend;
    private void Awake()
    {

    }
    void Start()
    {
        Initial();
    }
    public void NextRound()
    {
        int CurrentAnswersIndex = 0;
        int CurrentFaultAnswersIndex = 0;

        CurrentLevelCount++;
        CurrentLevel = levels[CurrentLevelCount];
        Boss.Talk(CurrentLevel);
        CurrentLevelAnswerIndex = 0;
        CurrentLevelAnswer = CurrentLevel.Answers[CurrentLevelAnswerIndex];

        List<People> list = new List<People>(Friend);

        for (int i = 0; i < TalkLimit; i++)
        {
            if(list.Count==0) break;
            int ran = Random.Range(0, list.Count);
            if (CurrentAnswersIndex < CurrentLevel.Answers.Length)
            {
                list[ran].Talking(CurrentLevel.Answers[CurrentAnswersIndex]);
                CurrentAnswersIndex++;
            }
            else if(CurrentFaultAnswersIndex< CurrentLevel.WrongAnswers.Length)
            {
                list[ran].Talking(CurrentLevel.WrongAnswers[CurrentFaultAnswersIndex]);
                CurrentFaultAnswersIndex++;
            }
                list.RemoveAt(ran);
        }
    }
    public void WinRound()
    {
        List<People> list = new List<People>(people);

        for (int i = 0; i < WinFriend; i++)
        {
            int ran = Random.Range(0, people.Count);
            people[ran].ChangeType(Type.Friend);
            Friend.Add(people[ran]);
            people.RemoveAt(ran);
        }
    }
    public void LostRound()
    {
        if (Friend.Count > 5)
        {
            int ran = Random.Range(0, Friend.Count);
            Friend[ran].ChangeType(Type.None);
            people.Add(Friend[ran]);
            Friend.RemoveAt(ran);
        }
    }
    public void ApplyAnswer(string Answer)
    {
        if (Answer == CurrentLevelAnswer)
        {
            Debug.Log("字對了");
            if (CurrentLevelAnswerIndex < CurrentLevel.Answers.Length-1)
            {
                CurrentLevelAnswerIndex++;
                CurrentLevelAnswer = CurrentLevel.Answers[CurrentLevelAnswerIndex];
            }
            else 
            {
                Debug.Log("通關");
                NextRound();
            }
            //eturn true;
        }
        else Debug.Log("字錯了");
        //return false;
    }
    public void Initial()
    {
        for (int i = 0; i < StartFriendCount; i++)
        {
            int ran = Random.Range(0, people.Count);
            people[ran].ChangeType(Type.Friend);
            Friend.Add(people[ran]);
            people.Remove(people[ran]);
        }
    }
}
