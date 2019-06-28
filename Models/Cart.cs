using System.ComponentModel.DataAnnotations;
using Proj.Models;
namespace Proj.Models{
    public class Cart{
        [Key]
        public int CartId {get;set;}
        public Item item {get;set;}
        public int Quant {get;set;}
    }
}