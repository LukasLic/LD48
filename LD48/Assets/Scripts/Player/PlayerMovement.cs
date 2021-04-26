using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public AudioClip jumpAudioClip;
	public AudioSource playerAudioSource;

	[Header("WallDetection")]
	public float wallDetectionOffset = .25f;
	public Vector2 wallDetectionDimensions;

	public PlayerAnimationController animationController;

	[Header("JetpackEffects")]
	public ParticleSystem jetpackParticleSystem;
	public Light jetpackLight;

	public Light miningLight;

	public float speed = 5f;
	public float maxSpeed = 10f;

	public float jumpSpeed = 5f;
	public float jetPackSpeed = 5f;

	public float horizontalDrag = 2f;

	public Vector2 groundCheckPosition;
	public float groundCheckRadius;
	public LayerMask groundMask;

	public LayerMask wallMask;

	public string horizontalInput;
	public KeyCode jumpInput;
	public KeyCode jetpackInput;

	public float maxJetpackFuel;
	public float maxJetpackVelocity = 10f;
	public float refuelSpeed = 2f;
	public float waitBeforeRefuel = 3f;
	private float waitBeforeRefuelTimer = 0f;
	private float currentJetpackFuel;

	//public Animator animator;

	private Rigidbody2D rb;

	
	private float movement;
	private bool jump;
	private bool usingJetpack;

	private bool isGrounded;
	private bool isWaitingToRefuel;

	private bool isFlipped;
	public Transform graphicsTransform;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		currentJetpackFuel = maxJetpackFuel;
	}

	// Update is called once per frame
	void Update()
	{
		movement = Input.GetAxisRaw(horizontalInput) * speed;

		if (Input.GetKeyDown(jumpInput) && isGrounded)
			jump = true;

		// Check for a wall
		if (movement != 0f)
		{
			var offset = (movement > 0f) ? wallDetectionOffset : -wallDetectionOffset;
			Collider2D wallCollider = Physics2D.OverlapBox(
				new Vector2(rb.position.x + offset, rb.position.y),
				wallDetectionDimensions,
				0f,
				wallMask);


			if (wallCollider != null)
            {
                //Debug.Log("Wall");
                movement = 0f;
            }
        }

		if (/*Input.GetKey(jetpackInput) && */Input.GetKey(jumpInput))
		{
			if (currentJetpackFuel > 0f)
			{
				currentJetpackFuel -= Time.deltaTime;
				usingJetpack = true;
			}
		}
		
		if(currentJetpackFuel < maxJetpackFuel)
        {
			if(isGrounded)
            {
				if(isWaitingToRefuel)
                {
					if (waitBeforeRefuelTimer > 0f)
					{
						waitBeforeRefuelTimer -= Time.deltaTime;
					}
					else
					{
						currentJetpackFuel = Mathf.Min(maxJetpackFuel, currentJetpackFuel + Time.deltaTime * refuelSpeed);
					}
				}
				else
                {
					isWaitingToRefuel = true;
					waitBeforeRefuelTimer = waitBeforeRefuel;
				}
			}
			else if(!isGrounded)
            {
				isWaitingToRefuel = false;
			}
        }

		if(isGrounded && waitBeforeRefuelTimer <= 0f)
		{			
			if (currentJetpackFuel < 0f)
			{
				currentJetpackFuel = 0f;
			}

			
		}

		//animator.SetFloat("Speed", Mathf.Abs(movement));

			Collider2D collider = Physics2D.OverlapCircle(rb.position + groundCheckPosition, groundCheckRadius, groundMask);
		if (collider != null)
		{
			if (rb.velocity.y < 0f)
			{
				isGrounded = true;
				//animator.SetBool("IsGrounded", isGrounded);
			}
		}
		else
		{
			isGrounded = false;
			//animator.SetBool("IsGrounded", isGrounded);
		}

		animationController.SetXDirection(movement);
		animationController.SetSpeed(movement > 0f ? 1f : (movement < 0f) ? 1f : 0f);
		animationController.SetGround(isGrounded);
	}

	private void FixedUpdate()
	{
		miningLight.enabled = transform.position.y < -0.5f;

		float t = rb.velocity.x / maxSpeed;

		float lerp = 0f;

		if (t >= 0f)
			lerp = Mathf.Lerp(maxSpeed, 0f, t);
		else if (t < 0f)
			lerp = Mathf.Lerp(maxSpeed, 0f, Mathf.Abs(t));

		Vector2 force = new Vector2(movement * lerp * speed * Time.fixedDeltaTime, 0f);
		Vector2 drag = new Vector2(-rb.velocity.x * horizontalDrag * Time.fixedDeltaTime, 0f);

		rb.AddForce(force, ForceMode2D.Force);
		rb.AddForce(drag, ForceMode2D.Force);

		if (movement >= .1f && isFlipped)
		{
			Vector2 flipScale = new Vector2(-1f, 1f);
			//animator.transform.localScale *= flipScale;
			//transform.localScale *= flipScale;
			graphicsTransform.localScale *= flipScale;
			isFlipped = false;
		}
		else if (movement <= -.1f && !isFlipped)
		{
			Vector2 flipScale = new Vector2(-1f, 1f);
			//animator.transform.localScale *= flipScale;
			//transform.localScale *= flipScale;
			graphicsTransform.localScale *= flipScale;
			isFlipped = true;
		}

		if (jump)
		{
			playerAudioSource.PlayOneShot(jumpAudioClip);
			Vector2 vel = rb.velocity;
			vel.y = jumpSpeed;
			rb.velocity = vel;
			jump = false;
			isGrounded = false;
			//animator.SetBool("IsGrounded", isGrounded);
			//animator.SetTrigger("Jump");
		}


		// Jetpack effects
		if (usingJetpack) { jetpackParticleSystem.Play(); }
		else { jetpackParticleSystem.Stop(); }
		jetpackLight.enabled = usingJetpack;

		if (usingJetpack)
        {
			Vector2 vel = rb.velocity;
			vel.y = Mathf.Min(maxJetpackVelocity, vel.y + jetPackSpeed * Time.deltaTime);
			rb.velocity = vel;
			usingJetpack = false;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosition, groundCheckRadius);
		Gizmos.DrawWireCube(
			new Vector3(transform.position.x + wallDetectionOffset, transform.position.y),
			new Vector3(wallDetectionDimensions.x, wallDetectionDimensions.y));
		Gizmos.DrawWireCube(
			new Vector3(transform.position.x - wallDetectionOffset, transform.position.y),
			new Vector3(wallDetectionDimensions.x, wallDetectionDimensions.y));
	}

	private void OnDisable()
	{
		// Disable animator
		//animator.SetFloat("Speed", 0f);
	}

	// Set from animation
	public void Animation_StopMining()
	{

	}
}
