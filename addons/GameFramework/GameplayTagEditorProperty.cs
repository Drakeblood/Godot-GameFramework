#if TOOLS
using System.Collections.Generic;
using GameFramework.GameplayTags;
using Godot;

public partial class GameplayTagEditorProperty : EditorProperty
{
    private OptionButton propertyControl = new OptionButton();
    private GameplayTag currentValue;
    private bool updating = false;
    private static Dictionary<StringName, int> tagNames;

    public GameplayTagEditorProperty()
    {
        if (tagNames == null)
        {
            tagNames = new Dictionary<StringName, int>();
            int index = 0;

            foreach (var tag in GameplayTagsManager.Instance.Tags)
            {
                tagNames.Add(tag.TagName, index++);
            }
        }

        foreach (var tag in GameplayTagsManager.Instance.Tags)
        {
            propertyControl.AddItem(tag.TagName);
        }
        
        propertyControl.ItemSelected += OnItemSelected;

        AddChild(propertyControl);
        AddFocusable(propertyControl);
    }

    public override void _EnterTree()
    {
        currentValue = GameplayTagsManager.Instance.GetTag(GetEditedObject().Get(GetEditedProperty()).AsGodotObject().Get("tagName").ToString());
        RefreshControlText();
        propertyControl.Select(tagNames[currentValue.TagName]);
    }

    private void OnItemSelected(long index)
    {
        if (updating) { return; }

        currentValue = GameplayTagsManager.Instance.GetTag(propertyControl.GetItemText((int)index));
        RefreshControlText();
        EmitChanged(GetEditedProperty(), currentValue);
    }

    public override void _UpdateProperty()
    {
        var newValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTag;
        if (newValue == null || newValue == currentValue) { return; }

        updating = true;
        currentValue = newValue;
        RefreshControlText();
        updating = false;
    }

    private void RefreshControlText()
    {
        propertyControl.Text = currentValue?.TagName;
    }
}
#endif