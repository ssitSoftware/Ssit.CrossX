namespace Nokemono.Core.Game.Parameters;

public class AttackParameters
{
    public class AttackValue
    {
        public float Multiplier { get; set; }
        public float Add { get; set; }

        public float Calculate(float strength)
        {
            return strength * Multiplier + Add;
        }
    }
    
    public float Width { get; set; }
    public float NegWidth { get; set; } = -1.0f;
    public float Height { get; set; }
    public AttackValue Value { get; set; }
}