using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine("--> Getting Commands...");

            var platformExists = _repository.PlatformExists(platformId);
            if (!platformExists)
                return NotFound();

            var commands = _repository.GetCommandsForPlatform(platformId);

            var commandsDto = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

            return Ok(commandsDto);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            var platformExists = _repository.PlatformExists(platformId);
            if (!platformExists)
                return NotFound();

            var command = _repository.GetCommand(platformId, commandId);
            if (command is null)
                return NotFound();

            var commandDto = _mapper.Map<CommandReadDto>(command);

            return Ok(commandDto);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            var platformExists = _repository.PlatformExists(platformId);
            if (!platformExists)
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtAction(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandDto);
        }
    }
}