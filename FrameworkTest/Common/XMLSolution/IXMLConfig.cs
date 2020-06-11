using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.Common.XMLSolution
{
    public interface IXMLConfig
    {
        XElement ToXElement();
    }
}
