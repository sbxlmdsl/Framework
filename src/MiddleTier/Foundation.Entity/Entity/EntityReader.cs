//-----------------------------------------------------------------------
// <copyright file="EntityReader.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Genesys.Extensions;
using Genesys.Foundation.Activity;
using Genesys.Foundation.Data;
using Genesys.Foundation.Operation;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// EntityReader - Read-only operation of an entity against a connection to a datastore
    /// </summary>
    [CLSCompliant(true)]
    public class EntityReader<TEntity> : IReadOperation<TEntity> where TEntity : EntityInfo<TEntity>, IEntity, new()
    {
        /// <summary>
        /// Class will throw exception if encountered
        /// </summary>
        public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public EntityReader() : base() { this.Initialize<EntityReader<TEntity>>(); }

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = default(IQueryable<TEntity>);

            returnValue = db.GetAll();

            return returnValue;
        }

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllExcludeDefault()
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = default(IQueryable<TEntity>);

            returnValue = db.GetAllExcludeDefault();

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        public TEntity GetByID(int id)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = new TEntity();

            returnValue = db.Data.Where(x => x.ID == id).FirstOrDefaultSafe();

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="key">The unique GUID of this object. ID and Key are both identifiers.</param>
        public TEntity GetByKey(Guid key)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = new TEntity();

            returnValue = db.Data.Where(x => x.Key == key).FirstOrDefaultSafe();

            return returnValue;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, Boolean>> whereClause)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = default(IQueryable<TEntity>);

            returnValue = (whereClause != null) ? db.Data.Where<TEntity>(whereClause) : db.Data;

            return returnValue;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <param name="orderByClause">Expression for order by clause</param>
        /// <param name="pageSize">Size of each result</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByPage(Expression<Func<TEntity, Boolean>> whereClause, Expression<Func<TEntity, Boolean>> orderByClause, int pageSize, int pageNumber)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var datastore = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = default(IQueryable<TEntity>);

            returnValue = (datastore.Data).AsQueryable();
            returnValue = (whereClause != null) ? returnValue.Where<TEntity>(whereClause).AsQueryable() : returnValue;
            returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
            returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable() : returnValue;

            return returnValue;
        }
    }
}
