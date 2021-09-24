using UnityEngine;

public class PlatformCtrlChild : MonoBehaviour
{
    [SerializeField] SpriteRenderer Platform_sr = null;

    [SerializeField] SpriteRenderer Back_sr = null;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("ThunderClouds").GetComponent<PlatformCtrl>().SetPlatformsr(Platform_sr);
        GameObject.Find("ThunderClouds").GetComponent<PlatformCtrl>().SetBacksr(Back_sr);
    }

}
