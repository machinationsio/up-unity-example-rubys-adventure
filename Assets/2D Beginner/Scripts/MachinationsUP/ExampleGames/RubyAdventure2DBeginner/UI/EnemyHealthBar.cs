using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    public GameObject panelHealthBarContainer;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text healthText;

    private Vector2 _positionToFollow;
    private Canvas _enemyUICanvas;
    private RectTransform _panelRectTransform;

    public void Init (Vector2 positionToFollow, Canvas enemyUICanvas)
    {
        _positionToFollow = positionToFollow;
        _enemyUICanvas = enemyUICanvas;

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(new Vector2(positionToFollow.x, positionToFollow.y - 0.2f));
        transform.SetParent(EnemySpawner.GetEnemyUICanvas().transform, false);
        transform.position = viewportPoint;

        _panelRectTransform = panelHealthBarContainer.GetComponent<RectTransform>();
        _panelRectTransform.SetParent(enemyUICanvas.transform, false);
        _panelRectTransform.anchorMax = viewportPoint;
        _panelRectTransform.anchorMin = viewportPoint;
    }

    public void UpdatePosition (Vector2 newPosition)
    {
        _positionToFollow = newPosition;
    }

    void Update ()
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(new Vector2(_positionToFollow.x, _positionToFollow.y - 0.2f));
        transform.SetParent(_enemyUICanvas.transform, false);
        transform.position = viewportPoint;
        _panelRectTransform.anchorMax = viewportPoint;
        _panelRectTransform.anchorMin = viewportPoint;
    }

    public void SetMaxHealth (int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
        healthText.text = slider.value + " / " + slider.maxValue;
    }

    public void SetHealth (int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        healthText.text = slider.value + " / " + slider.maxValue;
    }

    /// <summary>
    /// Deletes the Health Bar.
    /// </summary>
    public void Eliminate ()
    {
        Debug.Log("Attempt eliminate");
        Destroy(panelHealthBarContainer);
        Destroy(gameObject);
    }

}
