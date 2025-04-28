#if TOOLS
using Godot;

using GameFramework.GameplayTags;

public partial class GameplayTagOptionButton : OptionButton
{
    private GameplayTagContainer editedValue;
    private GameplayTag selectedTag;

    public GameplayTagOptionButton() : base()
    {
        ItemSelected += OnItemSelected;
    }

    public GameplayTagOptionButton(GameplayTagContainer _editedValue, GameplayTag initialSelectedTag = null) : this()
    {
        editedValue = _editedValue;
        selectedTag = initialSelectedTag;
    }

    private void OnItemSelected(long index)
    {
        int addedIndex = editedValue.AddTag(GameplayTagsManager.Instance.GetTag(GetItemText((int)index)));

        if (addedIndex != -1)
        {
            if (selectedTag != null)
            {
                editedValue.RemoveTag(selectedTag);
            }
            selectedTag = editedValue.GameplayTags[editedValue.GameplayTags.Count - 1];
        }
        else { Select(selectedTag != null ? GameplayTagEditorProperty.TagNames[selectedTag.TagName] : -1); }
    }
}
#endif