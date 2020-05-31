using System;
using System.Collections;
using System.Collections.Generic;
using MachinationsUP.ExampleGames.RubyAdventure2DBeginner;
using MachinationsUP.Integration.Elements;
using MachinationsUP.Integration.GameObject;
using MachinationsUP.Integration.Inventory;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Spawns additional enemies.
/// </summary>
public class EnemySpawner : MonoBehaviour
{

    //Machinations.

    private MachinationsGameObject _mgo;

    //Tracked Machinations Elements.
    private const string M_EXTRA_ENEMIES = "ExtraEnemies";

    //Manifest.
    static readonly private MachinationsGameObjectManifest _manifest = new MachinationsGameObjectManifest
    {
        GameObjectName = "EnemySpawner",
        PropertiesToSync = new List<DiagramMapping>
        {
            new DiagramMapping
            {
                GameObjectPropertyName = M_EXTRA_ENEMIES,
                DiagramElementID = 23,
                DefaultElementBase = new ElementBase(5)
            }
        },
    };

    /// <summary>
    /// Used to store spawn location for Enemies.
    /// </summary>
    struct Point
    {

        public double x, y;

        public Point (double px, double py)
        {
            x = px;
            y = py;
        }

    }

    /// <summary>
    /// Enemy Prefab to use.
    /// </summary>
    public GameObject enemyPrefab;

    /// <summary>
    /// Positions where to spawn enemies at.
    /// </summary>
    readonly private List<Point> _positions = new List<Point>();

    /// <summary>
    /// Canvas used for Enemy UI.
    /// </summary>
    public Canvas enemyUICanvas;

    /// <summary>
    /// Allow access to this Object from other areas of the game.
    /// </summary>
    public static EnemySpawner Instance { get; private set; }

    void Awake ()
    {
        Instance = this;
        //Initialize MachinationsGameObject.
        _mgo = new MachinationsGameObject(_manifest, OnBindersUpdated);
        //Initialize spawn locations. HARDCODED.
        _positions.Add(new Point(-1.491717, -2.536745));
        _positions.Add(new Point(4.97679, -4.343874));
        _positions.Add(new Point(-3.805256, 2.498485));
    }

    /// <summary>
    /// The MGL will notify this class when Binders concerning it are updated.
    /// </summary>
    private void OnBindersUpdated (object sender, EventArgs e)
    {
        SpawnEnemies();
    }

    void Start ()
    {
        //Already set via Unity Editor.
        //enemyPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/2D Beginner/Prefabs/Enemy.prefab", typeof(GameObject));
        FloatingTextController.Initialize(enemyUICanvas);
    }

    static public Canvas GetEnemyUICanvas ()
    {
        return Instance.enemyUICanvas;
    }

    /// <summary>
    /// Spawn the needed number of Enemies.
    /// </summary>
    public void SpawnEnemies ()
    {
        int i = 0;
        //Cannot spawn more than we have _positions.
        foreach (Point pnt in _positions)
        {
            //Only spawn up to the number retrieved from Machinations, minus 3 (which are already spawned).
            if (i++ >= _mgo[M_EXTRA_ENEMIES].Value - 3) break;

            Vector2 newEnemyPosition = new Vector2((float) pnt.x, (float) pnt.y);
            Instantiate(enemyPrefab, newEnemyPosition + Vector2.up * 0.5f, Quaternion.identity);
        }
    }

}
