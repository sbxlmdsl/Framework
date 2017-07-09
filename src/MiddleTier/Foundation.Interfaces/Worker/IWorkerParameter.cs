//-----------------------------------------------------------------------
// <copyright file="IWorkerParameter.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Session;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// Parameter and data for any process
    /// </summary>
    /// <typeparam name="TDataIn">Type of input data for the process</typeparam>
    [CLSCompliant(true)]
    public interface IWorkerParameter<TDataIn>
    {
        /// <summary>
        /// App, User, Device context
        /// </summary>
        ISessionContext Context { get; set; }

        /// <summary>
        /// Input data for the process
        /// </summary>
        TDataIn DataIn { get; set; }
    }
}
