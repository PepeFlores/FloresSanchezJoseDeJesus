// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ApiTwitter
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UITableView TwitterTableView { get; set; }


        [Action ("BtnIcons8_TouchUpInside:")]
        partial void BtnIcons8_TouchUpInside (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (TwitterTableView != null) {
                TwitterTableView.Dispose ();
                TwitterTableView = null;
            }
        }
    }
}