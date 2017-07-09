//-----------------------------------------------------------------------
// <copyright file="IApplication.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extras.Configuration;
using System;
using System.Threading.Tasks;

namespace Genesys.Foundation.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface IApplication : IFrame
    {
        /// <summary>
        /// MyWebService
        /// </summary>
        Uri MyWebService { get; }

        /// <summary>
        /// Configuration data, XML .config style
        /// </summary>
        IConfigurationManager ConfigurationManager { get; }
    }
}