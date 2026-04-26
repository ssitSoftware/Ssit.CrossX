# AI Skill: [Skill Name]

## Metadata
- **Skill Name**: XxGames Character Creator
- **Version**: 1.0.0
- **Author**: Sebastian Sejud
- **Created**: 2026-04-24
- **Last Modified**: 2026-04-24
- **Category**: Gameplay
- **Tags**: xxgames character create

## Description
Generates game characters classes using XxGames framework.

## Activation Keywords
- `xxgames character`

## Instructions
Create class based on
Ssit.CrossX.XxGames.Logic.Objects.Characters.CharacterObject<TCharacter>
where TCharacter is the character class.

Use SteringBehavior<ISteringCharacter> array to define stering state with stering behaviors.
Each stering behavior executes Update and FixedUpdate methods to update stering state.
Behaviors are executed in order of array index.  In case of true result, no more behaviors are executed.

Register state in SteringStateMachine object using method WithBehaviorState where you pass state name and array of behaviors.

