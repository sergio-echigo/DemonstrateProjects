using System.ComponentModel.DataAnnotations;

namespace DemonstrateProjects.Application.ValidationAttributes;

public class EmailDomainAttribute : ValidationAttribute
{
    List<string> allowedDomains = new()
    {
        "gmail",
        "outlook",
        "yahoo",
        "protonmail",
        "proton.me",
        "uol",
        "bol",
        "hotmail",
        "aol"
    };

    public override bool IsValid(object? value)
    {
        var email = value?.ToString();
        var domain = email?.Substring(email.IndexOf('@') + 1, email.LastIndexOf('.') - email.IndexOf('@') - 1);

        if (allowedDomains.Any(x => x == domain))
        {
            Console.WriteLine("CERTO: " + domain);
            return true;
        }

        if (!allowedDomains.Any(x => x == domain))
        {
            Console.WriteLine(domain);
            return false;
        }

        return true;
    }
}