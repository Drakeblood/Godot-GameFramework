using System.Collections.Generic;

using Godot;

namespace GameFramework.GameplayTags
{
    [GlobalClass]
    public partial class GameplayTag : Resource
    {
        [Export]
        private StringName tagName;
        public StringName TagName { get => tagName; }

        public GameplayTag() { }

        public GameplayTag(StringName tagName)
        {
            this.tagName = tagName;
        }

        /// <summary>
        /// "A.B".MatchesTag("A") = True
        /// </summary>
        public bool MatchesTag(GameplayTag tagToCheck)
        {
            if (this == tagToCheck) { return true; }
            return MatchesSubTags(tagToCheck);
        }

        public bool MatchesSubTags(GameplayTag tagToCheck)
        {
            List<GameplayTag> separatedTag = GameplayTagsManager.Instance.GetSeparatedTag(this);
            List<GameplayTag> separatedTagToCheck = GameplayTagsManager.Instance.GetSeparatedTag(tagToCheck);

            if (separatedTagToCheck.Count > separatedTag.Count) return false;

            for (int i = 0; i < separatedTag.Count && i < separatedTagToCheck.Count; i++)
            {
                if (separatedTag[i] != separatedTagToCheck[i]) return false;
            }

            if (separatedTagToCheck.Count == 0 && separatedTag.Count > 0) return separatedTag[0] == tagToCheck;

            return true;
        }

        public override bool Equals(object obj)
        {
            GameplayTag other = obj as GameplayTag;
            if (other == null) return base.Equals(obj);

            return tagName == other.tagName;
        }

        public static bool Equals(GameplayTag x, GameplayTag y)
        {
            if ((object)x == (object)y) return true;
            if (x is null || y is null) return false;
            return x.Equals(y);
        }

        public static bool operator ==(GameplayTag x, GameplayTag y) => Equals(x, y);
        public static bool operator !=(GameplayTag x, GameplayTag y) => !Equals(x, y);

        public override int GetHashCode() => tagName.GetHashCode();
        public override string ToString() => tagName;
    }
}