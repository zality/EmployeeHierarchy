namespace EmployeeHierarchy
{
    public class ValidationError
    {
        /// <summary>
        /// Store entity errors
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="memberName"></param>
        public ValidationError(string errorMessage, string memberName)
        {
            this.ErrorMessage = errorMessage;
            this.MemberName = memberName;
        }

        public string ErrorMessage { get; private set; }
        public string MemberName { get; private set; }
    }
}