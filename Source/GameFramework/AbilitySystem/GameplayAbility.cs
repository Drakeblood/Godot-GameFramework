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
        /// This ability has these tags
        /// </summary>
        [Export] public GameplayTagContainer AbilityTags;

        /// <summary>
        /// Abilities with these tags are cancelled when this ability is executed
        /// </summary>
        [Export] public GameplayTagContainer CancelAbilitiesWithTag;

        /// <summary>
        /// Abilities with these tags are blocked while this ability is active
        /// </summary>
        [Export] public GameplayTagContainer BlockAbilitiesWithTag;

        /// <summary>
        /// Tags to apply to activating owner while this ability is active.
        /// </summary>
        [Export] public GameplayTagContainer ActivationOwnedTags;

        /// <summary>
        /// This ability can only be activated if the activating actor/component has all of these tags
        /// </summary>
        [Export] public GameplayTagContainer ActivationRequiredTags;

        /// <summary>
        /// This ability is blocked if the activating actor/component has any of these tags
        /// </summary>
        [Export] public GameplayTagContainer ActivationBlockedTags;

        /// <summary>
        /// This ability can only be activated if the source actor/component has all of these tags
        /// </summary>
        //[Export] public GameplayTagContainer SourceRequiredTags;

        /// <summary>
        /// This ability is blocked if the source actor/component has any of these tags
        /// </summary>
        //[Export] public GameplayTagContainer SourceBlockedTags;

        /// <summary>
        /// This ability can only be activated if the target actor/component has all of these tags
        /// </summary>
        //[Export] public GameplayTagContainer TargetRequiredTags;

        /// <summary>
        /// This ability is blocked if the target actor/component has any of these tags
        /// </summary>
        //[Export] public GameplayTagContainer TargetBlockedTags;

        [ExportCategory("Input")]

        [Export] public StringName InputActionName { get; private set; }

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
            AbilityTags ??= new GameplayTagContainer();
            CancelAbilitiesWithTag ??= new GameplayTagContainer();
            BlockAbilitiesWithTag ??= new GameplayTagContainer();
            ActivationOwnedTags ??= new GameplayTagContainer();
            ActivationRequiredTags ??= new GameplayTagContainer();
            ActivationBlockedTags ??= new GameplayTagContainer();

            AbilitySystemComponent = abilitySystemComponent;
            SourceObject = sourceObject;

            AbilityID = IDCounter++;
        }

        public virtual void OnGiveAbility() { }

        public virtual bool CanActivateAbility()
        {
            if (IsActive) return false;
            Assert.IsNotNull(AbilitySystemComponent, "OwningAbilityManager is not valid");

            if (AbilityTags.HasAny(AbilitySystemComponent.GetBlockedAbilityTags())) return false;

            if (ActivationBlockedTags.Length > 0 || ActivationRequiredTags.Length > 0)
            {
                GameplayTagContainer OwnedTags = AbilitySystemComponent.GetOwnedGameplayTags();

                if (OwnedTags.HasAny(ActivationBlockedTags)) return false;
                if (!OwnedTags.HasAll(ActivationRequiredTags)) return false;
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