using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Toggle))]
public class ShopItemChoice : MonoBehaviour 
{
    [SerializeField] Image m_icon = default;
    [SerializeField] TextMeshProUGUI m_name = default;
    [SerializeField] TextMeshProUGUI m_description = default;
    [SerializeField] TextMeshProUGUI m_cost = default;
    protected static int s_chosenItemIndex;
    protected int m_itemIndex = default;
    protected bool m_locked = false;
    Animator m_anim;
    Toggle m_toggle;
    public static int ChosenItemIndex => s_chosenItemIndex;
    void OnEnable()
    {
        m_anim = GetComponent<Animator>();
        m_toggle = GetComponent<Toggle>();
    }
    public void Init(int itemIndex, int cost, string name, string description, Sprite icon, bool locked = false)
    {
        // setup states
        m_itemIndex = itemIndex;
        m_name.text = name;
        m_description.text = m_locked ? "Locked" : description;
        m_cost.text = $"Cost:{cost.ToString()}";
        m_icon.sprite = icon;
        // disable option is locked
        SetLocked(locked);
    }
    public void SetLocked(bool locked) 
    {
        m_locked = locked;
        m_toggle.interactable = !locked;
        m_toggle.isOn = !locked;
        if (locked)
        {
            m_anim.SetTrigger("Disabled");
            m_description.text = "Locked";
        }
        else 
        {
            m_anim.SetTrigger("Normal");
        }
    }
    public void OnItemToggled(bool isOn) 
    {
        s_chosenItemIndex = isOn? m_itemIndex : -1;
    }
}