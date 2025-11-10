// using EbatianoSoftware.CrossX.Primitives;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
//
// namespace XxGames.Platformer.Objects;
//
// [CreationParametersType(typeof(Parameters))]
// public class Target : ITarget
// {
//     public class Parameters
//     {
//         public IObjectSource<ITarget> Next { get; set; }
//     }
//
//     public PointF Position { get; }
//
//     public ITarget Next { get; private set; }
//
//     public Target(Parameters parameters, CreateObjectParameters createObjectParameters)
//     {
//         parameters.Next.OnObjectResolved(obj =>Next = obj);
//         Position = createObjectParameters.Position;
//     }
// }