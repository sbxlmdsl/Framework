//-----------------------------------------------------------------------
// <copyright file="IApplicationUUID.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Device
{
    /// <summary>
    /// Device ID
    /// </summary>
    [CLSCompliant(true)]
    public interface IApplicationUUID
    {
        /// <summary>
        /// Universally Unique ID (UUID) of the software application, that identifies this Application + Device combination
        /// </summary>
        string ApplicationUUID { get; set; }
    }
}
