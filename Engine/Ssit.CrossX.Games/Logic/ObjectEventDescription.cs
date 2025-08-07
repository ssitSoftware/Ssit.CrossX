using Newtonsoft.Json.Linq;

namespace Ssit.CrossX.Games.Logic;

public class ObjectEventDescription(string name, string sequence, int frame, JObject parameters)
{
    public string Name { get; } = name;
    public string Sequence { get; } = sequence;
    public int Frame { get; } = frame;
    public JObject Parameters { get; } = parameters;
}