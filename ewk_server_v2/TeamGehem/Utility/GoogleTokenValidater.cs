using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.Utility
{
    class GoogleTokenValidater : Singleton<GoogleTokenValidater>
    {
        //public static string ValidateToken(string token, Tokeninfo info = null)
        //{
        //    //return Instance.ValidateToken_(token, info);
        //    return string.Empty;
        //}

        //public static bool ValidateToken2()
        //{

        //}

        /// <summary>Performs validation steps on an OAuth 2.0 token.</summary>
        /// <param name="token">The access or id token to validate.</param>
        /// <param name="info">(Optional) The response from Tokeninfo.</param>
        /// <returns>The identifier for the user associated with the token.</returns>
        //private string ValidateToken_(string token, Tokeninfo info)
        //{
        //    bool isValid = false;

        //    // ID tokens have four parts, and Access tokens have two parts, separated by '.'.
        //    bool isIdToken = IsIdToken(token);

        //    Oauth2Service service = new Oauth2Service(
        //        new Google.Apis.Services.BaseClientService.Initializer());

        //    var request = service.Tokeninfo();
        //    if (!isIdToken)
        //    {
        //        // TODO - Try an authorized API call?
        //        request.AccessToken = token;
        //    }
        //    else
        //    {
        //        request.IdToken = token;
        //    }
            //try
            //{
            //    if (info == null)
            //    {
            //        info = request.Execute();
            //    }
            //    if (info.IssuedTo == PlusHelper.GetClientConfiguration().Secrets.ClientId &&
            //        info.ExpiresIn > 0)
            //    {
            //        isValid = true;
            //    }
            //    else
            //    {
            //        throw new PlusHelper.GoogleTokenExpirationException();
            //    }

            //    if (isValid)
            //    {
            //        return info.UserId;
            //    }
            //    else
            //    {
            //        throw new PlusHelper.TokenVerificationException();
            //    }
            //}
            //catch (GoogleApiException gae)
            //{
            //    // Occurs when you have an invalid token, send an error to the client.
            //    throw new PlusHelper.TokenVerificationException();
            //}
 //       }

        /// <summary>
        /// Tries to parse the base64 payload of a token to identify whether it is an ID token.
        /// </summary>
        /// <param name="token">The token to test.</param>
        /// <returns>True if the token is an ID token.</returns>
        private bool IsIdToken(string token)
        {
            try
            {
                string[] segments = token.Split('.');

                string base64EncoodedJsonBody = segments[1];
                int mod4 = base64EncoodedJsonBody.Length % 4;
                if (mod4 > 0)
                {
                    base64EncoodedJsonBody += new string('=', 4 - mod4);
                }
                byte[] encodedBodyAsBytes =
                    System.Convert.FromBase64String(base64EncoodedJsonBody);
                string jsonBody =
                    System.Text.Encoding.UTF8.GetString(encodedBodyAsBytes);

                //IDTokenJsonBodyObject bodyObject =
                //    JsonConvert.DeserializeObject<IDTokenJsonBodyObject>(jsonBody);

                //// Test just in case the access token parses.
                //return bodyObject.Audience.Equals(
                //    PlusHelper.GetClientConfiguration().Secrets.ClientId);
            }
            catch (JsonReaderException jre)
            {
                // Ignore, the body could not be parsed because this is an access token.
            }
            catch (FormatException fe)
            {
                // Ignore, the base64 string could not be parsed because this is an access token.
            }
            return false;
        }
    }
}
