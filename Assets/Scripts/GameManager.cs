using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{


    public Player player;

    public OpenItemMenu MotherShipInv;

    public Camerascript camerascript;

    public GenerateWorld WorldGenerator;

    public static GameManager instance;
    [Space(50)]
    [SerializeField, Header("Debugging")]
    GameObject DBGWindow;
    public List<GameObject> SkinList = new List<GameObject>();
    [SerializeField]
    Transform SkinPickerGrid;
    [SerializeField]
    GameObject ButtonPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        int index = 0;
        foreach(GameObject skin in SkinList)
        {

            GameObject button = Instantiate(ButtonPrefab, SkinPickerGrid);
            int i = index;
            button.GetComponent<Button>().onClick.AddListener(delegate { player.SetSkin(i); });
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            DBGWindow.SetActive(!DBGWindow.activeSelf);
        }
    }

    public void ShrinkDespawn(GameObject gameObject, float delay = 0)
    {
        StartCoroutine(SDespawn(gameObject, delay));
    }

    IEnumerator SDespawn(GameObject gameObject, float delay)
    {
        float Rate = 2;
        float i = 0;
        yield return new WaitForSeconds(delay);
        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, i);
            yield return 0;
        }
        Destroy(gameObject);
    }

}

[Serializable]
public class LootObj
{
    public GameObject ItemPrefab;
    public int MinAmount;
    public int MaxAmount;
    public int SetAmount;
}