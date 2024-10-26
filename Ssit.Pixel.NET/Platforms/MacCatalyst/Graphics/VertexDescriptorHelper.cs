using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public static class VertexDescriptorHelper
{
    private static readonly Dictionary<VertexMode, MTLVertexDescriptor> VertexDescriptors = new();
    
    public static MTLVertexDescriptor GetVertexDescriptor(this VertexMode mode)
    {
        if (VertexDescriptors.TryGetValue(mode, out var descriptor))
        {
            return descriptor;
        }
        
        var vertexDescriptor = new MTLVertexDescriptor();

        var index = 0;
        uint offset = 0;
        
        if (mode.HasFlag(VertexMode.Position))
        {
            vertexDescriptor.Attributes[index].Format = MTLVertexFormat.Float4;
            vertexDescriptor.Attributes[index].BufferIndex = 0;
            vertexDescriptor.Attributes[index].Offset = 0;
            index++;
            offset += sizeof(float) * 4;
        }

        if (mode.HasFlag(VertexMode.Color))
        {
            vertexDescriptor.Attributes[index].Format = MTLVertexFormat.UChar4Normalized;
            vertexDescriptor.Attributes[index].BufferIndex = 0;
            vertexDescriptor.Attributes[index].Offset = offset;
            index++;
            offset += sizeof(byte) * 4;
        }
        
        if (mode.HasFlag(VertexMode.Texture))
        {
            vertexDescriptor.Attributes[index].Format = MTLVertexFormat.Float2;
            vertexDescriptor.Attributes[index].BufferIndex = 0;
            vertexDescriptor.Attributes[index].Offset = offset;
            index++;
            offset += sizeof(float) * 2;
        }
        
        vertexDescriptor.Layouts[0].Stride = offset;
        vertexDescriptor.Layouts[0].StepRate = 1;
        vertexDescriptor.Layouts[0].StepFunction = MTLVertexStepFunction.PerVertex;

        VertexDescriptors.Add(mode, vertexDescriptor);
        return vertexDescriptor;
    }
}