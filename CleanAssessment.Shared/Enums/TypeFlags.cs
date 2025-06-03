using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Enums;

[Flags]
public enum TypeFlags
{
    None,
    Class,
    Interface,
    Enum,
}
