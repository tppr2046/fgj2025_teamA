
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Character")]
    public List<People> people;
    public List<People> Friend;
    public boss101 Boss;
    [Header("Level")]
    public ListContentSOS listContentSOS;
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
    [SerializeField] MainUIController mainUIController;
    private void Awake()
    {

    }
    void Start()
    {
        levels = new List<ContentLevel>(listContentSOS.ContentLevel);
        Initial();
        NextRound();
    }
    public void NextRound()
    {
        int CurrentAnswersIndex = 0;
        int CurrentFaultAnswersIndex = 0;

        CurrentLevelCount++;
        CurrentLevel = levels[CurrentLevelCount];
        Boss.Talk(CurrentLevel);

        if (CurrentLevel.isQuestion)
        {
            CurrentLevelAnswerIndex = 0;
            CurrentLevelAnswer = CurrentLevel.Answers[CurrentLevelAnswerIndex];

            List<People> list = new List<People>(Friend);
            List<People> Templist = new List<People>();

            Templist.Clear();

            for (int i = 0; i < TalkLimit; i++)
            {
                if (list.Count == 0) break;
                int ran = Random.Range(0, list.Count);
                if (CurrentAnswersIndex < CurrentLevel.Answers.Length)
                {
                    list[ran].Talking(CurrentLevel.Answers[CurrentAnswersIndex]);
                    CurrentAnswersIndex++;
                    Templist.Add(list[ran]);
                }
                else if (CurrentFaultAnswersIndex < CurrentLevel.WrongAnswers.Length)
                {
                    list[ran].Talking(CurrentLevel.WrongAnswers[CurrentFaultAnswersIndex]);
                    CurrentFaultAnswersIndex++;
                    Templist.Add(list[ran]);
                }
                list.RemoveAt(ran);
            }
            mainUIController.ActionStart(CurrentLevel.Content, Templist, CurrentLevel.Answers);


        }
        else mainUIController.OnlyUpdateText(CurrentLevel.Content);
    }
    public void WinRound()
    {
        List<People> list = new List<People>(people);

        for (int i = 0; i < WinFriend; i++)
        {
            if (people.Count == 0) break;
            int ran = Random.Range(0, people.Count);
            people[ran].ChangeType(Type.Friend);
            Friend.Add(people[ran]);
            people.RemoveAt(ran);
        }
        mainUIController.UpdateFriend(Friend);
        NextRound();
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
        mainUIController.UpdateFriend(Friend);
        NextRound();
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
        mainUIController.UpdateFriend(Friend);
    }
}
