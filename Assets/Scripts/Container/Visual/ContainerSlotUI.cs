using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContainerSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;    
    [SerializeField] private Button _clickButton;

    private IContainerItem _item;
    public IContainerItem Item => _item;
    
    private ContainerWindowUI.OnClickDelegate _onClick;
    
    public void Setup(IContainerItem item, ContainerWindowUI.OnClickDelegate onClick)
    {
        _item = item;
        _onClick = onClick;        
        _nameText.text = item.Name;
        _iconImage.sprite = item.Icon;

        // Подписываемся на клик кнопки
        _clickButton.onClick.RemoveAllListeners();
        _clickButton.onClick.AddListener(OnClicked);

        OnSlotSetup();
    }
    
    protected virtual void OnSlotSetup() {}
    
    public void SetAccessibility(bool bAccessible)
    {
        if (_clickButton)
            _clickButton.interactable = bAccessible;
    }
    
    private void OnClicked()
    {
        if (_onClick != null && _item != null)
        {
            _onClick(_item);
        }
    }
}
