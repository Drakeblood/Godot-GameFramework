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
        string selectedTag = optionButton.GetItemText(optionButton.Selected);
        if (!string.IsNullOrEmpty(selectedTag))
        {
            editedValue.RemoveTag(GameplayTagsManager.Instance.GetTag(selectedTag));
        }

        itemsContainer.RemoveChild(hBoxContainer);
        hBoxContainer.QueueFree();
    }
}
#endif