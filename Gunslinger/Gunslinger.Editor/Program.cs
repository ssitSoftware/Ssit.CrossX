using Gunslinger.Core;
using Ssit.CrossX.Editor;

namespace Gunslinger.Editor;

class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        EditorRunner.Run(args, new GunslingerTemplate());
}