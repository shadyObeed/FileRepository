using WebAPI.Domain.Entities;

namespace WebAPI.Interfaces;
public interface ILaunchersRepository
{
    public Task<Launcher> Add(Launcher launcher);
    
    public Task<Launcher> Get(string launcherName);

    public Task<Launcher> Update(Launcher launcher);
}
