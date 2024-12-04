namespace Ssit.CrossX.Text;

public interface ICharProvider
{
    int Length { get; }
    char this[int index] { get; }
}