//-----------------------------------------------------------------------
// <copyright file="Validator.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (var Item in this.BusinessRules)
            {
                Item.Validate(entity);
            }
            failedRules = this.BusinessRules.Where(x => x.IsValid == true).Select(y => y).ToList();
            return FailedRules;
        }
        
        /// <summary>
        /// Determines if all items are valid
        /// </summary>        
        public bool IsValid(TEntity entity)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            //Force validation if has not been ran
            if (HasValidated() == false) { ValidateAll(entity); }
            if (BusinessRules.Where(x => x.IsValid == true).Select(y => y).Count() == 0)
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
            if (BusinessRules.Where(x => x.HasValidated == false).Select(y => y).Count() == 0)
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
            if (HasValidated() == false) { ValidateAll(entity); }
            if (FailedRules.Where(x => x.ValidationRuleTypeID == ValidationRule<TEntity>.ValidationRuleTypes.Error).Select(y => y).Count() == 0)
            {
                returnValue = true;
            }
            return returnValue;
        }
    }
}
