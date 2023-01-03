using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] GameObject[] playerIndex;
    [SerializeField] int[] prices;
    [SerializeField] TMP_Text priceText;

    int coins;
    void Awake() => FreeCharacterCheck();

    void Start()
    {
        PriceUpdater(0);
        LoadBuyedCars();
        int selected = PlayerPrefs.GetInt("playerindex");
        PlayerPrefs.SetInt("selected", selected);
        SelectedUpdate(selected);
        UpdateShowRoom();

    }

    void Update()
    {
        coins = PlayerPrefs.GetInt("coins");
        coinText.text = coins.ToString();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("coins", coins += 50);
            Debug.Log(PlayerPrefs.GetInt("coins"));
        }

    }

    public void BuyButton() => Buy(PlayerPrefs.GetInt("selected"));
    void Buy(int index)
    {
        if (playerIndex[index].transform.GetChild(0).gameObject.activeSelf == true)
        {
            if (coins >= prices[index])
            {
                int coinLeft = coins - prices[index];
                PlayerPrefs.SetInt("coins", coinLeft);

                PlayerPrefs.SetInt("playerindex", index);
                PlayerPrefs.SetInt("purchased" + index, 1);

                playerIndex[index].transform.GetChild(0).gameObject.SetActive(false);
                PriceUpdater(index);
            }
            else { Debug.Log("Yok"); }
        }
        
        else if (playerIndex[index].transform.GetChild(0).gameObject.activeSelf == false)
        {
            PlayerPrefs.SetInt("playerindex", index);
            SelectedUpdate(index);
        }
    }

    public void OnClickNext()
    {
        int index = PlayerPrefs.GetInt("selected");
        if (index == playerIndex.Length - 1) { index = 0; }
        else { index++; }
        PlayerPrefs.SetInt("selected", index);
        UpdateShowRoom();
        PriceUpdater(index);
        SelectedUpdate(index);
    }

    public void OnClickBack()
    {
        int index = PlayerPrefs.GetInt("selected");
        if (index == 0) { index = playerIndex.Length - 1; }
        else { index--; }
        PlayerPrefs.SetInt("selected", index);
        UpdateShowRoom();
        PriceUpdater(index);
        SelectedUpdate(index);
    }

    void UpdateShowRoom()
    {
        for (int i = 0; i < playerIndex.Length; i++)
        {
            if (i != PlayerPrefs.GetInt("selected"))
            {
                playerIndex[i].transform.gameObject.SetActive(false);
            }
            else if (i == PlayerPrefs.GetInt("selected"))
            {
                playerIndex[PlayerPrefs.GetInt("selected")].transform.gameObject.SetActive(true);
            }
        }
    }

    void LoadBuyedCars()
    {
        for (int i = 0; i < playerIndex.Length; i++) PurchasedCheck(i);
    }

    void PurchasedCheck(int index)
    {
        if (PlayerPrefs.GetInt("purchased" + index) == 1)
            playerIndex[index].transform.GetChild(0).gameObject.SetActive(false); 
    }

    void PriceUpdater(int index)
    {
        if (PlayerPrefs.GetInt("purchased" + index) != 1)
            priceText.SetText("$ " + prices[PlayerPrefs.GetInt("selected")]);

        else
            priceText.SetText("Select");
    }

    void FreeCharacterCheck()
    {
        if (PlayerPrefs.GetInt("purchased" + 0) != 1)
            PlayerPrefs.SetInt("purchased" + 0, 1);
    }

    void SelectedUpdate(int index)
    {
        for (int i = 0; i < playerIndex.Length; i++)
        {
            if (PlayerPrefs.GetInt("playerindex") == index)
            {
                priceText.SetText("Selected");
            }
        }
    }

    public void CloseShop() => SceneManager.LoadScene(sceneName: "Game");
}
