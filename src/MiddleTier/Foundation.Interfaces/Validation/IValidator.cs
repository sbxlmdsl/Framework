//-----------------------------------------------------------------------
// <copyright file="IValidator.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Genesys.Foundation.Validation
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
