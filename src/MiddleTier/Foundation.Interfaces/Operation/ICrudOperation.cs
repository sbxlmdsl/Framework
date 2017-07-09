//-----------------------------------------------------------------------
// <copyright file="ICrudOperation.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Foundation.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    [CLSCompliant(true)]
    public interface ICrudOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns></returns>
        TEntity Create();

        /// <summary>
        /// Retrieve TEntity objects operation
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Update the object
        /// </summary>
        TEntity Update();

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        bool Delete();      
    }
}
