using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    
 */


public class BehaviourBox : Box, IPlayParticle
{
    public enum BehaviourBoxTypes { movingPlatform, musicBox, sizeBox, gravityBox, speedBox, teleportBox, enemySpawn, bounceBox, physicsBox, transformBox, powerUp, cameraShot }

    #region movingPlatform
    public Transform moveTarget;
    public Vector3 moveStartPosition;
    public void MovePlatform(Vector3 targetPosition)
    {
        moveStartPosition = transform.position;
        transform.position = Vector3.Lerp(moveStartPosition, moveTarget.position, 1);
    }
    #endregion

    #region sizeBox
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
    private Transform targetArea;

    public void Teleport(float timeLength, Transform target)
    {
        if (!isTeleporting)
        {
            isTeleporting = true;
            targetArea = target;

            StartCoroutine(TeleportAfterTime(timeLength));
        }
    }

    private System.Collections.IEnumerator TeleportAfterTime(float timeLength)
    {
        yield return new WaitForSeconds(timeLength);

        if (isTeleporting)
        {
            transform.position = targetArea.position;
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

    private void OnTriggerEnter(Collider other)
    {
        jumpIncreased = true;
        if (other.CompareTag("Player"))
        {
            IncreaseJumpSpeed(jumpFactor);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (jumpIncreased == true && other.CompareTag("Player"))
        {
            IncreaseJumpSpeed(1 / jumpFactor);
            jumpIncreased = false;
        }
    }
    #endregion

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
}
