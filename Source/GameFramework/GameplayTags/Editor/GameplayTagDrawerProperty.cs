#if TOOLS
using GameFramework.GameplayTags;
using Godot;

public partial class GameplayTagDrawerProperty : EditorProperty
{
    private OptionButton _propertyControl = new OptionButton();
    private GameplayTag _currentValue;
    private bool _updating = false;

    public GameplayTagDrawerProperty()
    {
        foreach (var tag in GameplayTagsManager.Instance.Tags)
        {
            _propertyControl.AddItem(tag.TagName);
        }

        _propertyControl.ItemSelected += OnItemSelected;

        AddChild(_propertyControl);
        AddFocusable(_propertyControl);
    }

    public override void _EnterTree()
    {
        _currentValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTag;
        RefreshControlText();
    } 

    private void OnItemSelected(long index)
    {
        if (_updating) { return; }

        _currentValue = GameplayTagsManager.Instance.GetTag(_propertyControl.GetItemText((int)index));
        RefreshControlText();
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    public override void _UpdateProperty()
    {
        var newValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTag;
        if (newValue == null || newValue == _currentValue) { return; }

        _updating = true;
        _currentValue = newValue;
        RefreshControlText();
        _updating = false;
    }

    private void RefreshControlText()
    {
        _propertyControl.Text = _currentValue?.TagName;
    }
}
#endif