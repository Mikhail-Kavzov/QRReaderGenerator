using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRLib
{
    public interface ICodeGenerator
    {
        string Generate(string input, string dataDir);
    }
}
