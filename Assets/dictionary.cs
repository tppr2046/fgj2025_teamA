using System.Collections.Generic;
using UnityEngine;

public class dictionary : MonoBehaviour
{
    // �R�A�r��A�s��y�y
    public static Dictionary<string, string> Phrases = new Dictionary<string, string>()
    {
        { "greet", "�A�n�I" },
        { "bye", "�A���I" },
        { "thanks", "���§A�I"},
    };

    // �A�i�H�ھڻݭn�s�W��h�y�y
    void Start()
    {
        // �d�ҡG�b Unity Console ��ܤ@�ӻy�y
        Debug.Log(Phrases["greet"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
