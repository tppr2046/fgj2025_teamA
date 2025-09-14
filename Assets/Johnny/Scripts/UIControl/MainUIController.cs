using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Texture2D[] nonePic;
    [SerializeField] Texture2D[] friendPic;
    [SerializeField] Texture2D[] actorPic;
    [SerializeField] Color normalColor;
    [SerializeField] Color RightColor;
    [SerializeField] float actionTime = 5;
    [SerializeField] float rightTime = 3;
    [SerializeField] float wrongTime = 3;
    [SerializeField] float noAnswerTime = 3;
    [SerializeField] float movingLabelTime = 1;
    [SerializeField] float audHighFreqTime = 0.1f;
    [SerializeField] UnityEvent OnTalk;
    [SerializeField] ParticleSystem sucessParticle;
    [SerializeField] AudioSource successAudio;
    [SerializeField] AudioSource failureAudio;

    private bool _canPress = false;
    private bool _audHigh = false;
    private bool[] _startMove = new bool[5];
    private Vector2[] _moveSpeed = new Vector2[5];
    private float[] _moveStartTime = new float[5];
    private float _actionStartTime = 0;
    private float _audStartTime = 0;
    private int _currentPress;
    private int _maxPress;
    private string[] _answerString = new string[5];
    private List<int> _canPressList = new List<int>();
    private List<int> _infectPoepleList = new List<int>();

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
    Label[] MovingLabels = new Label[5];

    //更新People狀態
    public void UpdateFriend(List<People> peopleList) //List people
    {
        _infectPoepleList.Clear();

        for (int i = 0; i < peopleList.Count; i++)
        {
            _infectPoepleList.Add(peopleList[i].ID); //這裡改成People List的ID
        }

        int rndNumber = 0;

        for (int i = 0; i < 45; i++)
        {
            rndNumber = UnityEngine.Random.Range(0, 3);

            if (_infectPoepleList.Contains(i)) AudButtons[i].style.backgroundImage = friendPic[rndNumber];
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

        if (OnTalk != null) OnTalk.Invoke();

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
        _canPressList.Clear();

        int rndNumber = 0;

        for (int i = 0; i < peopleList.Count; i++)
        {
            actID = peopleList[i].ID;
            actText = peopleList[i].Talk;
            _canPressList.Add(actID);

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
        CountDownBar.style.visibility = Visibility.Hidden;

        successAudio.Play();
        sucessParticle.Play();

        _audStartTime = 0;
        _audHigh = true;
        //播放成功動畫

        StartCoroutine(Right());
    }

    private void JudgeWrong()
    {
        _canPress = false;

        failureAudio.Play();

        CountDownBar.style.visibility = Visibility.Hidden;
        StartCoroutine(Wrong());
    }

    private void ClickActor(ClickEvent clickEvent)
    {
        if (_canPress == false) return;

        var panel = clickEvent.target as Button;
        int pressID = Int32.Parse(panel.text);

        if (!_canPressList.Contains(pressID)) return;

        string pressString = AudLabels[pressID].text;
        string correctString = _answerString[_currentPress];

        _canPressList.Remove(pressID);
        AudLabels[pressID].style.visibility = Visibility.Hidden;

        if (pressString == correctString)
        {
            //播放正確字串移動
            int xLeft = ((pressID + 1) % 15) - 1;
            if (xLeft == -1) xLeft = 14;
            int yLeft = Mathf.FloorToInt(pressID / 15);

            Vector2 startPos = new Vector2(1 + xLeft * 6.5f, 41 + yLeft * 20);
            _moveSpeed[_currentPress] = (new Vector2(46.5f + _currentPress * 10.875f, 29) - startPos) / movingLabelTime;

            MovingLabels[_currentPress].style.left = Length.Percent(startPos.x);
            MovingLabels[_currentPress].style.top = Length.Percent(startPos.y);
            MovingLabels[_currentPress].text = pressString;
            MovingLabels[_currentPress].style.visibility = Visibility.Visible;
           
            _moveStartTime[_currentPress] = 0;
            _startMove[_currentPress] = true;
            
            _currentPress++;
            if (_currentPress >= _maxPress) JudgeRight();
        }
        else JudgeWrong();
    }

    private void ActorPointed(MouseEnterEvent mouseEnterEvent)
    {
        if (_canPress == false) return;

        var panel = mouseEnterEvent.target as Button;
        int pressID = Int32.Parse(panel.text);

        if (!_canPressList.Contains(pressID)) return;

        panel.style.scale = new Vector2(1.2f, 1.2f);

    }

    private void ActorLeave(MouseLeaveEvent mouseLeaveEvent)
    {
        var panel = mouseLeaveEvent.target as Button;
        int pressID = Int32.Parse(panel.text);

        panel.style.scale = new Vector2(1, 1);
    }

    private void Awake()
    {
        //初始設定
        _canPress = false;
        _audHigh = false;

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

            MovingLabels[i] = rootVisualElement.Q<Label>("MovingLabel" + i.ToString());
            MovingLabels[i].text = "";
            MovingLabels[i].style.visibility = Visibility.Hidden;
            _startMove[i] = false;

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
            AudButtons[i].RegisterCallback<MouseEnterEvent>(ActorPointed);
            AudButtons[i].RegisterCallback<MouseLeaveEvent>(ActorLeave);
        }
    }

    private void Update()
    {


        if (_audHigh)
        {
            _audStartTime += Time.deltaTime;

            if (_audStartTime >= audHighFreqTime)
            {
                int rndNum = 0;

                for (int i = 0; i < _infectPoepleList.Count; i++)
                {
                    rndNum = UnityEngine.Random.Range(0, 3);
                    AudButtons[_infectPoepleList[i]].style.backgroundImage = friendPic[rndNum];
                }

                _audStartTime = 0;
            }
        }

        if (_canPress == false) return;

        _actionStartTime += Time.deltaTime;

        float barRatio = 1 - _actionStartTime / actionTime;

        if (_actionStartTime >= actionTime) barRatio = 0;

        CountDownBar.style.width = Length.Percent(barRatio * 100);

        if (_actionStartTime >= actionTime) JudgeWrong();
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            if (_startMove[i])
            {
                MovingLabels[i].style.left = Length.Percent(MovingLabels[i].style.left.value.value + _moveSpeed[i].x * Time.deltaTime);
                MovingLabels[i].style.top = Length.Percent(MovingLabels[i].style.top.value.value + _moveSpeed[i].y * Time.deltaTime);

                _moveStartTime[i] += Time.deltaTime;

                if (_moveStartTime[i] >= movingLabelTime)
                {
                    _startMove[i] = false;
                    MovingLabels[i].style.visibility = Visibility.Hidden;
                    ActionLabels[i].style.color = RightColor;
                }

            }
        }
    }

    IEnumerator Right()
    {
        yield return new WaitForSeconds(rightTime);
        _audHigh = false;
        successAudio.Stop();
        gameManager.WinRound();
    }
    IEnumerator Wrong()
    {
        ShowLabel.text = "失敗!";
        ShowLabel.style.visibility = Visibility.Visible;
        yield return new WaitForSeconds(wrongTime);
        ShowLabel.style.visibility = Visibility.Hidden;
        failureAudio.Stop();
        gameManager.LostRound();

    }
    IEnumerator None()
    {
        yield return new WaitForSeconds(noAnswerTime);
        gameManager.NextRound();
    }
}
