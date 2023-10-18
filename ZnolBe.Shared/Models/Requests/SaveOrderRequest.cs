using System;
using System.Collections.Generic;
using System.Text;

namespace ZnolBe.Shared.Models.Requests;
public class SaveOrderRequest
{
    public Guid? Id { get; set; }

    public double TotalPrice { get; set; }
}
