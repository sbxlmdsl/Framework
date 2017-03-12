//-----------------------------------------------------------------------
// <copyright file="ReadOnlyEntity.cs" company="Genesys Source">
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
using System.Linq;
using System.Data;
using System.Reflection;
using System.Data.Entity;
using System.Linq.Expressions;
using Genesys.Extensions;
using Genesys.Foundation.Activity;
using Genesys.Extras.Serialization;
using Genesys.Extras.Configuration;
using Genesys.Foundation.Data;
using Genesys.Foundation.Validation;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// ReadOnlyEntityBase
    /// </summary>
    /// <remarks>ReadOnlyEntityBase</remarks>
    [CLSCompliant(true)]
    public abstract class ReadOnlyEntity<TEntity> : Validator<TEntity>, IReadOnlyEntity where TEntity : ReadOnlyEntity<TEntity>, new()
    {
        private const string connectionStringDefault = "DefaultConnection";
        
        /// <summary>
        /// ID of record
        /// </summary>
        public virtual int ID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Workflow activity that created this record
        /// </summary>
        public virtual int ActivityID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Status of this record
        /// </summary>        
        public virtual int RecordStatus { get; set; } = (int)RecordStatusValues.Default;

        /// <summary>
        /// Serialize and Deserialize this class
        /// </summary>
        JsonSerializer<TEntity> Serializer = new JsonSerializer<TEntity>();
        
        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public ReadOnlyEntity() : base() { this.Initialize<ReadOnlyEntity<TEntity>>(); }
        
        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        public static TEntity GetByID(int id)
        {
            var dbContext = new DatabaseContext();
            var returnValue = new TEntity();

            try
            {
                if (id != TypeExtension.DefaultInteger)
                {
                    returnValue = dbContext.EntityData.Where(x => x.ID == id).FirstOrDefaultSafe();
                }                
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("ReadOnlyEntityBase.GetByID({0})", id.ToString()), "DefaultConnection", "MiddleTier");
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="Key">The unique ID of the object</param>
        public static TEntity GetByKey(Guid Key)
        {
            var returnValue = new TEntity();
            var dbContext = new DatabaseContext();

            try
            {
                if (Key != TypeExtension.DefaultGuid)
                {
                    returnValue = dbContext.EntityData.Where(x => x.Key == Key).FirstOrDefaultSafe();
                }
                    
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("ReadOnlyEntityBase.GetByKey({0})", Key.ToString()), "DefaultConnection", "MiddleTier");
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all from database
        /// Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public static IQueryable<TEntity> GetAll()
        {
            var dbContext = new DatabaseContext();
            IQueryable<TEntity> returnValue = default(IQueryable<TEntity>);

            try
            {
                returnValue = dbContext.EntityData.Take(new TEntity().GetAttributeValue<TakeRows, int>(100));
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "ReadOnlyEntityBase.GetAll()", "DefaultConnection", "MiddleTier");
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
        public IQueryable<TEntity> GetWithPaging(Expression<Func<TEntity, Boolean>> whereClause, Expression<Func<TEntity, Boolean>> orderByClause, int pageSize, int pageNumber)
        {
            var dbContext = new DatabaseContext();
            IQueryable<TEntity> result = null;
            result = (dbContext.EntityData.Select(x => x)).AsQueryable();
            if (whereClause != null) { result = result.Where<TEntity>(whereClause).AsQueryable(); }
            if (orderByClause != null) { result = result.OrderBy(orderByClause).AsQueryable(); }
            if (pageNumber > 0 && pageSize > 0)
                result = result.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable();
            return result;
        }
        
        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="newItem">Item to compare</param>
        public bool Equals(TEntity newItem)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            Type newObjectType = newItem.GetType();
            returnValue = true;

            foreach (var newObjectProperty in newObjectType.GetProperties())
            {                
                PropertyInfo CurrentProperty = typeof(TEntity).GetProperty(newObjectProperty.Name);
                if (CurrentProperty.CanWrite == true)
                {
                    if (object.Equals(CurrentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false)
                    {
                        returnValue = false;
                        break;
                    }
                }
            }
            
            return returnValue;
        }

        /// <summary>
        /// Start with ID as string representation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Key.ToString();
        }

        /// <summary>
        /// Pulls the connection string from an attribute, used to feed DbContext the full EF connection string
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            var returnValue = TypeExtension.DefaultString;
            ConnectionStringSafe configConnectString = new ConnectionStringSafe();
            ConfigurationManagerFull configManager = new ConfigurationManagerFull();

            configConnectString = configManager.ConnectionString(new TEntity().GetAttributeValue<ConnectionString>(connectionStringDefault));
            returnValue = configConnectString.ToEF(typeof(TEntity));
            if (returnValue == TypeExtension.DefaultString) { throw new System.Exception("Connection string could not be found. A valid connection string required for data access."); }

            return returnValue;
        }

        /// <summary>
        /// DB Context - Entity Framework uses this to connect to the database
        /// </summary>
        protected class DatabaseContext : System.Data.Entity.DbContext
        {
            /// <summary>
            /// Worker DbSet class that saves the entity
            /// </summary>
            public System.Data.Entity.DbSet<TEntity> EntityData { get; set; }

            /// <summary>
            /// Class will throw exception if encountered
            /// </summary>
            public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;
            
            /// <summary>
            /// Constructor. Explicitly set database connection.
            /// </summary>            
            public DatabaseContext()
                : base(ConnectionString())
            {
                #if (DEBUG)
                    ThrowException = true;
                #endif
            }
        }
        
        /// <summary>
        /// Initializes database, code first style
        /// </summary>
        protected class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context">User, device and app context</param>            
            protected override void Seed(DatabaseContext context)
            {
                base.Seed(context);
            }
        }
        
        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            JsonSerializer<TEntity> serializer = new JsonSerializer<TEntity>();
            return serializer.Serialize(this);
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyEntity<TEntity> Deserialize(string jsonString)
        {
            JsonSerializer<TEntity> serializer = new JsonSerializer<TEntity>();
            return serializer.Deserialize(jsonString);
        }
    }
}
