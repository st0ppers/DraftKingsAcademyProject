﻿using MessagePack;

namespace Keyboard.Models.Models
{
    [MessagePackObject()]
    public class OrderModel : IGetId
    {
        [Key(0)]
        public int OrderID { get; set; }
        public int KeyboardID { get; set; }
        public int ClientID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }


        public int Get()
        {
            return KeyboardID;
        }
    }
}
