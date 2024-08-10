using System;
using System.IO;
using System.Collections.Generic;
using Godot;
using GameFramework.Assertion;

namespace GameFramework.GameplayTags
{
    public static class GameplayTagsManager
    {
        private static string[] tagsNames;
        public static string[] TagsNames => tagsNames;

        private static GameplayTag[] tags = new GameplayTag[0];
        private static Tuple<GameplayTag, GameplayTag[]>[] tagsWithSubTags = new Tuple<GameplayTag, GameplayTag[]>[0];

#if TOOLS
        static GameplayTagsManager()
        {
            //ConfigFile tags = new ConfigFile();
            //Assert.AreEqual(tags.Load("res://Content/GameFramework/DefaultGameplayTags.ini"), Error.Ok, "Could not load DefaultGameplayTags.ini file");

            //tagsNames = tags.GetSectionKeys("GameplayTagList");

            //InitializeTags();
        }
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadAsset()
        {
            UnityEngine.TextAsset TagsAsset = UnityEngine.Resources.Load<UnityEngine.TextAsset>(ProjectStatics.GameplayTagsAssetPath);
            if (TagsAsset == null) return;

            tagsNames = TagsAsset.text.Split('\n');

            for (int i = 0; i < tagsNames.Length; i++)
            {
                tagsNames[i] = tagsNames[i].Split(',')[0];
            }
            InitializeTags();
        }
#endif

        private static void InitializeTags()
        {
            tags = new GameplayTag[tagsNames.Length];
            tagsWithSubTags = new Tuple<GameplayTag, GameplayTag[]>[tagsNames.Length];

            for (int i = 0; i < tagsNames.Length; i++)
            {
                List<GameplayTag> parentTags = new();
                {
                    string tag = tagsNames[i];
                    for (int index = tag.LastIndexOf('.'); index != -1; index = tag.LastIndexOf('.'))
                    {
                        tag = tag[..index];
                        parentTags.Add(GetTag(tag));
                    }
                }

                GameplayTag[] parentTagsArray = new GameplayTag[parentTags.Count];
                {
                    int n = 0;
                    for (int j = parentTags.Count - 1; j >= 0; j--)
                    {
                        parentTagsArray[n++] = parentTags[j];
                    }
                }

                tags[i] = new GameplayTag(tagsNames[i], i);
                tagsWithSubTags[i] = new Tuple<GameplayTag, GameplayTag[]>(tags[i], parentTagsArray);
            }
        }

        public static GameplayTag GetTag(string tagName)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i].TagName == tagName)
                {
                    return tags[i];
                }
            }
            return null;
        }

        public static GameplayTag GetTag(int id)
        {
            if (tags.Length > id)
            {
                return tags[id];
            }
            return null;
        }

        public static GameplayTag[] GetTags() => tags;
        public static int GetTagsCount() => tags.Length;

        public static GameplayTag[] GetSeparatedTag(GameplayTag tag)
        {
            for (int i = 0; i < tagsWithSubTags.Length; i++)
            {
                if (tagsWithSubTags[i].Item1.TagName == tag.TagName)
                {
                    return tagsWithSubTags[i].Item2;
                }
            }
            return new GameplayTag[0];
        }
    }
}
