using System;
using System.Collections.Generic;

using Godot;
using Godot.Collections;

namespace GameFramework.GameplayTags
{
    [GlobalClass]
    public partial class GameplayTagContainer : Resource
    {
        [Export] public Array<GameplayTag> GameplayTags = new ();
        protected System.Collections.Generic.Dictionary<GameplayTag, GameplayTagDelegate> GameplayTagEventArray = new ();

        public int AddTag(GameplayTag tag)
        {
            if (!GameplayTags.Contains(tag))
            {
                GameplayTags.Add(tag);

                if (GameplayTagEventArray.ContainsKey(tag))
                {
                    List<GameplayTagDelegate> invalidDelegates = null;
                    Delegate[] delegates = GameplayTagEventArray[tag].GetInvocationList();
                    for (int i = 0; i < delegates.Length; i++)
                    {
                        if (delegates[i] is not GameplayTagDelegate tagDelegate) continue;
                        tagDelegate.Invoke(tag, 1);
                    }

                    if (invalidDelegates != null)
                    {
                        for (int i = 0; i < invalidDelegates.Count; i++)
                        {
                            GameplayTagEventArray[tag] -= invalidDelegates[i];
                        }
                    }
                }

                return GameplayTags.Count - 1;
            }

            return -1;
        }

        public bool RemoveTag(GameplayTag tag)
        {
            if (GameplayTags.Remove(tag))
            {
                if (GameplayTagEventArray.ContainsKey(tag))
                {
                    List<GameplayTagDelegate> invalidDelegates = null;
                    Delegate[] delegates = GameplayTagEventArray[tag].GetInvocationList();
                    for (int i = 0; i < delegates.Length; i++)
                    {
                        if (delegates[i] is not GameplayTagDelegate tagDelegate) continue;
                        tagDelegate.Invoke(tag, 0);
                    }

                    if (invalidDelegates == null) { return true; }

                    for (int i = 0; i < invalidDelegates.Count; i++)
                    {
                        GameplayTagEventArray[tag] -= invalidDelegates[i];
                    }
                }
                return true;
            }
            return false;
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