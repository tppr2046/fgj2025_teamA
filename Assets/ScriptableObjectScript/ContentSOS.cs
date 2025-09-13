using UnityEngine;

[CreateAssetMenu(fileName = "ContentSOS", menuName = "Scriptable Objects/ContentSOS")]
public class ContentLevel : ScriptableObject
{
public bool isQuestion;
public string Content;
public string[] Answers = new string[5] {"","","","",""};
public string[] WrongAnswers = new string[5] {"","","","",""};

}
