using ApiReservas.Data;
using ApiReservas.DTOs.ReservationDTOs;
using ApiReservas.DTOs.RoomsDTOs;
using ApiReservas.Models;
using ApiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EncPassword _encPassword;

        public ReservationsController
        (
            ApplicationDbContext context, 
            JwtService jwtService, 
            EncPassword encPassword
        )
        {
            _context = context;
            _jwtService = jwtService;
            _encPassword = encPassword;
        }
        #endregion

        [HttpPost("reservar")]
        public async Task<IActionResult> NewReservation(CreateReservationDTO reservationDTO)
        {
            #region VALIDANDO DATA DA RESERVA

            DateTime now = DateTime.UtcNow;
            DateTime today = now.Date;

            if (reservationDTO.StartAt < now)
            {
                return BadRequest(new { message = "Data de início não pode ser no passado!" });
            }

            if (reservationDTO.EndAt < now)
            {
                return BadRequest(new { message = "Data final não pode ser no passado!" });
            }

            if (reservationDTO.StartAt.Date != today || reservationDTO.EndAt.Date != today)
            {
                return BadRequest(new { message = "Reservas só podem ser feitas para o dia atual!" });
            }

            if (reservationDTO.EndAt <= reservationDTO.StartAt)
            {
                return BadRequest(new { message = "A data final deve ser após à data inicial!" });
            }

            #endregion

            #region VERIFICAR SE A SALA TEM DISPONIBILIDADE

            var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);

            var reservation = await _context
                .Reservations
                .FirstOrDefaultAsync
                (r => 
                    r.RoomId == reservationDTO.RoomId &&
                    r.IsActive &&
                    r.StartAt < reservationDTO.EndAt &&
                    r.EndAt > reservationDTO.StartAt
                );

            var roomIsDisponible = false;
            var reservationInSameHour = true;
            var qtdHours = (reservationDTO.EndAt - reservationDTO.StartAt).Hours;

            if (reservation != null)
            {
                return Conflict(new { message = "Já existe uma reserva para essa sala no horário informado!" });
            }
            else
            {
                reservationInSameHour = false;
            }

            if (room.isActive == false || room == null)
            {
                return NotFound(new { message = "Sala não encontrada ou indisponível!" });
            }

            if (room.CapacityInHours > 0 && reservationDTO.QtdPeoples < room.PeopleCapacity && room.CapacityInHours < qtdHours)
            {
                roomIsDisponible = true;
            }
            else
            {
                return Conflict(new { message = "A sala está indisponível para essa quantidade de pessoas ou está sem horário livre" });
            }

            #endregion

            #region CRIANDO A RESERVA

            if (roomIsDisponible && !reservationInSameHour)
            {

                var newReservation = new Reservations
                {
                    UserId = reservationDTO.UserId,
                    RoomId = reservationDTO.RoomId,
                    QtdPeoples = reservationDTO.QtdPeoples,
                    StartAt = reservationDTO.StartAt,
                    EndAt = reservationDTO.EndAt,
                    IsActive = true
                };


                room.RoomName = room.RoomName;
                room.CapacityInHours = (room.CapacityInHours - qtdHours);
                room.PeopleCapacity = (room.PeopleCapacity - reservationDTO.QtdPeoples);

                _context.Rooms.Update(room);
                _context.Reservations.Add(newReservation);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Reserva criada com sucesso!", newReservation });
            }
            else
            {
                return Conflict(new { message = "A sala está indisponível para essa quantidade de pessoas ou está sem horário livre" });
            }

            #endregion
        }
    }
}
