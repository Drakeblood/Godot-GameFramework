using System;
using System.Collections.Generic;

using Godot;

namespace GameFramework.GameplayTags
{
    public delegate void GameplayTagDelegate(GameplayTag tag, int newCount);

    public partial class GameplayTagCountContainer : GodotObject
    {
        public Dictionary<GameplayTag, int> GameplayTagCountMap { get; private set; } = new Dictionary<GameplayTag, int>();
        //public Dictionary<GameplayTag, int> ExplicitTagCountMap = new Dictionary<GameplayTag, int>();
        public GameplayTagContainer ExplicitTags { get; private set; } = new GameplayTagContainer();
        public Dictionary<GameplayTag, GameplayTagDelegate> GameplayTagEventMap { get; private set; } = new Dictionary<GameplayTag, GameplayTagDelegate>();

        public bool UpdateTagCount(GameplayTag tag, int countDelta)
        {
            if (countDelta == 0) return false;

            if (GameplayTagCountMap.ContainsKey(tag))
            {
                GameplayTagCountMap[tag] = Math.Max(GameplayTagCountMap[tag] + countDelta, 0);
            }
            else
            {
                GameplayTagCountMap.Add(tag, Math.Max(countDelta, 0));
            }

            if (ExplicitTags.HasTag(tag))
            {
                if (GameplayTagCountMap[tag] == 0)
                {
                    ExplicitTags.RemoveTag(tag);
                }
            }
            else if (GameplayTagCountMap[tag] != 0)
            {
                ExplicitTags.AddTag(tag);
            }

            if (GameplayTagEventMap.ContainsKey(tag))
            {
                List<GameplayTagDelegate> invalidDelegates = null;
                Delegate[] delegates = GameplayTagEventMap[tag].GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    if (delegates[i] is not GameplayTagDelegate tagDelegate) continue;
                    
                    if (tagDelegate.Target == null)
                    {
                        invalidDelegates ??= new List<GameplayTagDelegate>();
                        invalidDelegates.Add(tagDelegate);
                        continue;
                    }

                    tagDelegate.Invoke(tag, GameplayTagCountMap[tag]);
                }

                if (invalidDelegates != null)
                {
                    for (int i = 0; i < invalidDelegates.Count; i++)
                    {
                        GameplayTagEventMap[tag] -= invalidDelegates[i];
                    }
                }
            }

            return true;
        }

        public void RegisterGameplayTagEvent(GameplayTag tag, GameplayTagDelegate tagDelegate)
        {
            if (!GameplayTagEventMap.ContainsKey(tag))
            {
                GameplayTagEventMap.Add(tag, tagDelegate);
                return;
            }

            GameplayTagEventMap[tag] += tagDelegate;
        }
    }
}