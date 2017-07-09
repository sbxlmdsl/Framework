//-----------------------------------------------------------------------
// <copyright file="IEntityKey.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Used for cases where Entity ID needs to be carried over to Flow via interface
    /// </summary>    
    [CLSCompliant(true)]
    public interface IEntityKey
    {
        /// <summary>
        /// EntityID
        /// </summary>
        Guid EntityKey { get; set; }
    }
}
