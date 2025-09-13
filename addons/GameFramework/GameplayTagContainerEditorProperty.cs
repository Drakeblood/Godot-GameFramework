#if TOOLS
using Godot;
using Godot.Collections;

using GameFramework.GameplayTags;

public partial class GameplayTagContainerEditorProperty : EditorProperty
{
    private VBoxContainer vBoxContainer = new VBoxContainer();
    private VBoxContainer itemsContainer = new VBoxContainer();
    public GameplayTagContainer currentValue;

    private bool enteredTree;

    public GameplayTagContainerEditorProperty()
    {
        GameplayTagEditorProperty.InitTagNames();

        vBoxContainer.AddChild(itemsContainer);
        
        Button addElementButton = new Button();
        addElementButton.Text = "Add tag";
        addElementButton.Pressed += OnAddElementButtonPressed;
        vBoxContainer.AddChild(addElementButton);

        AddChild(vBoxContainer);
    }

    public override void _EnterTree()
    {
        enteredTree = true;
        GameplayTagContainer tagContainer = new GameplayTagContainer();

        using (var enumerator = GetEditedObject()?.Get(GetEditedProperty()).AsGodotObject()?.Get("GameplayTags").AsGodotArray().GetEnumerator())
        {
            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    tagContainer.AddTag(GameplayTagsManager.Instance.GetTag(((Resource)enumerator.Current).Get("tagName").ToString()));
                }
            }
        }

        if (tagContainer.Length <= 0) { return; }
        currentValue = tagContainer;

        for (int i = 0; i < currentValue.Length; i++)
        {
            GameplayTagOptionButton optionButton = GetOptionButton(currentValue.GameplayTags[i]);
            SetupRowAttachment(optionButton);
        }
    }

    public override void _UpdateProperty()
    {
        var newValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTagContainer;
        if (enteredTree) { enteredTree = false; return; }
        if (newValue == currentValue) { return; }

        Array<Node> children = itemsContainer.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            itemsContainer.RemoveChild(children[i]);
            children[i].QueueFree();
        }
    }

    private void OnItemSelected(long index)
    {
        EmitChanged(GetEditedProperty(), currentValue);
    }

    private void OnAddElementButtonPressed()
    {
        currentValue ??= new GameplayTagContainer();
        GameplayTagOptionButton optionButton = GetOptionButton();
        SetupRowAttachment(optionButton);
    }

    private void SetupRowAttachment(GameplayTagOptionButton optionButton)
    {
        HBoxContainer hBoxContainer = new HBoxContainer();
        hBoxContainer.AddChild(optionButton);

        GameplayTagDeleteButton deleteButton = new GameplayTagDeleteButton(itemsContainer, hBoxContainer, optionButton, currentValue);
        deleteButton.Text = " - ";
        deleteButton.Pressed += () => { EmitChanged(GetEditedProperty(), currentValue); };
        hBoxContainer.AddChild(deleteButton);

        itemsContainer.AddChild(hBoxContainer);
    }

    private GameplayTagOptionButton GetOptionButton(GameplayTag initialSelectedTag = null)
    {
        GameplayTagOptionButton optionButton = new GameplayTagOptionButton(currentValue, initialSelectedTag);

        using (var enumerator = GameplayTagsManager.Instance.Tags.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                optionButton.AddItem(enumerator.Current.TagName);
            }
        }

        optionButton.Select(initialSelectedTag != null ? GameplayTagEditorProperty.TagNames[initialSelectedTag.TagName] : -1);
        optionButton.ItemSelected += OnItemSelected;
        return optionButton;
    }
}
#endif