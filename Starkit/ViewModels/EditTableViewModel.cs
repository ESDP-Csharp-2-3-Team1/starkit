using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditTableViewModel
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string IconUrl { get; set; }
        public IFormFile File { get; set; }
        public State State { get; set; } = State.Available;
        public string Desc { get; set; }
        public Location Location { get; set; } = Location.Regular;
        public bool IsSmoking { get; set; } = false;
        public bool IsQuiet { get; set; } = true;
        public int Floor { get; set; } = 1;
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
        public DateTime EditedDate { get; set; }
    }
}