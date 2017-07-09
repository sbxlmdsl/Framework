//-----------------------------------------------------------------------
// <copyright file="ISessionContext.cs" company="Genesys Source">
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

namespace Genesys.Foundation.Session
{
    /// <summary>
    /// Context of user, device, application for all session flows
    /// </summary>
	[CLSCompliant(true)]
    public interface ISessionContext : IEntityKey, IDeviceUUID, IApplicationUUID, IIdentityUserName
    {
    }
}
