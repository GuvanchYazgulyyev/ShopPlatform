using Microsoft.AspNetCore.Components;
using ShopPlatform.Shared.ModelsDTO;
using ShopPlatform.Shared.Responses;
using ShopPlatform.Shared.Results;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShopPlatform.Client.Pages.PagesOperations
{
    public class UserListOperation : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        protected List<UserDTO> userList = new List<UserDTO>();

        public ModalManager modal { get; set; }
        protected async override Task OnInitializedAsync()
        {
            await LoadList();
        }
        /// <summary>
        /// APIden Verileri Getir
        /// </summary>
        /// <returns></returns>
        protected async Task LoadList()
        {
            var serviceResponse = await HttpClient.GetFromJsonAsync<ShopPlatform.Shared.Responses.ServiceResponse<List<UserDTO>>>("api/User/Users");
            if (serviceResponse.IsSuccess)
                userList = serviceResponse.Value;
        }

        [Inject] NavigationManager navigation { get; set; }

        /// <summary>
        /// Ekleme Yap
        /// </summary>
        protected void goToCreateUser()
        {
            navigation.NavigateTo("/users/add");
        }

        /// <summary>
        /// Id Ye göre veriyi getir
        /// </summary>
        /// <param name="id"></param>
        protected void goToUpdatePage(Guid id)
        {
            navigation.NavigateTo($"/users/edit/{id}");
        }

        /// <summary>
        /// Kullancıyı Sil.
        /// </summary>
        protected async Task goToDeleteUser(Guid guid)
        {
            bool isConfirmed = await modal.ConfirmationAsync("Emin misin?", "Bu kullanıcıyı silmek istiyor musun?");

            if (!isConfirmed) return;

            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/User/Delete", guid);
                if (response.IsSuccessStatusCode)
                {
                    await LoadList();
                }
                else
                {
                    modal.Show("Silme işlemi başarısız.");
                }
            }
            catch (Exception ex)
            {
                modal.Show($"Hata: {ex.Message}");
            }
        }

    }
}
