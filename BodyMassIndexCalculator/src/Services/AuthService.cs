﻿using BodyMassIndexCalculator.src.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;

namespace BodyMassIndexCalculator.src.Services
{
    public class AuthService
    {
        public static async Task<ResponseResult<Session>> SignIn(string email, string password)
        {
            try
            {
                Session? response = await SupabaseService.Client.Auth.SignIn(email, password);
                return new(response, null);
            }
            catch (GotrueException gex)
            {
                string userFriendlyError = GetUserFriendlyError(gex);
                return new(null, userFriendlyError);
            }
            catch (Exception)
            {
                return new(null, "Произошла непредвиденная ошибка");
            }
        }

        public static async Task<ResponseResult<Session>> SignUp(string firstName, string lastName, string email, string password)
        {
            try
            {
                Session? signUpResponse = await SupabaseService.Client.Auth.SignUp(
                    Constants.SignUpType.Email,
                    email,
                    password,
                    new SignUpOptions
                    {
                        Data = new Dictionary<string, object>
                        {
                            { "first_name", firstName },
                            { "last_name", lastName }
                        }
                    });

                if (signUpResponse == null)
                    return new(null, "Не удалось создать аккаунт");

                return new(signUpResponse, null);
            }
            catch (GotrueException gex)
            {
                string userFriendlyError = GetUserFriendlyError(gex);
                return new(null, userFriendlyError);
            }
            catch (Exception)
            {
                return new(null, "Произошла непредвиденная ошибка");
            }
        }

        private static string GetUserFriendlyError(GotrueException gex) => gex.StatusCode == 0
            ? "Произошла непредвиденная ошибка"
            : gex.StatusCode switch
            {
                400 => "Некорректные данные",
                401 => "Ошибка авторизации",
                404 => "Ресурс не найден",
                422 => "Данные не могут быть обработаны",
                500 => "Ошибка на сервере",
                _ => $"Произошла непредвиденная ошибка (код: {gex.StatusCode})"
            };
    }
}
