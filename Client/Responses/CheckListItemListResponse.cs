using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Responses;

public class CheckListItemListResponse : BaseResponse
{
    public List<CheckListItem> CheckListItems { get; set; } = new();  
}
