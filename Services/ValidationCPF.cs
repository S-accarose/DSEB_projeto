
namespace DSEB_projeto.Services;

public class ValidationCPF
{
    public static bool IsValidCPF(string cpf)
    {

        var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        // Verifica se todos os dígitos são iguais
        if (numbers.Distinct().Count() == 1)
            return false;

        // Calcula o primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += numbers[i] * (10 - i);
        int firstCheckDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        // Calcula o segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += numbers[i] * (11 - i);
        int secondCheckDigit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

        return firstCheckDigit == numbers[9] && secondCheckDigit == numbers[10];
    }
}