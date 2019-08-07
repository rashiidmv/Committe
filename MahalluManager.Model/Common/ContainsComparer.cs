using System;
using System.Collections.Generic;

namespace MahalluManager.Model.Common {
    public class ContainsComparer : IEqualityComparer<string> {
        public bool Equals(string x, string y) {
            if(String.IsNullOrEmpty(y))
                return false;
            if(y.ToLower().Contains(x.ToLower()))
                return true;
            else
                return false;
        }

        public int GetHashCode(string obj) {
            return obj.GetHashCode();
        }
    }
}
