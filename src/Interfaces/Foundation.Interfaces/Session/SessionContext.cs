//-----------------------------------------------------------------------
// <copyright file="SessionContext.cs" company="Genesys Source">
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

namespace Genesys.Foundation.Session
{
    /// <summary>
    /// Context identity that includes user identity info (user name), application ID and entityID
    /// </summary>
    [CLSCompliant(true)]
    public class SessionContext : ISessionContext
    {
        /// <summary>
        /// Universally Unique ID (UUID) of the device. Typically same as IMEI number, or DeviceID from the OS
        /// </summary>
        public string DeviceUUID { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Universally Unique ID (UUID) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUUID { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Entity (business or person)
        /// </summary>
        public Guid EntityKey { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Name, typically user name
        /// </summary>
        public string IdentityUserName { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionContext() : base() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceUUID">Device ID sending request</param>
        /// <param name="applicationUUID">Application ID sending request</param>
        /// <param name="identityUserName">Name of user/authentication name sending request</param>
        public SessionContext(string deviceUUID, string applicationUUID, string identityUserName) : this()
        {
            this.DeviceUUID = deviceUUID;
            this.ApplicationUUID = applicationUUID;
            this.IdentityUserName = identityUserName;            
        }
                
    }
}
