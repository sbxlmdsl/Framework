//-----------------------------------------------------------------------
// <copyright file="GenericLayout.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Genesys.Foundation.Themes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenericLayout : Page
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericLayout()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// On-navigated to, supports passing of data
        /// </summary>
        /// <param name="e">Event args with data</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Content frame for use in navigation
        /// </summary>
        public Frame ContentFrame
        {
            get { return this.Body; }
        }
    }
}
