// using EbatianoSoftware.CrossX.IoC;
// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Ssit.CrossX.XxGames.Platformer;
// using Ssit.CrossX.XxGames.Platformer.Objects;
// using XxGames.Logic;
// using XxGames.PhysicsCore;
//
// namespace XxGames.Platformer.Objects;
//
// public abstract class StateGameObject<TObject> : GameObject, IVisualStateSource where TObject: StateGameObject<TObject>
// {
//     public class ProcessParameters
//     {
//         public double TimeDelta { get; internal set; }
//         public ProcessEvent Event { get; internal set; }
//         public Collision Collision { get; internal set; }
//         public HitKind HitKind { get; internal set; }
//         public ReturnValue<bool> ReturnBool { get; } = new ReturnValue<bool>();
//     }
//
//     public StateMachine<TObject, ProcessParameters> StateMachine { get; }
//
//     public bool FacingLeft { get; set; }
//
//     protected readonly ProcessParameters ProcessParametersObj;
//
//     public event Action VisualStateChanged;
//
//     public IObjectFlags<string> Flags { get; }
//     public ObjectValues<string, double> Values { get; } = new ObjectValues<string, double>();
//
//     public string VisualState => StateMachine.CurrentState;
//
//     public VectorF VisualTransform => new VectorF(FacingLeft ? -1 : 1, 1);
//
//     protected StateGameObject(IIoCFactory iocFactory, ISimulation simulation, IEnumerable<IState> states): base(simulation)
//     {
//         Flags = iocFactory.IoCConstruct<IObjectFlags<string>>();
//         StateMachine = new StateMachine<TObject, ProcessParameters>((TObject)this, states.Select( o=> (State<TObject, ProcessParameters>)o));
//         StateMachine.StateChanged += StateMachine_StateChanged;
//
//         ProcessParametersObj = new ProcessParameters
//         {
//             Event = ProcessEvent.None,
//             TimeDelta = 0
//         };
//     }
//
//     public void RaiseVisualEvent(string name, IParameter[] parameters) => StateMachine.RaiseCurrentStateEvent(name, parameters);
//
//     private void StateMachine_StateChanged(string state) => VisualStateChanged?.Invoke();
//
//     public override void OnFixedUpdate(out bool cancelUpdate)
//     {
//         ProcessParametersObj.TimeDelta = Body.Simulation.SimulationParameters.TimeDelta;
//         ProcessParametersObj.Event = ProcessEvent.FixedUpdate;
//
//         StateMachine.Process(ProcessParametersObj);
//
//         base.OnFixedUpdate(out cancelUpdate);
//     }
//
//     public override void OnPostFixedUpdate()
//     {
//         ProcessParametersObj.TimeDelta = Body.Simulation.SimulationParameters.TimeDelta;
//         ProcessParametersObj.Event = ProcessEvent.PostFixedUpdate;
//         StateMachine.Process(ProcessParametersObj);
//     }
//
//     public override void OnUpdate(double time)
//     {
//         ProcessParametersObj.TimeDelta = time;
//         ProcessParametersObj.Event = ProcessEvent.Update;
//         StateMachine.Process(ProcessParametersObj);
//     }
//
//     public override bool Hit(HitKind kind, ICollider collider, VectorF impact)
//     {
//         ProcessParametersObj.Event = ProcessEvent.Hit;
//         ProcessParametersObj.HitKind = kind;
//         ProcessParametersObj.Collision = new Collision(collider, impact, false);
//         ProcessParametersObj.ReturnBool.Value = false;
//
//         StateMachine.Process(ProcessParametersObj);
//         return ProcessParametersObj.ReturnBool.Value;
//     }
// }