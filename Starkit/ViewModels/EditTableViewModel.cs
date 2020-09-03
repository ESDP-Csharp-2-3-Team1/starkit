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
        public TableState State { get; set; }
        public string Desc { get; set; }
        public Location Location { get; set; }
        public bool IsSmoking { get; set; }
        public bool IsQuiet { get; set; }
        public int Floor { get; set; }
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
        public DateTime EditedDate { get; set; }
    }
}