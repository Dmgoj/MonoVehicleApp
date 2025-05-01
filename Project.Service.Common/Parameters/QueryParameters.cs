using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Common;

namespace Project.Service.Common.Parameters
{
    #region VehicleSortByFields
    public enum VehicleMakeSortField
    {
        Name,
        Abrv
    }

    public enum VehicleModelSortField
    {
        Name,
        Abrv
    }

    public enum VehicleOwnerSortField
    {
        FirstName,
        LastName,
        DOB
    }

    public enum VehicleEngineTypeSortField
    {
        Type
    }

    public enum VehicleRegistrationSortField
    {
        RegistrationNumber,
        VehicleModel,
        VehicleEngineType,
        VehicleOwner
    }
    #endregion
    public class QueryParameters : PagingParameters
    {
        public string? Filter { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
