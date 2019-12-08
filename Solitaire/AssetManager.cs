using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Solitaire.Lang;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api.SignIn;
using System.Net;
using System.Drawing;

namespace Solitaire
{
    public class AssetManager
    {
        private static string[] images = { "images/avatar_red.png", "images/avatar_orange.png", "images/avatar_blue.png", "images/avatar_purple.png", "images/avatar_green.png" };
        private static Dictionary<string, Drawable> imageCache = new Dictionary<string, Drawable>();
        public static List<Board> boards = new List<Board>();
        public static List<Contributor> contributors = new List<Contributor>();
        public static GoogleSignInAccount thisGoogleAccount;
        public static TimeSpan VibrateTime;        

        // Interesting const is automatically static, don't need to write static
        private const string BOARDS_FILE = "boards.json";
        //private static string localPath;

        static AssetManager()
        {
            VibrateTime = TimeSpan.FromMilliseconds(150);
        }


        /// 
        /// 
        ///     Used for getting images from the assets dir 
        ///
        ///
        public static Drawable GetDrawable(Context _context, string url)
        {
            // If the asset isn't already loaded and insidie our dictionary:
            if (!imageCache.ContainsKey(url))
            {
                // If we can't find the asset we will return null
                try
                {
                    imageCache.Add(url, Drawable.CreateFromStream(_context.Assets.Open(url), null));
                }
                catch
                {
                    return null;
                }                
            }
            return imageCache[url];
        }

        /// 
        /// 
        ///     Picks a random number and returns a file attached
        /// 
        /// 
        public static Drawable GetRandomDrawable(Context _context)
        {
            return Drawable.CreateFromStream(_context.Assets.Open(images[new Random().Next(0, 4)]), null);
        }

        /// 
        /// 
        ///     Reads all boards from file and loads them into our this.boards dictionary
        /// 
        /// 
        public static async void ReadAllBoardsFromFile()
        {
            // https://docs.microsoft.com/en-us/xamarin/essentials/file-system-helpers?context=xamarin%2Fxamarin-forms&tabs=android
            // https://www.youtube.com/watch?v=FNIoCWHtWpc
            // This reminds me of java with passing in a StreamReader into a BufferedReader when reading from sockets   
            try
            {
                using (var stream = await FileSystem.OpenAppPackageFileAsync(BOARDS_FILE))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        // Reading our json file and converting them into board instances in our programs memory
                        List<Board> testPtr = JsonConvert.DeserializeObject<List<Board>>(await reader.ReadToEndAsync());

                        // We dont wannt assign it null if the file was empty because that will cause many problems
                        if (testPtr != null)
                        {
                            boards = testPtr;
                        }                        
                    }                    
                }
            }
            catch
            {





                // TODO: Create a new file if we can't find the file, this mean the user could be a first time user





            }
        }

        /// 
        /// 
        ///     Writes all boards from our this.board collection into our boards.json file
        /// 
        ///
        public static async void WriteToBoardsOnFile()
        {                  



            // TODO: Fix the writing problem...



            using (var stream = await FileSystem.OpenAppPackageFileAsync(BOARDS_FILE))
            {
                using (var writer = new StreamWriter(stream))
                {
                    // Writing the board to our file for save storage
                    writer.Write(JsonConvert.SerializeObject(boards));
                }
            }
        }

        /// 
        /// 
        ///     Checks to see if an email has already been used, as we will be treating emails as primary keys
        ///
        ///
        public static bool IsEmailAlreadyUsed(string _email)
        {
            // If the email is already used return false
            if (contributors.Any(x => x.Email.Contains(_email)))
            {
                return true;
            }
            // If it hasn't been used return true
            else
            {
                return false;
            }
        }

        /// 
        /// 
        ///     Querys the avatar of the user using the scheme and schemeSpecificPart provided inside the GoogleSignInAccount
        /// 
        ///     Notes: This is really cool, I was seeing watch properties were inside the GoogleSignInAccount and I found the url information.
        ///         So I plugged them into my brower and it took me to a picture of my famined looking self
        ///
        ///
        public static Drawable QueryGoogleAccountAvatar()
        {
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_scheme + ":" + _schemeSpecificPart);

            //// returned values are returned as a stream, then read into a string
            //using (HttpWebResponse webResponce = (HttpWebResponse)webRequest.GetResponse())
            //{
            //    using (BinaryReader reader = new BinaryReader(webResponce.GetResponseStream()))
            //    {
            //        byte[] imageInBytes = reader.ReadBytes(1 * 1024 * 1024 * 10);
            //        using (FileStream lxFS = new FileStream("34891.jpg", FileMode.Create))
            //        {
            //            lxFS.Write(imageInBytes, 0, imageInBytes.Length);
            //        }
            //    }
            //}
            WebRequest webRequest = WebRequest.Create(thisGoogleAccount.PhotoUrl.Scheme + ":" + thisGoogleAccount.PhotoUrl.SchemeSpecificPart);
            WebResponse webResponce = webRequest.GetResponse();
            Drawable avatar = Drawable.CreateFromStream(webResponce.GetResponseStream(), "Google");

            return avatar;            
        }
    }
}