#if TOOLS
using Godot;

using GameFramework.GameplayTags;

public partial class GameplayTagContainerEditorProperty : EditorProperty
{
    private VBoxContainer vBoxContainer = new VBoxContainer();
    private VBoxContainer itemsContainer = new VBoxContainer();
    public GameplayTagContainer currentValue;

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
        currentValue = new GameplayTagContainer();

        using (var enumerator = GetEditedObject().Get(GetEditedProperty()).AsGodotObject().Get("GameplayTags").AsGodotArray().GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                currentValue.GameplayTags.Add(GameplayTagsManager.Instance.GetTag(((Resource)enumerator.Current).Get("tagName").ToString()));
            }
        }

        for (int i = 0; i < currentValue.GameplayTags.Count; i++)
        {
            GameplayTagOptionButton optionButton = GetOptionButton();

            optionButton.ItemSelected += OnItemSelected;
            optionButton.Select(GameplayTagEditorProperty.TagNames[currentValue.GameplayTags[i].TagName]);

            Button deleteButton = new Button();
            deleteButton.Text = " - ";
            deleteButton.Pressed += DeleteButton_Pressed;

            HBoxContainer hBoxContainer = new HBoxContainer();
            hBoxContainer.AddChild(optionButton);
            hBoxContainer.AddChild(deleteButton);

            itemsContainer.AddChild(hBoxContainer);
        }
    }

    public override void _UpdateProperty()
    {
        //var newValue = GetEditedObject().Get(GetEditedProperty()).AsGodotObject() as GameplayTagContainer;
        //if (newValue == null || newValue == currentValue) { return; }

        //Array<Node> children = itemsContainer.GetChildren();
        //for (int i = 0; i < children.Count; i++)
        //{
        //    itemsContainer.RemoveChild(children[i]);
        //}
    }

    private void OnItemSelected(long index)
    {
        EmitChanged(GetEditedProperty(), currentValue);
    }

    private void OnAddElementButtonPressed()
    {
        GameplayTagOptionButton optionButton = GetOptionButton();

        optionButton.ItemSelected += OnItemSelected;
        optionButton.Select(-1);

        Button deleteButton = new Button();
        deleteButton.Pressed += DeleteButton_Pressed;
        deleteButton.Text = " - ";

        HBoxContainer hBoxContainer = new HBoxContainer();
        hBoxContainer.AddChild(optionButton);
        hBoxContainer.AddChild(deleteButton);

        itemsContainer.AddChild(hBoxContainer);
    }

    private void DeleteButton_Pressed()
    {
        
    }

    private GameplayTagOptionButton GetOptionButton()
    {
        GameplayTagOptionButton optionButton = new GameplayTagOptionButton(currentValue);

        using (var enumerator = GameplayTagsManager.Instance.Tags.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                optionButton.AddItem(enumerator.Current.TagName);
            }
        }

        return optionButton;
    }
}
#endif