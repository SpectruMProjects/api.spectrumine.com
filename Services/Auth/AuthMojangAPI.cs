using SpectruMineAPI.Models.MojangResponses;
using System.Text.Json;

namespace SpectruMineAPI.Services.Auth
{
    public static class AuthMojangAPI
    {
        public static async Task<string?> GetUUIDFromMojang(string username)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://api.mojang.com/users/profiles/minecraft/" + username);
            var resp = JsonSerializer.Deserialize<MojangSuccessResponse>(await response.Content.ReadAsStringAsync());
            return resp!.id;
        }
        public static async Task<string?> GetUsernameByUUID(string uuid)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
            return JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("name").GetString();
        }
    }
}
