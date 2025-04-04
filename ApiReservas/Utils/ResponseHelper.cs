using ApiReservas.DTOs.ServicesDTOs;
using ApiReservas.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ApiReservas.Utils
{
    public static class ResponseHelper
    {
        public static IActionResult ToActionResultReserve(this ValidateReserveDTO result, ControllerBase controller)
        {
            return result.Type switch
            {
                ResponseType.BadRequest => controller.BadRequest(new { message = result.Message }),
                ResponseType.NotFound => controller.NotFound(new { message = result.Message }),
                ResponseType.Conflict => controller.Conflict(new { message = result.Message }),
                ResponseType.Ok => controller.Ok(new { message = result.Message }),
                _ => controller.StatusCode(500, new { message = "Erro inesperado!" })
            };
        }

        public static IActionResult ToActionResultUser(this ValidateUsersDTO result, ControllerBase controller)
        {
            return result.Type switch
            {
                ResponseType.BadRequest => controller.BadRequest(new { message = result.Message }),
                ResponseType.NotFound => controller.NotFound(new { message = result.Message }),
                ResponseType.Conflict => controller.Conflict(new { message = result.Message }),
                ResponseType.Ok => controller.Ok(new { message = result.Message }),
                _ => controller.StatusCode(500, new { message = "Erro inesperado!" })
            };
        }
    }
}
