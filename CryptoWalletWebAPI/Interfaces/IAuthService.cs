using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Interfaces
{
    public interface IAuthService
    {
        string GeneratePrivateKey();

        string GenerateJsonWebToken(CryptoUser user);
    }
}
