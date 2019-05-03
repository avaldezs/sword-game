using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponAnimation : MonoBehaviour
{
    protected float lastSoundTime;
    public float fireRate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ((Time.time - lastSoundTime) > fireRate))
        {
            lastSoundTime = Time.time;
            GetComponent<Animation>().Play("WeaponAttack");
            GetComponent<AudioSource>().Play();
        }
    }
}
