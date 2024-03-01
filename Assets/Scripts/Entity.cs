using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int lives;
    // Получение урона
    public virtual void GetDamage()
    {
        lives--;
		print(lives);
		if (lives < 1)
        {
            Die();
        }
    }

	// Смерть
	public virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
