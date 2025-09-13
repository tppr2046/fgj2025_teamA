using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    [SerializeField] Texture2D nonePic;
    [SerializeField] Texture2D friendPic;
    [SerializeField] Texture2D actorPic;
    [SerializeField] Color normalColor;
    [SerializeField] Color RightColor;
    [SerializeField] float actionTime = 5;

    private bool _canPress = false;
    private float _actionStartTime = 0;
    private int _currentPress;
    private int _maxPress;

    //UI����
    VisualElement rootVisualElement;
    Label MainTextLabel;
    Label[] ActionLabels = new Label[5];
    Label[] ActionDashs = new Label[4];
    VisualElement CountDownBar;
    VisualElement[] AudPanels = new VisualElement[45];
    Label[] AudLabels = new Label[45];
    Button[] AudButtons = new Button[45];

    //��sPeople���A
    public void UpdateFriend() //List people
    {
        List<int> idList = new List<int>();

        int friendNum = 5; //�o�̧令People List��Count��

        for (int i = 0; i < friendNum; i++)
        {
            idList.Add(i); //�o�̧令People List��ID
        }

        for (int i = 0; i < 45; i++)
        {
            if (idList.Contains(i)) AudButtons[i].style.backgroundImage = friendPic;
            else AudButtons[i].style.backgroundImage = nonePic;

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
    }

    public void ActionStart(string textSHow) //List people 5�ӤH �B���װ}�C
    {
        MainTextLabel.text = textSHow;

        _maxPress = 0;  //���װ}�CCount
        _currentPress = 0;

        for (int i = 0; i < 5; i++) 
        {
            if (i < _maxPress) 
            {
                ActionLabels[i].text = ""; //���װ}�C��r
                ActionLabels[i].style.color = normalColor;
                ActionLabels[i].style.visibility = Visibility.Visible;
            }
            else ActionLabels[i].style.visibility = Visibility.Hidden;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < _maxPress - 1) ActionDashs[i].style.visibility = Visibility.Hidden;
            else ActionDashs[i].style.visibility = Visibility.Hidden;
        }

        int actID = 0;
        string actText = "";

        for (int i = 0; i < 5; i++)
        {
            actID = 0; //People List ID 
            actText = ""; //People List String 

            AudButtons[actID].style.backgroundImage = actorPic;
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


    }

    private void JudgeWrong()
    {
        _canPress = false;

        //���񥢱ѭ���

    }

    private void TimesUp()
    {
        _canPress = false;
    }

    private void Awake()
    {
        //��l�]�w
        _canPress = false;

        //���wUI���� & ��l��ܳ]�w
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
        }

        //Button���w



    }

    private void Update()
    {
        if (_canPress == false) return;

        _actionStartTime += Time.deltaTime;

        float barRatio = 1 - _actionStartTime / actionTime;

        if (_actionStartTime >= actionTime) barRatio = 0;

        CountDownBar.style.width = Length.Percent(barRatio * 100);

        if (_actionStartTime >= actionTime) TimesUp();
    }

}
