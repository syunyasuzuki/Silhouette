using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade_ctr : MonoBehaviour
{
    Camera cam;

    //[SerializeField] GameObject death_fade_image1;
    //[SerializeField] GameObject death_fade_image2;

    [SerializeField] Image fade_image;

    float image1_pos_y;
    float image2_pos_y;

    float alpha;

    float stay_time;
    const float main_fade_speed = 1.0f;
    const float death_fade_speed = 50.0f;

    bool death_fade;
    bool death_fade_in;
    bool death_fade_out;

    public static bool main_fade;
    public static bool main_fade_in;
    public static bool main_fade_out;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        death_fade = false;
        death_fade_in = false;
        death_fade_out = false;

        main_fade = true;
        main_fade_in = true;
        main_fade_out = false;

        image1_pos_y = -10.0f;
        image2_pos_y = 10.0f;

        alpha = 1.0f;

        fade_image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        //death_fade_image1.transform.position = new Vector3(0.0f, image1_pos_y, 0.0f);
        //death_fade_image2.transform.position = new Vector3(0.0f, image2_pos_y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            death_fade = true;
            death_fade_in = true;
        }

        if(death_fade == true)
        {
            if(death_fade_in == true)
            {
                DeathFadeIn();
            }
            if(death_fade_out == true)
            {
                DeathFadeOut();
            }
        }

        if(main_fade == true)
        {
            if(main_fade_in == true)
            {
                MainFadeIn();
            }
            if(main_fade_out == true)
            {
                MainFadeOut();
            }
        }
    }

    void MainFadeIn()
    {
        alpha -= main_fade_speed * Time.deltaTime;
        if(alpha <= 0.0f)
        {
            main_fade = false;
            main_fade_in = false;
        }
        fade_image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
    }
    void MainFadeOut()
    {
        alpha += main_fade_speed * Time.deltaTime;
        if (alpha >= 1.0f)
        {
            SceneManager.LoadScene("GameScene");
            main_fade = false;
            main_fade_out = false;
        }
        fade_image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
    }

    void DeathFadeIn()
    {
        image1_pos_y += death_fade_speed * Time.deltaTime;
        image2_pos_y -= death_fade_speed * Time.deltaTime;
        if(image1_pos_y >= 0.0f && image2_pos_y <= 0.0f)
        {
            image1_pos_y = 0.0f;
            image2_pos_y = 0.0f;
            stay_time += Time.deltaTime;
            if(stay_time >= 1.5f)
            {
                death_fade_in = false;
                death_fade_out = true;
                stay_time = 0.0f;
            }
        }

        //death_fade_image1.transform.position = new Vector3(0.0f, image1_pos_y, 0.0f);
        //death_fade_image2.transform.position = new Vector3(0.0f, image2_pos_y, 0.0f);
    }

    void DeathFadeOut()
    {
        image1_pos_y -= death_fade_speed * Time.deltaTime;
        image2_pos_y += death_fade_speed * Time.deltaTime;
        if (image1_pos_y <= -10.0f && image2_pos_y >= 10.0f)
        {
            image1_pos_y = -10.0f;
            image2_pos_y = 10.0f;
            death_fade = false;
            death_fade_out = false;
        }

        //death_fade_image1.transform.position = new Vector3(0.0f, image1_pos_y, 0.0f);
        //death_fade_image2.transform.position = new Vector3(0.0f, image2_pos_y, 0.0f);
    }
}
