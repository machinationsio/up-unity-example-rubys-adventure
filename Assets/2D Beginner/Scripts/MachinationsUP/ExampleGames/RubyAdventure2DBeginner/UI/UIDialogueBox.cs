using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class take care of updating the dialogue text, hiding/showing the dialogue box and changing the portrait.
/// It is a singleton so the text/portrait can be changed from anywhere.
/// </summary>
public class UIDialogueBox : MonoBehaviour
{
	public static UIDialogueBox Instance { get; private set; }
	
	public Image portrait;
	public TextMeshProUGUI text;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		//disable on start, will be shown/hidden by the dialogue triggers.
		gameObject.SetActive(false);	
	}


	public void DisplayText(string content)
	{
		text.text = content;
	}

	public void DisplayPortrait(Sprite spr)
	{
		portrait.sprite = spr;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}
	
	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
