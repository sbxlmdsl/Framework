//-----------------------------------------------------------------------
// <copyright file="GenericLayout.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Controls;

namespace Genesys.Foundation.Themes
{
    /// <summary>
    /// Default layout for Generic theme
    /// </summary>
	public class GenericLayout : Control
    {
        /// <summary>
        /// Constructor
        /// </summary>
		static GenericLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericLayout), new FrameworkPropertyMetadata(typeof(GenericLayout)));
        }

        /// <summary>
        /// Title
        /// </summary>
		public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Title Property
        /// </summary>
		public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GenericLayout), new UIPropertyMetadata());

        /// <summary>
        /// Content Pane Header
        /// </summary>
		public object ContentHeader
        {
            get { return (object)GetValue(ContentHeaderProperty); }
            set { SetValue(ContentHeaderProperty, value); }
        }
        /// <summary>
        /// ContentHeader Property
        /// </summary>
		public static readonly DependencyProperty ContentHeaderProperty =
            DependencyProperty.Register("ContentHeader", typeof(object), typeof(GenericLayout), new UIPropertyMetadata());

        /// <summary>
        /// Main content pane body
        /// </summary>
		public object ContentBody
        {
            get { return (object)GetValue(ContentBodyProperty); }
            set { SetValue(ContentBodyProperty, value); }
        }

        /// <summary>
        /// ContentBody Property
        /// </summary>
		public static readonly DependencyProperty ContentBodyProperty =
            DependencyProperty.Register("ContentBody", typeof(object), typeof(GenericLayout), new UIPropertyMetadata());
    }
}
