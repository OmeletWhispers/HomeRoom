using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeRoom.Enumerations
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female
    }

    public enum AccountType
    {
        [Display(Name = "Teacher")]
        Teacher,
        [Display(Name = "Student")]
        Student,
        [Display(Name = "Parent")]
        Parent
    }

    public enum AssignmentStatus
    {
        [Display(Name = "Created")]
        Created,
        [Display(Name = "Open")]
        Open,
        [Display(Name = "Closed")]
        Closed
    }

    public enum QuestionType
    {
        [Display(Name="Short Answer")]
        ShortAnswer,
        [Display(Name="Multiple Choice")]
        MultipleChoice
    }
}