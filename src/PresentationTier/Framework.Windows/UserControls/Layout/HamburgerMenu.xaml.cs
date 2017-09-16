//-----------------------------------------------------------------------
// <copyright file="HamburgerMenu.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Pages;
using Genesys.Framework.Entity;
using Genesys.Extensions;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows;

namespace Genesys.Framework.UserControls
{
    /// <summary>
    /// Interaction logic for HamburgerMenu.xaml
    /// </summary>
    public partial class HamburgerMenu : ReadOnlyControl
    {
        private bool isOpen = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Current state of the menu, open or closed
        /// </summary>
        public bool IsOpen { get { return isOpen; } }

        /// <summary>
        /// Open/Close animation duration
        /// </summary>
        public double AnimationDuration { get; set; } = 0.5;

        /// <summary>
        /// Width of menu bar when closed
        /// </summary>
        public int WidthClosed { get; set; } = 20;

        /// <summary>
        /// Width of menu bar when open
        /// </summary>
        public int WidthOpen { get; set; } = 200;

        /// <summary>
        /// Constructor
        /// </summary>
        public HamburgerMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <param name="modelData">Data to bind to page</param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, EventArgs e)
        {
            base.Partial_Loaded(sender, e);
        }

        /// <summary>
        /// Handles menu clicks to open or close the menu
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Menu_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.IsOpen)
            {
                MenuClose(sender, e);
            } else
            {
                MenuOpen(sender, e);
            }
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigate(MyApplication.HomePage);
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Search_Click(object sender, RoutedEventArgs e)
        { 
            
        }

        /// <summary>
        /// Navigates to CustomerCreate screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// Opens the menu based on mouse event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">EventArgs of event</param>
        public void MenuOpen(object sender, MouseEventArgs e)
        {
            var senderStrong = sender.CastSafe<Canvas>();
            var animation = new DoubleAnimation() { From = senderStrong.Width, To = WidthOpen, Duration = TimeSpan.FromSeconds(this.AnimationDuration), AutoReverse = false, RepeatBehavior = new RepeatBehavior(1) };
            senderStrong.BeginAnimation(Canvas.WidthProperty, animation);
            this.isOpen = true;
        }

        /// <summary>
        /// Closes the menu based on mouse event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">EventArgs of event</param>
        public void MenuClose(object sender, MouseEventArgs e)
        {
            var senderStrong = sender.CastSafe<Canvas>();
            var animation = new DoubleAnimation() { From = senderStrong.Width, To = WidthClosed, Duration = TimeSpan.FromSeconds(this.AnimationDuration),
                AutoReverse = false, RepeatBehavior = new RepeatBehavior(1) };
            senderStrong.BeginAnimation(Canvas.WidthProperty, animation);
            this.isOpen = false;
        }

        /// <summary>
        /// Validate this control
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return true;
        }
    }
}