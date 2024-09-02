using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // 총 이름
    public float range; // 총 사정 거리
    public float accuracy; // 정확도
    public float fireRate; // 연사 속도
    public float reloadTime; // 재장전 속도

    public float damage; // 총 데미지 

    public int reloadBulletCount; // 총알 재장전 갯수 
    public int currentBulletCount; // 현재 탄창속 총알 갯수
    public int maxBulletCount; // 최대 소유 가능 총알 갯수
    public int carryBulletCount; // 현재 소유 총알 갯수

    public float retroActionForce; // 반동세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;
}
