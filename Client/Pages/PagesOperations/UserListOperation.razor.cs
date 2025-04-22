using Microsoft.AspNetCore.Components;
using ShopPlatform.Shared.ModelsDTO;
using ShopPlatform.Shared.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace ShopPlatform.Client.Pages.PagesOperations
{
    public class UserListOperation : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        public ModalManager modal { get; set; }
        protected List<UserDTO> userList = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadList();
        }

        /// <summary>
        /// Kullanıcı listesini API'den getir.
        /// </summary>
        protected async Task LoadList()
        {
            try
            {
                // Token'ı al ve header'a ekle
                var token = await LocalStorage.GetItemAsStringAsync("token");
                if (!string.IsNullOrEmpty(token))
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await HttpClient.GetFromJsonAsync<ServiceResponse<List<UserDTO>>>("api/User/Users");

                if (response != null && response.IsSuccess)
                {
                    userList = response.Value;
                }
                else
                {
                    modal?.Show("Kullanıcılar getirilemedi.");
                }
            }
            catch (Exception ex)
            {
                modal?.Show($"Veri çekme hatası: {ex.Message}");
            }
        }

        protected void goToCreateUser()
        {
            Navigation.NavigateTo("/users/add");
        }

        protected void goToUpdatePage(Guid id)
        {
            Navigation.NavigateTo($"/users/edit/{id}");
        }

        protected async Task goToDeleteUser(Guid guid)
        {
            bool isConfirmed = await modal.ConfirmationAsync("Emin misin?", "Bu kullanıcıyı silmek istiyor musun?");

            if (!isConfirmed) return;

            try
            {
                var token = await LocalStorage.GetItemAsStringAsync("token");
                if (!string.IsNullOrEmpty(token))
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await HttpClient.PostAsJsonAsync("api/User/Delete", guid);
                if (response.IsSuccessStatusCode)
                {
                    await LoadList();
                }
                else
                {
                    modal?.Show("Silme işlemi başarısız.");
                }
            }
            catch (Exception ex)
            {
                modal?.Show($"Hata: {ex.Message}");
            }
        }
    }
}
