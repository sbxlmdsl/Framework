//-----------------------------------------------------------------------
// <copyright file="SessionContext.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
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
            DeviceUUID = deviceUUID;
            ApplicationUUID = applicationUUID;
            IdentityUserName = identityUserName;            
        }
                
    }
}
