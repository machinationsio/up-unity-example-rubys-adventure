using System.Collections.Generic;
using MachinationsUP.Engines.Unity;
using MachinationsUP.ExampleGames.MachinationsSupport.Events;
using MachinationsUP.GameEngineAPI.Game;
using MachinationsUP.GameEngineAPI.States;
using MachinationsUP.Integration.Binder;
using MachinationsUP.Integration.Elements;
using MachinationsUP.Integration.GameObject;
using MachinationsUP.Integration.Inventory;
using UnityEngine;

namespace MachinationsUP.ExampleGames.RubyAdventure2DBeginner
{
    /// <summary>
    /// Handle the projectile launched by the player to fix the robots.
    /// </summary>
    public class Projectile : MonoBehaviour
    {

        //Machinations.
        private MachinationsGameAwareObject _mgao;

        //Tracked Machinations Elements.
        private const string M_DAMAGE = "Damage";

        private ElementBinder damageBinder;

        //Manifest.
        static readonly private MachinationsGameObjectManifest _manifest = new MachinationsGameObjectManifest
        {
            GameObjectName = "Projectile",
            PropertiesToSync = new List<DiagramMapping>
            {
                new DiagramMapping
                {
                    GameObjectPropertyName = M_DAMAGE,
                    DiagramElementID = 76,
                    DefaultElementBase = new FormulaElement("50+D60")
                }
            },
            StatesAssociationsPerPropertyName = new Dictionary<string, List<StatesAssociation>>
            {
                {
                    M_DAMAGE, new List<StatesAssociation>
                        {new StatesAssociation("Exploring", new List<GameStates>() {GameStates.Exploring})}
                }
            },
            EventsToEmit = new List<string>
            {
                RubyAdventureGameObjectEvents.PROJECTILE_LAUNCHED,
                RubyAdventureGameObjectEvents.PROJECTILE_HIT
            }
        };

        /// <summary>
        /// This function will be called ONCE per Scene. The MachinationsGameLayer is available
        /// and now is the time to declare this object's manifest, so that the MachinationsGameLayer can initialize.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoadRuntimeMethod ()
        {
            MachinationsGameLayer.DeclareManifest(_manifest);
            UIConsole.Instance.AddTrackedItem(_manifest.GetDiagramMapping(M_DAMAGE));
        }

        Rigidbody2D rigidbody2d;

        public void Awake ()
        {
            //Initialize MachinationsGameObject.
            _mgao = new MachinationsGameAwareObject(_manifest);
            //Get Damage Binder.
            damageBinder = _mgao[M_DAMAGE];

            //TODO: Projectiles perhaps shouldn't instantiate ElementBase.
            //FIRE EVENT ---> CREATED.
            _mgao.OnGameObjectEvent(RubyAdventureGameObjectEvents.PROJECTILE_LAUNCHED);

            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void Update ()
        {
            //destroy the projectile when it reach a distance of 1000.0f from the origin
            if (transform.position.magnitude > 1000.0f)
                Destroy(gameObject);
        }

        //called by the player controller after it instantiate a new projectile to launch it.
        public void Launch (Vector2 direction, float force)
        {
            rigidbody2d.AddForce(direction * force);
        }

        private void OnCollisionEnter2D (Collision2D other)
        {
            Enemy e = other.collider.GetComponent<Enemy>();

            //if the object we touched wasn't an enemy, just destroy the projectile.
            if (e != null)
            {
                _mgao.OnGameObjectEvent(RubyAdventureGameObjectEvents.PROJECTILE_HIT);
                e.Damage(damageBinder.Value);
            }

            Destroy(gameObject);
        }

    }
}
