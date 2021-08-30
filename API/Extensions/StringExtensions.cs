namespace API.Extensions
{
    public static class StringExtensions
    {
        public static string GetStoredProcedureName(this string fullName)
        {
            var result = fullName.Substring(0, fullName.LastIndexOf("Async"));

            return result;
        }
    }
}
