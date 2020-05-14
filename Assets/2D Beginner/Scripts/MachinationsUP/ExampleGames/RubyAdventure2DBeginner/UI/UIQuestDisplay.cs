using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class take care of showing and updating the list of quest.
/// It is a singleton so it can be accessed from anywhere (e.g. the QuestManager)
/// </summary>
public class UIQuestDisplay : MonoBehaviour
{
    public static UIQuestDisplay Instance { get; private set; }
    
    public Text questText;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void SetText(string text)
    {
        questText.text = text;
    }
}
