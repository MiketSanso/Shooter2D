using UnityEngine;
using System.Collections;

public abstract class Weapon : Item
#warning Or GameItem
{
    public float damage, toxic, bleeding, bulletproof, rupture, radiation;
    public bool isToxic, isRadiation;

    public AudioClip audioClipShoot;
    public AudioSource audioSource;


    public abstract void Hit();
}
