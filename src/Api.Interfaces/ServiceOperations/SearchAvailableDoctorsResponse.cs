using System.Collections.Generic;
using Application.Resources;

namespace Api.Interfaces.ServiceOperations
{
    public class SearchAvailableDoctorsResponse : SearchOperationResponse
    {
        public List<Doctor> Doctors { get; set; }
    }
}