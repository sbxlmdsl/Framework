//-----------------------------------------------------------------------
// <copyright file="IValidator.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
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
