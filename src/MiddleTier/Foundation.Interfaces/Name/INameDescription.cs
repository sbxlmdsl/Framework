//-----------------------------------------------------------------------
// <copyright file="INameDescription.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Name
{
    /// <summary>
    /// Name, ID and Description used in nearly all lookup items
    /// </summary>
    [CLSCompliant(true)]
    public interface INameDescription : INameKey
    {
        /// <summary>
        /// Description
        /// </summary>
        string Description { get; set; }
    }
}
