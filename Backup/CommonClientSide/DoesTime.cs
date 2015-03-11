using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClientSide {
    public interface DoesTime {
        string TimesDateControlDatabaseField { get;set;}
        string DatesTimeControlDatabaseField { get;set;}
    }
}
