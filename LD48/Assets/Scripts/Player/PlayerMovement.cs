using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	public float maxSpeed = 10f;

	public float jumpSpeed = 5f;

	public float horizontalDrag = 2f;

	public Vector2 groundCheckPosition;
	public float groundCheckRadius;
	public LayerMask groundMask;

	public LayerMask wallMask;

	public string horizontalInput;
	public string jumpInput;

	//public Animator animator;

	Rigidbody2D rb;

	private float movement;
	private bool jump;

	private bool isGrounded;

	private bool isFlipped;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		movement = Input.GetAxisRaw(horizontalInput) * speed;
		if (Input.GetButtonDown(jumpInput) && isGrounded)
			jump = true;

		// Check for a wall
		if (movement != 0f)
		{
			var offset = (movement > 0f) ? 0.25f : -0.25f;
			Collider2D wallCollider = Physics2D.OverlapBox(
				new Vector2(rb.position.x + offset, rb.position.y),
				new Vector2(0.15f, 0.6f),
				0f,
				wallMask);

			if (wallCollider != null)
			{
				Debug.Log("Wall");
				movement = 0f;
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
	}

	private void FixedUpdate()
	{
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
			transform.localScale *= flipScale;
			isFlipped = false;
		}
		else if (movement <= -.1f && !isFlipped)
		{
			Vector2 flipScale = new Vector2(-1f, 1f);
			//animator.transform.localScale *= flipScale;
			transform.localScale *= flipScale;
			isFlipped = true;
		}

		if (jump)
		{
			Vector2 vel = rb.velocity;
			vel.y = jumpSpeed;
			rb.velocity = vel;
			jump = false;
			isGrounded = false;
			//animator.SetBool("IsGrounded", isGrounded);
			//animator.SetTrigger("Jump");
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosition, groundCheckRadius);
		Gizmos.DrawWireCube(
			new Vector3(transform.position.x + 0.25f, transform.position.y),
			new Vector3(0.15f, 0.6f));
		Gizmos.DrawWireCube(
			new Vector3(transform.position.x - 0.25f, transform.position.y),
			new Vector3(0.15f, 0.6f));
	}

	private void OnDisable()
	{
		// Disable animator
		//animator.SetFloat("Speed", 0f);
	}
}
