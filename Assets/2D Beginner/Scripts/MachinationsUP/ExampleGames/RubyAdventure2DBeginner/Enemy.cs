using System;
using System.Collections.Generic;
using MachinationsUP.Integration.Elements;
using MachinationsUP.Integration.GameObject;
using MachinationsUP.Integration.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace MachinationsUP.ExampleGames.RubyAdventure2DBeginner
{
    /// <summary>
    /// This class handles Enemy behaviour. It will make them walk back & forth as long as they aren't fixed, and then just dance
    /// without being able to interact with the player anymore once fixed.
    /// </summary>
    public class Enemy : MonoBehaviour
    {

        //Machinations.

        private MachinationsGameObject _mgo;

        //Tracked Machinations Elements.
        private const string M_HEALTH = "Health";

        //Manifest.
        static readonly private MachinationsGameObjectManifest _manifest = new MachinationsGameObjectManifest
        {
            GameObjectName = "Enemy",
            PropertiesToSync = new List<DiagramMapping>
            {
                new DiagramMapping
                {
                    GameObjectPropertyName = M_HEALTH,
                    DiagramElementID = 74,
                    DefaultElementBase = new ElementBase(200)
                }
            },
        };

        private Guid enemyGUID;

        //Health Bar.
        public EnemyHealthBar healthBarPrefab;
        private EnemyHealthBar theRealBar;

        // ====== ENEMY MOVEMENT ========
        public float speed;
        public float timeToChange;
        public bool horizontal;

        public float timeToFlick = 1;
        float remainingTimeToFlick;
        private bool flicked = false;

        public GameObject smokeParticleEffect;
        public ParticleSystem fixedParticleEffect;

        public AudioClip hitSound;
        public AudioClip fixedSound;

        Rigidbody2D rigidbody2d;
        float remainingTimeToChange;
        Vector2 direction = Vector2.right;
        bool repaired;

        // ===== ANIMATION ========
        Animator animator;

        // ================= SOUNDS =======================
        AudioSource audioSource;

        public void Awake ()
        {
            enemyGUID = Guid.NewGuid();
            theRealBar = Instantiate(healthBarPrefab);
            //TODO: it's a hack to store a Canvas for Enemy UI inside EnemySpawner.
            theRealBar.Init(transform.position, EnemySpawner.GetEnemyUICanvas());

            //Initialize MachinationsGameObject.
            _mgo = new MachinationsGameObject(_manifest, OnBindersUpdated);
        }

        public void Start ()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            remainingTimeToChange = timeToChange;
            remainingTimeToFlick = timeToFlick;

            direction = horizontal ? Vector2.right : Vector2.down;

            animator = GetComponent<Animator>();

            audioSource = GetComponent<AudioSource>();

            UIConsole.Instance.AddTrackedItem(_manifest.GetDiagramMapping(M_HEALTH));
        }

        private void OnBindersUpdated (object sender, EventArgs e)
        {
            MachinationsGameObject mgo = (MachinationsGameObject) sender;
            //Debug.Log("Enemy Health set to " + mgo[M_HEALTH].BaseValue);
            //Debug.Log("Health Bar Hash Code for Enemy " + enemyGUID + " is " + theRealBar.slider.GetHashCode());
            theRealBar.SetMaxHealth(mgo[M_HEALTH].BaseValue);
            theRealBar.SetHealth(mgo[M_HEALTH].Value);
        }

        private bool iza;

        public void Update ()
        {
            if (repaired)
                return;

            remainingTimeToChange -= Time.deltaTime;
            remainingTimeToFlick -= Time.deltaTime;

            if (remainingTimeToChange <= 0)
            {
                remainingTimeToChange += timeToChange;
                direction *= -1;
            }

            //These robots can go stealth!
            if (remainingTimeToFlick <= 0)
            {
                remainingTimeToFlick += timeToFlick;
                flicked = !flicked;
                gameObject.GetComponent<Renderer>().enabled = flicked;
                gameObject.GetComponent<BoxCollider2D>().enabled = flicked;
            }

            rigidbody2d.MovePosition(rigidbody2d.position + direction * (speed * Time.deltaTime));

            animator.SetFloat("ForwardX", direction.x);
            animator.SetFloat("ForwardY", direction.y);

            theRealBar.UpdatePosition(transform.position);
        }

        void OnCollisionStay2D (Collision2D other)
        {
            if (repaired)
                return;

            RubyController controller = other.collider.GetComponent<RubyController>();

            if (controller != null)
                controller.ChangeHealth(-1);
        }

        public void Damage (int damage)
        {
            _mgo[M_HEALTH].ChangeValueWith(-damage);

            FloatingTextController.CreateFloatingText(damage.ToString(), transform.position);
            theRealBar.SetHealth(_mgo[M_HEALTH].Value);
            Debug.Log("Health Bar Hash Code for Enemy " + enemyGUID + " is " + theRealBar.slider.GetHashCode());

            Debug.Log("Damage taken: " + damage + ". Health left: " + _mgo[M_HEALTH].Value);
            if (_mgo[M_HEALTH].Value < 0)
            {
                animator.SetTrigger("Fixed");
                repaired = true;
                theRealBar.Eliminate();

                smokeParticleEffect.SetActive(false);

                Instantiate(fixedParticleEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);

                //we don't want that enemy to react to the player or bullet anymore, remove its reigidbody from the simulation
                rigidbody2d.simulated = false;

                audioSource.Stop();
                audioSource.PlayOneShot(hitSound);
                audioSource.PlayOneShot(fixedSound);
            }
        }

    }
}
