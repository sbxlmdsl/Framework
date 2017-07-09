//-----------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extras.Collections;

namespace Genesys.Foundation.Validation
{
    /// <summary>
    /// class containing basics of a ValidationRule, used to pull out result data from a ValidationRule
    /// </summary>
    [CLSCompliant(true)]
    public class ValidationResult : KeyValuePairString
    {
        /// <summary>
        /// Language to localize messages
        /// </summary>
        public string LanguageISO { get { return base.Key; } set { base.Key = value; } } 

        /// <summary>
        /// Validation message (localized)
        /// </summary>
        public string Message { get { return base.Value; } set { base.Value = value; } }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <param name="languageISO">Language of message</param>
        public ValidationResult(string message, string languageISO = "eng") : base() { Message = message; this.LanguageISO = languageISO; }
        
    }
}
