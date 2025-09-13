using UnityEngine;
using UnityEngine.UI;


public enum Type
{
    None,
    Friend,
    NotFriend
}
public class People : MonoBehaviour
{
    public int ID;
    public Type type;
    public string Talk;
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
        //UpdateSkin();
    }
    public void ChangeType(Type _type)
    {
        type = _type;
        UpdateSkin();
    }
    public void Talking(string content)
    {
        Debug.Log(gameObject + ": "+ content);
        Talk = content;
    }
    void UpdateSkin()
    {
        switch (type)
        {
            case Type.None:
              
                image.color = Color.white;
                break;
            case Type.Friend:
               
                image.color = Color.red;
                break;
            case Type.NotFriend:
                
                image.color = Color.blue;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
