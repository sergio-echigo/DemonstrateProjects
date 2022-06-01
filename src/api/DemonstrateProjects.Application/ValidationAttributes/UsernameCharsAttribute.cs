namespace DemonstrateProjects.Application.ValidationAttributes;

public class UsernameCharsAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var username = value.ToString().Trim();
        foreach(char c in username)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                return false;
        }

        return true;
    }
}