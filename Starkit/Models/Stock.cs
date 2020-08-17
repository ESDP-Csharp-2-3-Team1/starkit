using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cms;

namespace Starkit.Models
{
    public class Stock
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Type { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Validity { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public DateTime At { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public DateTime To { get; set; }
        
        public string Avatar { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public IFormFile File { get; set; }
        
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
        
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
    }
}