using UnityEngine;
using System.Collections;

public abstract class Weapon : Item
{
    public float damage;

    public AudioClip audioClipShoot;
    public AudioSource audioSource;


    public abstract void Hit();
}
