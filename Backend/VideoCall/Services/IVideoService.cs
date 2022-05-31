using System.Collections.Generic;
using System.Threading.Tasks;
using VideoCall.Models;

namespace VideoCall.Services
{
    public interface IVideoService
    {
        /// <summary>
        /// Gets the Twilio JSON web token so the client can connect to video services.
        /// </summary>
        string GetTwilioJwt(string identity);

        /// <summary>
        /// Gets all of the current active rooms for the app.
        /// </summary>
        Task<IEnumerable<RoomDetails>> GetAllRoomsAsync();
    }
}