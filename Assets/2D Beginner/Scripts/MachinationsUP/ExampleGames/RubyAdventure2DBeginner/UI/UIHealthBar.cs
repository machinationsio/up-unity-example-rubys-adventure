using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class take care of scaling the UI image that is used as a health bar, based on the ratio sent to it.
/// It is a singleton so it can be called from anywhere (e.g. PlayerController SetHealth)
/// </summary>
public class UIHealthBar : MonoBehaviour
{

    public static UIHealthBar Instance { get; private set; }

    public Image bar;
    public Text healthText;

    float originalSize;

    // Use this for initialization
    void Awake ()
    {
        Instance = this;
    }

    void OnEnable ()
    {
        originalSize = bar.rectTransform.rect.width;
    }

    public void SetPercentValueOf1 (float value)
    {
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetNumericValue (int value)
    {
        healthText.text = value.ToString();
    }

}
