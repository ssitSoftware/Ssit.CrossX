using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Ssit.CrossX.Games.Logic.Map;

public static class PolygonMerge
{
    private class LineEquality : IEqualityComparer<Line>
    {
        public bool Equals(Line a, Line b)
        {
            return (VectorEquals(a.P0, b.P0) && VectorEquals(a.P1, b.P1)) ||
                   (VectorEquals(a.P0, b.P1) && VectorEquals(a.P1, b.P0));
        }

        public int GetHashCode(Line obj)
        {
            return HashCode.Combine(obj.P0, obj.P1) ^ HashCode.Combine(obj.P1, obj.P0);
        }
    }
    
    private struct Line
    {
        public Vector2 P0;
        public Vector2 P1;

        public Vector2? GetNextPoint(Vector2 point)
        {
            if(VectorEquals(P0, point)) return P1;
            if(VectorEquals(P1, point)) return P0;
            return null;
        }
    }

    private static readonly float Epsilon = 0.01f;
    
    private static bool VectorEquals(Vector2 v1, Vector2 v2)
    {
        return MathF.Abs(v1.X-v2.X) < Epsilon && MathF.Abs(v1.Y - v2.Y) < Epsilon;
    }
    
    public static IReadOnlyList<(Vector2[], bool)> Merge(IReadOnlyList<Vector2[]> polygons, float maxX, float maxY)
    {
        var list = GetLinesList(polygons);
        
        var lines = new Dictionary<Line, int>(new LineEquality());

        foreach (var line in list)
        {
            if (lines.TryGetValue(line, out var count))
            {
                count++;
                lines[line] = count;
            }
            else
            {
                lines[line] = 0;
            }
        }
        list = lines.Where(o => o.Value == 0).Select(o => o.Key).ToList();

        for (var idx = 0; idx < list.Count;)
        {
            var line = list[idx];
        
            if ((line.P1.X < Epsilon && line.P0.X < Epsilon) ||
                (line.P1.Y < Epsilon && line.P0.Y < Epsilon) ||
                (line.P1.X > maxX - Epsilon && line.P0.X > maxX - Epsilon) ||
                (line.P1.Y > maxY - Epsilon && line.P0.Y > maxY - Epsilon))
            {
                list.RemoveAt(idx);
                continue;
            }
            ++idx;
        }
        
        var outPolygons = new List<(Vector2[], bool)>();
        while (list.Count > 0)
        {
            var polygonList = new List<Vector2>();
            bool continueNext = true;
            
            var line = list[0];
            
            polygonList.Add(line.P0);
            polygonList.Add(line.P1);
            
            list.RemoveAt(0);

            while (continueNext)
            {
                continueNext = false;
                for (var idx = 0; idx < list.Count;)
                {
                    var next = list[idx].GetNextPoint(polygonList[0]);

                    if (next.HasValue)
                    {
                        polygonList.Insert(0, next.Value);
                        list.RemoveAt(idx);
                        continueNext = true;
                        continue;
                    }

                    next = list[idx].GetNextPoint(polygonList[^1]);

                    if (next.HasValue)
                    {
                        polygonList.Add(next.Value);
                        list.RemoveAt(idx);
                        continueNext = true;
                        continue;
                    }

                    ++idx;
                }
            }

            var close = VectorEquals(polygonList[0], polygonList[^1]);
            if (close)
            {
                polygonList.RemoveAt(polygonList.Count-1);
            }
            
            Reduce(polygonList, close);

            if (polygonList.Count > 1)
            {
                outPolygons.Add((polygonList.ToArray(), close));
            }
            else
            {
                Console.WriteLine("Something is wrong");
            }
        }

        return outPolygons;
    }

    private static void Reduce(List<Vector2> polygonList, bool close)
    {
        for (var idx = 1; idx < (close ? polygonList.Count * 2 : polygonList.Count - 1);)
        {
            var dir1 = Vector2.Normalize(polygonList[idx % polygonList.Count] - polygonList[(idx - 1) % polygonList.Count]);
            var dir2 = Vector2.Normalize(polygonList[(idx+1) % polygonList.Count] - polygonList[idx%polygonList.Count]);

            if (VectorEquals(dir1, dir2))
            {
                polygonList.RemoveAt(idx % polygonList.Count);
            }
            else
            {
                ++idx;
            }
        }
    }

    private static List<Line> GetLinesList(IReadOnlyList<Vector2[]> polygons)
    {
        var list = new List<Line>();

        foreach (var polygon in polygons)
        {
            var length = polygon.Length;

            if (length < 3)
            {
                length--;
            }
            
            for (var idx = 0; idx < length; ++idx)
            {
                var p0 = polygon[idx];
                var p1 = polygon[(idx + 1) % polygon.Length];
                
                list.Add(new Line
                {
                    P0 = p0,
                    P1 = p1
                });
            }
        }

        return list;
    }
}