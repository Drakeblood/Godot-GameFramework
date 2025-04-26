#if TOOLS
using Godot;

using GameFramework.GameplayTags;

public partial class GameplayTagDeleteButton : Button
{
    private VBoxContainer itemsContainer;
    private HBoxContainer hBoxContainer;
    private GameplayTagOptionButton optionButton;
    private GameplayTagContainer editedValue;

    public GameplayTagDeleteButton() : base()
    {
        Pressed += OnPressed;
    }

    public GameplayTagDeleteButton(VBoxContainer _itemsContainer, HBoxContainer _hBoxContainer, GameplayTagOptionButton _optionButton, GameplayTagContainer _editedValue) : this()
    {
        itemsContainer = _itemsContainer;
        hBoxContainer = _hBoxContainer;
        optionButton = _optionButton;
        editedValue = _editedValue;
    }

    private void OnPressed()
    {
        editedValue.RemoveTag(GameplayTagsManager.Instance.GetTag(optionButton.GetItemText(optionButton.Selected)));
        itemsContainer.RemoveChild(hBoxContainer);
        hBoxContainer.QueueFree();
    }
}
#endif