using Gunslinger.Core;
using Ssit.CrossX.Editor;

class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        EditorRunner.Run(args, new GunslingerTemplate());
}