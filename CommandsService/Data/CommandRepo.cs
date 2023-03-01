using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        // Platforms
        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platfroms.ToList();
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
                throw new ArgumentNullException(nameof(platform));

            _context.Platfroms.Add(platform);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platfroms.Any(p => p.Id == platformId);
        }


        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platfroms.Any(p => p.ExternalId == externalPlatformId);
        }

        // Commands
        public void CreateCommand(int platformId, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId)
                .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}