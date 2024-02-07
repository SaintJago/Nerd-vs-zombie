﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using MyGame;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int health;
    [HideInInspector] public int maxHealth;

    Rigidbody2D rb;
    Vector2 moveVelocity;

    Animator anim;
    SpriteRenderer spR;

    [SerializeField] TextMeshProUGUI text;

    public static Player Instance;

    [SerializeField] GameObject hitEffect;

    [SerializeField] float dashForce, timeBtwDash, dashTime;
    float dashTimer;
    bool isDashing = false;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider dashSlider;
    [SerializeField] ParticleSystem footParticle;
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject dronePrefab;
    public GameObject droneInstance { get; private set; }

    bool canBeDamaged = true;

    [SerializeField] AudioClip heartClip, deathClip, dashSound;
    [SerializeField] AudioClip[] footClips;
    AudioSource audS;

    Vector2 moveInput;

    private void Awake()
    {
        Instance = this;
        Shop.Instance.buySeconPosition += UpdateTimeBtwShoot;
    }

    // Запускается в старте
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();
        audS = GetComponent<AudioSource>();

        dashTimer = timeBtwDash;
        maxHealth = health;

        UpdateHealthUI();
        droneInstance = Instantiate(dronePrefab, transform.position, Quaternion.identity);
        droneInstance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;

        dashSlider.value = dashTimer / timeBtwDash;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashTimer >= timeBtwDash)
            {
                dashTimer = 0;
                ActivateDash();
            }
        }

        // if (timeBtwShoot - shootTimer < 0) return;

        // text.text = ((int)((timeBtwShoot - shootTimer) * 100) / 100f).ToString();
    }

    private void FixedUpdate()
    {
        Move();

        if (isDashing) Dash();
    }

    void UpdateTimeBtwShoot()
    {
        //timeBtwShoot -= 0.1f;
        // timeBtwSuperShoot -= 0.5f;
    }

    #region Base Function

    void Dash()
    {
        rb.AddForce(moveInput * Time.fixedDeltaTime * dashForce * 100);
    }

    void ActivateDash()
    {
        isDashing = true;
        canBeDamaged = false;

        SoundManager.Instance.PlayerSound(dashSound);

        Invoke(nameof(DeActivateDash), dashTime);
    }

    void DeActivateDash()
    {
        isDashing = false;
        canBeDamaged = true;
    }

    void Move()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("run", true);
            footParticle.Pause();
            footParticle.Play();

            var emission = footParticle.emission;
            emission.rateOverTime = 10;

            if (!audS.isPlaying)
            {
                audS.clip = footClips[Random.Range(0, footClips.Length)];
                audS.Play();
            }
        }
        else
        {
            anim.SetBool("run", false);
            var emission = footParticle.emission;
            emission.rateOverTime = 0;
        }

        ScalePlayer(moveInput.x);
        moveVelocity = moveInput.normalized * speed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void ScalePlayer(float x)
    {
        if (x == 1)
            spR.flipX = false;
        else if (x == -1)
            spR.flipX = true;
    }

    #endregion

    public void Damage(int damage)
    {
        if (!canBeDamaged) return;

        health -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        CameraFollow.Instance.CamShake();
        SoundManager.Instance.PlayerSound(heartClip);
        UpdateHealthUI();

        if (health <= 0 && deathPanel.activeInHierarchy == false)
        {
        // Остановка времени
        PauseManager.PauseGame();
        SoundManager.Instance.PlayerSound(deathClip);
        deathPanel.SetActive(true);
        gameObject.SetActive(false);
        }
    }

    public void AddHealth(int value)
    {
        if (health <= 0) health = 0;
        health += value;

        if (health > maxHealth) health = maxHealth;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthSlider.value = (float)health / maxHealth;
    }

    [HideInInspector] public int currentMoney;
    [SerializeField] TextMeshProUGUI coinsText;

    public void AddMoney(int value)
    {
        currentMoney += value;

        LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Coins").Completed += op =>
        {
            if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                coinsText.text = op.Result + ": " + currentMoney.ToString();
            }
        };
    }
}
