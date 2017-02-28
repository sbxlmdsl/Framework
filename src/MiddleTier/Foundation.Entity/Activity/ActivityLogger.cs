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
using Genesys.Extras.Exceptions;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Code-first class that records exceptions to a 100% uptime database
    /// Default connection string is: DefaultConnection. 
    /// Can be changed via passing new ConnectionString name to the constructor
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    public class ActivityLogger : Activity
    {
        /// <summary>
        /// Connection string key name, from Web.config or App_Data\ConnectionStrings.config
        /// Default is DefaultConnection
        /// </summary>
        protected virtual string ConnectionStringName { get; set; } = "DefaultConnection";

        /// <summary>
        /// Database schema name
        /// Default is dbo
        /// </summary>
        protected virtual string DatabaseSchemaName { get; set; } = "Activity";

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
        /// <param name="connectStringName">Key/Name of the connection string in the .config file or database</param>
        /// <remarks></remarks>
        public ActivityLogger(string connectStringName) : this()
        {
            ConnectionStringName = connectStringName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectStringName"></param>
        /// <param name="databaseSchemaName"></param> 
        /// <remarks></remarks>
        public ActivityLogger(string connectStringName, string databaseSchemaName) : this(connectStringName)
        {
            DatabaseSchemaName = databaseSchemaName;
        }

        /// <summary>
        /// Hydrates object and saves the record 
        /// </summary>
        public static int Create(string connectStringName, string databaseSchema)
        {
            ActivityLogger log = new ActivityLogger();
            log.ConnectionStringName = connectStringName;
            log.DatabaseSchemaName = databaseSchema;
            return log.Save();
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        public static Activity GetByID(int id, string connectStringName, string databaseSchema)
        {
            Activity returnValue = new Activity();
            var dbContext = new DatabaseContext(connectStringName, databaseSchema);

            try
            {
                if (id != TypeExtension.DefaultInteger)
                {
                    returnValue = dbContext.EntityData.Where(x => x.ActivityID == id).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(Activity), String.Format("ActivityLogger.GetByID({0})", id.ToString()), connectStringName, databaseSchema);
            }

            return returnValue;
        }

        /// <summary>
        /// Saves object to database
        /// </summary>
        public virtual int Save()
        {
            var dbContext = new DatabaseContext(this.ConnectionStringName, this.DatabaseSchemaName);

            try
            {
                if (ActivityID == TypeExtension.DefaultInteger)
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

            return ActivityID;
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
            public System.Data.Entity.DbSet<Activity> EntityData { get; set; }

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
