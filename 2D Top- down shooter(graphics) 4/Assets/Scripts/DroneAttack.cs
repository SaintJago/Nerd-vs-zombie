using UnityEngine;

public class DroneAttack : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб снаряда
    public float fireRate = 1f;
    public float attackRange = 10f; // Дистанция атаки

    private float nextFireTime = 0f;
    private bool canShoot = true; // Переменная для отслеживания возможности стрелять
    public float bulletCooldown = 2f; // Задержка между выстрелами

    void Update()
    {
        // Проверяем, прошло ли достаточно времени для следующего выстрела
        if (Time.time >= nextFireTime)
        {
            if (CanAttack()) // Проверяем, находится ли враг в дистанции атаки
            {
                ShootOneByOne();
                canShoot = false; // Устанавливаем флаг, что дрон не может стрелять
                nextFireTime = Time.time + 1f / fireRate; // Обновляем время следующего выстрела
            }
        }

        // Обновляем состояние canShoot после задержки
        if (!canShoot && Time.time >= nextFireTime + bulletCooldown)
        {
            canShoot = true; // Устанавливаем флаг, что дрон может стрелять снова
        }
    }

    bool CanAttack()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);

            if (distance <= attackRange)
            {
                return true; // Если хотя бы один враг находится в дистанции атаки, возвращаем true
            }
        }

        return false; // Если ни одного врага в дистанции атаки не найдено
    }

    void ShootOneByOne()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);

            if (distance <= attackRange)
            {
                // Получаем трансформ найденного зомби
                Transform zombieTransform = zombie.transform;

                // Создаем снаряд в позиции дрона
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // Направляем снаряд в сторону цели (зомби)
                bullet.transform.right = (zombieTransform.position - transform.position).normalized;

                // Ждем перед следующим выстрелом
                break;
            }
        }
    }
}
