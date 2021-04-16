namespace PaymentGateway.Services.Cryptography.Interface
{
    public interface ICryptographyService
    {
        string Encyrpt(string plainText);
        string Decrypt(string encyrptedText);
    }
}