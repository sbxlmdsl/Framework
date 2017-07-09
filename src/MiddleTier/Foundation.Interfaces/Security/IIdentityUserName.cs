//-----------------------------------------------------------------------
// <copyright file="IUserPrincipal.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Security.Principal;
using Genesys.Foundation.Session;

namespace Genesys.Foundation.Security
{
    /// <summary>
    /// User of any system
    /// </summary>
	[CLSCompliant(true)]
    public interface IIdentityUserName
    {
        /// <summary>
        /// User name of the person logged in, not the principal user name of the process doing the work
        /// </summary>
        string IdentityUserName { get; set; }
    }
}
