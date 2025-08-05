using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ItemAssetLookup m_defaultItemList = default;
    [SerializeField] UnityEvent m_onInteract = default;
    [SerializeField] List<ShopUI> m_optionUI = default;
    ItemAssetLookup m_currentList;
    void OnEnable()
    {
        HideaAllOptons();
    }
    void Start()
    {
        SetToDefault();
    }
    public void SetToDefault() 
    {
        m_currentList = m_defaultItemList;
    }
    public void ChangeItemList(ItemAssetLookup list)
    {
        m_currentList = list;
    }
    // activate this when user chooses interact option (e.g. E button)
    public override void Interact()
    {
        m_onInteract?.Invoke();
        // get 3 random items to choose from
        // show UI
        if (m_currentList == null) 
        {
            SetToDefault();
        }
        List<ItemAsset> options = m_currentList.GetUniqueRandomItems();
        for (int i = 0; i < m_optionUI.Count; ++ i) 
        {
            m_optionUI[i]?.Init(options[i]);
        }
        StartCoroutine(ShowOptions());
    }
    public void HideaAllOptons() 
    {
        foreach (var item in m_optionUI) 
        {
            item.Hide();
        }
    }
    IEnumerator ShowOptions() 
    { 
        foreach(var item in m_optionUI) 
        {
            item.Show();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
