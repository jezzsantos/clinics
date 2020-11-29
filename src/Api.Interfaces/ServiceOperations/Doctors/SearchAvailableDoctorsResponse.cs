using System.Collections.Generic;
using Application.Resources;

namespace Api.Interfaces.ServiceOperations.Doctors
{
    public class SearchAvailableDoctorsResponse : SearchOperationResponse
    {
        public List<Doctor> Doctors { get; set; }
    }
}