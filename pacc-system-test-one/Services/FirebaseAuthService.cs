using Microsoft.JSInterop;
using pacc_system_test_one.Models;

namespace pacc_system_test_one.Services
{
    public class FirebaseAuthService
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthUser? CurrentUser { get; private set; }

        public FirebaseAuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsAuthenticated =>
            CurrentUser != null;

        public bool IsAdmin =>
            CurrentUser?.UserType.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase
            ) == true;


        // =====================================================
        // WAIT FOR FIREBASE
        // =====================================================

        private async Task WaitForFirebaseAsync()
        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var ready =
                        await _jsRuntime.InvokeAsync<bool>(
                            "isFirebaseReady"
                        );

                    if (ready)
                    {
                        return;
                    }
                }
                catch (JSException)
                {
                    // Firebase is still loading.
                }

                await Task.Delay(100);
            }

            throw new Exception(
                "Firebase could not be initialized."
            );
        }


        // =====================================================
        // LOGIN
        // =====================================================

        public async Task<AuthUser?> LoginAsync(
            string email,
            string password)
        {
            await WaitForFirebaseAsync();

            var result =
                await _jsRuntime.InvokeAsync<AuthResult>(
                    "firebaseAuth.login",
                    email,
                    password
                );

            if (!result.Success)
            {
                throw new Exception(
                    result.Message ??
                    "Login failed."
                );
            }

            CurrentUser = new AuthUser
            {
                Uid = result.Uid ?? "",

                FirstName =
                    result.FirstName ?? "",

                LastName =
                    result.LastName ?? "",

                EmailAddress =
                    result.EmailAddress ?? email,

                UserType =
                    result.UserType ?? "User"
            };

            return CurrentUser;
        }


        // =====================================================
        // REGISTER
        // =====================================================

        public async Task<AuthUser?> RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string password)
        {
            await WaitForFirebaseAsync();

            var result =
                await _jsRuntime.InvokeAsync<AuthResult>(
                    "firebaseAuth.register",
                    email,
                    password,
                    firstName,
                    lastName
                );

            if (!result.Success)
            {
                throw new Exception(
                    result.Message ??
                    "Registration failed."
                );
            }

            CurrentUser = new AuthUser
            {
                Uid = result.Uid ?? "",

                FirstName = firstName,

                LastName = lastName,

                EmailAddress = email,

                UserType = "User"
            };

            return CurrentUser;
        }


        // =====================================================
        // CURRENT USER
        // =====================================================

        public async Task<AuthUser?> LoadCurrentUserAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var result =
                    await _jsRuntime.InvokeAsync<AuthUser?>(
                        "firebaseAuth.getCurrentUser");

                CurrentUser = result;

                return CurrentUser;
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getCurrentUser error: {ex.Message}"
                );

                CurrentUser = null;

                return null;
            }
        }


        // =====================================================
        // DISPLAY NAME
        // =====================================================

        public string GetDisplayName(string email)
        {
            var username = email.Split('@')[0];

            if (string.IsNullOrWhiteSpace(username))
            {
                return "User";
            }

            username = username
                .Replace(".", " ")
                .Replace("_", " ")
                .Replace("-", " ");

            var parts = username.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            return string.Join(
                " ",
                parts.Select(part =>
                    char.ToUpper(part[0]) +
                    part.Substring(1).ToLower()));
        }


        // =====================================================
        // GET ALL USERS
        // =====================================================

        public async Task<List<User>> GetUsersAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var users =
                    await _jsRuntime.InvokeAsync<List<User>>(
                        "firebaseAuth.getUsers");

                return users ?? new List<User>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getUsers error: {ex.Message}"
                );

                throw;
            }
        }


        // =====================================================
        // UPDATE USER ROLE
        // =====================================================

        public async Task UpdateUserRoleAsync(
            string userId,
            string userType)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updateUserRole",
                    userId,
                    userType
                );
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updateUserRole error: {ex.Message}"
                );

                throw;
            }
        }

        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var announcements =
                    await _jsRuntime.InvokeAsync<List<Announcement>>(
                        "firebaseAuth.getAnnouncements");

                return announcements ?? new List<Announcement>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getAnnouncements error: {ex.Message}");

                throw;
            }
        }


        public async Task CreateAnnouncementAsync(
            string title,
            string description,
            DateTime announcementDate,
            string location,
            string category)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.createAnnouncement",
                    title,
                    description,
                    announcementDate,
                    location,
                    category);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase createAnnouncement error: {ex.Message}");

                throw;
            }
        }


        public async Task UpdateAnnouncementAsync(
            string announcementId,
            string title,
            string description,
            DateTime announcementDate,
            string location,
            string category)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updateAnnouncement",
                    announcementId,
                    title,
                    description,
                    announcementDate,
                    location,
                    category);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updateAnnouncement error: {ex.Message}");

                throw;
            }
        }


        public async Task DeleteAnnouncementAsync(
            string announcementId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteAnnouncement",
                    announcementId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteAnnouncement error: {ex.Message}");

                throw;
            }
        }


        // =====================================================
        // DELETE USER PROFILE
        // =====================================================

        public async Task DeleteUserProfileAsync(
            string userId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteUserProfile",
                    userId
                );
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteUserProfile error: {ex.Message}"
                );

                throw;
            }
        }
        public async Task<List<Donation>> GetDonationsAsync()
        {
            await WaitForFirebaseAsync();

            var donations =
                await _jsRuntime.InvokeAsync<List<Donation>>(
                    "firebaseAuth.getDonations");

            return donations ?? new List<Donation>();
        }

        public async Task CreateDonationAsync(Donation donation)
        {
            await WaitForFirebaseAsync();

            await _jsRuntime.InvokeVoidAsync(
                "firebaseAuth.createDonation",
                donation.UserId,
                donation.Amount,
                donation.Date,
                donation.DonorName,
                donation.GivingType,
                donation.PaymentMethod);
        }

        public async Task UpdateDonationAsync(Donation donation)
        {
            await WaitForFirebaseAsync();

            await _jsRuntime.InvokeVoidAsync(
                "firebaseAuth.updateDonation",
                donation.DonationId,
                donation.UserId,
                donation.Amount,
                donation.Date,
                donation.DonorName,
                donation.GivingType,
                donation.PaymentMethod);
        }

        public async Task DeleteDonationAsync(string donationId)
        {
            await WaitForFirebaseAsync();

            await _jsRuntime.InvokeVoidAsync(
                "firebaseAuth.deleteDonation",
                donationId);
        }

        //Gallery 

        public async Task<List<Gallery>> GetGalleryAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var gallery =
                    await _jsRuntime.InvokeAsync<List<Gallery>>(
                        "firebaseAuth.getGallery");

                return gallery ?? new List<Gallery>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getGallery error: {ex.Message}");

                throw;
            }
        }

        public async Task CreateGalleryImageAsync(Gallery gallery)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.createGalleryImage",
                    gallery.Title,
                    gallery.ImageUrl);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase createGalleryImage error: {ex.Message}");

                throw;
            }
        }

        public async Task UpdateGalleryImageAsync(Gallery gallery)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updateGalleryImage",
                    gallery.ImageId,
                    gallery.Title,
                    gallery.ImageUrl);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updateGalleryImage error: {ex.Message}");

                throw;
            }
        }

        public async Task DeleteGalleryImageAsync(string imageId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteGalleryImage",
                    imageId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteGalleryImage error: {ex.Message}");

                throw;
            }
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var result =
                    await _jsRuntime.InvokeAsync<List<Message>>(
                        "firebaseAuth.getMessages");

                return result ?? new List<Message>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getMessages error: {ex.Message}");

                throw;
            }
        }


        public async Task MarkMessageAsReadAsync(
            int messageId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.markMessageAsRead",
                    messageId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase markMessageAsRead error: {ex.Message}");

                throw;
            }
        }


        public async Task DeleteMessageAsync(
            int messageId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteMessage",
                    messageId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteMessage error: {ex.Message}");

                throw;
            }
        }

        //outreach methods
        public async Task<List<OutreachProgram>> GetOutreachProgramsAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var result =
                    await _jsRuntime.InvokeAsync<List<OutreachProgram>>(
                        "firebaseAuth.getOutreachPrograms");

                return result ?? new List<OutreachProgram>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getOutreachPrograms error: {ex.Message}");

                throw;
            }
        }


        public async Task CreateOutreachProgramAsync(
            OutreachProgram program)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.createOutreachProgram",
                    program);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase createOutreachProgram error: {ex.Message}");

                throw;
            }
        }


        public async Task UpdateOutreachProgramAsync(
            OutreachProgram program)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updateOutreachProgram",
                    program);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updateOutreachProgram error: {ex.Message}");

                throw;
            }
        }


        public async Task DeleteOutreachProgramAsync(
            int outreachId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteOutreachProgram",
                    outreachId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteOutreachProgram error: {ex.Message}");

                throw;
            }
        }

        //sermon methods
        public async Task<List<Sermon>> GetSermonsAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var sermons =
                    await _jsRuntime.InvokeAsync<List<Sermon>>(
                        "firebaseAuth.getSermons");

                return sermons ?? new List<Sermon>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getSermons error: {ex.Message}");

                throw;
            }
        }


        public async Task CreateSermonAsync(
            Sermon sermon)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.createSermon",
                    sermon);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase createSermon error: {ex.Message}");

                throw;
            }
        }


        public async Task UpdateSermonAsync(
            Sermon sermon)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updateSermon",
                    sermon);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updateSermon error: {ex.Message}");

                throw;
            }
        }


        public async Task DeleteSermonAsync(
            int sermonId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deleteSermon",
                    sermonId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deleteSermon error: {ex.Message}");

                throw;
            }
        }

        //prayer requests methods
        public async Task<List<PrayerRequest>> GetPrayerRequestsAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var requests =
                    await _jsRuntime.InvokeAsync<List<PrayerRequest>>(
                        "firebaseAuth.getPrayerRequests");

                return requests ?? new List<PrayerRequest>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getPrayerRequests error: {ex.Message}");

                throw;
            }
        }


        public async Task<List<PrayerRequest>> GetMyPrayerRequestsAsync()
        {
            await WaitForFirebaseAsync();

            try
            {
                var requests =
                    await _jsRuntime.InvokeAsync<List<PrayerRequest>>(
                        "firebaseAuth.getMyPrayerRequests");

                return requests ?? new List<PrayerRequest>();
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase getMyPrayerRequests error: {ex.Message}");

                throw;
            }
        }


        public async Task CreatePrayerRequestAsync(
            string title,
            string content)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.createPrayerRequest",
                    title,
                    content);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase createPrayerRequest error: {ex.Message}");

                throw;
            }
        }


        public async Task UpdatePrayerRequestStatusAsync(
            string requestId,
            string status)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.updatePrayerRequestStatus",
                    requestId,
                    status);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase updatePrayerRequestStatus error: {ex.Message}");

                throw;
            }
        }


        public async Task DeletePrayerRequestAsync(
            string requestId)
        {
            await WaitForFirebaseAsync();

            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "firebaseAuth.deletePrayerRequest",
                    requestId);
            }
            catch (JSException ex)
            {
                Console.WriteLine(
                    $"Firebase deletePrayerRequest error: {ex.Message}");

                throw;
            }
        }

        // =====================================================
        // LOGOUT
        // =====================================================

        public async Task LogoutAsync()
        {
            await WaitForFirebaseAsync();

            await _jsRuntime.InvokeAsync<object>(
                "firebaseAuth.logout"
            );

            CurrentUser = null;
        }


        // =====================================================
        // FIREBASE RESULT
        // =====================================================

        public class AuthResult
        {
            public bool Success { get; set; }

            public string? Message { get; set; }

            public string? Uid { get; set; }

            public string? FirstName { get; set; }

            public string? LastName { get; set; }

            public string? EmailAddress { get; set; }

            public string? UserType { get; set; }
        }
    }


}