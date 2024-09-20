using System;
using System.Collections.Generic;

using Godot;
using Godot.Collections;

using GameFramework.GameplayTags;
using GameFramework.Assertion;

namespace GameFramework.AbilitySystem
{
    public partial class AbilitySystemComponent : Node
    {
        [Export] 
        private Array<GameplayAbility> startupAbilities = new Array<GameplayAbility>();
        protected List<GameplayAbility> ActivatableAbilities = new List<GameplayAbility>();//TO DO: Implemet AbilitySpec and AbilitySpecHandle

        protected GameplayTagCountContainer GameplayTagCountContainer = new GameplayTagCountContainer();
        protected GameplayTagCountContainer BlockedAbilityTags = new GameplayTagCountContainer();

        public override void _Ready()
        {
            for (int i = 0; i < startupAbilities.Count; i++)
            {
                GiveAbility(startupAbilities[i]);
            }
        }

        #region Abilities

        public void GiveAbility(GameplayAbility abilityTemplate, object sourceObject = null)
        {
            Assert.IsNotNull(abilityTemplate, "Ability template is not vaild");

            GameplayAbility ability = abilityTemplate.Duplicate() as GameplayAbility;
            ActivatableAbilities.Add(ability);

            //if (ability.AbilityDefinition.InputActionReference != null)
            //{
            //    ability.AbilityDefinition.InputActionReference.action.started += AbilityInputPressed;
            //    ability.AbilityDefinition.InputActionReference.action.canceled += AbilityInputReleased;
            //    ability.AbilityDefinition.InputActionReference.action.Enable();
            //}

            ability.SetupAbility(this, sourceObject);
            ability.OnGiveAbility();
        }

        public void ClearAbility(GameplayAbility ability)
        {
            Assert.IsNotNull(ability, "AbilityDefinition is not vaild");

            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (ActivatableAbilities[i] == ability)
                {
                    if (ActivatableAbilities[i].IsActive)
                    {
                        ActivatableAbilities[i].OnAbilityEnded += (bool wasCanceled) =>
                        {
                            ClearAbility(ability);
                        };
                        return;
                    }

                    //if (ActivatableAbilities[i].AbilityDefinition.InputActionReference != null)
                    //{
                    //    ActivatableAbilities[i].AbilityDefinition.InputActionReference.action.started -= AbilityInputPressed;
                    //    ActivatableAbilities[i].AbilityDefinition.InputActionReference.action.canceled -= AbilityInputReleased;
                    //    ActivatableAbilities[i].AbilityDefinition.InputActionReference.action.Disable();
                    //}

                    ActivatableAbilities.RemoveAt(i);
                    return;
                }
            }
        }

        public bool TryActivateAbility(Type abilityClass)
        {
            Assert.IsNotNull(abilityClass, "AbilityClass is not vaild");

            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (ActivatableAbilities[i].GetType() == abilityClass)
                {
                    if (!ActivatableAbilities[i].CanActivateAbility()) return false;

                    ActivatableAbilities[i].ActivateAbility();
                    return true;
                }
            }
            return false;
        }

        public void CancelAbilitiesWithTags(GameplayTag[] tags)
        {
            if (tags.Length == 0) return;

            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (!ActivatableAbilities[i].IsActive) continue;

                if (GameplayTag.HasAny(ActivatableAbilities[i].AbilityTags, tags))
                {
                    ActivatableAbilities[i].EndAbility(true);
                }
            }
        }

        #endregion

        #region GameplayTags

        public GameplayTag[] GetExplicitGameplayTags() => GameplayTagCountContainer.ExplicitGameplayTags.ToArray();
        public GameplayTag[] GetBlockedAbilityTags() => BlockedAbilityTags.ExplicitGameplayTags.ToArray();

        public void UpdateTagMap(GameplayTag tag, int countDelta)
        {
            if (GameplayTagCountContainer.UpdateTagCount(tag, countDelta))
            {
                //OnTagUpdated
            }
        }

        public void RegisterGameplayTagEvent(GameplayTag tag, GameplayTagDelegate tagDelegate)
        {
            GameplayTagCountContainer.RegisterGameplayTagEvent(tag, tagDelegate);
        }

        public void UpdateBlockedAbilityTags(GameplayTag tag, int countDelta)
        {
            BlockedAbilityTags.UpdateTagCount(tag, countDelta);
        }

        #endregion

        #region Input

        //public void AbilityInputPressed(CallbackContext callbackContext)
        //{
        //    for (int i = 0; i < ActivatableAbilities.Count; i++)
        //    {
        //        if (ActivatableAbilities[i].InputActionReference.action == callbackContext.action)
        //        {
        //            ActivatableAbilities[i].IsInputPressed = true;

        //            if (ActivatableAbilities[i].IsActive)
        //            {
        //                ActivatableAbilities[i].InputPressed();
        //            }
        //            else
        //            {
        //                TryActivateAbility(ActivatableAbilities[i].GetType());
        //            }
        //        }
        //    }
        //}

        //public void AbilityInputReleased(CallbackContext callbackContext)
        //{
        //    for (int i = 0; i < ActivatableAbilities.Count; i++)
        //    {
        //        if (ActivatableAbilities[i].InputActionReference.action == callbackContext.action)
        //        {
        //            ActivatableAbilities[i].IsInputPressed = false;

        //            if (ActivatableAbilities[i].IsActive)
        //            {
        //                ActivatableAbilities[i].InputReleased();
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}