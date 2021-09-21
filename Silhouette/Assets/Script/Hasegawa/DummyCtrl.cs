using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCtrl : MonoBehaviour
{
    [SerializeField] GameObject DummyObject = null;

    [SerializeField] int CreateNumberOfDummy = 10;

    private struct DummyData
    {
        private GameObject dummy;
        private SpriteRenderer sr;

        public void Create(GameObject d)
        {
            dummy = Instantiate(d);
            dummy.transform.localScale = new Vector3(Random.Range(1.5f, 3.5f), Random.Range(3f, 5f), 1);
            dummy.transform.position = new Vector3(Random.Range(-7.3f, 7.3f), -4 + dummy.transform.localScale.y / 2f, 0);
            sr = dummy.GetComponent<SpriteRenderer>();
        }

        public void Delete()
        {
            Destroy(dummy.gameObject);
            sr = null;
        }

        public void ChangeAlpha(float a)
        {
            sr.color = new Color(1f / 255 * 60, 1f / 255 * 60, 1f / 255 * 60, a);
        }

        public void Color_LightBlack()
        {
            //sr.color = new Color(1f / 255 * 33, 1f / 255 * 33, 1f / 255 * 33, 1);
            sr.color = new Color(0, 0, 0, 1);
        }

        public void Color_Default()
        {
            //sr.color = new Color(1f / 255 * 199, 1f / 255 * 21, 1f / 255 * 133, 1);
            sr.color = new Color(1f / 255 * 60, 1f / 255 * 60, 1f / 255 * 60, 1);
        }
    }

    DummyData[] DummyDataList = null;

    private void CreateDummyObjects()
    {
        DummyDataList = new DummyData[CreateNumberOfDummy];
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Create(DummyObject);
            DummyDataList[i].Color_LightBlack();
        }


    }

    public void ChangeAlpha(float a)
    {
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            //DummyDataList[i].ChangeAlpha(a);
        }
    }

    public void ChangeDefaultColor()
    {
        for(int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Color_Default();
        }
    }

    public void ChangeDummyColor()
    {
        for (int i = 0; i < CreateNumberOfDummy; ++i)
        {
            DummyDataList[i].Color_LightBlack();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDummyObjects();
    }

}
