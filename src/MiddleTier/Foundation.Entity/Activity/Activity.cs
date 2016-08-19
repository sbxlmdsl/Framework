//-----------------------------------------------------------------------
// <copyright file="Activity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
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
    public class Activity : IActivity
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
        public int ActivityID { get; set; } = TypeExtension.DefaultInteger;

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
            return this.ActivityID.ToString();
        }
    }
}
