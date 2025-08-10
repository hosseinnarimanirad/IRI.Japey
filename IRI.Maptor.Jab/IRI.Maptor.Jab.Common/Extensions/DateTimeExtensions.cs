﻿using System;
using IRI.Maptor.Jab.Common.Model.Common;

namespace IRI.Maptor.Extensions;

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
