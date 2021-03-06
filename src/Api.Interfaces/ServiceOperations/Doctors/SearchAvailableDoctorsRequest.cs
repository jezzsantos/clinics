﻿using System;
using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Doctors
{
    [Route("/doctors/available", "GET")]
    public class SearchAvailableDoctorsRequest : SearchOperation<SearchAvailableDoctorsResponse>
    {
        public DateTime? FromUtc { get; set; }

        public DateTime? ToUtc { get; set; }
    }
}