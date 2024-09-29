namespace EventsWebApplication.Core.Contracts
{
    public record MembersResponse(string name, string surname, DateOnly birthDate, DateOnly registrationDate, string email);
}
