﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Zombie : MonoBehaviour
{
    Animator anim;
    PlayerMove player;
    Rigidbody2D rb;
    ZombieSpawner zs;

    public float[] speedScale = new float[3];
    public float speed;

    float[] DistanceScale = new float[2] { 3f, 25f };
    float distance;

    public Color[] LightColor = new Color[2];

    public float ZombieHP = 100;

    float ZombieStartHP;

    public Vector3 Start;

    public GameObject[] WayPoint;

    public Image HPRateZombie;

    void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = LightColor[0];
        player = FindObjectOfType<PlayerMove>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Start = transform.position;
        ZombieStartHP = ZombieHP;
        zs = FindObjectOfType<ZombieSpawner>();
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (ZombieHP>=0)
        {
            anim.SetTrigger("Stand");
        }
        if (distance <= DistanceScale[0])
        {
            Attack();
        }
        if (distance <= DistanceScale[1] && distance >= DistanceScale[0])
        {
            Follow();
        }
        if (distance >= DistanceScale[1])
        {
            if(Mathf.Round(transform.position.x) == Mathf.Round(Start.x)&& (Mathf.Round(transform.position.y) == Mathf.Round(Start.y)))
            {
                Stand();
            }
            else
            {
                MoveBack();
            }
        }
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");

        Vector3 zombiePoition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Vector3 direction = playerPosition - zombiePoition;

        Move(direction);
        speed = speedScale[2];
        Rotate(direction);
    }

    public void Follow()
    {
        anim.SetTrigger("Follow");

        Vector3 zombiePoition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Vector3 direction = playerPosition - zombiePoition;
        Move(direction);
        speed = speedScale[1];
        Rotate(direction);
    }

    public void Stand()
    {
        anim.SetTrigger("Stand");
        Vector3 playerPosition = player.transform.position;
        Vector3 zombiePoition = transform.position;

        Vector3 direction = playerPosition - zombiePoition;
        Rotate(direction);

        speed = speedScale[0];
    }

    public void MoveBack()
    {
        anim.SetTrigger("Follow");
        Vector3 playerPosition = Start;
        Vector3 zombiePoition = transform.position;

        Vector3 direction = playerPosition - zombiePoition;
        Move(direction);
        speed = speedScale[1];
        Rotate(direction);
    }

    void Move(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }
    void Rotate(Vector3 direction)
    {
        direction.z = 0;
        transform.up = -direction;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Light")
        {
            gameObject.GetComponent<SpriteRenderer>().color = LightColor[1];
        }
        if (collision.gameObject.tag == "Bullet")
        {
            ZombieHP -= 30;
            collision.gameObject.SetActive(false);
            if (ZombieHP <= 0)
            {
                anim.SetTrigger("Death");
            }
            HealthCheck();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            gameObject.GetComponent<SpriteRenderer>().color = LightColor[0];
        }
    }

    void HealthCheck()
    {
        HPRateZombie.fillAmount = ZombieHP / ZombieStartHP;
    }

    void DestroyItself()
    {
        player.PistolUpgradeCheck();
        player.ZombieKilled++;
        player.ZombieKilledText.text = player.ZombieKilled.ToString();
        zs.currentLenght++;
        zs.UpdateZombie();
        if (player.ZombieKilled >= zs.NeedsToBeKilled)
        {
            zs.YouWon.SetActive(true);
            Time.timeScale = 0f;
        }
        Destroy(gameObject);
    }
    void AttackPlayer()
    {
        PlayerMove player;
        player = FindObjectOfType<PlayerMove>();

        if (distance <= 2f)
        {
            player.Damage();
        }
    }
}
