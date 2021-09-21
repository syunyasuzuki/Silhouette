using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSECtrl : MonoBehaviour
{
    [SerializeField] AudioClip[] ThunderSE = new AudioClip[3];

    AudioSource audiosource = null;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlaySE()
    {
        audiosource.clip = ThunderSE[Random.Range(0, 3)];
        audiosource.Play();
    }




}
