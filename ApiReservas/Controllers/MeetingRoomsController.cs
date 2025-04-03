using ApiReservas.Data;
using ApiReservas.DTOs.RoomsDTOs;
using ApiReservas.Middleware;
using ApiReservas.Models;
using ApiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/salas")]
    [Authorize]
    public class MeetingRoomsController : ControllerBase
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EncPassword _encPassword;

        public MeetingRoomsController(ApplicationDbContext context, JwtService jwtService, EncPassword encPassword)
        {
            _context = context;
            _jwtService = jwtService;
            _encPassword = encPassword;
        }
        #endregion

        [HttpPost("criar")]
        [AdminOnly]
        public async Task<IActionResult> CreateMeetRoom(CreateRoomDTO roomDTO)
        {
            int remainingHours = 24 - DateTime.UtcNow.Hour;

            if (roomDTO.PeopleCapacity <= 0)
            {
                return BadRequest(new { message = $"A sala capacidade de pessoas não pode ser igual ou menor que 0!" });
            }

            if (roomDTO.CapacityInHours > 0 && roomDTO.CapacityInHours <= remainingHours)
            {
                var room = new Room
                {
                    RoomName = roomDTO.RoomName,
                    CapacityInHours = roomDTO.CapacityInHours,
                    PeopleCapacity = roomDTO.PeopleCapacity,
                    isActive = true,
                };

                var roomExists = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomName == roomDTO.RoomName);

                if (roomExists != null)
                {
                    return Conflict(new { message = $"A sala {roomDTO.RoomName} já existe!" });
                }

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Sala criada com sucesso!" });
            }
            else
            {
                return BadRequest(new { message = $"A capacidade de horas informadas não condiz com a data de hoje!" });
            }
        }

        [HttpPut("editar/{id}")]
        [AdminOnly]
        public async Task<IActionResult> EditMeetRoom(int id, EditRoomDTO roomDTO)
        {
            int remainingHours = 24 - DateTime.UtcNow.Hour;

            if (roomDTO.PeopleCapacity <= 0)
            {
                return BadRequest(new { message = $"A sala capacidade de pessoas não pode ser igual ou menor que 0!" });
            }

            if (roomDTO.CapacityInHours > 0 && roomDTO.CapacityInHours <= remainingHours)
            {
                var room = await _context.Rooms.FindAsync(id);

                if (room == null)
                {
                    return Conflict(new { message = $"A sala {roomDTO.RoomName} já existe!" });
                }

                room.RoomName = roomDTO.RoomName;
                room.CapacityInHours = roomDTO.CapacityInHours;
                room.PeopleCapacity = roomDTO.PeopleCapacity;
                room.isActive = roomDTO.IsActive;

                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Sala editada com sucesso!", roomDTO });
            }
            else
            {
                return BadRequest(new { message = $"A capacidade de horas informadas não condiz com a data de hoje!" });
            }
        }

        [HttpDelete("inativar/{id}")]
        [AdminOnly]
        public async Task<IActionResult> InativeRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Sala não encontrada!" });
            }

            room.isActive = false;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sala inativada com sucesso!" });
        }

        [HttpGet("listar_salas")]
        [AdminOnly]
        public async Task<IActionResult> ListRooms()
        {
            var rooms = await _context.Rooms.Select(r => new RoomsResponseDTO
            {
                Id = r.Id,
                RoomName = r.RoomName,
                CapacityInHours = r.CapacityInHours,
                IsActive = r.isActive,
                PeopleCapacity = r.PeopleCapacity
            })
            .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("listar_salas_ativas")]
        [AdminOnly]
        public async Task<IActionResult> ListActiveRooms()
        {
            var rooms = await _context.Rooms
            .Where(r => r.isActive)
            .Select(r => new RoomsResponseDTO
            {
                Id = r.Id,
                RoomName = r.RoomName,
                CapacityInHours = r.CapacityInHours,
                IsActive = r.isActive,
                PeopleCapacity = r.PeopleCapacity
            })
            .ToListAsync();

            return Ok(rooms);
        }
    }
}
