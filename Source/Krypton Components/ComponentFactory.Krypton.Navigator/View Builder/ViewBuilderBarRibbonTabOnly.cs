﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006-2020, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to license terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2020. All rights reserved. (https://github.com/Wagnerp/Krypton-NET-5.480)
//  Version 5.480.0.0  www.ComponentFactory.com
// *****************************************************************************

using ComponentFactory.Krypton.Toolkit;

namespace ComponentFactory.Krypton.Navigator
{
    /// <summary>
    /// Implements the NavigatorMode.BarRibbonTabOnly mode.
    /// </summary>
    internal class ViewBuilderBarRibbonTabOnly : ViewBuilderBarRibbonTabBase
    {
        #region Public
        /// <summary>
        /// Gets a value indicating if the mode is a tab strip style mode.
        /// </summary>
        public override bool IsTabStripMode => true;

        /// <summary>
        /// User has used the keyboard to select the currently selected page.
        /// </summary>
        public override void KeyPressedPageView()
        {
            // If there is a currently selected page
            if (Navigator.SelectedPage != null)
            {
                // Grab the view for the page
                INavCheckItem checkItem = _pageLookup[Navigator.SelectedPage];

                // If the item also has the focus
                if (checkItem.HasFocus)
                {
                    // Then perform the click action for the button
                    checkItem.PerformClick();
                }
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Create the view hierarchy for this view mode.
        /// </summary>
        protected override void CreateCheckItemView()
        {
            // Create the view element that lays out the check buttons
            _layoutBar = new ViewLayoutBar(Navigator.StateCommon.Bar,
                                           PaletteMetricInt.RibbonTabGap,
                                           Navigator.Bar.ItemSizing,
                                           Navigator.Bar.ItemAlignment,
                                           Navigator.Bar.BarMultiline,
                                           Navigator.Bar.ItemMinimumSize,
                                           Navigator.Bar.ItemMaximumSize,
                                           Navigator.Bar.BarMinimumHeight,
                                           Navigator.Bar.TabBorderStyle,
                                           true);

            // Create the scroll spacer that restricts display
            _layoutBarViewport = new ViewLayoutViewport(Navigator.StateCommon.Bar,
                                                        PaletteMetricPadding.BarPaddingTabs,
                                                        PaletteMetricInt.RibbonTabGap,
                                                        Navigator.Bar.BarOrientation,
                                                        Navigator.Bar.ItemAlignment,
                                                        Navigator.Bar.BarAnimation)
            {
                _layoutBar
            };

            // Create the button bar area docker
            _layoutBarDocker = new ViewLayoutDocker
            {
                { _layoutBarViewport, ViewDockStyle.Fill }
            };

            // Add a separators for insetting items
            _layoutBarSeparatorFirst = new ViewLayoutSeparator(0);
            _layoutBarSeparatorLast = new ViewLayoutSeparator(0);
            _layoutBarDocker.Add(_layoutBarSeparatorFirst, ViewDockStyle.Left);
            _layoutBarDocker.Add(_layoutBarSeparatorLast, ViewDockStyle.Right);

            // Create the docker used to layout contents of main panel and fill with group
            _layoutPanelDocker = new ViewLayoutDocker
            {
                { _layoutBarDocker, ViewDockStyle.Fill },
                { new ViewLayoutPageHide(Navigator), ViewDockStyle.Top }
            };

            // Create the top level panel and put a layout docker inside it
            _drawPanel = new ViewDrawPanel(Navigator.StateNormal.Back)
            {
                _layoutPanelDocker
            };
            _newRoot = _drawPanel;

            // Must call the base class to perform common actions
            base.CreateCheckItemView();
        }
        #endregion
    }
}
