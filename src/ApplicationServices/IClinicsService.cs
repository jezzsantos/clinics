using Application.Resources;

namespace ApplicationServices
{
    public interface IClinicsService
    {
        Doctor GetDoctor(string id);
    }
}