using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    [SerializeField] Texture2D nonePic;
    [SerializeField] Texture2D friendPic;
    [SerializeField] Texture2D actorPic;

    private bool canPress = false;

    //UI元素
    VisualElement rootVisualElement;
    Label MainTextLabel;
    Label[] ActionLabels = new Label[5];
    Label[] ActionDashs = new Label[4];
    VisualElement CountDownBar;
    VisualElement[] AudPanels = new VisualElement[45];
    Label[] AudLabels = new Label[45];
    Button[] AudButtons = new Button[45];

    public void InitialSetup() //List<> audData
    {
        for (int i = 0; i < 45; i++)
        {
            //是Friend
            AudButtons[i].style.backgroundImage = friendPic;

            //不是Friend
            AudButtons[i].style.backgroundImage = nonePic;
        }
    }







    private void Awake()
    {
        //初始設定
        canPress = false;

        //指定UI元素 & 初始顯示設定
        MainTextLabel = rootVisualElement.Q<Label>("MainTextLabel");

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
    }
}
