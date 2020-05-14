using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{

    private static FloatingText floatingTextPrefab;
    private static Canvas enemyUICanvas;

    public static void Initialize (Canvas enemyUICanvas)
    {
        FloatingTextController.enemyUICanvas = enemyUICanvas;
        if (!floatingTextPrefab)
            floatingTextPrefab = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
    }

    public static void CreateFloatingText (string text, Vector2 positionToFollow)
    {
        FloatingText floatingText = Instantiate(floatingTextPrefab);
        floatingText.Initialize(enemyUICanvas, positionToFollow, text);

        Vector2 screenPosition = Camera.main.WorldToViewportPoint(new Vector2(positionToFollow.x, positionToFollow.y + 2));
        floatingText.transform.SetParent(enemyUICanvas.transform, false);
        floatingText.transform.position = screenPosition;
    }

    public static EnemyHealthBar CreateHealthBar (EnemyHealthBar healthBarPrefab, int baseHealth, int maxHealth, Vector2 positionToFollow)
    {
        EnemyHealthBar theRealBar = Instantiate(healthBarPrefab);
        theRealBar.SetMaxHealth(baseHealth);
        theRealBar.SetHealth(maxHealth);
        theRealBar.Init(positionToFollow, EnemySpawner.GetEnemyUICanvas());
        Debug.Log("Enemy start @ " + positionToFollow);

        Vector2 screenPosition = Camera.main.WorldToViewportPoint(new Vector2(positionToFollow.x , positionToFollow.y - 2));
        theRealBar.transform.SetParent(EnemySpawner.GetEnemyUICanvas().transform, false);
        theRealBar.transform.position = screenPosition;

        return theRealBar;
    }

}
