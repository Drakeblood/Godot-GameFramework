using Godot;
using System;
using System.Collections.Generic;

namespace GameFramework.GameplayTags
{
    public partial class GameplayTag : GodotObject
    {
        [Export]
        private string tagName;
        public string TagName { get => tagName; }

        private int tagId = -1;
        public int TagId { get => tagId; }

        internal GameplayTag(string stateName, int stateId)
        {
            this.tagName = stateName;
            this.tagId = stateId;
        }

        /// <summary>
        /// "A.B".MatchesTag("A") = True
        /// </summary>
        public bool MatchesTag(GameplayTag tagToCheck)
        {
            GameplayTag[] separatedTag = GameplayTagsManager.GetSeparatedTag(this);
            GameplayTag[] separatedTagToCheck = GameplayTagsManager.GetSeparatedTag(tagToCheck);

            if (separatedTag.Length == separatedTagToCheck.Length) return this == tagToCheck;
            if (separatedTagToCheck.Length > separatedTag.Length) return false;

            for (int i = 0; i < separatedTag.Length && i < separatedTagToCheck.Length; i++)
            {
                if (separatedTag[i] != separatedTagToCheck[i]) return false;
            }

            if (separatedTagToCheck.Length == 0 && separatedTag.Length > 0) return separatedTag[0] == tagToCheck;
            
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

            return tagId == other.tagId;
        }

        public static bool Equals(GameplayTag x, GameplayTag y)
        {
            if ((object)x == (object)y) return true;
            if (x is null || y is null) return false;
            return x.Equals(y);
        }

        public static bool operator ==(GameplayTag x, GameplayTag y) => Equals(x, y);
        public static bool operator !=(GameplayTag x, GameplayTag y) => !Equals(x, y);

        public override int GetHashCode() => tagId;
        public override string ToString() => tagName;

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            if(tagName == null || tagName == "") return;

            GameplayTag tag;           
            if (TagId != -1)
            {
                tag = GameplayTagsManager.GetTag(tagId);
                if(tag != null)
                {
                    if (tag.tagName == tagName) return;
                }
            }
            
            tag = GameplayTagsManager.GetTag(tagName);
            if (tag == null) return;
            
            tagId = tag.tagId;
        }
    }
}