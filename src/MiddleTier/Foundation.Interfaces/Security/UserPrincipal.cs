//-----------------------------------------------------------------------
// <copyright file="UserPrincipal.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Security.Principal;
using Genesys.Extensions;
using Genesys.Foundation.Device;
using Genesys.Foundation.Entity;

namespace Genesys.Foundation.Security
{
    /// <summary>
    /// User Identity based on IPrincipal and IIdentity
    /// </summary>
    [CLSCompliant(true)]
    public class UserPrincipal : IUserPrincipal, IEntityKey, IDeviceUUID, IApplicationUUID
    {
        private string principalUserName = TypeExtension.DefaultString;

        /// <summary>
        /// Universally Unique ID (UUID) of the device. Typically same as IMEI number, or DeviceID from the OS
        /// </summary>
        public string DeviceUUID { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Universally Unique ID (UUID) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUUID { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Person/business submitting the request
        /// </summary>
        public Guid EntityKey { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Same as IdentityUserName
        /// User running process is IPrincipal.Name
        /// User logged in is IIDentity.Name
        /// </summary>
        public string IdentityUserName
        {
            get
            {
                principalUserName = principalUserName == TypeExtension.DefaultString ? this.Identity.Name : principalUserName;
                return principalUserName;
            }
            set { principalUserName = value; }
        }

        /// <summary>
        /// User running process is IPrincipal.Name
        /// User logged in is IIDentity.Name
        /// </summary>
        public string PrincipalUserName
        {
            get
            {
                principalUserName = principalUserName == TypeExtension.DefaultString ? this.Identity.Name : principalUserName;
                return principalUserName;
            }
            set { principalUserName = value; }
        }

        /// <summary>
        /// Activity tracking record of this session flow
        /// </summary>
        public int ActivitySessionflowID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Identity of requester
        /// </summary>
        public IIdentity Identity { get; set; } = new UserIdentity();
        
        /// <summary>
        /// Constructor
        /// </summary> 
        public UserPrincipal() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceUUID">Device requesting</param>
        /// <param name="identity">Identity of user of request</param>
        protected UserPrincipal(string deviceUUID, IIdentity identity)
            : base()
        {
            if (identity == null)
                throw new ArgumentNullException("IIdentity");
            this.DeviceUUID = deviceUUID;
            Identity = identity;
        }
        
        /// <summary>
        /// Is In Role?
        /// </summary>
        /// <param name="role"></param>        
        public bool IsInRole(string role)
        {
            if ("Admin".Equals(role))
                return true;

            throw new ArgumentException("Role " + role + " is not supported");
        }
        
    }
}
