using System;
using System.Collections.Generic;
using MachinationsUP.Engines.Unity;
using MachinationsUP.GameEngineAPI.Game;
using MachinationsUP.GameEngineAPI.States;
using MachinationsUP.Integration.Elements;
using MachinationsUP.Integration.GameObject;
using MachinationsUP.Integration.Inventory;
using UnityEngine;

namespace MachinationsUP.ExampleGames.RubyAdventure2DBeginner
{
    public class RubyController : MonoBehaviour
    {

        //Machinations.
        private MachinationsGameAwareObject _mgao;

        //Tracked Machinations Elements.
        private const string M_HEALTH = "Health";

        private const string M_SPEED = "Speed";

        //Manifest.
        static readonly private MachinationsGameObjectManifest _manifest = new MachinationsGameObjectManifest
        {
            GameObjectName = "Ruby",
            PropertiesToSync = new List<DiagramMapping>
            {
                new DiagramMapping
                {
                    GameObjectPropertyName = M_HEALTH,
                    DiagramElementID = 19,
                    DefaultElementBase = new ElementBase(105)
                },
                new DiagramMapping
                {
                    GameObjectPropertyName = M_SPEED,
                    DiagramElementID = 102,
                    DefaultElementBase = new ElementBase(25)
                }
            },
            CommonStatesAssociations = new List<StatesAssociation>
            {
                new StatesAssociation("Exploring", new List<GameStates>() {GameStates.Exploring})
            }
        };

        // ======== HEALTH ==========
        public float timeInvincible = 2.0f;
        public Transform respawnPosition;
        public ParticleSystem hitParticle;

        // ======== PROJECTILE ==========
        public GameObject projectilePrefab;

        // ======== AUDIO ==========
        public AudioClip hitSound;
        public AudioClip shootingSound;

        // =========== MOVEMENT ==============
        Rigidbody2D rigidbody2d;

        // ======== HEALTH ==========
        float invincibleTimer;
        bool isInvincible;

        // ==== ANIMATION =====
        Animator animator;
        Vector2 lookDirection = new Vector2(1, 0);

        // ================= SOUNDS =======================
        AudioSource audioSource;

        public void Awake ()
        {
            //Initialize MachinationsGameObject.
            _mgao = new MachinationsGameAwareObject(_manifest);
        }

        public void Start ()
        {
            // =========== MOVEMENT ==============
            rigidbody2d = GetComponent<Rigidbody2D>();

            // ======== HEALTH ==========
            invincibleTimer = -1.0f;

            // ==== ANIMATION =====
            animator = GetComponent<Animator>();

            // ==== AUDIO =====
            audioSource = GetComponent<AudioSource>();

            //Ask the debug console to keep track of this item.
            UIConsole.Instance.AddTrackedItem(_manifest.GetDiagramMapping(M_SPEED));
            UIConsole.Instance.AddTrackedItem(_manifest.GetDiagramMapping(M_HEALTH));
            //Update Health bar with the proper value.
            UIHealthBar.Instance.SetNumericValue(_mgao[M_HEALTH].Value);
            //Subscribe to data notifications from MGL so that we can update UI elements.
            _mgao.OnBindersUpdated += BindersUpdated;
        }

        private void BindersUpdated (object sender, EventArgs e)
        {
            UIHealthBar.Instance.SetNumericValue(_mgao[M_HEALTH].Value);
        }

        public void Update ()
        {
            //Game is paused.
            if (Time.timeScale == 0) return;

            // ================= HEALTH ====================
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }

            // ============== MOVEMENT ======================
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            Vector2 position = rigidbody2d.position;
            position = position + move * (_mgao[M_SPEED].Value * Time.deltaTime);
            rigidbody2d.MovePosition(position);

            // ============== ANIMATION =======================
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            // ============== PROJECTILE ======================
            if (Input.GetKeyDown(KeyCode.C))
                LaunchProjectile();

            // ======== DIALOGUE ==========
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f,
                    1 << LayerMask.NameToLayer("NPC"));
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }

        // ===================== HEALTH ==================
        public void ChangeHealth (int amount)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;

                isInvincible = true;
                invincibleTimer = timeInvincible;

                animator.SetTrigger("Hit");
                audioSource.PlayOneShot(hitSound);

                Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

            _mgao[M_HEALTH].ChangeValueTo
                (Mathf.Clamp(_mgao[M_HEALTH].Value + amount, 0, _mgao[M_HEALTH].BaseValue));

            Debug.Log("Ruby Health changed by: " + amount + ". Health left: " + _mgao[M_HEALTH].Value);

            if (_mgao[M_HEALTH].Value == 0)
                Respawn();

            UIHealthBar.Instance.SetPercentValueOf1(_mgao[M_HEALTH].Value / (float) _mgao[M_HEALTH].BaseValue);
            UIHealthBar.Instance.SetNumericValue(_mgao[M_HEALTH].Value);
        }

        void Respawn ()
        {
            ChangeHealth(_mgao[M_HEALTH].BaseValue);
            transform.position = respawnPosition.position;
        }

        // =============== PROJECTICLE ========================
        void LaunchProjectile ()
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");
            audioSource.PlayOneShot(shootingSound);
        }

        // =============== SOUND ==========================

        //Allow to play a sound on the player sound source. used by Collectible
        public void PlaySound (AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

    }
}
