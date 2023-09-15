using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController2D : MonoBehaviourPunCallbacks, IDamageable
{
    public Joystick joystick; // Ссылка на компонент джойстика
    public Animator animator;
    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint; // Точка выстрела пули
    public float moveSpeed = 5f; // Скорость перемещения персонажа
    public float maxHealth = 100; // Максимальное здоровье персонажа
    private float currentHealth; // Текущее здоровье персонажа
    [SerializeField] Image healthbarImage;
    public int coinsCount;
    public int killsCount;

    PhotonView PV;

    PlayerManager playerManager;

    private void Start()
    {
        currentHealth = maxHealth;
        animator.GetComponent<Animator>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        // Управление перемещением персонажа
        float moveX = joystick.Horizontal;
        bool flipped = moveX > 0;
        animator.SetBool("isMove", moveX != 0);
        
        float moveY = joystick.Vertical;
        Vector2 movement = new Vector2(flipped ? -moveX : moveX, moveY);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        if(moveX != 0)
            transform.rotation = new Quaternion(0, flipped ? -180 : 0, 0, 0);

        // Выстрел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        PV.RPC("RPC_Shoot", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().direction = transform.rotation.y <= 0 ? Vector2.left : Vector2.right;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    void Die()
    {
        playerManager.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "coin")
        {
            playerManager.GetCoin();
            coinsCount++;
            Destroy(collision.gameObject);
        }
    }
}
