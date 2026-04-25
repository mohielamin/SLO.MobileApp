using Microsoft.Maui;
using Microsoft.Maui.Controls;
using SLO.MobileApp.Controls.Bases;
using SLO.MobileApp.Controls.TemplatedViews;

namespace SLO.MobileApp.Controls.Entries;

public partial class NumericEntry : TemplatedEntryView
{
    public NumericEntry()
    {
        InitializeComponent();
    }

    private void TextChangedEvent(
        object sender, TextChangedEventArgs e)
    {
        if (sender is not EntryBase entryBase)
        {
            return;
        }

        if (entryBase.Keyboard != Keyboard.Numeric)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            return;
        }

        bool isParsed =
            int.TryParse(s: e.NewTextValue, out _);

        if (isParsed is false)
        {
            entryBase.Text = e.OldTextValue;
        }
    }
}