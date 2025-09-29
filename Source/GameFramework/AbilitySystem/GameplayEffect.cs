using GameFramework.GameplayTags;
using Godot;

namespace GameFramework.AbilitySystem
{
    public partial class GameplayEffect : Resource
    {
        public DurationPolicy DurationPolicy = DurationPolicy.Instant;

        /// <summary>
        /// Period in seconds. 0 for non-periodic effects.
        /// </summary>
        public float Period = 0f;

        /// <summary>
        /// Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.
        /// </summary>
        [Export] public GameplayTagContainer GrantedTagsAdded;

        /// <summary>
        /// Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.
        /// </summary>
        [Export] public GameplayTagContainer GrantedTagsRemoved;

        /// <summary>
        /// Once applied, these tags determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing Tag Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.
        /// </summary>
        [Export] public GameplayTagContainer OngoingTagRequirementsRequired;

        /// <summary>
        /// Once applied, these tags determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing Tag Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.
        /// </summary>
        [Export] public GameplayTagContainer OngoingTagRequirementsIgnored;

        /// <summary>
        /// Tags on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.
        /// </summary>
        [Export] public GameplayTagContainer ApplicationTagRequirementsRequired;

        /// <summary>
        /// Tags on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.
        /// </summary>
        [Export] public GameplayTagContainer ApplicationTagRequirementsIgnored;

        /// <summary>
        /// Once applied, these tags determine whether the GameplayEffect should be removed. Also prevents effect application.
        /// </summary>
        [Export] public GameplayTagContainer RemovalTagRequirementsRequired;

        /// <summary>
        /// Once applied, these tags determine whether the GameplayEffect should be removed. Also prevents effect application.
        /// </summary>
        [Export] public GameplayTagContainer RemovalTagRequirementsIgnored;

        /// <summary>
        /// GameplayEffects on the Target that have any of these tags in their Asset Tags or Granted Tags will be removed from the Target when this GameplayEffect is successfully applied.
        /// </summary>
        [Export] public GameplayTagContainer RemoveGameplayEffectsWithTags;

        /// <summary>
        /// Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.
        /// </summary>
        //public TypeReference[] GrantedAbilities;
    }

    public enum DurationPolicy
    {
        Instant,
        Infinite,
        HasDuration
    }
}