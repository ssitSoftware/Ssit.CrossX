#if FALSE
using System.Numerics;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Utils;

public static class PhysicsDebugDisplay
{
    public struct RenderData
    {
        public World World;
        public int TileSize;
        public float Scale;
        public Size TargetSize;
        public Vector2 DisplayOffset;
        public Vector2 MapOffset;
        public RgbaColor[] MaterialColors;
    }

    public static void Render(RenderData renderData)
    {
        var targetScale = renderData.Scale * renderData.TileSize;
        var targetSize = renderData.TargetSize.ToVector() * renderData.Scale;
        var targetOffset = renderData.DisplayOffset + targetSize with {X = 0};
        var transform = Matrix3x2.CreateTranslation(-renderData.MapOffset) *
                        Matrix3x2.CreateScale(targetScale) *
                        Matrix3x2.CreateTranslation(targetOffset);

        if (renderData.World is null)
            return;

        for (var idx = 0; idx < renderData.World.BodyList.Count; ++idx)
        {
            DrawBody(renderData.World.BodyList[idx], transform, renderData, idx);
        }
    }

    private static void DrawBody(Body body, Matrix3x2 transform, RenderData renderData, int index)
    {
        for (var idx = 0; idx < body.FixtureList.Count; ++idx)
        {
            DrawFixture(body, body.FixtureList[idx], transform, renderData, index);
        }
    }

    private static void DrawFixture(Body body, Fixture fixture, Matrix3x2 transform, RenderData renderData, int index)
    {
        RgbaColor color = RgbaColor.Azure;
        
        switch (body.BodyType)
        {
            case BodyType.Dynamic:
                color = body.Awake ? RgbaColor.OrangeRed : RgbaColor.Green;
                break;

            case BodyType.Kinematic:
                color = RgbaColor.MediumVioletRed;
                break;

            case BodyType.Static:
                color = renderData.MaterialColors[body.MaterialIndex];
                color.R = (byte) (index * 64);
                break;
        }

        switch (fixture.Shape.ShapeType)
        {
            case ShapeType.Circle:
                var circle = (CircleShape) fixture.Shape;
                DrawCircle(transform, color, circle.Position + body.Position, circle.Radius);
                break;

            case ShapeType.Chain:
                var chain = (ChainShape) fixture.Shape;
                DrawPolyline(transform, color, chain.Vertices, body.Position, false);
                break;

            case ShapeType.Edge:
                var edge = (EdgeShape) fixture.Shape;
                DrawLine(transform, color, edge.Vertex1 + body.Position, edge.Vertex2 + body.Position);

                if (edge.HasVertex0)
                {
                    DrawLine(transform, color.WithAlpha(192), edge.Vertex0 + body.Position,
                        edge.Vertex1 + body.Position);
                }

                if (edge.HasVertex3)
                {
                    DrawLine(transform, color.WithAlpha(192), edge.Vertex2 + body.Position,
                        edge.Vertex3 + body.Position);
                }

                break;

            case ShapeType.Polygon:
                var polygon = (PolygonShape) fixture.Shape;
                DrawPolyline(transform, color, polygon.Vertices, body.Position, true);
                break;
        }
    }
}
#endif