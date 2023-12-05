using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
	RaycastHit2D[] wallHits = new RaycastHit2D[5];
	RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
	private float groundDistance = 0.05f;
	private float wallDistance = 0.2f;
	private float ceilingDistance = 0.05f;
	Vector2 wallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

	[SerializeField]
	private bool _isGrounded = true;
	public bool isGrounded
    {
        get
        { 
            return _isGrounded;
        }
        set 
        { 
            _isGrounded=value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

	[SerializeField]
	private bool _isOnWall = false;
	public bool isOnWall
	{
		get
		{
			return _isOnWall;
		}
		set
		{
			_isOnWall = value;
			animator.SetBool(AnimationStrings.isOnWall, value);
		}
	}

	[SerializeField]
	private bool _isOnCeiling = true;
	public bool isOnCeiling
	{
		get
		{
			return _isOnCeiling;
		}
		set
		{
			_isOnCeiling = value;
			animator.SetBool(AnimationStrings.isOnCeiling, value);
		}
	}
	// Start is called before the first frame update
	void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded=touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance)>0;
		isOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
		isOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
	}
}
