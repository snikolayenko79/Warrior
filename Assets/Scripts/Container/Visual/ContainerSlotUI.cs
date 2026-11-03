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
    
    private IContainerItemView _itemView;
    public IContainerItemView ItemView => _itemView;
    
    private ContainerWindowUI.OnClickDelegate _onClick;
    
    public void Setup(IContainerItem item, IContainerItemView itemView, ContainerWindowUI.OnClickDelegate onClick)
    {
        _item = item;
        _onClick = onClick;
        _itemView = itemView;
        if (_itemView != null)
            _itemView.ItemData = item;
        if (_nameText)
            _nameText.text = item.Name;
        if (_iconImage)
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
        if (_onClick != null)
            _onClick(this);
    }
}
