using System.Threading.Tasks;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Tools;

public interface IXmlFileConverter
{
    Task Generate();
    string Convert(XNode node);
}