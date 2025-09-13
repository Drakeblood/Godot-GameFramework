#if TOOLS
using System.Collections.Generic;

using Godot;

using GameFramework.GameplayTags;

public partial class GameplayTagEditorProperty : EditorProperty
{
    private OptionButton propertyControl = new OptionButton();
    private GameplayTag currentValue;
    private bool updating = false;
    public static Dictionary<StringName, int> TagNames;

    public GameplayTagEditorProperty()
    {
        InitTagNames();

        foreach (var tag in GameplayTagsManager.Instance.Tags)
        {
            propertyControl.AddItem(tag.TagName);
        }
        
        propertyControl.ItemSelected += OnItemSelected;

        AddChild(propertyControl);
        AddFocusable(propertyControl);
    }

    public static void InitTagNames()
    {
        if (TagNames == null)
        {
            TagNames = new Dictionary<StringName, int>();
            int index = 0;

            foreach (var tag in GameplayTagsManager.Instance.Tags)
            {
                TagNames.Add(tag.TagName, index++);
            }
        }
        else if (TagNames.Count != GameplayTagsManager.Instance.Tags.Count)
        {
            TagNames = null;
            InitTagNames();
        }
    }

    public override void _EnterTree()
    {
        string tagName = GetEditedObject()?.Get(GetEditedProperty()).AsGodotObject()?.Get("tagName").ToString();

        currentValue = !string.IsNullOrEmpty(tagName) 
            ? GameplayTagsManager.Instance.GetTag(tagName) 
            : GameplayTagsManager.Instance.GetTag(propertyControl.GetItemText(0));

        propertyControl.Select(TagNames[currentValue.TagName]);
    }

    private void OnItemSelected(long index)
    {
        if (updating) { return; }

        currentValue = GameplayTagsManager.Instance.GetTag(propertyControl.GetItemText((int)index));
        EmitChanged(GetEditedProperty(), currentValue);
    }

    public override void _UpdateProperty()
    {
        var newValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTag;
        if (newValue == null || newValue == currentValue) { propertyControl.Select(TagNames[currentValue.TagName]); return; }

        updating = true;
        currentValue = newValue;
        updating = false;
    }
}
#endif