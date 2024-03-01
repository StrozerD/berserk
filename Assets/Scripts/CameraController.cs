using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		pos.y += 1f;

		transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);	// Плавное движение
	}
}
