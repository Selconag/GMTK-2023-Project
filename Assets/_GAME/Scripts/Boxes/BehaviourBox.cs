using SelocanusToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BehaviourBox : Box, IPlayBoxEvents
{
    public enum BehaviourBoxTypes { movingPlatform, musicBox, sizeBox, gravityBox, speedBox, teleportBox, enemySpawn, bounceBox, physicsBox, transformBox, powerUp, cameraShot, doorBox, levelEndBox }

    public bool IsTriggered;
    private Transform PlayerTransform;

    private void Update()
    {
        if (!IsTriggered) { return; }

        switch (bbt)
        {
            case BehaviourBoxTypes.movingPlatform:
                if (m_IsMoving)
                    if (m_MoveTimer < 1)
                    {
                         MovePlatform();
                    }
                    else
                    {
                        PlayerTransform.SetParent(null);
                        PlayerTransform.localScale = Vector3.one;
                        m_IsMoving = false;
                    }
                break;
            case BehaviourBoxTypes.teleportBox:
                if (isTeleporting && !teleported) teleportTimer += Time.deltaTime;

                if (teleportTimer > teleportTimeLength)
                {
                    NewTeleport(teleportTarget);
                    isTeleporting = false;
                    teleportTimer = 0;
                }
                break;

            case BehaviourBoxTypes.enemySpawn:
                // Increment the timer
                timer += Time.deltaTime;

                // Check if the spawn interval has passed and the box is not broken
                if (timer >= spawnInterval && !isBroken)
                {
                    SpawnCreature();
                    timer = 0f; // Reset the timer
                }
                break;
        }
    }

    #region movingPlatform
    public Transform MoveTarget;
    public Transform MoveBase;
    public Transform CarryZone;
    public Vector3 MoveStartPosition;
    private float m_MoveTimer;
    private bool m_IsMoving;
    public void MovePlatform()
    {
        m_MoveTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(MoveBase.position, MoveTarget.position, m_MoveTimer);
    }
    #endregion

    #region musicBox
    public AudioSource AudioSource;

    public void PlayMusic(AudioClip musicClip)
    {
        if (AudioSource != null)
        {        
            AudioSource.Stop();
            AudioSource.clip = musicClip;
            AudioSource.Play();
        }
    }
    #endregion

    #region sizeBox
    public int sizeFactor = 2;
        public void EnlargePlayer(int sizeFactor)
        {
            Player.Instance.transform.localScale *= sizeFactor;
            PlayParticleEffect();
        }
    #endregion

    #region gravityBox
    public bool isUpsideDown = false;

    public void ReverseGravity()
    {
        //Vector3 playerScale = Player.Instance.transform.localScale;
        //playerScale.y *= -1.0f;
        Physics.gravity *= -1;
        //Player.Instance.transform.rotation

        //if (isUpsideDown)
        //{
        //    Physics.gravity = new Vector3(0, -9.81f);
        //}
        //else
        //{
        //    Physics.gravity = new Vector3(0, 9.81f);
        //}

        //Player.Instance.transform.localScale = playerScale;
    }
    #endregion

    #region speedBox
    private bool isBoosted = false;
    private float baseSpeed;
    private float boostedSpeed;
    private float boostTimeRemaining;
    public float speedTimeLength = 10.0f;
    public float speedFactor = 2.0f;

    public void BoostSpeed(float timeLength, float speedCofactor)
    {
        if (!isBoosted)
        {
            isBoosted = true;
            baseSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            boostedSpeed = baseSpeed * speedCofactor;
            boostTimeRemaining = timeLength;

            // Start a coroutine to return the speed to the base after the time is up
            StartCoroutine(ResetSpeedAfterTime());
        }
    }

    private System.Collections.IEnumerator ResetSpeedAfterTime()
    {
        yield return new WaitForSeconds(boostTimeRemaining);
        isBoosted = false;
        boostedSpeed = baseSpeed;
    }

    #endregion

    #region teleportBox
    public Transform teleportTarget;
    [SerializeField] private bool isTeleporting = false;
    private Vector3 teleportTargetArea;
    public float teleportTimeLength = 1.5f;
    private float teleportTimer = 0f;
    private float ttL;
    public bool teleported;

    public void NewTeleport(Transform target)
    {
        teleportTargetArea = target.position + Vector3.up;
        PlayerTransform.GetComponent<CharacterController>().enabled = false;
        PlayerTransform.position = teleportTargetArea;
        PlayerTransform.GetComponent<CharacterController>().enabled = true;
        target.GetComponent<BehaviourBox>().teleported = true;
    }
    #endregion

    #region enemySpawn
    public int spawnInterval = 10; // The interval in seconds between creature spawns
    public bool isBroken = false; // Indicates if the box is broken
    public Color boxColor; // The color of the box

    public List<GameObject> blueEnemies; // List of blue enemy objects
    public List<GameObject> redEnemies; // List of red enemy objects

    private float timer = 0f; // Timer to track spawn intervals


    #region Creature Events

    private void SpawnCreature()
    {
        // Spawn a creature based on the box color
        if (boxColor == Color.red)
        {
            SpawnRedCreature();
        }
        else if (boxColor == Color.blue)
        {
            SpawnBlueCreature();
        }
        else
        {
            Debug.LogError("Wrong Color baby!");
        }
    }

    private void SpawnRedCreature()
    {
        
        Debug.Log("Spawning red creature");
       
        redEnemies.Add(new GameObject()); // Replace with actual spawn logic
    }

    private void SpawnBlueCreature()
    {
        
        Debug.Log("Spawning blue creature");
        
        blueEnemies.Add(new GameObject()); // Replace with actual spawn logic
    }
    #endregion

    public void BreakBox()
    {
        
        foreach (GameObject blueEnemy in blueEnemies)
        {
            Destroy(blueEnemy);
        }
        blueEnemies.Clear();

        
        foreach (GameObject redEnemy in redEnemies)
        {
            Destroy(redEnemy);
        }
        redEnemies.Clear();

        // Break the box, preventing further creature spawns
        isBroken = true;
        Debug.Log("Box is broken");
        PlayParticleEffect();
    }
    #endregion

    #region BounceBox
    public bool jumpIncreased = false;
    public float jumpFactor = 2.0f;
    public void IncreaseJumpSpeed(float factor)
    {
        // Get the current velocity of the player
         
        float jumpForce = Player.Instance.m_ActiveJumpSpeed;

        // Multiply the y component of the velocity by the factor
        jumpForce *= factor;

        // Update the player's velocity
        Player.Instance.m_ActiveJumpSpeed = jumpForce;
        PlayParticleEffect();
    }


    #endregion

    #region physicsBox
    public List<Transform> platforms = new List<Transform>();

    public float fallDelay = 1f;
    public float riseDelay = 10f;
    public float fallSpeed = 2f;
    public float riseSpeed = 1f;

    public Color startColor = Color.green;
    public Color endColor = Color.red;

    private Vector3[] originalPositions;
    private Renderer[] platformRenderers;

    public void ActivatePlatforms()
    {
        StartCoroutine(FallAndRisePlatforms());
    }

    private IEnumerator FallAndRisePlatforms()
    {
        // Store the original positions of the platforms
        originalPositions = new Vector3[platforms.Count];
        for (int i = 0; i < platforms.Count; i++)
        {
            originalPositions[i] = platforms[i].position;
        }

        // Get the renderers of the platforms
        platformRenderers = new Renderer[platforms.Count];
        for (int i = 0; i < platforms.Count; i++)
        {
            platformRenderers[i] = platforms[i].GetComponent<Renderer>();
        }

        // Change the color of the platforms to startColor
        SetPlatformColors(startColor);

        // Wait for 1-2 seconds
        float waitTime = Random.Range(1f, 2f);
        yield return new WaitForSeconds(waitTime);

        // Make the platforms fall down
        for (int i = 0; i < platforms.Count; i++)
        {
            yield return new WaitForSeconds(fallDelay);
            StartCoroutine(MovePlatform(platforms[i], originalPositions[i], fallSpeed, true));
        }

        // Wait for 10 seconds
        yield return new WaitForSeconds(riseDelay);

        // Make the platforms rise back up
        for (int i = 0; i < platforms.Count; i++)
        {
            StartCoroutine(MovePlatform(platforms[i], originalPositions[i], riseSpeed, false));
        }

        // Change the color of the platforms to endColor
        SetPlatformColors(endColor);
    }

    private IEnumerator MovePlatform(Transform platform, Vector3 targetPosition, float speed, bool isFalling)
    {
        Vector3 startPosition = platform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            platform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // If falling, disable any physics interactions
        if (isFalling)
        {
            Rigidbody platformRigidbody = platform.GetComponent<Rigidbody>();
            if (platformRigidbody != null)
            {
                platformRigidbody.isKinematic = false;
            }
        }
    }

    private void SetPlatformColors(Color color)
    {
        for (int i = 0; i < platformRenderers.Length; i++)
        {
            platformRenderers[i].material.color = color;
        }
    }
    #endregion

    #region transformBox

    public GameObject playerMesh;
    public GameObject triggerBox;
    public float maxRollingTime = 30f;
    public int maxEnemiesHit = 3;

    private bool isRolling = false;
    private int enemiesHit = 0;

    private float rollingTimer = 0f;
    private Color transformStartColor = Color.red;
    private Color rollingColor = Color.green;
    private Renderer triggerRenderer;

    private CharacterController characterController;

    private void Start()
    {
        characterController = Player.Instance.Controller;
    }

    public void StartRolling()
    {
        if (!isRolling)
        {
            isRolling = true;
            rollingTimer = 0f;
            enemiesHit = 0;

            // Change trigger box color to rollingColor
            SetTriggerBoxColor(rollingColor);

            // Convert player mesh into a ball
            ConvertToBall(true);

            StartCoroutine(RollingTimer());
        }
    }

    private IEnumerator RollingTimer()
    {
        while (rollingTimer < maxRollingTime && enemiesHit < maxEnemiesHit)
        {
            rollingTimer += Time.deltaTime;
            yield return null;
        }

        // End of rolling, change trigger box color to startColor
        SetTriggerBoxColor(transformStartColor);

        // Convert player mesh back to normal
        ConvertToBall(false);
    }

    private void SetTriggerBoxColor(Color color)
    {
        if (triggerRenderer == null)
        {
            triggerRenderer = triggerBox.GetComponent<Renderer>();
        }

        triggerRenderer.material.color = color;
    }

    private void ConvertToBall(bool isBall)
    {
        playerMesh.SetActive(!isBall);
        characterController.enabled = !isBall;
    }

    #endregion

    #region powerup
    public float smashTimer = 30f;

    private IEnumerator LemmeSmash()
    {
        Player.Instance.CanSmash = true;

        yield return new WaitForSeconds(smashTimer);

        Player.Instance.CanSmash = false;
    }

    #endregion

    #region doorBox

    public List<Transform> Doors;

    #endregion

    public BehaviourBoxTypes bbt;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;

        IsTriggered = true;
        PlayerTransform = other.transform;
        if(m_ActivatedImage != null)
            m_Sprite.sprite = m_ActivatedImage;

        switch (bbt)
        {
            case BehaviourBoxTypes.movingPlatform:
                m_IsMoving = true;
                PlayerTransform = other.transform;
                //Change hierarchy of player to here
                PlayerTransform.SetParent(CarryZone);
                break;
            case BehaviourBoxTypes.musicBox:
                PlayMusic(m_AudioClip);
                break;
            case BehaviourBoxTypes.sizeBox:
                EnlargePlayer(sizeFactor);
                break;

            case BehaviourBoxTypes.gravityBox:
                ReverseGravity();
                break;

            case BehaviourBoxTypes.speedBox:
                BoostSpeed(speedTimeLength, speedFactor);
                break;

            case BehaviourBoxTypes.teleportBox:
                isTeleporting = true;
                PlayParticleEffect();
                //ttL = teleportTimeLength;
                //Teleport(teleportTimeLength, teleportTarget);
                break;

            case BehaviourBoxTypes.bounceBox:
                jumpIncreased = true;
                if (other.transform.tag == "Player")
                {
                    //IncreaseJumpSpeed(jumpFactor);
                    other.transform.GetComponent<Player>().ImmediateJump();
                }
                break;

            case BehaviourBoxTypes.physicsBox:
                ActivatePlatforms();
                break;

            case BehaviourBoxTypes.transformBox:
                if (isRolling && other.CompareTag("Enemy"))
                {
                    // Destroy the enemy
                    Destroy(other.gameObject);

                    // Increment enemies hit count
                    enemiesHit++;

                    if (enemiesHit >= maxEnemiesHit)
                    {
                        // Reached max enemies hit, end the rolling
                        StopCoroutine(RollingTimer());
                        SetTriggerBoxColor(transformStartColor);
                        ConvertToBall(false);
                    }
                }
                break;

            case BehaviourBoxTypes.powerUp:
                StartCoroutine(LemmeSmash());
                break;
                
            case BehaviourBoxTypes.doorBox:
                foreach (var item in Doors)
                {
                    item.transform.GetComponent<Door>().ToggleDoor();
                }
                PlaySoundEffect();
                break;

            case BehaviourBoxTypes.levelEndBox:
                GameManager.Instance.LoadNextScene();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.transform.tag != "Player") return;

        IsTriggered = false;
        PlayerTransform = null;
        teleported = false;
        switch (bbt)
        {
            
            case BehaviourBoxTypes.movingPlatform:
                m_IsMoving = false;
                PlayerTransform = other.transform;
                //Change hierarchy of player to here
                PlayerTransform.SetParent(null);
                PlayerTransform.localScale = Vector3.one;
                break;
            /*
            case BehaviourBoxTypes.sizeBox:
                EnlargePlayer(sizeFactor);
                break;
            case BehaviourBoxTypes.gravityBox:
                ReverseGravity();
                break;
            case BehaviourBoxTypes.speedBox:
                BoostSpeed(speedTimeLength, speedFactor);
                break;
            */
            case BehaviourBoxTypes.teleportBox:
                isTeleporting = false;
                //teleportTimeLength = ttL;
                //CancelTeleport();
                StopParticleEffect();
                break;
            
            case BehaviourBoxTypes.bounceBox:
                if (jumpIncreased == true && other.CompareTag("Player"))
                {
                    IncreaseJumpSpeed(1 / jumpFactor);
                    jumpIncreased = false;
                }
                break;
        }

    }

    //When Switch switches, apply effect here
    public override void ApplyBoxEffect()
    {

    }

    public void PlayParticleEffect()
    {
        if (m_ParticleSystem == null) return;
        m_ParticleSystem.gameObject.SetActive(true);
        m_ParticleSystem.Play();
    }

    public void StopParticleEffect()
    {
        if (m_ParticleSystem == null) return;
        m_ParticleSystem.Stop();
    }

    public void PlaySoundEffect()
    {
        if (m_AudioClip == null) return;
        AudioManager.Instance.PlayCustomSoundSound(m_AudioClip);
    }

    public void PlayAnimationEffect()
    {
        if (m_AnimClip == null) return;
        Player.Instance.Animator.Play(m_AnimClip.ToString());
    }

    public void PlayLightEffect()
    {
        if (m_ChangeLight)
        {
            m_ChangeLight.intensity = 0.5f;
        }

    }
}
