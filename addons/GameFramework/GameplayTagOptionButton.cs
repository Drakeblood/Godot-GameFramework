#if TOOLS
using Godot;

using GameFramework.GameplayTags;

public partial class GameplayTagOptionButton : OptionButton
{
    private GameplayTagContainer editedValue;
    private int oldSelected;

    public GameplayTagOptionButton() : base()
    {
        ItemSelected += OnItemSelected;
    }

    public GameplayTagOptionButton(GameplayTagContainer _editedValue) : this()
    {
        editedValue = _editedValue;
    }

    public new void Select(int idx)
    {
        base.Select(idx);
        oldSelected = idx;
    }

    private void OnItemSelected(long index)
    {
        if (editedValue.GameplayTags.Count <= GetIndex())
        {
            if (!editedValue.AddTag(GameplayTagsManager.Instance.GetTag(GetItemText((int)index))))
            {
                Select(-1);
            }
        }
        else
        {
            if (oldSelected != -1)
            {
                editedValue.RemoveTag(GameplayTagsManager.Instance.GetTag(GetItemText(oldSelected)));
            }
            if (!editedValue.AddTag(GameplayTagsManager.Instance.GetTag(GetItemText((int)index))))
            {
                Select(-1);
            }
        }

        oldSelected = (int)index;
    }
}
#endif