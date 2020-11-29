using Application.Resources;

namespace ApplicationServices
{
    public interface IPersonsService
    {
        Person Get(string id);

        Person Create(string firstName, string lastName);
    }
}