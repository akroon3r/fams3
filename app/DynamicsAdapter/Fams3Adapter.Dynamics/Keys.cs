﻿namespace Fams3Adapter.Dynamics
{
    public class Keys
    {
        public const string DYNAMICS_STATUS_CODE_FIELD = "statuscode";

        public const string GLOBAL_OPTIONS_SET_DEFINTION_URL_TEMPLATE = "GlobalOptionSetDefinitions(Name='ssg_{0}s')";

        public const string GLOBAL_STATUS_CODE_URL_TEMPLATE =
            "EntityDefinitions(LogicalName='{0}')/Attributes(LogicalName='statuscode')/Microsoft.Dynamics.CRM.StatusAttributeMetadata?$select=LogicalName&$expand=OptionSet";

    }
}