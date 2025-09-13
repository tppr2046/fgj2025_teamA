using UnityEngine;

[CreateAssetMenu(fileName = "ContentSOS", menuName = "Scriptable Objects/ContentSOS")]
public class NewScriptableObjectScript : ScriptableObject
{

public string Content;
public string[] Answers = new string[5] {"","","","",""};
}
