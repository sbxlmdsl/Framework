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

using Genesys.Foundation.Text;

namespace Genesys.Foundation.Validation
{   
    /// <summary>
    /// Validation Rule contract
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IValidationRule<TEntity>
    {
        /// <summary>
        /// Is this rule valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Has this rule been validated
        /// </summary>
        bool HasValidated { get; }

        /// <summary>
        /// Can save to the data store, depending on ValidationTypeTypeID != CannotSave
        /// </summary>
        
        bool CanSave { get; }

        /// <summary>
        /// Validate this entity
        /// </summary>
        /// <param name="entity"></param>
        
        bool Validate(TEntity entity);

        /// <summary>
        /// Type of rule, drives database behavior
        /// </summary>
        int ValidationRuleTypeID { get; }

        /// <summary>
        /// Result message
        /// </summary>
        IText Result { get; }
    }    
}
