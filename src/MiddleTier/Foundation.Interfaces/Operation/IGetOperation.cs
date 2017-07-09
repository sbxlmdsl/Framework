//-----------------------------------------------------------------------
// <copyright file="IGetOperation.cs" company="Genesys Source">
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
    /// Read operations against an Async datastore, such as Http resource server
    /// Both ID and Key can be used as 1-1 unique idenfiers
    ///  For Internal, high-performance, multi-join lookups: int ID
    ///  For External, low-volume tables, obfuscated, guaranteed-unique: Guid Key
    /// </summary>
    /// <typeparam name="TEntity">Entity type to be read</typeparam>
    [CLSCompliant(true)]
    public interface IGetOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets all items from the datastore
        /// Expects additional constraints to be attached by the caller
        ///  Usage: customer.GetAll().Where(x => x.FirstName == "Jo")
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// All data in this datastore subset, except records with default ID/Key
        ///  Criteria: Where ID != TypeExtension.DefaultInteger And Also Key != TypeExtension.DefaultGuid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        IQueryable<TEntity> GetAllExcludeDefault();

        /// <summary>
        /// Gets one or no items based on exact ID match
        /// </summary>
        /// <returns>One or no TEntity based on exact ID match</returns>
        TEntity GetByID(int id);

        /// <summary>
        /// Gets one or no items based on exact Key match
        /// </summary>
        /// <returns>One or no TEntity based on exact Key match</returns>
        TEntity GetByKey(Guid key);

        /// <summary>
        /// Get entities list by where clause
        /// </summary>
        /// <param name="whereClause">Where clause expression</param>
        /// <returns>Roughly: Entity.Where(whereClause)</returns>
        IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, Boolean>> whereClause);

        /// <summary>
        /// Get entities list with paging system
        /// </summary>
        /// <param name="whereClause">Where clause expression</param>
        /// /// <param name="orderByClause">Order by clause expression</param>
        /// /// <param name="pageSize">Max number of results to be returned and in each page</param>
        /// /// <param name="pageNumber">Which page to retrieve</param>
        /// <returns>Roughly: Entity.Where(whereClause).OrderBy(orderByClause).Skip(pageSize*pageNumger).Take(pageSize)</returns>
        IQueryable<TEntity> GetByPage(Expression<Func<TEntity, Boolean>> whereClause, Expression<Func<TEntity, Boolean>> orderByClause, int pageSize, int pageNumber);
    }
}
