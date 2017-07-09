//-----------------------------------------------------------------------
// <copyright file="UserIdentity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Security.Principal;
using Genesys.Extensions;

namespace Genesys.Foundation.Security
{
    /// <summary>
    /// User Identity based on IPrincipal and IIdentity
    /// </summary>
    [CLSCompliant(true)]
    public class UserIdentity : IIdentity
    {
        /// <summary>
        ///  Authentication Type
        /// </summary>
        public string AuthenticationType { get; protected set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Is Authenticated
        /// </summary>
        public bool IsAuthenticated { get; protected set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// User running process is IPrincipal.Name
        /// User logged in is IIDentity.Name
        /// </summary>
        public string Name { get; protected set; } = TypeExtension.DefaultString;
    }
}
