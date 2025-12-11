using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;                  // 따라갈 플레이어
    public SpriteRenderer mapRenderer;        // 맵 스프라이트

    private float minX, maxX, minY, maxY;

    void Start()
    {
        // 카메라의 절반 크기 계산
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        // 맵(스프라이트)의 실제 월드 경계값 읽기
        Bounds mapBounds = mapRenderer.bounds;

        minX = mapBounds.min.x + camWidth;
        maxX = mapBounds.max.x - camWidth;

        minY = mapBounds.min.y + camHeight;
        maxY = mapBounds.max.y - camHeight;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 플레이어 위치값 가져오기
        Vector3 targetPos = player.position;

        // Clamp(카메라가 맵 밖으로 못 나가게)
        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

