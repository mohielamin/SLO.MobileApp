using SLO.MobileApp.Controls.TemplatedViews;
using System;

namespace SLO.MobileApp.Controls.Buttons;

public partial class CircularButton : TemplatedButtonView
{
    public CircularButton()
    {
        InitializeComponent();
    }

    public override event EventHandler Clicked;

    private void ButtonClicked(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}