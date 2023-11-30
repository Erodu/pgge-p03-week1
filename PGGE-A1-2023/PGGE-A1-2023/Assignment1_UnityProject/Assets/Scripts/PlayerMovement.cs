using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;

    public float mWalkSpeed = 1.5f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = false;
    public float mTurnRate = 10.0f;

    public AudioSource mAudioSource;
    public AudioClip[] defaultFootsteps;
    public AudioClip[] grassFootsteps;
    public AudioClip[] woodFootsteps;

    public LayerMask groundLayer;

#if UNITY_ANDROID
    public FixedJoystick mJoystick;
#endif

    private float hInput;
    private float vInput;
    private float speed;
    private bool jump = false;
    private bool crouch = false;
    public float mGravity = -30.0f;
    public float mJumpHeight = 1.0f;

    private Vector3 mVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    private float lerpedValue = 0f;

    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    public void HandleInputs()
    {
        // We shall handle our inputs here.
    #if UNITY_STANDALONE
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    #endif

    #if UNITY_ANDROID
        hInput = 2.0f * mJoystick.Horizontal;
        vInput = 2.0f * mJoystick.Vertical;
    #endif

        speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
            // Doing this lerp allows for the walking and running animations to transition between each other more smoothly.
            lerpedValue = Mathf.Lerp(lerpedValue, 0.9f, 0.05f);
        }
        else
        {
            lerpedValue = Mathf.Lerp(lerpedValue, vInput * 0.5f, 0.05f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            //Debug.Log(mCharacterController.isGrounded);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            crouch = !crouch;
            Crouch();
        }
    }

    public void Move()
    {
        if (crouch) return;

        // We shall apply movement to the game object here.
        if (mAnimator == null) return;
        if (mFollowCameraForward)
        {
            // rotate Player towards the camera forward.
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;

        mCharacterController.Move(forward * vInput * speed * Time.deltaTime);
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", lerpedValue);

        if(jump)
        {
            Jump();
            jump = false;
        }

        mCharacterController.Move(mVelocity * Time.deltaTime);

    }

    void Jump()
    {
        mAnimator.SetTrigger("Jump");
        mVelocity.y += Mathf.Sqrt(mJumpHeight * -2f * mGravity);
        //Debug.Log(jump);
    }

    private Vector3 HalfHeight;
    private Vector3 tempHeight;
    void Crouch()
    {
        mAnimator.SetBool("Crouch", crouch);
        if(crouch)
        {
            tempHeight = CameraConstants.CameraPositionOffset;
            HalfHeight = tempHeight;
            HalfHeight.y *= 0.5f;
            CameraConstants.CameraPositionOffset = HalfHeight;
        }
        else
        {
            CameraConstants.CameraPositionOffset = tempHeight;
        }
    }

    void ApplyGravity()
    {
        // apply gravity.
        mVelocity.y += mGravity * Time.deltaTime;
        // Apply a small downward force to the Y vector to make sure that mCharacterController.IsGrounded() returns true while grounded.
        if (mCharacterController.isGrounded && mVelocity.y < 0) mVelocity.y = -0.01f;

    }

    void Footsteps()
    {
        // Assign concrete footstep sounds by default.
        AudioClip[] footsteps = defaultFootsteps;

        RaycastHit hit;

        //Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, groundLayer))
        {

            // Depending on what the surface is tagged with, assign a different array of audio clips.
            //Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Grass"))
            {
                //Debug.Log("Detected Grass");
                footsteps = grassFootsteps;
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                //Debug.Log("Detected Wood");
                footsteps = woodFootsteps;
            }
        }
        // Randomly select an audio clip from the assigned array.
        AudioClip playedSound = footsteps[Random.Range(0, footsteps.Length)];
        mAudioSource.clip = playedSound;
        mAudioSource.PlayOneShot(playedSound);
    }

}
