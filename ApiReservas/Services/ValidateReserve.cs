using ApiReservas.Data;
using ApiReservas.DTOs.ReservationDTOs;
using ApiReservas.DTOs.ServicesDTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Services
{
    public class ValidateReserve
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;

        public ValidateReserve
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }
        #endregion

        public async Task<ValidateReserveDTO> Validate (CreateReservationDTO reservationDTO)
        {
            #region VALIDANDO DATA DA RESERVA

            DateTime now = DateTime.Now;

            if (reservationDTO.StartAt < now)
            {
                return new ValidateReserveDTO 
                { 
                    IsDisponible = false, 
                    Message = "Data de início não pode ser no passado!", 
                    Type = Enums.ResponseType.BadRequest 
                };
            }

            if (reservationDTO.EndAt < now)
            {
                return new ValidateReserveDTO 
                { 
                    IsDisponible = false, 
                    Message = "Data final não pode ser no passado!", 
                    Type = Enums.ResponseType.BadRequest 
                };
            }

            if (reservationDTO.EndAt <= reservationDTO.StartAt)
            {
                return new ValidateReserveDTO 
                { 
                    IsDisponible = false,
                    Message = "A data final deve ser após à data inicial!", 
                    Type = Enums.ResponseType.BadRequest 
                };
            }

            #endregion

            #region VERIFICAR SE A SALA TEM DISPONIBILIDADE

            var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);

            if (room == null)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "A sala informada não foi encontrada!",
                    Type = Enums.ResponseType.NotFound
                };
            }

            var reservation = await _context
                .Reservations
                .FirstOrDefaultAsync
                (r =>
                    r.RoomId == reservationDTO.RoomId &&
                    r.IsActive &&
                    r.StartAt < reservationDTO.EndAt &&
                    r.EndAt > reservationDTO.StartAt
                );

            if (reservation != null)
            {
                return new ValidateReserveDTO 
                { 
                    IsDisponible = false, 
                    Message = "Já existe uma reserva para essa sala no horário informado!",  
                    Type = Enums.ResponseType.Conflict 
                };
            }

            if (room == null || !room.isActive)
            {
                return new ValidateReserveDTO 
                { 
                    IsDisponible = false,
                    Message = "Sala não encontrada ou indisponível!",
                    Type = Enums.ResponseType.NotFound
                };
            }

            if (reservationDTO.QtdPeoples >= room.PeopleCapacity)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "A sala está indisponível para essa quantidade de pessoas!",
                    Type = Enums.ResponseType.Conflict
                };
            }

            #endregion

            return new ValidateReserveDTO
            {
                IsDisponible = true,
                Message = "A sala está disponível!",
                Type = Enums.ResponseType.Ok
            };
        }

        public async Task<ValidateReserveDTO> ValidateToEdit (EditReservationDTO reservationDTO)
        {
            #region VALIDANDO DATA DA RESERVA

            DateTime now = DateTime.Now;
            DateTime today = now.Date;

            if (reservationDTO.StartAt < now)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "Data de início não pode ser no passado!",
                    Type = Enums.ResponseType.BadRequest
                };
            }

            if (reservationDTO.EndAt < now)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "Data final não pode ser no passado!",
                    Type = Enums.ResponseType.BadRequest
                };
            }

            if (reservationDTO.EndAt <= reservationDTO.StartAt)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "A data final deve ser após à data inicial!",
                    Type = Enums.ResponseType.BadRequest
                };
            }

            #endregion

            #region VERIFICAR SE A SALA TEM DISPONIBILIDADE

            var room = await _context.Rooms.FindAsync(reservationDTO.RoomId);

            if (room == null)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "A sala informada não foi encontrada!",
                    Type = Enums.ResponseType.NotFound
                };
            }

            var reservation = await _context
                .Reservations
                .FirstOrDefaultAsync
                (r =>
                    r.RoomId == reservationDTO.RoomId &&
                    r.IsActive &&
                    r.StartAt < reservationDTO.EndAt &&
                    r.EndAt > reservationDTO.StartAt
                );


            if (reservation != null)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "Já existe uma reserva para essa sala no horário informado!",
                    Type = Enums.ResponseType.Conflict
                };
            }

            if (room == null || !room.isActive)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "Sala não encontrada ou indisponível!",
                    Type = Enums.ResponseType.NotFound
                };
            }

            if (reservationDTO.QtdPeoples >= room.PeopleCapacity)
            {
                return new ValidateReserveDTO
                {
                    IsDisponible = false,
                    Message = "A sala está indisponível para essa quantidade de pessoas!",
                    Type = Enums.ResponseType.Conflict
                };
            }

            #endregion

            return new ValidateReserveDTO
            {
                IsDisponible = true,
                Message = "A sala está disponível!",
                Type = Enums.ResponseType.Ok
            };
        }
    }
}
