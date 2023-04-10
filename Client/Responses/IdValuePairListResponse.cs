using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Responses;

public class IdValuePairListResponse : BaseResponse
{
    public List<IdValuePair> IdValuePairs { get; set; }
}
