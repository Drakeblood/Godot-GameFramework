using Godot;
using System;
using System.Collections.Generic;

namespace GameFramework.GameplayTags
{
    public delegate void GameplayTagDelegate(GameplayTag tag, int newCount);

    public class GameplayTagCountContainer
    {
        public Dictionary<GameplayTag, int> GameplayTagCountArray = new();
        public List<GameplayTag> ExplicitGameplayTags = new();

        protected Dictionary<GameplayTag, GameplayTagDelegate> GameplayTagEventArray = new();
        
        public bool UpdateTagCount(GameplayTag tag, int countDelta)
        {
            if (countDelta == 0) return false;

            if (GameplayTagCountArray.ContainsKey(tag))
            {
                GameplayTagCountArray[tag] = Math.Max(GameplayTagCountArray[tag] + countDelta, 0);
            }
            else
            {
                GameplayTagCountArray.Add(tag, Math.Max(countDelta, 0));
            }

            if (ExplicitGameplayTags.Contains(tag))
            {
                if (GameplayTagCountArray[tag] == 0)
                {
                    ExplicitGameplayTags.Remove(tag);
                }
            }
            else if (GameplayTagCountArray[tag] != 0)
            {
                ExplicitGameplayTags.Add(tag);
            }

            if (GameplayTagEventArray.ContainsKey(tag))
            {
                List<GameplayTagDelegate> invalidDelegates = null;
                Delegate[] delegates = GameplayTagEventArray[tag].GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    if (delegates[i] is not GameplayTagDelegate tagDelegate) continue;

                    //if (tagDelegate.Target is MonoBehaviour behaviour)
                    //{
                    //    if (behaviour == null)
                    //    {
                    //        invalidDelegates ??= new List<GameplayTagDelegate>();
                    //        invalidDelegates.Add(tagDelegate);
                    //        continue;
                    //    }
                    //}

                    tagDelegate.Invoke(tag, GameplayTagCountArray[tag]);
                }

                if (invalidDelegates == null) return true;

                for (int i = 0; i < invalidDelegates.Count; i++)
                {
                    GameplayTagEventArray[tag] -= invalidDelegates[i];
                }
            }

            return true;
        }

        public void RegisterGameplayTagEvent(GameplayTag tag, GameplayTagDelegate tagDelegate)
        {
            if (!GameplayTagEventArray.ContainsKey(tag))
            {
                GameplayTagEventArray.Add(tag, tagDelegate);
                return;
            }

            GameplayTagEventArray[tag] += tagDelegate;
        }
    }
}