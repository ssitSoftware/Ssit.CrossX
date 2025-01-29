using System.Linq;

namespace Ssit.CrossX.Games.Template;

public class ImageDescription
{
    public readonly string Name;
    public readonly string[] Tags;
    public readonly string GameObject;
    public readonly string Sequence;
    public readonly string FullName;
    
    public ImageDescription(string name, string gameObject, string defaultSequence = null)
    {
        FullName = name;
        Tags = name.Split('/');
        Name = Tags.Last().Trim();
        Tags = Tags.Take(Tags.Length - 1).ToArray();
        
        for(var idx =0; idx < Tags.Length; ++idx)
        {
            Tags[idx] = Tags[idx].Trim();
        }
        
        GameObject = gameObject;
        Sequence = defaultSequence;
    }
}