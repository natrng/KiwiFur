using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proj.Models
{
    public class Item
    {
       [Key]
       public int ItemId {get;set;}
       [DataType(DataType.Text)]
       public string Name {get;set;}
       public double Price {get;set;}
       [NotMapped]
       public string Phote {get;}
    }
}