using Ssit.CrossX.XxGames.Audio;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ICharacter
{
    bool FaceLeft { get; set; }
    IBody Body { get; }
}

public interface IGameObject
{
    ContextSoundContainer SoundContainer { get; }
    TParameters Get<TParameters>() where TParameters : class;
}

public interface ISteringCharacter: ICharacter, IGameObject
{
    ISteringInput SteringInput { get; }
    CharacterSteringParameters SteringParameters { get; }
    ICharacterPhysicsValues PhysicsValues { get; }
    SteringState<ISteringCharacter> CurrentSteringState { get; }
    void SetSteringState(string name);
    
}