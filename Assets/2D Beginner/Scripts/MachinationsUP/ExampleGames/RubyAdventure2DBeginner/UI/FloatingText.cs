using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{

    public Animator animator;
    private Text damageText;
    public Vector2 positionToFollow;
    private Canvas targetCanvas;

    public void Initialize (Canvas targetCanvas, Vector2 positionToFollow, string text)
    {
        this.positionToFollow = positionToFollow;
        this.targetCanvas = targetCanvas;
        damageText.text = text;
    }

    void OnEnable ()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clipInfo.Length);
        damageText = animator.GetComponent<Text>();
        Debug.Log("Destroy queued @ " + clipInfo[0].clip.length);
        Destroy(gameObject, clipInfo[0].clip.length);
        Destroy(animator, clipInfo[0].clip.length);
        Destroy(damageText, clipInfo[0].clip.length);
    }

    void Update ()
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(new Vector2(positionToFollow.x, positionToFollow.y + 2));
        damageText.transform.SetParent(targetCanvas.transform, false);
        damageText.rectTransform.position = viewportPoint;
        damageText.rectTransform.anchorMin = viewportPoint;
        damageText.rectTransform.anchorMax = viewportPoint;
        Debug.Log("Damage @ " + viewportPoint);
    }

}
