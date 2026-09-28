using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Exceptions
{
    /// <summary>Clasifica por qué falló una llamada HTTP.</summary>
    public enum ApiFailureReason
    {
        Unknown,
        Network,
        Timeout,
        HttpError,
        Deserialization
    }
}
