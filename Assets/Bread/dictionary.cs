using System.Collections.Generic;
using UnityEngine;

public class dictionary : MonoBehaviour
{
    // 靜態字典，存放語句
    public static Dictionary<string, string> Phrases = new Dictionary<string, string>()
    {
        { "greet", "你好！" },
        { "bye", "再見！" },
        { "thanks", "謝謝你！"},
    };

    // 你可以根據需要新增更多語句
    void Start()
    {
        // 範例：在 Unity Console 顯示一個語句
        Debug.Log(Phrases["greet"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
