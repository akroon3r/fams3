﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.Services.Dynamics.Model
{
    public class SSG_SearchRequests
    {
        public string SSG_PersonGivenName { get; set; }
        public string SSG_PersonSurname { get; set; }
        public DateTime SSG_PersonBirthDate { get; set; }
        public string StatusCode { get; set; }

    }
}