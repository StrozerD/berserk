using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
	[SerializeField] private float speed = 3f;            // скорость перса
	[SerializeField] private int lives = 5;               //  жизни перса
	[SerializeField] private float jumpForce = 15f;       // высота прыжка перса
	private bool isGrounded = false;

	private Rigidbody2D rb;
	private SpriteRenderer sprite;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	private void Run()
	{
		Vector3 dir = transform.right * Input.GetAxis("Horizontal");

		transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime); // движение по гориз.
		sprite.flipX = dir.x < 0.0f;   // изменение положения по гориз
	}

	private void FixedUpdate()
	{
		CheckGround();
	}
	private void Update()
	{
		if (Input.GetButton("Horizontal")) Run();
		if (isGrounded && Input.GetButtonDown("Jump")) Jump();
	}

	private void Jump()
	{
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
	}

	private void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
		isGrounded = colliders.Length > 1;
	}



}
