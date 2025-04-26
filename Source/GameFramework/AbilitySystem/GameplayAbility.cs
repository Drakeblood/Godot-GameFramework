using System.Collections;

using Godot;

using GameFramework.GameplayTags;
using GameFramework.Assertion;

namespace GameFramework.AbilitySystem
{
    public partial class GameplayAbility : Resource
    {
        public AbilitySystemComponent AbilitySystemComponent { get; private set; }

        public static long IDCounter = 0;
        public long AbilityID = -1;

        public bool IsActive { get; private set; }
        public bool IsInputPressed { get; set; }
        public object SourceObject { get; private set; }

        [ExportCategory("Tags")]

        /// <summary>
        /// GameplayTags that the GameplayAbility owns. These are just GameplayTags to describe the GameplayAbility.
        /// </summary>
        [Export]
        public GameplayTag[] AbilityTags;

        /// <summary>
        /// Other GameplayAbilities that have these GameplayTags in their Ability Tags will be canceled when this GameplayAbility is activated.
        /// </summary>
        [Export]
        public GameplayTag[] CancelAbilitiesWithTag;

        /// <summary>
        /// Other GameplayAbilities that have these GameplayTags in their Ability Tags are blocked from activating while this GameplayAbility is active.
        /// </summary>
        [Export]
        public GameplayTag[] BlockAbilitiesWithTag;

        /// <summary>
        /// These GameplayTags are given to the GameplayAbility's owner while this GameplayAbility is active.
        /// </summary>
        [Export]
        public GameplayTag[] ActivationOwnedTags;

        /// <summary>
        /// This GameplayAbility can only be activated if the owner has all of these GameplayTags.
        /// </summary>
        [Export]
        public GameplayTag[] ActivationRequiredTags;

        /// <summary>
        /// This GameplayAbility cannot be activated if the owner has any of these GameplayTags.
        /// </summary>
        [Export]
        public GameplayTag[] ActivationBlockedTags;

        /// <summary>
        /// This GameplayAbility can only be activated if the Source has all of these GameplayTags. The Source GameplayTags are only set if the GameplayAbility is triggered by an event.
        /// </summary>
        [Export] public GameplayTagContainer SourceRequiredTags = new GameplayTagContainer();

        /// <summary>
        /// This GameplayAbility cannot be activated if the Source has any of these GameplayTags. The Source GameplayTags are only set if the GameplayAbility is triggered by an event.
        /// </summary>
        //public GameplayTag[] SourceBlockedTags;

        /// <summary>
        /// This GameplayAbility can only be activated if the Target has all of these GameplayTags. The Target GameplayTags are only set if the GameplayAbility is triggered by an event.
        /// </summary>
        //public GameplayTag[] TargetRequiredTags;

        /// <summary>
        /// This GameplayAbility cannot be activated if the Target has any of these GameplayTags. The Target GameplayTags are only set if the GameplayAbility is triggered by an event.
        /// </summary>
        //public GameplayTag[] TargetBlockedTags;

        [ExportCategory("Input")]

        [Export]
        public StringName InputActionName { get; private set; }

        //private List<Coroutine> coroutines = new List<Coroutine>();

        public delegate void AbilityEnded(bool wasCanceled);
        public event AbilityEnded OnAbilityEnded;

        public void StartCoroutine(IEnumerator routine)
        {
            //if (AbilitySystemComponent == null) { Debug.LogError("OwningAbilityManager is not valid"); return; }

            //coroutines.Add(AbilitySystemComponent.StartCoroutine(routine));
        }

        public void SetupAbility(AbilitySystemComponent abilitySystemComponent, object sourceObject = null)
        {
            AbilitySystemComponent = abilitySystemComponent;
            SourceObject = sourceObject;

            AbilityID = IDCounter++;
        }

        public virtual void OnGiveAbility() { }

        public virtual bool CanActivateAbility()
        {
            if (IsActive) return false;
            Assert.IsNotNull(AbilitySystemComponent, "OwningAbilityManager is not valid");

            if (GameplayTag.HasAny(AbilitySystemComponent.GetBlockedAbilityTags(), AbilityTags)) return false;

            if (ActivationBlockedTags.Length > 0 || ActivationRequiredTags.Length > 0)
            {
                GameplayTag[] AbilityManagerStates = AbilitySystemComponent.GetExplicitGameplayTags();

                if (GameplayTag.HasAny(AbilityManagerStates, ActivationBlockedTags)) return false;
                if (!GameplayTag.HasAll(AbilityManagerStates, ActivationRequiredTags)) return false;
            }

            return true;
        }

        public virtual void ActivateAbility()
        {
            Assert.IsNotNull(AbilitySystemComponent, "OwningAbilityManager is not valid");

            for (int i = 0; i < ActivationOwnedTags.Length; i++)
            {
                AbilitySystemComponent.UpdateTagMap(ActivationOwnedTags[i], 1);
            }

            for (int i = 0; i < BlockAbilitiesWithTag.Length; i++)
            {
                AbilitySystemComponent.UpdateBlockedAbilityTags(BlockAbilitiesWithTag[i], 1);
            }

            AbilitySystemComponent.CancelAbilitiesWithTags(CancelAbilitiesWithTag);

            IsActive = true;
        }

        public virtual void EndAbility(bool wasCanceled = false)
        {
            Assert.IsNotNull(AbilitySystemComponent, "OwningAbilityManager is not valid");
            IsActive = false;

            for (int i = 0; i < ActivationOwnedTags.Length; i++)
            {
                AbilitySystemComponent.UpdateTagMap(ActivationOwnedTags[i], -1);
            }

            for (int i = 0; i < BlockAbilitiesWithTag.Length; i++)
            {
                AbilitySystemComponent.UpdateBlockedAbilityTags(BlockAbilitiesWithTag[i], -1);
            }

            //for (int i = 0; i < coroutines.Count; i++)
            //{
            //    AbilitySystemComponent.StopCoroutine(coroutines[i]);
            //}

            OnAbilityEnded?.Invoke(wasCanceled);
        }

        public virtual void InputPressed() { }
        public virtual void InputReleased() { }
    }
}