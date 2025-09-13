using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    [SerializeField] Texture2D[] nonePic;
    [SerializeField] Texture2D[] friendPic;
    [SerializeField] Texture2D[] actorPic;
    [SerializeField] Color normalColor;
    [SerializeField] Color RightColor;
    [SerializeField] float actionTime = 5;
    [SerializeField] float rightTime = 3;
    [SerializeField] float wrongTime = 3;
    [SerializeField] float noAnswerTime = 3;
    [SerializeField] GameManager gameManager;

    private bool _canPress = false;
    private float _actionStartTime = 0;
    private int _currentPress;
    private int _maxPress;
    private string[] _answerString = new string[5];

    //UI元素
    VisualElement rootVisualElement;
    Label MainTextLabel;
    Label[] ActionLabels = new Label[5];
    Label[] ActionDashs = new Label[4];
    VisualElement CountDownBar;
    VisualElement[] AudPanels = new VisualElement[45];
    Label[] AudLabels = new Label[45];
    Button[] AudButtons = new Button[45];
    Label ShowLabel;

    //更新People狀態
    public void UpdateFriend(List<People> peopleList) //List people
    {
        List<int> idList = new List<int>();

        int friendNum = peopleList.Count; 

        for (int i = 0; i < friendNum; i++)
        {
            idList.Add(peopleList[i].ID); //這裡改成People List的ID
        }

        int rndNumber = 0;

        for (int i = 0; i < 45; i++)
        {
            rndNumber = UnityEngine.Random.Range(0, 3);

            if (idList.Contains(i)) AudButtons[i].style.backgroundImage = friendPic[rndNumber];
            else AudButtons[i].style.backgroundImage = nonePic[rndNumber];

            AudLabels[i].text = "";
            AudLabels[i].style.visibility = Visibility.Hidden;
        }
    }

    public void OnlyUpdateText(string textSHow)
    {
        MainTextLabel.text = textSHow;

        for (int i = 0; i < 5; i++)
        {
            ActionLabels[i].style.visibility = Visibility.Hidden;

            if (i < 4)
            {
                ActionDashs[i].style.visibility = Visibility.Hidden;
            }
        }

        StartCoroutine(None());
    }

    public void ActionStart(string textSHow, List<People> peopleList, string[] answers) 
    {
        MainTextLabel.text = textSHow;

        _maxPress = answers.Length; 
        _currentPress = 0;

        for (int i = 0; i < 5; i++) 
        {
            if (i < _maxPress) 
            {
                ActionLabels[i].text = answers[i];
                ActionLabels[i].style.color = normalColor;
                ActionLabels[i].style.visibility = Visibility.Visible;
                _answerString[i] = answers[i];
            }
            else ActionLabels[i].style.visibility = Visibility.Hidden;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < _maxPress - 1) ActionDashs[i].style.visibility = Visibility.Visible;
            else ActionDashs[i].style.visibility = Visibility.Hidden;
        }

        int actID = 0;
        string actText = "";

        int rndNumber = 0;

        for (int i = 0; i < peopleList.Count; i++)
        {
            actID = peopleList[i].ID;
            actText = peopleList[i].Talk;

            rndNumber = UnityEngine.Random.Range(0, 3);

            AudButtons[actID].style.backgroundImage = actorPic[rndNumber];
            AudLabels[actID].text = actText;
            AudLabels[actID].style.visibility = Visibility.Visible;
        }

        CountDownBar.style.width = Length.Percent(100);
        CountDownBar.style.visibility = Visibility.Visible;

        _canPress = true;
        _actionStartTime = 0;
    }

    private void JudgeRight()
    {
        _canPress = false;
        
        //播放成功動畫
        CountDownBar.style.visibility = Visibility.Hidden;
        StartCoroutine(Right());
    }

    private void JudgeWrong()
    {
        _canPress = false;
        //播放失敗動畫
        CountDownBar.style.visibility = Visibility.Hidden;
        StartCoroutine(Wrong());
    }

    private void ClickActor(ClickEvent clickEvent)
    {
        if (_canPress==false) return;
        var panel = clickEvent.target as Button;
        int pressID = Int32.Parse(panel.text);

        string pressString = AudLabels[pressID].text;
        string correctString = _answerString[_currentPress];

        if (pressString == correctString)
        {
            //播放正確字串移動

            ActionLabels[_currentPress].style.color = RightColor;
            _currentPress++;
            if (_currentPress >= _maxPress) JudgeRight();
        }
        else JudgeWrong();
    }

    private void Awake()
    {
        //初始設定
        _canPress = false;

        //指定UI元素 & 初始顯示設定

        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        ShowLabel = rootVisualElement.Q<Label>("ShowLabel");
        ShowLabel.style.visibility = Visibility.Hidden;
        MainTextLabel = rootVisualElement.Q<Label>("MainTextLabel");
        MainTextLabel.text = "";

        for (int i = 0; i < 5; i++)
        {
            ActionLabels[i] = rootVisualElement.Q<Label>("ResLabel" + i.ToString());
            ActionLabels[i].text = "";
            ActionLabels[i].style.visibility = Visibility.Hidden;

            if (i < 4)
            {
                ActionDashs[i] = rootVisualElement.Q<Label>("DashLabel" + i.ToString());
                ActionDashs[i].style.visibility = Visibility.Hidden;
            }
        }

        CountDownBar = rootVisualElement.Q<VisualElement>("CountDownBar");
        CountDownBar.style.visibility = Visibility.Hidden;

        for (int i = 0; i < 45; i++)
        {
            AudPanels[i] = rootVisualElement.Q<VisualElement>("AudPanel" + i.ToString());
            AudLabels[i] = AudPanels[i].Q<Label>("AudLabel");
            AudLabels[i].text = "";
            AudLabels[i].style.visibility = Visibility.Hidden;
            AudButtons[i] = AudPanels[i].Q<Button>("AudButton");
            AudButtons[i].text = i.ToString();
            AudButtons[i].RegisterCallback<ClickEvent>(ClickActor);
        }
    }

    private void Update()
    {
        if (_canPress == false) return;

        _actionStartTime += Time.deltaTime;

        float barRatio = 1 - _actionStartTime / actionTime;

        if (_actionStartTime >= actionTime) barRatio = 0;

        CountDownBar.style.width = Length.Percent(barRatio * 100);

        if (_actionStartTime >= actionTime) JudgeWrong();
    }
    IEnumerator Right()
    {
        ShowLabel.text = "成功!";
        ShowLabel.style.visibility = Visibility.Visible;
        yield return new WaitForSeconds(rightTime);
        ShowLabel.style.visibility = Visibility.Hidden;
        gameManager.WinRound();

    }
    IEnumerator Wrong()
    {
        ShowLabel.text = "失敗!";
        ShowLabel.style.visibility = Visibility.Visible;
        yield return new WaitForSeconds(wrongTime);
        ShowLabel.style.visibility = Visibility.Hidden;
        gameManager.LostRound();

    }
    IEnumerator None()
    {
        yield return new WaitForSeconds(noAnswerTime);
        gameManager.NextRound();
    }
}
