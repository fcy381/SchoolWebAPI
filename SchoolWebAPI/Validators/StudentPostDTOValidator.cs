using FluentValidation;
using SchoolWebAPI.Models.Student;

namespace SchoolWebAPI.Validators
{
    public class StudentPostDTOValidator: AbstractValidator<StudentPostDTO>
    {
        public StudentPostDTOValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(15);
        }    
    }
}
