using NugetPackage_Rest.Interfaces.IFluents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Interfaces.IRequests
{
    /// <summary>Primer eslabón de un request SIN body (GET): elegir autenticación.</summary>
    public interface INotContentRequest
    {
        IFluentAuth<IFluentContent> WithoutAuth();
        IFluentAuth<IFluentContent> WithBearer(string token);
        IFluentAuth<IFluentContent> WithBasic(string user, string password);
    }
}
