using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Interfaces.IFluents
{
    /// <summary>Contrato reservado con la misma forma que IFluentContent. Ningún builder
/// lo implementa hoy; se conserva por compatibilidad con versiones anteriores delpaquete.</summary>

    public interface IFluentResponse
    {
        Task<string> GetContentAsStringAsync();
        Task<byte[]> GetContentAsByteArrayAsync();
        Task<T> DeserializeWithAsync<T>();
        TaskAwaiter<HttpResponseMessage> GetAwaiter();
    }
}
