using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int coinCount { get; private set; }

    void Awake()
    {
        coinCount = PlayerPrefs.GetInt("Coins", 0);
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        SaveCoins();
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coinCount);
        PlayerPrefs.Save();
    }
}

