using System.Collections.Generic;

using Godot;

using GameFramework.Assertion;

namespace GameFramework.GameplayTags
{
    public partial class GameplayTagsManager : GodotObject
    {
        private static GameplayTagsManager instance;
        public static GameplayTagsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameplayTagsManager();
                    instance.InitializeTags();
                }
                return instance;
            }
        }

        private HashSet<GameplayTag> tags;
        public HashSet<GameplayTag> Tags => tags;

        private Dictionary<GameplayTag, List<GameplayTag>> tagsWithSubTags;

        private void InitializeTags()
        {
            string[] gameplayTagsFilesPaths = ProjectSettings.GetSetting("application/game_framework/gameplay_tags_files").AsStringArray();
            List<StringName> tagsNames = new List<StringName>();

            for (int i = 0; i <  gameplayTagsFilesPaths.Length; i++)
            {
                FileAccess file = FileAccess.Open(gameplayTagsFilesPaths[i], FileAccess.ModeFlags.Read);
                Assert.IsTrue(file != null || file.IsOpen(), string.Format("Could not load {0} file", gameplayTagsFilesPaths[i]));

                while (file.GetPosition() < file.GetLength())
                {
                    tagsNames.Add(file.GetLine());
                }
            }

            tags = new HashSet<GameplayTag>(tagsNames.Count);
            tagsWithSubTags = new Dictionary<GameplayTag, List<GameplayTag>>(tagsNames.Count);

            for (int i = 0; i < tagsNames.Count; i++)
            {
                List<GameplayTag> parentTags = new List<GameplayTag>();
                {
                    string tag = tagsNames[i];
                    for (int index = tag.LastIndexOf('.'); index != -1; index = tag.LastIndexOf('.'))
                    {
                        tag = tag[..index];
                        parentTags.Add(GetTag(tag));
                    }
                }

                List<GameplayTag> parentTagsArray = new List<GameplayTag>(parentTags.Count);
                {
                    for (int j = parentTags.Count - 1; j >= 0; j--)
                    {
                        parentTagsArray.Add(parentTags[j]);
                    }
                }

                GameplayTag gameplayTag = new GameplayTag(tagsNames[i]);
                tags.Add(gameplayTag);
                tagsWithSubTags.Add(gameplayTag, parentTagsArray);
            }
        }

        public GameplayTag GetTag(StringName tagName)
        {
            Assert.IsTrue(tags.TryGetValue(new GameplayTag(tagName), out GameplayTag result));
            return result;
        }

        public List<GameplayTag> GetSeparatedTag(GameplayTag tag)
        {
            Assert.IsTrue(tagsWithSubTags.TryGetValue(tag, out List<GameplayTag> result));
            return result;
        }
    }
}
