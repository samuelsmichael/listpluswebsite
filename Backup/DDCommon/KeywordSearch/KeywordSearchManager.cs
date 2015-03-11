using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCommon.cs.KeywordSearch {
    public class KeywordSearchManager {
        private IStringSearchAlgorithm _searchAlgorithm;
        public KeywordSearchManager() {
            _searchAlgorithm=new StringSearch();
        }
        public static string[] stemmicateSearchString(string str) {
            if (str == null) {
                return new string[0];
            } else {
                PorterStemmer stem = new PorterStemmer();
                List<string> ar2 = new List<string>();
                StringBuilder sb = new StringBuilder();
                int d = 0;
                bool inWord = true;
                foreach (char cc in str) {
                    if (cc == ' ') {
                        if (inWord) {
                            ar2.Add(sb.ToString());
                            sb.Length = 0;
                        } else {
                            inWord = false;
                        }
                    } else {
                        sb.Append(cc);
                    }
                }
                if (sb.Length > 0) {
                    ar2.Add(sb.ToString());
                }
                for (d = 0; d < ar2.Count; d++) {
                    ar2[d] = stem.stemTerm(ar2[d]);
                }
                List<string> ar3 = new List<string>();
                foreach (string str44 in ar2) {
                    if (str44.Trim().Length>2) {
                        ar3.Add(str44);
                    }
                }
                int c=0;
                string[] sa = new string[ar3.Count];
                foreach (string str2 in ar3) {
                    sa[c] = str2.ToLower();
                    c++;
                }
                return sa;
            }
        }
        public double rate(string stringBeingSearched, string[] keywords) {
            double rating=0d;
            _searchAlgorithm.Keywords=keywords;
            foreach(StringSearchResult result in _searchAlgorithm.FindAll(stringBeingSearched)) {
                rating++;
            }
            return rating;
        }
        public double rateALaPatternMatching(string pattern, string stringBeingSearched) {
            string[] stringsInPattern = pattern.Split(new char[] {' '});
            double count=0d;
            foreach (string str in stringsInPattern) {
                string strt = str.Trim();
                if (strt.Length > 0) {
                    Regex regex = new Regex(str);
                    MatchCollection mc = regex.Matches(stringBeingSearched, 0);
                    count += (double)mc.Count;
                }
            }
            return count;
        }
    }
}
