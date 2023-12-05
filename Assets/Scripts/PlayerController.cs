using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
	public float runSpeed = 7f;
	public float airSpeed = 3f;
	public float jumpImpulse = 10f;
	public bool isMoving
	{
		get
		{
			return _isMoving;
		}
		set 
		{ 
			_isMoving = value;
			animator.SetBool(AnimationStrings.isMoving, _isMoving);
		}
	}

	public bool isRunning
	{
		get
		{
			return _isRunning;
		}
		set
		{
			_isRunning = value;
			animator.SetBool(AnimationStrings.isRunning, _isRunning);
		}
	}

	public float currentMoveSpeed
	{
		get
		{
			if(isMoving && !touchingDirections.isOnWall&&canMove)
			{
				if(touchingDirections.isGrounded)
				{
					if (isRunning)
					{
						return runSpeed;
					}
					else
					{
						return walkSpeed;
					}
				}
				else
				{
					return airSpeed;
				}
            }
			else { return 0; }
        }
	}

	public bool isFacingRight
	{
		get
		{
			return _isFacingRight;
		}
		set
		{
			if(_isFacingRight!=value)
			{
				transform.localScale *= new Vector2(-1, 1);
			}
			_isFacingRight = value;
		}
	}

	public bool canMove
	{
		get
		{
			return animator.GetBool(AnimationStrings.canMove);
		}
	}
	private bool _isMoving = false;
	private bool _isRunning = false;
	private bool _isFacingRight = true;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private Animator animator;
	TouchingDirections touchingDirections;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		touchingDirections=GetComponent<TouchingDirections>();
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
		animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
	}

	public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = moveInput != Vector2.zero;
		SetFacingDirection(moveInput);
    }

	private void SetFacingDirection(Vector2 moveInput)
	{
		if(moveInput.x>0 && !isFacingRight)
		{
			isFacingRight = true;
		}
		else if(moveInput.x<0 && isFacingRight)
		{
			isFacingRight= false;
		}
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			isRunning = true;
		}
		else if(context.canceled)
		{
			isRunning = false;
		}
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		if(context.started&&touchingDirections.isGrounded&&canMove)
		{
			animator.SetTrigger(AnimationStrings.jumpTrigger);
			rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
		}
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			animator.SetTrigger(AnimationStrings.attackTrigger);
		}
	}
}
