using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

public class Target
{
    [Required(AllowEmptyStrings =true, ErrorMessage = "In..")]
    //[Range(4,10)]
    //[StringLength(4)]
    [MaxLength(10,ErrorMessage="> big")]
    [MinLength(4,ErrorMessage="< small")]
    public string? Code { get; set; }
} ;


//Validator.TryValidateValue(a);


namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = new Target
            {
                Code = " " //null""// "123"
            };

            var context = new ValidationContext(target, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Console.WriteLine(Validator.TryValidateObject(target, context, validationResults, true)) ;

            foreach (var result in validationResults){
                Console.WriteLine(result.ErrorMessage) ;
            }
            
        }
    }
}