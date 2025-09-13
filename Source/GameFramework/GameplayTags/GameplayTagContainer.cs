using Godot;
using Godot.Collections;

namespace GameFramework.GameplayTags
{
    [GlobalClass]
    public partial class GameplayTagContainer : Resource
    {
        [Export] public Array<GameplayTag> GameplayTags { get; private set; } = new Array<GameplayTag>();

        public int Length => GameplayTags.Count;
        public GameplayTag this[int index] => GameplayTags[index];

        public void AddTag(GameplayTag tag)
        {
            if (!GameplayTags.Contains(tag))
            {
                GameplayTags.Add(tag);
            }
        }

        public void RemoveTag(GameplayTag tag)
        {
            GameplayTags.Remove(tag);
        }

        public bool HasTag(GameplayTag tagToCheck)
        {
            if (HasTagExact(tagToCheck)) { return true; }

            for (int i = 0; i < Length; i++)
            {
                if (GameplayTags[i].MatchesSubTags(tagToCheck)) { return true; }
            }

            return false;
        }

        public bool HasTagExact(GameplayTag tagToCheck) => GameplayTags.Contains(tagToCheck);

        public bool HasAny(GameplayTagContainer tagContainer)
        {
            for (int i = 0; i < tagContainer.Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (GameplayTags[j].MatchesTag(tagContainer.GameplayTags[i])) { return true; }
                }
            }
            return false;
        }

        public bool HasAll(GameplayTagContainer tagContainer)
        {
            for (int i = 0; i < tagContainer.Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (!GameplayTags[j].MatchesTag(tagContainer.GameplayTags[i])) return false;
                }
            }
            return true;
        }

        public bool HasAllExact(GameplayTagContainer tagContainer)
        {
            for (int i = 0; i < tagContainer.Length; i++)
            {
                if (!GameplayTags.Contains(tagContainer.GameplayTags[i])) return false;
            }
            return true;
        }
    }
}