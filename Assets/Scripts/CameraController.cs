using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform player;   // Сам игрок

	private Vector3 pos;        // Позиция камеры

	private void Awake()
	{
		if(!player) player = FindAnyObjectByType<Hero>().transform;
	}

	private void Update()
	{
		pos = player.position;	// Новое позиция камеры
		pos.z = -10f;           // Оставляем отдаление
		pos.y = ((player.position.y / 3.5f) + 1f) * 3.5f;

		if (Hero.Instance.State == States.jump)
		{
			pos.y -= 1.25f;
		}

		if (pos.y < 0)
		{
			pos.y = 0;
		}

		if (pos.x < 0)
		{
			pos.x = 0;
		}

		transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime + 0.015f);    // Плавное движение
		
		
	}
}
