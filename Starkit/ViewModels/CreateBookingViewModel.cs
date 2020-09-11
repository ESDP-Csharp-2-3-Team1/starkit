using System;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class CreateBookingViewModel
    {
        public Booking Booking { get; set; }
        public int TableId { get; set; }
        public string Date { get; set; }
        
    }
}