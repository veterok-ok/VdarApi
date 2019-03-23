namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IConfirmationRepository ConfirmationKey { get; }
        ITokenRepository Token { get; }
    }
}
