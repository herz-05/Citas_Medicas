using NugetPackage_Rest.Interfaces.IFluents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Interfaces.IRequests
{
    /// <summary>Primer eslabón de un request CON body (POST/PUT/PATCH/DELETE).</summary>
    public interface IWithContentRequest
    {
        IFluentAuth<IFluentFormat> WithoutAuth();
        IFluentAuth<IFluentFormat> WithBearer(string token);
        IFluentAuth<IFluentFormat> WithBasic(string user, string password);
    }
}
