using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;

public class boss101 : MonoBehaviour
{   
 

    public void NextRound()
    {

    }
    public void Talk(ContentLevel contentLevel)
    {
        Debug.Log(contentLevel.Content);
    }
    public void OnButtonClick()
    {
        Debug.Log("你的夢想是什麼");
        // Add your desired actions here
    }
}

