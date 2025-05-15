using System; 
using IRI.Jab.Common.Model.Common;

namespace IRI.Extensions;

public static class DateTimeExtensions
{
    public static SpecialDateTime AsSpecial(this DateTime dateTime)
    {
        return new SpecialDateTime(dateTime);
    }

    public static SpecialDateTime AsSpecial(this DateTime? dateTime)
    {
        return new SpecialDateTime(dateTime);
    }

    
}
