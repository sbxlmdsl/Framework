//-----------------------------------------------------------------------
// <copyright file="WorkerResult.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;
using Genesys.Extras.Collections;
using Genesys.Foundation.Validation;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    [CLSCompliant(true)]
    public class WorkerResult : IWorkerResult
    {      
        /// <summary>
        /// Current state of the process
        /// </summary>
        public WorkerStates CurrentState { get; set; } = WorkerStates.NeverRan;

        /// <summary>
        /// Errors
        /// </summary>
        /// <value></value>        
        public KeyValueListString FailedRules { get; set; } = new KeyValueListString();

        /// <summary>
        /// ID to be returned to caller
        /// </summary>
        public int ReturnID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Key to be returned to caller
        /// </summary>
        public Guid ReturnKey { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Serialized data to be returned to caller
        /// </summary>
        public string ReturnData { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkerResult() : base()
        {
        }
        
        /// <summary>
        /// Adds to failed rules from a valid IValidationRule or IValidationResult
        /// </summary>
        /// <param name="validatable"></param>
        public void RuleFailed(ValidationResult validatable)
        {
            FailedRules.Add(validatable);
        }

        /// <summary>
        /// Adds a failed rule message that does not have access to a full IValidationRule or IValidationResult
        /// </summary>
        /// <param name="resultMessageWithNoValidationRule"></param>
        public void RuleFailed(string resultMessageWithNoValidationRule)
        {
            FailedRules.Add(new ValidationResult(resultMessageWithNoValidationRule));
        }
        
        /// <summary>
        /// Starts a process
        /// </summary>        
        public void OnStart()
        {
            CurrentState = WorkerStates.Running;
        }

        /// <summary>
        /// Records failure of a process
        /// </summary>
        public void OnFail(string errorMessage)
        {
            CurrentState = WorkerStates.OnHold;
            RuleFailed(errorMessage);
        }

        /// <summary>
        /// Finalizes a process
        /// </summary>
        public void OnSuccess()
        {
            CurrentState = WorkerStates.Completed;
        }
    }
}
