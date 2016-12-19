//-----------------------------------------------------------------------
// <copyright file="ProcessResult.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;
using Genesys.Extras.Collections;
using Genesys.Foundation.Validation;

namespace Genesys.Foundation.Process
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    [CLSCompliant(true)]
    public class ProcessResult : IProcessResult
    {
        /// <summary>
        /// States of a current, past and future process
        /// </summary>
        public struct States
        {
            /// <summary>
            /// Process never executed
            /// </summary>
            public static int NeverRan = TypeExtension.DefaultInteger;
            /// <summary>
            /// Process is pending execution
            /// </summary>
            public static int Pending = 1;
            /// <summary>
            /// Process is currently running
            /// </summary>
            public static int Running = 2;
            /// <summary>
            /// Process completed with no errors
            /// </summary>
            public static int Completed = 3;
            /// <summary>
            /// Process failed to complete
            /// </summary>
            public static int Failed = 4;
        }
        
        /// <summary>
        /// Current state of the process
        /// </summary>
        public int CurrentState { get; set; } = States.NeverRan;

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
        public ProcessResult() : base()
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
            CurrentState = States.Running;
        }

        /// <summary>
        /// Records failure of a process
        /// </summary>
        public void OnFail(string errorMessage)
        {
            CurrentState = States.Failed;
            RuleFailed(errorMessage);
        }

        /// <summary>
        /// Finalizes a process
        /// </summary>
        public void OnSuccess()
        {
            CurrentState = States.Completed;
        }
    }
}
