using ApiReservas.Data;
using ApiReservas.DTOs.ReservationDTOs;
using ApiReservas.DTOs.RoomsDTOs;
using ApiReservas.Middleware;
using ApiReservas.Middlewares;
using ApiReservas.Models;
using ApiReservas.Services;
using ApiReservas.Utils;
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
        private readonly ValidateReserve _validateReserve;
        private readonly ValidateUser _validateUser;

        public ReservationsController
        (
            ApplicationDbContext context, 
            JwtService jwtService, 
            EncPassword encPassword,
            ValidateReserve validateReserve,
            ValidateUser validateUser
        )
        {
            _context = context;
            _jwtService = jwtService;
            _encPassword = encPassword;
            _validateReserve = validateReserve;
            _validateUser = validateUser;
        }
        #endregion

        [HttpPost("reservar")]
        [UserOnly]
        public async Task<IActionResult> NewReservation(CreateReservationDTO reservationDTO)
        {
            var validateReserve = await _validateReserve.Validate(reservationDTO);
            var userIsValid = await _validateUser.Validate(reservationDTO.UserId);

            #region CRIANDO A RESERVA

            if (!userIsValid.IsValidated) 
            {
                return userIsValid.ToActionResultUser(this);
            }

            if (!validateReserve.IsDisponible) 
            {
                return validateReserve.ToActionResultReserve(this);
            }            

            var newReservation = new Reservation
            {
                UserId = reservationDTO.UserId,
                RoomId = reservationDTO.RoomId,
                QtdPeoples = reservationDTO.QtdPeoples,
                StartAt = reservationDTO.StartAt,
                EndAt = reservationDTO.EndAt,
                IsActive = true
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Reserva criada com sucesso!" });

            #endregion
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditReservation(int id, EditReservationDTO reservationDTO)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (reservation == null)
            {
                return BadRequest(new { message = "Reserva não encontrada!" });
            }

            var validateNewReserve = await _validateReserve.ValidateToEdit(reservationDTO);
            var userIsValid = await _validateUser.Validate(reservation.UserId);

            #region EDITANDO A RESERVA

            if (!userIsValid.IsValidated)
            {
                return userIsValid.ToActionResultUser(this);
            }

            if (!validateNewReserve.IsDisponible)
            {
                return validateNewReserve.ToActionResultReserve(this);
            }

            reservation.RoomId = reservationDTO.RoomId;
            reservation.QtdPeoples = reservationDTO.QtdPeoples;
            reservation.StartAt = reservationDTO.StartAt;
            reservation.EndAt = reservationDTO.EndAt;
            reservation.IsActive = true;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Reserva editada com sucesso!" });

            #endregion
        }

        [HttpDelete("inativar/{id}")]
        public async Task<IActionResult> InativeReservation(int id)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (reservation == null)
            {
                return NotFound(new { message = "Reserva não encontrada!" });
            }

            var qtdHours = (int)Math.Ceiling((reservation.EndAt - reservation.StartAt).TotalHours);

            reservation.IsActive = false;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Reserva inativada com sucesso!" });
        }

        [HttpGet("listar_reservas")]
        [AdminOnly]
        public async Task<IActionResult> ListReservations()
        {
            var reservations = await _context.Reservations.Select(r => new ReservationResponseDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                RoomId = r.RoomId,
                QtdPeoples = r.QtdPeoples,
                StartAt = r.StartAt,
                EndAt = r.EndAt,
                IsActive = r.IsActive
            })
            .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("listar_reservas_ativas")]
        public async Task<IActionResult> ListActiveReservations()
        {
            var reservations = await _context.Reservations
            .Where(r => r.IsActive)
            .Select(r => new ReservationResponseDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                RoomId = r.RoomId,
                QtdPeoples = r.QtdPeoples,
                StartAt = r.StartAt,
                EndAt = r.EndAt,
                IsActive = r.IsActive
            })
            .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("listar_reservas_filtradas")]
        public async Task<IActionResult> ListFilteredReservations([FromQuery] FilteredReservetionRequisitionDTO requisitionDTO)
        {
            if (requisitionDTO.UserId == 0 || requisitionDTO.RoomId == 0)
            {
                return BadRequest(new { message = "Informe o usuário e a sala!" });
            }

            var query = _context.Reservations
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(r => r.UserId == requisitionDTO.UserId);
            query = query.Where(r => r.RoomId == requisitionDTO.RoomId);

            if (requisitionDTO.IsActive != null)
                query = query.Where(r => r.IsActive == requisitionDTO.IsActive);

            if (requisitionDTO.ReservDate != null)
            {
                var date = DateTime.SpecifyKind(requisitionDTO.ReservDate.Value.Date, DateTimeKind.Utc);
                var nextDay = date.AddDays(1);

                query = query.Where(r => r.StartAt >= date && r.StartAt < nextDay);
            }

            var reservations = await query
            .Select(r => new ReservationResponseDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                RoomId = r.RoomId,
                QtdPeoples = r.QtdPeoples,
                StartAt = r.StartAt,
                EndAt = r.EndAt,
                IsActive = r.IsActive
            })
            .ToListAsync();

            return Ok(reservations);
        }
    }
}
