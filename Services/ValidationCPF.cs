using System.ComponentModel.DataAnnotations;
namespace DSEB_projeto.Services;

public class ValidationCPF
{
    public static ValidationResult IsValidCPF(string cpf, ValidationContext context)
    {
        if (string.IsNullOrEmpty(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            return new ValidationResult("CPF inválido.");

        var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        if (numbers.Distinct().Count() == 1)
            return new ValidationResult("CPF inválido.");

        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += numbers[i] * (10 - i);
        int firstCheckDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += numbers[i] * (11 - i);
        int secondCheckDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        if (firstCheckDigit == numbers[9] && secondCheckDigit == numbers[10])
            return ValidationResult.Success;

        return new ValidationResult("CPF inválido.");
    }
}