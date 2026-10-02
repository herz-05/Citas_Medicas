using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Interfaces.IFluents
{
    /// <summary>Último eslabón de la cadena: ejecuta el request y lee la respuesta.</summary>
    public interface IFluentContent
    {
        Task<string> GetContentAsStringAsync();
       
        Task<byte[]> GetContentAsByteArrayAsync();
        Task<T> DeserializeWithAsync<T>();
        TaskAwaiter<HttpResponseMessage> GetAwaiter();
    }
}
