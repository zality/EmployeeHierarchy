using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeHierarchy
{
    public class ValidationBase
    {
        //using List so I can use the .Add extension, normally I would advice IEnumerable to be used
        public List<ValidationError> ValidationErrors { get; set; }

        public string ValidationErrorMessage
        {
            get
            {
                return string.Join(Environment.NewLine, ValidationErrors.Select(p => p.ErrorMessage));
            }
        }

        public bool IsValid
        {
            get
            {
                return !ValidationErrors.Any();
            }
        }
    }
}