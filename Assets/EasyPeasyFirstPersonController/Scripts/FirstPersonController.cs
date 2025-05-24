namespace EasyPeasyFirstPersonController
{
    using SmallHedge.SoundManager;
    using System;
    using System.Collections;
    using UnityEngine;

    public partial class FirstPersonController : MonoBehaviour
    {

        public bool canMove = true;
        [Range(0, 100)] public float mouseSensitivity = 25f;
        [Range(0f, 200f)] public float snappiness = 100f;
        public float moveSpeed = 3f;
        public float walkSpeed = 3f;
        public float sprintSpeed = 5f;
        public float crouchSpeed = 1.5f;
        public float crouchHeight = 1f;
        public float crouchCameraHeight = 0.5f;
        public float slideSpeed = 9f;
        public float slideDuration = 0.7f;
        public float slideFovBoost = 5f;
        public float slideTiltAngle = 5f;
        public float gravity = -9.81f;
        public float jumpHeight = 1.5f;
        public float airControl = 0.3f;
        public bool coyoteTimeEnabled = true;
        public float coyoteTimeDuration = 0.2f;
        public float normalFov = 60f;
        public float sprintFov = 70f;
        public float fovChangeSpeed = 5f;
        public float bobAmount = 0.1f;
        public float bobSpeed = 2f;
        public bool canSlide = true;
        public bool canJump = true;
        public bool canSprint = true;
        public bool canCrouch = true;
        public Transform groundCheck;
        public float groundDistance = 0.2f;
        public LayerMask groundMask;
        public Transform playerCamera; // Camera
        public Transform cameraParent; // CameraParent
        private float xRotation;
        private float rotX, rotY;
        private float xVelocity, yVelocity;
        private CharacterController characterController;
        private Vector3 velocity;
        public Vector3 Velocity => velocity;
        private bool isGrounded;
        public bool IsGrounded => isGrounded;
        private Vector2 moveInput;
        private float stepTimer;
        private float stepTime;
        public bool isSprinting;
        public bool isCrouching;
        public bool isSliding;
        private float slideTimer;
        private float postSlideCrouchTimer;
        private Vector3 slideDirection;
        private float originalHeight;
        private float originalCameraParentHeight;
        private float coyoteTimer;
        private Camera cam;
        private AudioSource slideAudioSource;
        private float bobTimer;


        public float boostedSpeed = 10f;
        public float boostDuration = 2f;
        private float originalSpeed;
        private bool isBoosted = false;

        private bool isLook = true, isMove = true;
        public bool IsMove => isMove;
        public bool IsLook => isLook;

        // HeadBob
        public float CurrentCameraHeight => isCrouching || isSliding ? crouchCameraHeight : originalCameraParentHeight;

        float originalWalkSpeed;
        float originalSprintSpeed;
        private bool isSpeedBoosted = false;
        private Coroutine speedBoostCoroutine;
        private void Start()
        {
            originalWalkSpeed = walkSpeed;
            originalSprintSpeed = sprintSpeed;
        }

        private void Awake()
        {
            canMove = true;
            moveSpeed = walkSpeed;
            characterController = GetComponent<CharacterController>();
            cam = playerCamera.GetComponent<Camera>();
            originalHeight = characterController.height;
            originalCameraParentHeight = cameraParent.localPosition.y;
            slideAudioSource = gameObject.AddComponent<AudioSource>();
            slideAudioSource.playOnAwake = false;
            slideAudioSource.loop = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        public void ApplySpeedBoost(float boostMultiplier, float duration)
        {
            if (speedBoostCoroutine != null)
                StopCoroutine(speedBoostCoroutine);

            speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(boostMultiplier, duration));
        }

        private IEnumerator SpeedBoostRoutine(float boostMultiplier, float duration)
        {
            isSpeedBoosted = true;
           
           

            walkSpeed *= boostMultiplier;
            sprintSpeed *= boostMultiplier;
        

            yield return new WaitForSeconds(duration);

            walkSpeed = originalWalkSpeed;
            sprintSpeed = originalSprintSpeed;
       

            isSpeedBoosted = false;
            speedBoostCoroutine = null;
        }


        public void ApplyPermanentSpeedBoost(float boostMultiplier)
        {
            walkSpeed *= boostMultiplier;
            sprintSpeed *= boostMultiplier;

            // Also update moveSpeed if needed to reflect the change immediately
            moveSpeed = walkSpeed;
        }
       
       
        private void Update()
        {
            if (canMove)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                    coyoteTimer = coyoteTimeEnabled ? coyoteTimeDuration : 0f;
                }
                else if (coyoteTimeEnabled)
                {
                    coyoteTimer -= Time.deltaTime;
                }



                moveInput.x = Input.GetAxisRaw("Horizontal");
                moveInput.y = Input.GetAxisRaw("Vertical");
                isSprinting = canSprint && Input.GetKey(KeyCode.LeftShift) && moveInput.y > 0.1f && isGrounded && !isCrouching && !isSliding;

                // Crouching
                bool wantsToCrouch = canCrouch && Input.GetKey(KeyCode.LeftControl) && !isSliding;
                Vector3 point1 = transform.position + characterController.center - Vector3.up * (characterController.height * 0.5f);
                Vector3 point2 = point1 + Vector3.up * characterController.height;
                float capsuleRadius = characterController.radius * 0.95f;
                float castDistance = isSliding ? originalHeight + 0.2f : originalHeight - crouchHeight + 0.2f;
                bool hasCeiling = Physics.CapsuleCast(point1, point2, capsuleRadius, Vector3.up, castDistance, groundMask);
                Debug.DrawLine(point1, point1 + Vector3.up * castDistance, Color.red);
                Debug.DrawLine(point2, point2 + Vector3.up * castDistance, Color.red);
                if (isSliding)
                {
                    postSlideCrouchTimer = 0.1f;
                }
                if (postSlideCrouchTimer > 0)
                {
                    postSlideCrouchTimer -= Time.deltaTime;
                    isCrouching = canCrouch;
                }
                else
                {
                    isCrouching = canCrouch && (wantsToCrouch || (hasCeiling && !isSliding));
                }

                // Sliding
                if (canSlide && isSprinting && Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
                {
                    isSliding = true;
                    slideTimer = slideDuration;
                    slideDirection = moveInput.magnitude > 0.1f ? (transform.right * moveInput.x + transform.forward * moveInput.y).normalized : transform.forward;
                }

                if (isSliding)
                {
                    slideTimer -= Time.deltaTime;
                    if (slideTimer <= 0f || !isGrounded)
                    {
                        isSliding = false;
                    }
                    float slideProgress = slideTimer / slideDuration;
                    moveSpeed = slideSpeed * Mathf.Lerp(0.5f, 1f, slideProgress * slideProgress);
                    characterController.Move(slideDirection * moveSpeed * Time.deltaTime);
                }
                else
                {
                    moveSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);
                    Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
                    if (isMove)
                    {
                        float currentSpeed = isGrounded ? moveSpeed : moveSpeed * airControl;
                        characterController.Move(move * currentSpeed * Time.deltaTime);
                    }
                }

                // Height
                float targetHeight = isCrouching || isSliding ? crouchHeight : originalHeight;
                characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * 15f);
                characterController.center = new Vector3(0f, characterController.height * 0.5f, 0f);

                // HeadBob
                float defaultYPos = CurrentCameraHeight;
                float speed = isSprinting && !isSliding && !isCrouching ? sprintSpeed : walkSpeed;
                if (moveInput.magnitude > 0f && isMove && isGrounded && !isSliding)
                {
                    bobTimer += Time.deltaTime * bobSpeed * speed;
                    float sinWave = Mathf.Sin(bobTimer);
                    cameraParent.transform.localPosition = Vector3.Lerp(
                        cameraParent.transform.localPosition,
                        new Vector3(cameraParent.transform.localPosition.x, defaultYPos + sinWave * bobAmount, cameraParent.transform.localPosition.z),
                        Time.deltaTime * 15f);
                }
                else
                {
                    bobTimer = 0f;
                    cameraParent.transform.localPosition = Vector3.Lerp(
                        cameraParent.transform.localPosition,
                        new Vector3(cameraParent.transform.localPosition.x, defaultYPos, cameraParent.transform.localPosition.z),
                        Time.deltaTime * 15f);
                }

              

                velocity.y += gravity * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);

                // FOV
                float targetFov = isSprinting ? sprintFov : (isSliding ? sprintFov + slideFovBoost : normalFov);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * fovChangeSpeed);
            }
           
                if (isLook)
                {
                    float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime * 100f;
                    float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime * 100f;

                    rotX += mouseX;
                    rotY -= mouseY;
                    rotY = Mathf.Clamp(rotY, -90f, 90f);

                    xVelocity = Mathf.Lerp(xVelocity, rotX, snappiness * Time.deltaTime);
                    yVelocity = Mathf.Lerp(yVelocity, rotY, snappiness * Time.deltaTime);

                    if (isSliding)
                    {
                        playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, Quaternion.Euler(yVelocity - slideTiltAngle, 0f, 0f), Time.deltaTime * 10f);
                    }
                    else
                    {
                        playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, Quaternion.Euler(yVelocity, 0f, 0f), Time.deltaTime * 10f);
                    }
                    transform.rotation = Quaternion.Euler(0f, xVelocity, 0f);
                }
            
          

           
        }

        public void SetControl(bool newState)
        {
            SetLookControl(newState);
            SetMoveControl(newState);
        }

        public void SetLookControl(bool newState)
        {
            isLook = newState;
        }

        public void SetMoveControl(bool newState)
        {
            isMove = newState;
        }

        public void SetCursorVisibility(bool newVisibility)
        {
            Cursor.lockState = newVisibility ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = newVisibility;
        }
    }

    
}