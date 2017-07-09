//-----------------------------------------------------------------------
// <copyright file="Activity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// This data-only object is used by EF code-first to define the schema of the table that log Exceptions
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    public class ActivityContext : IActivityContext
    {
        private string identityUserName = TypeExtension.DefaultString;
        private string principalIP4Address = TypeExtension.DefaultString;
        private string activitySQLStatement = TypeExtension.DefaultString;
        private string stackTrace = TypeExtension.DefaultString;
        private string executingContext = TypeExtension.DefaultString;
        private string deviceUUID = TypeExtension.DefaultString;
        private string applicationUUID = TypeExtension.DefaultString;

        /// <summary>
        /// ID
        /// </summary>
        public int ActivityContextID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// IdentityUserName
        /// </summary>
        public string IdentityUserName
        {
            get { return identityUserName; }
            set { identityUserName = value.SubstringLeft(40); }
        }

        /// <summary>
        /// Universally Unique ID (UUID) of the device, typically the IMEI from the hardware, or DeviceID from the operating system
        /// </summary>
        public string DeviceUUID
        {
            get { return deviceUUID; }
            set { deviceUUID = value.SubstringLeft(250); }
        }

        /// <summary>
        /// Universally Unique ID (UUID) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUUID
        {
            get { return applicationUUID; }
            set { applicationUUID = value.SubstringLeft(250); }
        }

        /// <summary>
        /// PrincipalIP4Address
        /// </summary>
        public string PrincipalIP4Address
        { 
            get { return principalIP4Address; }
            set { principalIP4Address = value.SubstringLeft(15); }
        }

        /// <summary>
        /// StackTrace
        /// </summary>
        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value.SubstringLeft(4000); }
        }

        /// <summary>
        /// ActivitySQLStatement
        /// </summary>
        public string ActivitySQLStatement
        {
            get { return activitySQLStatement; }
            set { activitySQLStatement = value.SubstringLeft(2000); }
        }

        /// <summary>
        /// Runtime context info, like assembly and environment info
        /// </summary>
        public string ExecutingContext
        {
            get { return executingContext; }
            set { executingContext = value.SubstringLeft(2000); }
        }

        /// <summary>
        /// ModifiedDate
        /// </summary>
        public DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;           
        
        /// <summary>
        /// Returns the typed string of the primary property.
        /// </summary>
        public override string ToString()
        {
            return ActivityContextID.ToString();
        }
    }
}