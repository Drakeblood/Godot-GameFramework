using System.Collections.Generic;

using Godot;

namespace GameFramework.GameplayTags
{
    public partial class GameplayTag : GodotObject
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
            List<GameplayTag> separatedTag = GameplayTagsManager.Instance.GetSeparatedTag(this);
            List<GameplayTag> separatedTagToCheck = GameplayTagsManager.Instance.GetSeparatedTag(tagToCheck);

            if (separatedTag.Count == separatedTagToCheck.Count) return this == tagToCheck;
            if (separatedTagToCheck.Count > separatedTag.Count) return false;

            for (int i = 0; i < separatedTag.Count && i < separatedTagToCheck.Count; i++)
            {
                if (separatedTag[i] != separatedTagToCheck[i]) return false;
            }

            if (separatedTagToCheck.Count == 0 && separatedTag.Count > 0) return separatedTag[0] == tagToCheck;
            
            return true;
        }

        public static bool HasAny(GameplayTag[] tags1, GameplayTag[] tags2)
        {
            if (tags2.Length < 1) return false;

            for (int i = 0; i < tags2.Length; i++)
            {
                for (int j = 0; j < tags1.Length; j++)
                {
                    if (tags1[j].MatchesTag(tags2[i])) return true;
                }
            }
            return false;
        }

        public static bool HasAll(GameplayTag[] tags1, GameplayTag[] tags2)
        {
            if(tags2.Length < 1) return true;

            for (int i = 0; i < tags2.Length; i++)
            {
                for (int j = 0; j < tags1.Length; j++)
                {
                    if (!tags1[j].MatchesTag(tags2[i])) return false;
                }
            }
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