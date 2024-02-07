using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyGame;

public class Weapon : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform[] shootSuperPos;

    [SerializeField] TextMeshProUGUI text;

    Animator anim;
    SpriteRenderer spR;

    public static Weapon Instance;

    [SerializeField] float timeBtwShoot = 2;
    float shootTimer;

    [SerializeField] float timeBtwSuperShoot = 2;
    float shootSuperTimer;

    //[SerializeField] GameObject dronePrefab;
    // public GameObject droneInstance { get; private set; }

    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] SpriteRenderer muzzleFlashSpR;
    [SerializeField] Sprite[] spritesMuzzleFlash;

    [SerializeField] AudioClip[] shootClip, superShootClip;


    AudioSource audS;

    // private void Awake()
    // {
    //Instance = this;
    // Shop.Instance.buySeconPosition += UpdateTimeBtwShoot;
    //  }

    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();
        audS = GetComponent<AudioSource>();
        shootAudioSource = GetComponent<AudioSource>();

        // dashTimer = timeBtwDash;
        // maxHealth = health;

        // UpdateHealthUI();
        // droneInstance = Instantiate(dronePrefab, transform.position, Quaternion.identity);
        // droneInstance.SetActive(false);
    }

    void Update()
    {
        if (!PauseManager.IsGamePaused)
        {
        PlayerRotation();
        ShootLogic();// Обработка ввода и действия игрока
        }
        // dashTimer += Time.deltaTime;
        // dashSlider.value = dashTimer / timeBtwDash;
        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //if (dashTimer >= timeBtwDash)
        //{
        // dashTimer = 0;
        // ActivateDash();
        //}
        // }
        if (timeBtwShoot - shootTimer < 0) return;
        text.text = ((int)((timeBtwShoot - shootTimer) * 100) / 100f).ToString();
    }
    void PlayerRotation()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void UpdateTimeBtwShoot()
    {
        timeBtwShoot -= 0.1f;
        timeBtwSuperShoot -= 0.5f;
    }

    void ShootLogic()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer >= timeBtwShoot)
        {
            Shoot();
            shootTimer = 0;
        }

        shootSuperTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && shootSuperTimer >= timeBtwSuperShoot && PlayerPrefs.GetInt("Position1") == 1)
        {
            SuperShoot();
            shootSuperTimer = 0;
        }
    }

    void Shoot()
    {
        // Добавьте здесь логику стрельбы, например:
        audS.clip = shootClip[Random.Range(0, shootClip.Length)];
        audS.Play();
        Instantiate(bullet, shootPos.position, shootPos.rotation);
        shootAudioSource.Play();
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    void SuperShoot()
    {
        // Добавьте здесь логику супер стрельбы, например:
        for (int i = 0; i < shootSuperPos.Length; i++)
        {
            Instantiate(bullet, shootSuperPos[i].position, shootSuperPos[i].rotation);
        }
        SoundManager.Instance.PlayerSound(shootAudioSource.clip);
        CameraFollow.Instance.CamShake();
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    IEnumerator SetMuzzleFlash()
    {
        // Добавьте здесь логику отображения музыльной вспышки, например:
        muzzleFlashSpR.enabled = true;
        muzzleFlashSpR.sprite = spritesMuzzleFlash[Random.Range(0, spritesMuzzleFlash.Length)];
        yield return new WaitForSeconds(0.1f);
        muzzleFlashSpR.enabled = false;
    }
}
