//-----------------------------------------------------------------------
// <copyright file="IActivityContext.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Device;
using Genesys.Foundation.Entity;
using Genesys.Foundation.Security;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Activity record tracking the a transactional process, typically querying or committing of data.
    /// </summary>
    public interface IActivityContext : IIdentityUserName, IDeviceUUID, IApplicationUUID, ICreatedDate, IActivityContextID
    {
        /// <summary>
        /// IP4 Address of the process executing this activity
        /// </summary>
        string PrincipalIP4Address { get; set; }

        /// <summary>
        /// Runtime context of this activity
        /// </summary>
        string ExecutingContext { get; set; }
    }
}