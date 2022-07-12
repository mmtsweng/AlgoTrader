using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDAL
{
    /// <summary>
    /// From 
    /*EASObject:
  "access_token": "string",
  "refresh_token": "string",
  "token_type": "string",
  "expires_in": 0,
  "scope": "string",
  "refresh_token_expires_in": 0 */
    /// </summary>
    public class TDATokens
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string  token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public int refresh_token_expires_in { get; set; }

    }

}