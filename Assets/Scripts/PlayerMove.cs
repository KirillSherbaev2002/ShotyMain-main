﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Animator anim;
    private SoundScript SS;

    public GameObject BulletPref;
    public GameObject BloodyEffect;
    public GameObject GoldenPistol;

    [SerializeField] Transform pistolPos;

    [SerializeField] float PlayerHP;
    public float TotalHp;

    public Image HPRate;

    public int pistolLevel;
    int interval;
    public int ZombieKilled;
    public TMP_Text ZombieKilledText;

    public UnityAction UpdateHealth;

    public AudioClip ShootSound;
    public AudioClip RoundSound;
    void Awake()
    {
        SS = FindObjectOfType<SoundScript>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // Почему не работает ? UpdateHealth += Damage();
        PlayerHP = TotalHp;
    }

    private void FixedUpdate()
    {
        Move();
        ToMousePoint();
        Shoot();
    }
    void Move()
    {
        float XAxis = Input.GetAxis("Horizontal");
        float YAxis = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(XAxis, YAxis).normalized * speed;
        anim.SetFloat("Speed", rb.velocity.magnitude);
    }

    void ToMousePoint()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        transform.up = -direction;
    }

    void Shoot()
    {
        if (pistolLevel == 1)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (interval == 0)
                {
                    SS.PlayShoot(ShootSound);
                    anim.SetTrigger("Shoot");
                    Instantiate(BulletPref, pistolPos.transform.position, pistolPos.transform.rotation);
                }
                interval++;
                if (interval >= 11)
                {
                    interval = 0;
                }
            }
        }
        
        if (pistolLevel == 2)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (interval >= 6)
                {
                    SS.PlayALot();
                    anim.SetTrigger("Shoot");
                    Instantiate(BulletPref, pistolPos.transform.position, pistolPos.transform.rotation);
                    interval++;
                    if(interval >= 9)
                    {
                        interval = 0; 
                    }
                }
                else
                {
                    interval++;
                }
            }
        }
        if (pistolLevel == 3)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (interval >= 3)
                {
                    anim.SetTrigger("Shoot");
                    Instantiate(BulletPref, pistolPos.transform.position, pistolPos.transform.rotation);
                    interval++;
                    if (interval >= 20)
                    {
                        interval = 0;
                    }
                }
                else
                {
                    interval++;
                }
            }
        }
    }

    public void Damage()
    {
        PlayerHP -= Random.Range(70, 130);
        Instantiate(BloodyEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2), transform.rotation);
        HPRate.fillAmount = PlayerHP / TotalHp;
        IsAlive();
    }

    void IsAlive()
    {
        if (PlayerHP <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void PistolUpgradeCheck()
    {
        if (ZombieKilled >= 20)
        {
            GoldenPistol.SetActive(true);
            pistolLevel = 2;
        }
    }
}
