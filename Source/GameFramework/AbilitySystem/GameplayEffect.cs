using Godot;

namespace GameFramework.AbilitySystem
{
    public partial class GameplayEffect : Resource
    {
        public DurationPolicy DurationPolicy = DurationPolicy.Instant;

        //[Tooltip("Period in seconds. 0 for non-periodic effects.")]
        //public float Period = 0f;

        //[Tooltip("Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        //[GameplayTag] public GameplayTag[] GrantedTagsAdded;

        //[Tooltip("Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        //[GameplayTag] public GameplayTag[] GrantedTagsRemoved;

        //[Tooltip("Once applied, these tags determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing Tag Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.")]
        //[GameplayTag] public GameplayTag[] OngoingTagRequirementsRequired;

        //[Tooltip("Once applied, these tags determine whether the GameplayEffect is on or off. A GameplayEffect can be off and still be applied. If a GameplayEffect is off due to failing the Ongoing Tag Requirements, but the requirements are then met, the GameplayEffect will turn on again and reapply its modifiers. This only works for Duration and Infinite GameplayEffects.")]
        //[GameplayTag] public GameplayTag[] OngoingTagRequirementsIgnored;

        //[Tooltip("Tags on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.")]
        //[GameplayTag] public GameplayTag[] ApplicationTagRequirementsRequired;

        //[Tooltip("Tags on the Target that determine if a GameplayEffect can be applied to the Target. If these requirements are not met, the GameplayEffect is not applied.")]
        //[GameplayTag] public GameplayTag[] ApplicationTagRequirementsIgnored;

        //[Tooltip("Once applied, these tags determine whether the GameplayEffect should be removed. Also prevents effect application.")]
        //[GameplayTag] public GameplayTag[] RemovalTagRequirementsRequired;

        //[Tooltip("Once applied, these tags determine whether the GameplayEffect should be removed. Also prevents effect application.")]
        //[GameplayTag] public GameplayTag[] RemovalTagRequirementsIgnored;

        //[Tooltip("GameplayEffects on the Target that have any of these tags in their Asset Tags or Granted Tags will be removed from the Target when this GameplayEffect is successfully applied.")]
        //[GameplayTag] public GameplayTag[] RemoveGameplayEffectsWithTags;

        //[Tooltip("Tags that live on the GameplayEffect but are also given to the ASC that the GameplayEffect is applied to. They are removed from the ASC when the GameplayEffect is removed. This only works for Duration and Infinite GameplayEffects.")]
        //[Inherits(typeof(GameplayAbility))]
        //public TypeReference[] GrantedAbilities;
    }

    public enum DurationPolicy
    {
        Instant,
        Infinite,
        HasDuration
    }
}