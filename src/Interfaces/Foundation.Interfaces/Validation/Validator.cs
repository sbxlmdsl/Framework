//-----------------------------------------------------------------------
// <copyright file="Validator.cs" company="Genesys Source">
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
using System.Linq;
using Genesys.Extensions;

namespace Genesys.Foundation.Validation
{
    /// <summary>
    /// Self-validating rule based on Lambda expression
    /// </summary>
    /// <typeparam name="TEntity">Entity to validate</typeparam>
    [CLSCompliant(true)]
    public class Validator<TEntity> : IValidator<TEntity>
    {
        /// <summary>
        /// Failed rules
        /// </summary>
        private List<ValidationRule<TEntity>> failedRules = new List<ValidationRule<TEntity>>();
        
        /// <summary>
        /// Business rules to run
        /// </summary>
        public List<ValidationRule<TEntity>> BusinessRules { get; set; } = new List<ValidationRule<TEntity>>();

        /// <summary>
        /// Rules that failed validation
        /// </summary>
        public List<ValidationRule<TEntity>> FailedRules { get { return failedRules; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Validator() : base() { }
        
        /// <summary>
        /// Runs all business rules
        /// </summary>
        
        public List<ValidationRule<TEntity>> ValidateAll(TEntity entity)
        {
            foreach (IValidationRule<TEntity> Item in this.BusinessRules)
            {
                Item.Validate(entity);
            }
            failedRules = this.BusinessRules.Where(x => x.IsValid == true).Select(y => y).ToList();
            return this.FailedRules;
        }
        
        /// <summary>
        /// Determines if all items are valid
        /// </summary>        
        public bool IsValid(TEntity entity)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            //Force validation if has not been ran
            if (this.HasValidated() == false) { this.ValidateAll(entity); }
            if (this.BusinessRules.Where(x => x.IsValid == true).Select(y => y).Count() == 0)
            {
                returnValue = true;
            }
            return returnValue;
        }

        /// <summary>
        /// Determines if all items are valid
        /// </summary>
        
        public bool HasValidated()
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            if (this.BusinessRules.Where(x => x.HasValidated == false).Select(y => y).Count() == 0)
            {
                returnValue = true;
            }
            return returnValue;
        }
        
        /// <summary>
        /// Determines if any failures restrict persisting to database. Will run Validate() if has not been validated yet.
        /// </summary>
        
        public bool CanSave(TEntity entity)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            //Force validation if has not been ran
            if (this.HasValidated() == false) { this.ValidateAll(entity); }
            if (this.FailedRules.Where(x => x.ValidationRuleTypeID == ValidationRule<TEntity>.ValidationRuleTypes.ErrorCantSave).Select(y => y).Count() == 0)
            {
                returnValue = true;
            }
            return returnValue;
        }
    }
}
