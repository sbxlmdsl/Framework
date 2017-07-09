//-----------------------------------------------------------------------
// <copyright file="WorkerStates.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// States of a an operation that does work
    /// </summary>
    [CLSCompliant(true)]
    public enum WorkerStates
    {
        /// <summary>
        /// Process never executed
        /// </summary>
        NeverRan = 0x0,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        Pending = 0x1,

        /// <summary>
        /// Process is currently running
        /// </summary>
        Running = 0x2,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        OnHold = 0x4,

        /// <summary>
        /// Process completed with no errors
        /// </summary>
        Completed = 0x8,
    }
}
