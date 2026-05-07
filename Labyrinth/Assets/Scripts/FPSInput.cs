using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
//automatically creates a component when you add the script
[AddComponentMenu("Control Script/FPS Input")]
//makes it so that you can now add this script as a component without looking through scripts

public class FPSInput : MonoBehaviour
{
    [Header("Movement Attributes")]
    
    [SerializeField, Range(1.0f,20.0f)] float _speed = 10.0f;
    [SerializeField, Range(2.0f, 5.0f)] private float _runBoost = 3.0f;
    
    [SerializeField] private float _gravity = -9.81f;
    
    private float _verticalVelocity;
    [SerializeField, Range(5.0f, 20.0f)] private float _jumpVelocity = 15.0f;
    
    CharacterController _controller;

    private float boostTimer = 0;
    private bool boosting = false;
    private float boostCountdown;
    
    [SerializeField] TextMeshProUGUI boostText;
    
    [SerializeField] private float boostLength = 10.0f;
    [SerializeField] private float boostStrength = 10.0f;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _powerUp;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        
        boostCountdown = boostLength;
        
        boostText.enabled = false;
        
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += _runBoost;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed -= _runBoost;
        }

        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        Vector3 movement = new(deltaX, 0, deltaZ);
        
        //solves issue caused by movement being multiplied by speed twice when moving diagonally
        movement = Vector3.ClampMagnitude(movement, _speed);
        
        //jumping behavior
        if (_controller.isGrounded)
        {
            _verticalVelocity = _gravity;

            if (Input.GetButtonDown("Jump"))
            {
                _verticalVelocity = _jumpVelocity;
            }
        }
        else
        {
            _verticalVelocity += _gravity * 3.0f * Time.deltaTime;
        }
        
        movement.y = _verticalVelocity;
        
        movement *= Time.deltaTime;
        
        //convert world vector to local space
        movement = transform.TransformDirection(movement);
        
        //apply movement using the character controller componenet
        //CharacterController expects cooridinates in world scpae
        _controller.Move(movement);

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            
            boostCountdown -= Time.deltaTime;
            
            boostText.enabled = true;
            int seconds = Mathf.FloorToInt(boostCountdown % 60);
            boostText.text = string.Format("Speed Boost: "
            +"{0:00}", seconds);
            
            if (boostTimer >= boostLength)
            {
                _speed = 10.0f;
                boosting = false;
                boostTimer = 0;
                boostCountdown = boostLength;
                boostText.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            _audioSource.PlayOneShot(_powerUp);
            boosting = true;
            _speed = boostStrength;
            Destroy(other.gameObject);
        }
    }
}

