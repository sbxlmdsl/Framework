//-----------------------------------------------------------------------
// <copyright file="IWpfApplication.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface IWpfApplication : IApplication, INavigateUri
    {
        /// <summary>
        /// Currently active page Uri
        /// </summary>
        Uri CurrentPage { get; }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        Uri StartupUri { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        Uri HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        Uri ErrorPage { get; }
    }
}