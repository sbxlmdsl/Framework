//-----------------------------------------------------------------------
// <copyright file="ReadOnlyDatabase.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Foundation.Activity;
using Genesys.Foundation.Entity;
using Genesys.Foundation.Operation;
using System;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public class ReadOnlyDatabase<TEntity> : DbContext, IGetOperation<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Worker DbSet class that gets/saves the entity. Important: Do not alter this property
        /// - This property is automatically set by Entity Framework DbContext
        /// - Must match the following conventions:
        ///  -- TEntity must be a partial class to an object in {Namespace}.edmx
        ///  -- TEntity namespace must match {Namespace}.edmx
        ///    -- I.e.: GenesysFoundationTest.edmx contains Genesys.Foundation.Test.CustomerInfo
        ///  -- Public scope, public get and public set required
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Class will throw exception if encountered
        /// </summary>
        public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Concurrency model to follow in middle tier, and optionally in the data tier
        /// </summary>
        /// <returns></returns>
        internal DataConcurrencies DataConcurrency()
        {
            var returnValue = DataConcurrencies.Pessimistic;
            var itemType = new TEntity();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataConcurrency) == true)
                {
                    returnValue = ((DataConcurrency)item).Value;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Instantiates and initializes. 
        ///  Do not allow new() due to constructor needing to instantiate ReadOnlyDatabase to get the attributes
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyDatabase<TEntity> Construct()
        {
            var returnValue = new ReadOnlyDatabase<TEntity>();
            var configManager = new ConfigurationManagerFull();
            var configConnectString = new ConnectionStringSafe();

            configConnectString = configManager.ConnectionString(returnValue.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultValue));
            if (configConnectString.ToEF(typeof(TEntity)) == TypeExtension.DefaultString)
            {
                throw new System.Exception("Connection string could not be found. A valid connection string required for data access.");
            } else
            {
                returnValue = new ReadOnlyDatabase<TEntity>(configConnectString.ToEF(typeof(TEntity)));
            }

            return returnValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ReadOnlyDatabase()
            : base()
        {

#if (DEBUG)
            ThrowException = true;
#endif            
        }

        /// <summary>
        /// Constructor. Explicitly set database connection.
        /// </summary>
        /// <param name="connectionString"></param>
        public ReadOnlyDatabase(string connectionString)
            : base(connectionString)
        {
            this.ConnectionString = connectionString;

#if (DEBUG)
            ThrowException = true;
#endif
        }
        
        /// <summary>
        /// All data in this datastore subset
        ///  Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public IQueryable<TEntity> GetAll()
        {
            var returnValue = default(IQueryable<TEntity>);

            try
            {
                returnValue = Data;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetAll()");
            }

            return returnValue;
        }

        /// <summary>
        /// All data in this datastore subset, except records with default ID/Key
        ///  Criteria: Where ID != TypeExtension.DefaultInteger And Also Key != TypeExtension.DefaultGuid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        public IQueryable<TEntity> GetAllExcludeDefault()
        {
            var returnValue = default(IQueryable<TEntity>);

            try
            {
                returnValue = Data.Where(x => x.ID != TypeExtension.DefaultInteger && x.Key != TypeExtension.DefaultGuid);
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetAllExcludeDefault()");
            }

            return returnValue;
        }

        /// <summary>
        /// Gets database record with exact ID match
        /// </summary>
        /// <param name="id">Database ID of the record to pull</param>
        /// <returns>Single entity that maches by id, or an empty entity for not foound</returns>
        public TEntity GetByID(int id)
        {
            var returnValue = new TEntity();

            try
            {
                returnValue = Data.Where(x => x.ID == id).FirstOrDefaultSafe();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetByID()");
            }

            return returnValue;
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that maches by Key, or an empty entity for not foound</returns>
        public TEntity GetByKey(Guid key)
        {
            var returnValue = new TEntity();

            try
            {
                returnValue = Data.Where(x => x.Key == key).FirstOrDefaultSafe();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetByKey()");
            }

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

            try
            {
                returnValue = (whereClause != null) ? db.Data.Where<TEntity>(whereClause) : db.Data;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetByWhere()");
            }

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

            try
            {
                returnValue = (datastore.Data).AsQueryable();
                returnValue = (whereClause != null) ? returnValue.Where<TEntity>(whereClause).AsQueryable() : returnValue;
                returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
                returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable() : returnValue;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyDatabase.GetByPage()");
            }

            return returnValue;
        }
    }
}
