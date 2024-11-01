namespace Ssit.Utils.Mvvm.Abstractions;

public interface IDialogContext
{
    void Close();
}

public interface IDialogContext<in TResult>
{
    void Close(TResult result);
}