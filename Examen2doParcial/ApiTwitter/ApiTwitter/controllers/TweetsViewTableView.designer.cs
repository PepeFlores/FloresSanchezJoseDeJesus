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
    [Register ("TweetsViewTableView")]
    partial class TweetsViewTableView
    {
        [Outlet]
        UIKit.UIImageView imgRetweed { get; set; }


        [Outlet]
        UIKit.UILabel lblRetweed { get; set; }


        [Outlet]
        UIKit.UILabel lbltweet { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgRetweed != null) {
                imgRetweed.Dispose ();
                imgRetweed = null;
            }

            if (lblRetweed != null) {
                lblRetweed.Dispose ();
                lblRetweed = null;
            }

            if (lbltweet != null) {
                lbltweet.Dispose ();
                lbltweet = null;
            }
        }
    }
}