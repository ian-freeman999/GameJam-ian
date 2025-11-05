using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI hamCoinCounterText;
    public List<GameObject> productionMachineObjects;
    public List<GameObject> buyMachineButtons;
    public float hamCoins = 0;
    private bool buyMachineButtonsState = false;
    private bool canBuy;
    private int speedUpgradeLevel = 0;
    private int qualityUpgradeLevel = 0;
    private GameObject ui;
    public GameObject winButton;
    public GameObject wintext;
    public Camera cam;
    void Start()
    {
        ui = GameObject.Find("UI");
        foreach (Transform child in GameObject.Find("Floor1").transform)
        {
            productionMachineObjects.Add(child.gameObject);
            Debug.Log(child.gameObject.name);
        }
        foreach (Transform child in GameObject.Find("BuyMachineFolder").transform)
        {
            buyMachineButtons.Add(child.gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ToggleBuyMachineButtons()
    {
        foreach (Transform child in GameObject.Find("BuyMachineFolder").transform)
        {
            child.gameObject.SetActive(!buyMachineButtonsState);
        }
        buyMachineButtonsState = !buyMachineButtonsState;
    }

    // Update is called once per frame
    void Update()
    {
        hamCoinCounterText.text = "HamCoins: " + hamCoins;

        if (hamCoins == 10000)
        {
            winButton.SetActive(true);
        }
    }
    public void win()
    {
        GameObject[] allActiveObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allActiveObjects)
        {

            obj.SetActive(false); // Deactivate the GameObject

        }
        wintext.SetActive(true);
        cam.gameObject.SetActive(true);
    }

    public void BuyMachine(int machineNumber)
    {
        if (canBuy)
        {
            productionMachineObjects[machineNumber - 1].SetActive(true);

            GameObject buyMachineButton = buyMachineButtons[machineNumber - 1];

            buyMachineButton.GetComponent<Image>().enabled = false;
            buyMachineButton.GetComponent<Button>().enabled = false;

            Transform buyButtonText = buyMachineButton.transform.Find("Text (TMP)");
            buyButtonText.gameObject.SetActive(false);

            Transform upgradeSpeed = buyMachineButton.transform.Find("UpgradeSpeed");
            Transform upgradeQuality = buyMachineButton.transform.Find("UpgradeQuality");

            upgradeSpeed.gameObject.SetActive(true);
            upgradeQuality.gameObject.SetActive(true);
        }

        int activeMachines = 0;

        foreach (GameObject child in productionMachineObjects)
        {
            if (child.activeSelf)
            {
                activeMachines++;
            }
        }
    }

    public void BuySpeedUpgrade(int machineNumber)
    {
        switch (productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().speedUpgradeLevel)
        {
            case 0:
                YoinkTheCoins(100);
                break;
            case 1:
                YoinkTheCoins(200);
                break;
            case >= 2:
                YoinkTheCoins(300);
                if (canBuy)
                {
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeSpeed").gameObject.SetActive(false);
                }
                break;
        }

        productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().speedUpgradeLevel += 1;

        if (canBuy)
        {
            Transform hambugerSpanwer = productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner");
            hambugerSpanwer.GetComponent<HamburgerSpawner>().speed *= 1.5f;
            switch (productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().speedUpgradeLevel)
            {
                case 0:
                    break;
                case 1:
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeSpeed").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Upgrade Machine {machineNumber} Speed $200";
                    break;
                case >= 2:
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeSpeed").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Upgrade Machine {machineNumber} Speed $300";
                    break;
            }
        }
    }

    public void BuyQualityUpgrade(int machineNumber)
    {
        switch (productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().qualityUpgradeLevel)
        {
            case 0:
                YoinkTheCoins(100);
                break;
            case 1:
                YoinkTheCoins(200);
                break;
            case >= 2:
                YoinkTheCoins(300);
                if (canBuy)
                {
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeQuality").gameObject.SetActive(false);
                }
                break;
        }

        if (canBuy)
        {
            Transform seller = productionMachineObjects[machineNumber - 1].transform.Find("Seller");
            seller.GetComponent<Seller>().value *= 2;
            switch (productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().qualityUpgradeLevel)
            {
                case 0:
                    productionMachineObjects[machineNumber - 1].transform.Find("QualityUpgrader1").gameObject.SetActive(true);
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeQuality").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Upgrade Machine {machineNumber} Quality $200";
                    break;
                case 1:
                    productionMachineObjects[machineNumber - 1].transform.Find("QualityUpgrader2").gameObject.SetActive(true);
                    buyMachineButtons[machineNumber - 1].transform.Find("UpgradeQuality").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = $"Upgrade Machine {machineNumber} Quality $300";
                    break;
                case >= 2:
                    productionMachineObjects[machineNumber - 1].transform.Find("QualityUpgrader3").gameObject.SetActive(true);
                    break;
            }
        }
        productionMachineObjects[machineNumber - 1].transform.Find("HamburgerSpawner").GetComponent<HamburgerSpawner>().qualityUpgradeLevel += 1;
    }

    public void YoinkTheCoins(int cost)
    {
        if (hamCoins >= cost)
        {
            hamCoins -= cost;
            canBuy = true;
        }
        else
        {
            canBuy = false;
        }
    }
}
