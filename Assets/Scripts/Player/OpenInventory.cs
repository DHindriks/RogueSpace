using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OpenInventory : MonoBehaviour
{
    [SerializeField]
    Transform Playerpos;

    [SerializeField]
    Camera InventoryView;


    Player player;

    Camerascript cam;

    bool InventoryActive;

    [SerializeField]
    Button ToggleBtn;

    public void ToggleActive(GameObject Toggle)
    {
        Toggle.SetActive(!Toggle.activeSelf);
    }

    void Awake()
    {
        player = GameManager.instance.player;
        cam = GameManager.instance.camerascript;
        cam.Switched.AddListener(delegate { OpenInventoryUI();});
        ToggleBtn.onClick.AddListener(delegate { Open(); });

    }

    void Open()
    {
        Debug.Log("opening");
        if (!InventoryActive)
        {
            gameObject.SetActive(true);
            InventoryActive = true;
            player.transform.position = Playerpos.position;
            player.transform.GetChild(0).rotation = Playerpos.rotation;

            ToggleBtn.onClick.RemoveAllListeners();
            ToggleBtn.onClick.AddListener(CloseInventory);

            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Player>().CtrlEnabled = false;
            cam.ChangeCam(InventoryView);
            cam.ChangeZoom(20, 10);
            Debug.Log("Enabled");
        }
    }

    void OpenInventoryUI()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(InventoryActive);
        }

        if (!InventoryActive)
        {
            gameObject.SetActive(false);
            Close();
        }
    }

    public void CloseInventory()
    {
        InventoryActive = false;
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        cam.ChangeCam();
    }

    void Close()
    {
        cam.ChangeZoom();
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Player>().CtrlEnabled = true;
        ToggleBtn.onClick.RemoveAllListeners();
        ToggleBtn.onClick.AddListener(delegate { Open();});
    }
}
