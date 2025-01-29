using System;

namespace Ssit.CrossX.Games.Template;

public class ObjectDescription: ImageDescription
{
    
    public readonly Type TargetType;
    public readonly Type ParametersType;

    public ObjectDescription(string name, Type targetType, string sprite, string spriteSequence, Type parametersType = null): base(name, sprite, spriteSequence)
    {
        TargetType = targetType;
        ParametersType = parametersType;
    }
}