////-----------------------------------------------------------------------
//// <copyright file="SearchOperation.cs" company="Genesys Source">
////      Copyright (c) Genesys Source. All rights reserved.
////      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
////      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
////       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
//// </copyright>
////-----------------------------------------------------------------------
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Genesys.Extensions;
//using Genesys.Foundation.Entity;
//using Genesys.Foundation.Data;

//namespace Genesys.Foundation.Operation
//{
//    /// <summary>
//    /// Human-friendly text search operation
//    /// </summary>
//    /// <typeparam name="TEntity">Entity to Search</typeparam>
//    [CLSCompliant(true)]
//    public class SearchEntity<TEntity> where TEntity : IEntity
//    {
//        /// <summary>
//        /// Search rules to run
//        /// </summary>
//        public List<SearchClause<TEntity>> Clauses { get; set; } = new List<SearchClause<TEntity>>();

//        /// <summary>
//        /// Result of search
//        /// </summary>
//        public IQueryable<TEntity> Result { get; }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public SearchOperation() : base() { }

//        /// <summary>
//        /// Runs all business rules
//        /// </summary>

//        public List<SearchClause<TEntity>> DoSearch(TEntity entity)
//        {
//            foreach (var Item in Clauses)
//            {
//                Item..Search(entity);
//            }
//            failedRules = this.SearchRules.Where(x => x.IsFound == true).Select(y => y).ToList();
//            return FailedRules;
//        }

//        /// <summary>
//        /// Determines if all items are valid
//        /// </summary>        
//        public bool IsValid(TEntity entity)
//        {
//            bool returnValue = TypeExtension.DefaultBoolean;
//            //Force validation if has not been ran
//            if (HasSearched() == false) { DoSearch(entity); }
//            if (SearchRules.Where(x => x.IsFound == true).Select(y => y).Count() == 0)
//            {
//                returnValue = true;
//            }
//            return returnValue;
//        }

//        /// <summary>
//        /// Determines if all items are valid
//        /// </summary>

//        public bool HasSearched()
//        {
//            bool returnValue = TypeExtension.DefaultBoolean;
//            if (SearchRules.Where(x => x.HasSearched == false).Select(y => y).Count() == 0)
//            {
//                returnValue = true;
//            }
//            return returnValue;
//        }

//        /// <summary>
//        /// Determines if any failures restrict persisting to database. Will run Search() if has not been Searchd yet.
//        /// </summary>

//        public bool CanSave(TEntity entity)
//        {
//            bool returnValue = TypeExtension.DefaultBoolean;
//            //Force validation if has not been ran
//            if (HasSearched() == false) { DoSearch(entity); }
//            if (FailedRules.Where(x => x.SearchRuleTypeID == SearchClause<TEntity>.SearchRuleTypes.ErrorCantSave).Select(y => y).Count() == 0)
//            {
//                returnValue = true;
//            }
//            return returnValue;
//        }
//    }
//}
