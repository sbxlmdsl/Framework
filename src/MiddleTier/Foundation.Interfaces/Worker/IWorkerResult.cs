//-----------------------------------------------------------------------
// <copyright file="IWorkerResult.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extras.Collections;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// Result of any process
    /// </summary>
    [CLSCompliant(true)]
    public interface IWorkerResult
    {
        /// <summary>
        /// CurrentState
        /// </summary>
        WorkerStates CurrentState { get; set; }

        /// <summary>
        /// OnStart
        /// </summary>
        void OnStart();

        /// <summary>
        /// OnSuccess
        /// </summary>
        void OnSuccess();

        /// <summary>
        /// OnFail
        /// </summary>
        /// <param name="errorMessage"></param>
        void OnFail(string errorMessage);

        /// <summary>
        /// FailedRules
        /// </summary>
        KeyValueListString FailedRules { get; set; }

        /// <summary>
        /// Return ID - Primary Key of record
        /// </summary>
        int ReturnID { get; set; }

        /// <summary>
        /// Return Key - Guid Key for record
        /// </summary>
        Guid ReturnKey { get; set; }

        /// <summary>
        /// Serialized data to be returned to caller
        /// </summary>
        string ReturnData { get; set; }
    }
}
