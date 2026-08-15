using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour 
{
    [SerializeField] Button m_buyButton = default;
    [SerializeField] TextMeshProUGUI m_buyText = default;
    [SerializeField] PlayerCreditManager m_playerCreditManager = default;
    ShopOptionUI m_currentChosenRef;
    bool m_canBuy = false;
    void Start()
    {
        UpdateBuyButton();
    }
    void UpdateBuyButton() 
    {
        // check for choice reference, valid purchase limit, enough credit to buy
        m_canBuy = m_currentChosenRef != null &&
        m_currentChosenRef.BuyLimit > 0 &&
        m_currentChosenRef.CurrentItemRef.ShopState.Price < m_playerCreditManager.CurrentCredit;
        m_buyButton.interactable = m_canBuy;
        m_buyText.text = m_canBuy ? "Buy Item" : "Cannot Buy";
    }
    public void OnItemChosen(ShopOptionUI option) 
    {
        m_currentChosenRef = option;
        UpdateBuyButton();
    }
    public void CancelChoice() 
    {
        m_currentChosenRef = null;
        UpdateBuyButton();
    }
    public void ConfirmBuy() 
    {
        if (!m_canBuy) return;
        // pay
        m_playerCreditManager?.OnPayment(m_currentChosenRef.CurrentItemRef.ShopState.Price);
        // get item
        KDEventUtil.ObtainItemEvent(this, m_currentChosenRef.CurrentItemRef);
        // decrement purchase limit
        m_currentChosenRef?.UpdateBuyLimit(m_currentChosenRef.BuyLimit - 1);
        // update checks for another purchase
        UpdateBuyButton();
    }
}
