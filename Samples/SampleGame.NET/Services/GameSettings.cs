namespace SampleGame.Services;

public class GameSettings: IGameSettings
{
    public bool CameraShake { get; set; } = true;
    public bool AutoReload { get; set; } = true;
}