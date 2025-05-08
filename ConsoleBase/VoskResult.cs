using System.Text;
using System.Globalization;

namespace ConsoleBase;

public class VoskResult(string text)
{
    public string Text { get; set; } = text;
    
    public void RemoveDiacritics()
    {
        var normalized = Text.Normalize(NormalizationForm.FormD);
        Text = new string(normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);
    }
}