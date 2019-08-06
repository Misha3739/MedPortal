using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.WebApiClient.Models
{
    public interface ISearchItemModel
    {
        long Id { get; set; }

        string Alias { get; set; }

        string Name { get; set; }
    }
}
