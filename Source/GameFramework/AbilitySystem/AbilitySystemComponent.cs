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
        [Export] private Array<GameplayAbility> startupAbilities = new Array<GameplayAbility>();
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

        public void CancelAbilitiesWithTags(GameplayTagContainer tagContainer)
        {
            if (tagContainer.Length == 0) return;

            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (!ActivatableAbilities[i].IsActive) continue;

                if (ActivatableAbilities[i].AbilityTags.HasAny(tagContainer))
                {
                    ActivatableAbilities[i].EndAbility(true);
                }
            }
        }

        #endregion

        #region Effects

        public void ApplyGameplayEffectToSelf(GameplayEffect effectTemplate)
        {
            //Assert.IsNotNull(effectTemplate, "Effect template is not vaild");

            //GameplayEffect ability = effectTemplate.Duplicate() as GameplayEffect;
        }

        #endregion

        #region GameplayTags

        public GameplayTagContainer GetOwnedGameplayTags() => GameplayTagCountContainer.ExplicitTags;
        public GameplayTagContainer GetBlockedAbilityTags() => BlockedAbilityTags.ExplicitTags;

        public void UpdateTagMap(GameplayTag tag, int countDelta)
        {
            if (GameplayTagCountContainer.UpdateTagCount(tag, countDelta))
            {
                OnTagUpdated(tag, countDelta > 0);
            }
        }

        public virtual void OnTagUpdated(GameplayTag tag, bool tagExists) { }

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

        public void AbilityLocalInputPressed(StringName actionName)
        {
            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (actionName == ActivatableAbilities[i].InputActionName)
                {
                    ActivatableAbilities[i].IsInputPressed = true;

                    if (ActivatableAbilities[i].IsActive)
                    {
                        ActivatableAbilities[i].InputPressed();
                    }
                    else
                    {
                        TryActivateAbility(ActivatableAbilities[i].GetType());
                    }
                }
            }
        }

        public void AbilityLocalInputReleased(StringName actionName)
        {
            for (int i = 0; i < ActivatableAbilities.Count; i++)
            {
                if (actionName == ActivatableAbilities[i].InputActionName)
                {
                    ActivatableAbilities[i].IsInputPressed = false;

                    if (ActivatableAbilities[i].IsActive)
                    {
                        ActivatableAbilities[i].InputReleased();
                    }
                }
            }
        }

        #endregion
    }
}