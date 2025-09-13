using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class gugugaga : MonoBehaviour
{
    string greet = dictionary.Phrases["greet"];

    public void OnButtonClick()
    {
        Debug.Log(greet);
        // Add your desired actions here
    }
}