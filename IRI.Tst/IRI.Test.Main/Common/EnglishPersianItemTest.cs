
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model.Globalization; 

namespace IRI.Test.NetFrameworkTest.Common;


public class EnglishPersianItemTest
{
    [Fact]
    public void TestEquality()
    {
        PersianEnglishItem pe0 = new PersianEnglishItem(null, null);
        PersianEnglishItem pe1 = new PersianEnglishItem(null, null);

        Assert.Equal(pe0 == pe0, true);
        Assert.Equal(pe0 == pe1, true);

        PersianEnglishItem pe2 = new PersianEnglishItem(null, "english");
        PersianEnglishItem pe3 = new PersianEnglishItem(null, "English");

        Assert.Equal(pe0 == pe2, false);
        Assert.Equal(pe2 == pe3, true);

        PersianEnglishItem pe4 = new PersianEnglishItem("فارسی", "English", Jab.Common.Model.LanguageMode.English);
        PersianEnglishItem pe5 = new PersianEnglishItem("فارسی", "English", Jab.Common.Model.LanguageMode.Persian);

        Assert.Equal(pe4 == pe5, true);
        Assert.Equal(pe4 == pe3, false);
        Assert.Equal(pe4 == pe0, false);

    }
}
