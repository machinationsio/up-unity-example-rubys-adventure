using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class hold a reference to the text to update when the ammo count change.
/// It is a singleton so it can be called from anywhere (e.g. PlayerController SetAmmo)
/// </summary>
public class UIAmmoCount : MonoBehaviour 
{
	public static UIAmmoCount Instance { get; private set;}

	public Text countText;
	
	// Use this for initialization
	void Awake ()
	{
		Instance = this;
	}

	public void SetAmmo(int count, int max)
	{
		countText.text = "x" + count + "/" + max;
	}
}
