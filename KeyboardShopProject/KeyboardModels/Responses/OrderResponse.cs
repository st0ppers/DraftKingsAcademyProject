﻿using Keyboard.Models.Models;
using MessagePack;

namespace Keyboard.Models.Responses
{
    [MessagePackObject()]
    public class OrderResponse : BaseResponse
    {
        [Key(0)]
        public int OrderID { get; set; }
        [Key(1)]
        public decimal TotalPrice { get; set; }
        [Key(2)]
        public DateTime DateOfOrder { get; set; }
        [Key(3)]
        public List<KeyboardModel> Keyboard { get; set; }
    }
}
