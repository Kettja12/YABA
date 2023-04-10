using Client.Responses;
using Client.Services;
using Client.Shared;
using Microsoft.Extensions.Logging.Abstractions;
using System.Dynamic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Pages.Account
{
    public partial class Usergroups
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (StateContainer.CurrentUser.Functions.Contains(AppCodes.AddModifyUsergroups) == false)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }
                dynamic request = new ExpandoObject();
                IdValuePairListResponse? response = await ApiService.PostAsync<IdValuePairListResponse>("(db)/LoadUsergroups", request);
                if (response != null && (response.Status != -1))
                {
                    usergroups = response.IdValuePairs;
                }
                CheckListItemListResponse? response2 = await ApiService.PostAsync<CheckListItemListResponse>("(db)/LoadFunctions", request);
                if (response2 != null && (response2.Status != -1))
                {
                    functions = response2.CheckListItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            StateHasChanged();
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }
        }


        protected async Task<List<CheckListItem>> LoadUsergoupFunctionsAsync(int id)
        {

            try
            {
                dynamic request = new ExpandoObject();
                request.IntField = id;
                CheckListItemListResponse? response = await ApiService.PostAsync<CheckListItemListResponse>("(db)/LoadUsergroupFunctions", request);
                if (response != null && (response.Status != -1))
                {
                    return response.CheckListItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            StateHasChanged();
            return new List<CheckListItem>();
        }

        protected async Task SaveUsergoupFunctions()
        {
            if (usergroup == 0) return;
            //using AccountContext accountContext = contextFactory.GetContext<AccountContext>();
            //using AccountRepository accountRepo = new AccountRepository(stateContainer, accountContext);
            //SaveUsergroupFunctionsRequest request = new SaveUsergroupFunctionsRequest()
            //{
            //    Id=usergroup,
            //    CheckListItems=functions
            //};
            //var result= await accountRepo.SaveUsergroupFunctionsAsync(request);
        }
        protected async Task SaveNewUsergroup()
        {
            if (usergroup == 0) return;
            //using AccountContext accountContext = contextFactory.GetContext<AccountContext>();
            //using AccountRepository accountRepo = new AccountRepository(stateContainer, accountContext);
            //SimpleRequest request = new()
            //{
            //    StringField= newUsergroupName
            //};
            //var result = await accountRepo.SaveNewUsergroupAsync(request);
        }

        protected async Task<List<CheckListItem>> LoadUsergroupUsersAsync(int id)
        {


            try
            {
                dynamic request = new ExpandoObject();
                request.IntField = id;
                CheckListItemListResponse? response = await ApiService.PostAsync<CheckListItemListResponse>("(db)/LoadUsergroupUsers", request);
                if (response != null && (response.Status != -1))
                {
                    return response.CheckListItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            StateHasChanged();
            return new List<CheckListItem>();
        }

        protected async Task<List<CheckListItem>> FindUsersAsync(string searchKey, int usergroup)
        {
            try
            {
                if (searchKey.Length > 2)
                {
                    dynamic request = new ExpandoObject();
                    request.IntField = usergroup;
                    request.StringField = searchKey;
                    CheckListItemListResponse? response = await ApiService.PostAsync<CheckListItemListResponse>("(db)/LoadUsersNotInUsergroup", request);
                    if (response != null && (response.Status != -1))
                    {
                        return response.CheckListItems;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            StateHasChanged();
            return new List<CheckListItem>();
        }

        protected async Task<BaseResponse> RemoveUsergroupUsersAsync(int usergroup, List<CheckListItem> users)
        {
            //using AccountContext accountContext = contextFactory.GetContext<AccountContext>();
            //using AccountRepository accountRepo = new AccountRepository(stateContainer, accountContext);
            //var ids= users.Where(x=>x.IsChecked).Select(x=>x.Id).ToList();
            //return await accountRepo.RemoveUsergroupUsersAsync(usergroup,ids);
            return new BaseResponse();
        }
        protected async Task<BaseResponse> AddUsergroupUsersAsync(int usergroup, List<CheckListItem> users)
        {
            //using AccountContext accountContext = contextFactory.GetContext<AccountContext>();
            //using AccountRepository accountRepo = new AccountRepository(stateContainer, accountContext);
            //var ids = users.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            //return await accountRepo.AddUsergroupUsersAsync(usergroup, ids);
            return new BaseResponse();
        }

    }
}
