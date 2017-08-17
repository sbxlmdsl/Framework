//-----------------------------------------------------------------------
// <copyright file="IValidator.cs" company="Genesys Source">
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
using System.Collections.Generic;

namespace Genesys.Framework.Validation
{
    /// <summary>
    /// Supports self-validation, especially when data is to be persisted to the database.
    /// </summary>
    [CLSCompliant(true)]
    public interface IValidator<TEntity>
    {
        /// <summary>
        /// Business Rules to validate
        /// </summary>
        List<ValidationRule<TEntity>> BusinessRules { get; set; }

        /// <summary>
        /// Rules that failed validation
        /// </summary>
        List<ValidationRule<TEntity>> FailedRules { get; }

        /// <summary>
        /// Validate all rules
        /// </summary>
        
        List<ValidationRule<TEntity>> ValidateAll(TEntity Entity);
    }
}
