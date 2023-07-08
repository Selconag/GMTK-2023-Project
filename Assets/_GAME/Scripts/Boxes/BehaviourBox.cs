using SelocanusToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviourBox : Box, IPlayBoxEvents
{
    public enum BehaviourBoxTypes { movingPlatform, musicBox, sizeBox, gravityBox, speedBox, teleportBox, enemySpawn, bounceBox, physicsBox, transformBox, powerUp, cameraShot }

    #region movingPlatform
    public Transform moveTarget;
    public Transform moveBase;
    public Vector3 moveStartPosition;
    public void MovePlatform(Vector3 targetPosition)
    {
        
        moveStartPosition = transform.position;

        transform.position = Vector3.Lerp(moveStartPosition, moveTarget.position, 1);
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
        Vector3 playerScale = Player.Instance.transform.localScale;
        playerScale.y *= -1.0f;

        if (isUpsideDown)
        {
            Physics.gravity = new Vector3(0, -9.81f);
        }
        else
        {
            Physics.gravity = new Vector3(0, 9.81f);
        }

        Player.Instance.transform.localScale = playerScale;
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
    private bool isTeleporting = false;
    private Vector3 teleportTargetArea;
    public float teleportTimeLength = 1.5f;
    public void Teleport(float timeLength, Transform target)
    {
        if (!isTeleporting)
        {
            isTeleporting = true;
            teleportTargetArea = target.position;

            StartCoroutine(TeleportAfterTime(timeLength));
        }
    }

    private System.Collections.IEnumerator TeleportAfterTime(float timeLength)
    {
        yield return new WaitForSeconds(timeLength);

        if (isTeleporting)
        {
            transform.position = teleportTargetArea;
            isTeleporting = false;
            PlayParticleEffect();
        }
    }
    #endregion
    #region enemySpawn
    public int spawnInterval = 10; // The interval in seconds between creature spawns
    public bool isBroken = false; // Indicates if the box is broken
    public Color boxColor; // The color of the box

    public List<GameObject> blueEnemies; // List of blue enemy objects
    public List<GameObject> redEnemies; // List of red enemy objects

    private float timer = 0f; // Timer to track spawn intervals

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the spawn interval has passed and the box is not broken
        if (timer >= spawnInterval && !isBroken )
        {
            SpawnCreature();
            timer = 0f; // Reset the timer
        }
    }

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




    public BehaviourBoxTypes bbt;
    private void OnTriggerEnter(Collider other)
    {
        switch (bbt)
        {
            case BehaviourBoxTypes.movingPlatform:
                MovePlatform(moveTarget.position);
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
                Teleport(teleportTimeLength, teleportTarget);
                break;

            case BehaviourBoxTypes.bounceBox:
                jumpIncreased = true;
                if (other.CompareTag("Player"))
                {
                    IncreaseJumpSpeed(jumpFactor);
                }
                break;
            case BehaviourBoxTypes.physicsBox:
                ActivatePlatforms();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (bbt)
        {
            
            case BehaviourBoxTypes.movingPlatform:
                MovePlatform(moveBase.position);
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
            case BehaviourBoxTypes.teleportBox:
                Teleport(teleportTimeLength, teleportTarget);
                break;
            */
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
