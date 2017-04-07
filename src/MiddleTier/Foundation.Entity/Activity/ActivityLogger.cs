//-----------------------------------------------------------------------
// <copyright file="ActivityLogger.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
// 
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
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Foundation.Data;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Code-first class that records exceptions to a 100% uptime database
    /// Default connection string is: DefaultConnection. 
    /// Can be changed via passing new ConnectionString name to the constructor
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    public class ActivityLogger : ActivityContext
    {
        /// <summary>
        /// Name/key of connection string
        /// Defailt: DefaultConnection
        /// </summary>
        private string  ConnectionString { get; set; } = ConnectionStringName.DefaultValue;

        /// <summary>
        /// Database schema name for Activity records.
        /// Default: Activity
        /// </summary>
        private string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultActivityValue;

        /// <summary>
        /// This protected constructor should not be called. Factory methods should be used instead.
        /// </summary>
        protected ActivityLogger() : base()
        {
            ExecutingContext = ExecutingContextInfo();
            StackTrace = Environment.StackTrace;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        /// <remarks></remarks>
        public ActivityLogger(string connectStringName, string databaseSchema) : this()
        {
            ConnectionString = connectStringName;
            DatabaseSchema = databaseSchema;
        }

        /// <summary>
        /// Fills and saves an activity
        /// </summary>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        /// <remarks></remarks>
        public static int Create(string connectStringName, string databaseSchema)
        {
            ActivityLogger log = new ActivityLogger(connectStringName, databaseSchema) { };
            return log.Save();
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        public static IQueryable<ActivityContext> GetAll()
        {
            var logger = new ActivityLogger();
            return ActivityLogger.GetAll(logger.ConnectionString, logger.DatabaseSchema);
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        public static IQueryable<ActivityContext> GetAll(string connectStringName, string databaseSchema)
        {
            var returnValue = default(IQueryable<ActivityContext>);
            var dbContext = new DatabaseContext(connectStringName, databaseSchema);

            try
            {
                    returnValue = dbContext.EntityData;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(ActivityContext), String.Format("ActivityLogger.GetByID({0})"));
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        public static ActivityContext GetByID(int id)
        {
            var logger = new ActivityLogger();
            return ActivityLogger.GetByID(id, logger.ConnectionString, logger.DatabaseSchema);
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        public static ActivityContext GetByID(int id, string connectStringName, string databaseSchema)
        {
            var returnValue = new ActivityContext();
            var dbContext = new DatabaseContext(connectStringName, databaseSchema);

            try
            {
                if (id != TypeExtension.DefaultInteger)
                {
                    returnValue = dbContext.EntityData.Where(x => x.ActivityContextID == id).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(ActivityContext), String.Format("ActivityLogger.GetByID({0})", id.ToString()));
            }

            return returnValue;
        }

        /// <summary>
        /// Saves object to database
        /// </summary>
        public virtual int Save()
        {
            var dbContext = new DatabaseContext(ConnectionStringName.DefaultValue, DatabaseSchemaName.DefaultActivityValue);

            try
            {
                if (ActivityContextID == TypeExtension.DefaultInteger)
                {
                    ExecutingContext = ExecutingContextInfo();
                    dbContext.EntityData.Add(this);
                    dbContext.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dbContext.Dispose();
            }

            return ActivityContextID;
        }

        /// <summary>
        /// Builds runtime context in the format of: Assembly FQN || Executing location || Machine Name - Domain\User
        /// </summary>
        /// <returns></returns>
        private string ExecutingContextInfo()
        {
            var returnValue = TypeExtension.DefaultString;

            try
            {
                returnValue = String.Format(@"{0} || {1} || {2} - {3}\{4}",
                    Assembly.GetExecutingAssembly().FullName,
                    Assembly.GetExecutingAssembly().Location,
                    Environment.MachineName, Environment.UserDomainName, Environment.UserName);
            }
            catch
            {
                returnValue = TypeExtension.DefaultString;
            }

            return returnValue;
        }

        /// <summary>
        /// DB Context - Entity Framework uses this to connect to the database
        /// </summary>
        protected class DatabaseContext : System.Data.Entity.DbContext
        {
            private string databaseSchemaField = TypeExtension.DefaultString;

            /// <summary>
            /// BusinessEntity
            /// </summary>
            public System.Data.Entity.DbSet<ActivityContext> EntityData { get; set; }

            /// <summary>
            /// Constructor. Explicitly set database connection.
            /// </summary>
            /// <remarks></remarks>
            public DatabaseContext(string connectStringName, string databaseSchema)
                : base(ConfigurationManagerFull.ConnectionStrings.GetValue(connectStringName))
            {
                databaseSchemaField = databaseSchema;
            }

            /// <summary>
            /// SaveChanges - Saves the object to the database
            /// </summary>
            public override int SaveChanges()
            {
                return base.SaveChanges();
            }

            /// <summary>
            /// Set values when creating a model in the database
            /// </summary>
            /// <param name="modelBuilder"></param>
            /// <remarks></remarks>
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultSchema(this.databaseSchemaField);
                base.OnModelCreating(modelBuilder);
                modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            }
        }

        /// <summary>
        /// Initializes the database
        /// </summary>
        public void Initialize()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>((global::System.Data.Entity.IDatabaseInitializer<ActivityLogger.DatabaseContext>)new DatabaseInitializer());
        }

        /// <summary>
        /// Class that initializes and handles seed/identity
        /// </summary>
        protected class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context"></param>
            /// <remarks></remarks>
            protected override void Seed(DatabaseContext context)
            {
                base.Seed(context);
            }
        }

    }
}
