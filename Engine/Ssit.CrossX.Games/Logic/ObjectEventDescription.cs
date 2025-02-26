namespace Ssit.CrossX.Games.Logic;

public class ObjectEventDescription(string name, string sequence, int frame, string parameters)
{
    public string Name { get; } = name;
    public string Sequence { get; } = sequence;
    public int Frame { get; } = frame;
    public  string Parameters { get; } = parameters;
}