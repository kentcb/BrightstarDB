using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BrightstarNotes
{
    class NoteCell : ViewCell
    {
        public NoteCell()
        {
            var titleLayout = CreateTitleLayout();
            var contentLayout = CreateContentLayout();

            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = {titleLayout, contentLayout}
            };

            View = viewLayout;
        }

        static Label CreateTitleLayout()
        {
            var titleLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            titleLabel.SetBinding(Label.TextProperty, "Title");
            return titleLabel;
        }

        static Label CreateContentLayout()
        {
            var contentLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 8.0
            };
            contentLabel.SetBinding(Label.TextProperty, "ShortContent");
            return contentLabel;
        }

    }
}
