using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//雷の音をランダムで流す
public class ThunderSECtrl : MonoBehaviour
{
    //再生する雷の効果音
    [SerializeField] AudioClip[] ThunderSE = new AudioClip[3];

    //再生するところ
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
