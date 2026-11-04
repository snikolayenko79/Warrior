using UnityEngine;

public interface IContainerItemView
{
    IContainerItem ItemData { get; set; }
    IContainerInteractLogic InteractLogic { get; }
}