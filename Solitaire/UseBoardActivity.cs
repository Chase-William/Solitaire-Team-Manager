using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Syncfusion.SfKanban.Android;
using Solitaire.Lang;
using Android.Support.V7.App;
using System.Threading.Tasks;
using Solitaire.CustomGestures;
using Android.Widget;
using Xamarin.Essentials;
using System.IO;
using Newtonsoft.Json;
/*

Chase - My Master Piece -- Main activity for the user to interact with the cards

*/
namespace Solitaire
{
    [Activity(Label = "UseBoardActivity")]
    public class UseBoardActivity : AppCompatActivity
    {
        // The board acts as a *pointer to the working board, therefore all changes will occur to the original - NOT A COPY
        public Board thisBoard;
        // The SfKanban is merly used as a way for the user to interact with their board and change its data
        public static SfKanban thisKanban;
        // Contains a list off all the categories so we can keep track of all the categories each board needs to support
        private List<object> allSupportedCategories = new List<object>();
        // When we click on a card, we will save which card was clicked
        public KanbanModelWrapper clickedKanbanModel;
        // The for our details activity, readonly because accessing this variable through a pointer to 
        // it within DoubleClickGesture will cause an error
        public readonly int DETAILS_ACTIVITY_CODE = 2;
        // Identifies whether the current click is the first click or the second click in a chain of clicks
        // Used when determining double clicks vs single clicks and if each click was on another object
        public bool clickIdentifier = true;
        // Double click class listener
        DoubleClickGesture thisDoubleClickGestureListener;
        // Finished kanbanModels are marked via the collor swatch on the bottem right of their card
        private const string FINISHED_CARD_COLOR = "Red";
        private const string UNFINISHED_CARD_COLOR = "Green";


        public static List<KanbanModelWrapper> kanbanModels = new List<KanbanModelWrapper>();


        /*

            KanbanModel tags will continue to show the correct oder of which the contributors were added
                that tag property probably could be used to keep the leader at the 0 index

        */

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Applying this SfKanban as the content view for the app
            SetContentView(Resource.Layout.use_board_activity);

            // Setting our custom toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Your Board";
            SetSupportActionBar(toolbar);

            // Calling our initalizer class
            new SetupBoardAndSfkanban(this).InvokeInitEvent();            
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.use_board_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        ///
        /// 
        ///     Determines which toolbar button was pressed
        /// 
        ///
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.TitleFormatted.ToString())
            {
                // If Add Column is clicked we need to create a new default column
                /*

                    I wrap our dialogs in a using statement because they the IDisposable interface which will provide deallocation
                    I really don't want a memory leak, but will see if I have time to fix this
                    Amazing Article: https://www.codeproject.com/Articles/29534/IDisposable-What-Your-Mother-Never-Told-You-About

                */
                case "Add Deck":
                    new CreateDeckDialog(this);
                    break;
                case "Add Card":
                    // If a column doesn't exist for this kanbanModel to go inside of... then inform the user
                    // and skip the creation proccess
                    if (thisKanban.Columns.Count == 0)
                    {
                        Toast.MakeText(this, "Please add a deck before you add a card.", ToastLength.Short).Show();
                        break;
                    }
                    // If a column does exist we will create the dialog
                    new CreateCardDialog(this);
                        break;
                case "Add Contributor":
                    new ContributorOptionsDialog(this);
                    break;
                case "Show Finished Cards":
                    ShowFinishedCards();
                    break;
                case "Hide Finished Cards":
                    HideAllFinishedCards();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        ///
        /// 
        ///     Adds all the finished cards to the UI
        /// 
        /// 
        private void ShowFinishedCards()
        {
            // OrderBy will make unfinished cards appear ontop of finished cards
            thisKanban.ItemsSource = kanbanModels.OrderBy(kanban => kanban.ColorKey);
        }

        /// 
        /// 
        ///     Removes all finished cards from the UI
        /// 
        /// 
        private void HideAllFinishedCards()
        {
            // If kanbanModels contains unfinished cards we need to manually remove them from the colums
            if (kanbanModels.All(kanbanModel => (string)kanbanModel.ColorKey == FINISHED_CARD_COLOR))
            {
                var finishedKanbanModels = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == FINISHED_CARD_COLOR).ToList();
                foreach (var kanbanColumn in thisKanban.Columns)
                {
                    // Since the list inside the KanbanColumn and the finishedKanbanModels will truncate, we dont actually want the for loop to increment
                    for (int i = 0; i < kanbanColumn.ItemsCount; i++)
                    {
                        kanbanColumn.RemoveItem(finishedKanbanModels.ElementAt(i));
                        finishedKanbanModels.Remove(finishedKanbanModels.ElementAt(i));
                        i--;
                    }
                }
            }

            // Updating the ItemSource so it contains zero FINISHED kanbanModels
            thisKanban.ItemsSource = new List<KanbanModel>();
            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Will add a kanbanModel to a specified column within the current working board
        /// 
        /// 
        public void AddCard(string _nameCard, string _descriptionCard, string _parentDeck)
        {
            kanbanModels.Add(new KanbanModelWrapper()
            {
                // Generating Unique Ids
                ID = IdManager.GenerateId(),
                Title = _nameCard,
                Description = _descriptionCard,
                // Category Determines which deck this will be put in
                Category = _parentDeck,
                ColorKey = UNFINISHED_CARD_COLOR,
                ImageURL = this.Assets + "/images/avatar_blue"

                /*

                    TODO: MAKE THIS PATH TO A IMAGE WORK

                */

                // ImageURL = something...
            });
            string test = this.Assets + "/images/avatar_blue";
            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Will add a column with the user's provided values
        ///  
        ///
        public void AddDeck(string _nameColumn, string _descriptionColumn)
        {
            allSupportedCategories.Add(_nameColumn);

            var newColumn = new KanbanColumn(this)
            {
                Title = _nameColumn,
                // Categories is a list of all the "categories" this deck supports
                Categories = new List<object>() {  _nameColumn }
            };

            // Decorating the new KanbanColumn
            newColumn.ContentDescription = _descriptionColumn;
            newColumn.ErrorBarSettings.Color = Color.Green;
            newColumn.ErrorBarSettings.Height = 4;

            // We need to add this kanbanWorkflow because it will be used when moving and deciding where cards shall be placed 
            thisKanban.Workflows.Add(new KanbanWorkflow()
            {
                Category = _nameColumn,
                AllowedTransitions = allSupportedCategories
            });   
            
            // Assigning in the new column
            thisKanban.Columns.Add(newColumn);
        }

        /// 
        /// 
        ///     Takes our kanban values and loads them into the working board for saving
        /// 
        /// 
        public Task SaveKanbanIntoBoard()
        {
            lock (thisBoard)
            {
                // Createing decks from the kanbanColumn data into thisBoard.Decks list
                thisBoard.Decks = thisKanban.Columns.Select(kanbanColumn => new Deck(kanbanColumn.Title, kanbanColumn.ContentDescription)).ToList();
                
                // Creating cards from kanbanModel data into a list and assigning it to thisBoards.Cards list
                thisBoard.Cards = kanbanModels.Select(kanbanModel =>
                    new Card(kanbanModel.Title, kanbanModel.Description, (string)kanbanModel.Category)             
                    { 
                        IsFinished = ((string)kanbanModel.ColorKey) == FINISHED_CARD_COLOR ? true : false ,
                        ContributorEmails = kanbanModel.Tags == null ? null : kanbanModel.Tags.ToList()
                    }).ToList(); 
            }

            // Sending the data to the server
            SendBoardDataToServer();
            return Task.CompletedTask;
        }

        ///
        /// 
        ///     Handles a click event on one of our kanbanmodels inside our Sfkanban
        /// 
        ///         
        private void KanbanModelClicked(object sender, KanbanTappedEventArgs e)
        {
            var currentClickedKanbanModel = (KanbanModelWrapper)e.Data;

            // If the current click is the first click on the item:
            if (clickIdentifier)
            {
                clickIdentifier = false;
                clickedKanbanModel = currentClickedKanbanModel;
                thisDoubleClickGestureListener.timer.Start();
            }
            // If the current click is the second click on the item:
            else if (!clickIdentifier && clickedKanbanModel.Equals(currentClickedKanbanModel))
            {
                thisDoubleClickGestureListener.timer.Stop();
                clickIdentifier = true;
                // Now we need to make the current card as finished
                MarkCardAsFinished(e.Column ,(long)((KanbanModel)e.Data).ID);
                //Toast.MakeText(this, "Fire Double Click Event", ToastLength.Short).Show();
            }
            // A new kanbanModel was clicked therefore we are starting a new DoubleClickGesture for that object
            else if (!clickIdentifier && !currentClickedKanbanModel.Equals(clickedKanbanModel))
            {
                // First we stop the timer
                thisDoubleClickGestureListener.timer.Stop();
                // Secondly we sent the click identifier to true
                clickIdentifier = true;
                // Thirdly we assign the currently clicked kanbanmodel as the last
                clickedKanbanModel = currentClickedKanbanModel;
                // Then we start an intent on the currently click
                Intent showDetailsActivity = new Intent(this, typeof(DetailsCardActivity));
                showDetailsActivity.PutExtra("kanbanModelId", (long)clickedKanbanModel.ID);
                StartActivityForResult(showDetailsActivity, DETAILS_ACTIVITY_CODE);
            }
        }

        /// 
        /// 
        ///     Hides card from the kanbanWorkflow because the card is "finished"
        /// 
        /// 
        private void MarkCardAsFinished(KanbanColumn _kanbanColumn, long _id)
        {
            // ItemSource will not automatically remove finished card from Column
            // therefore we do it manually:
            _kanbanColumn.RemoveItem(clickedKanbanModel);

            // Assigning the tapped card in the list to be mark as finished
            kanbanModels = kanbanModels.Select(kanbanModel => 
            { 
                if (kanbanModel.ID == _id) { 
                    kanbanModel.ColorKey = FINISHED_CARD_COLOR; 
                } 
                return kanbanModel; 
            }).ToList();
            
            // Resetting the ItemSource to display only the UNFINISHED cards
            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Sends the new board data to the remote server
        /// 
        /// 
        private void SendBoardDataToServer()
        {
            ClientManager.SendBoardData(JsonConvert.SerializeObject(thisBoard));
        }

        /// 
        /// 
        ///     Updates the UI depending on whether the EditCardActivity was launched or not
        /// 
        ///      
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // If the resultCode is equal to Result.Ok then we will manually tell the UI to refresh
            if (requestCode == DETAILS_ACTIVITY_CODE && resultCode == Result.Ok)
            {
                // Triggering the ItemSource to update the UI by assigning the kanbanModels
                // I tried to find like a NotfifySubSetChanged method to trigger a UI update but this what I found works
                thisKanban.ItemsSource = kanbanModels; 
            }
        }

        /// 
        /// 
        ///     Overriding the back button so we automatically save before we exit
        /// 
        /// 
        public override async void OnBackPressed()
        {
            // Loading our kanban data back into the board
            await SaveKanbanIntoBoard();
            // AssetManager.WriteToBoardsOnFile();
            base.OnBackPressed();
        }

        /// 
        /// 
        ///     Responsible for setting up the board and sfkanban for the UseBoardActivity
        /// 
        ///
        private sealed class SetupBoardAndSfkanban
        {            
            private event Action setupBoardAndSfKanban;             // General initialization processes that will always run
            private event Action loadPreExistingBoardData;          // Fires if pre-existing Board data is needing to be loaded
            private readonly UseBoardActivity useBoardActivity;     // Context this was create from to access it's member variables

            /// 
            /// 
            ///     Constructor assigns Context this was called from to a variable
            ///         and chains all handlers to corresponding event
            ///
            /// 
            public SetupBoardAndSfkanban(UseBoardActivity _useboardActivity)
            {
                useBoardActivity = _useboardActivity;

                setupBoardAndSfKanban += GetRefToSfKanbanFromResource;
                setupBoardAndSfKanban += GetRefToBoardFromAssetsManager;
                setupBoardAndSfKanban += InitSfKanbanWorkflow;
                setupBoardAndSfKanban += InitSfKanbanColorKey;
                setupBoardAndSfKanban += InitSfKanbanGestures;
                // Initializing our kanbanModels to new collection each time we run this activity
                setupBoardAndSfKanban += delegate { UseBoardActivity.kanbanModels = new List<KanbanModelWrapper>(); };
                // If we have board data to load... load it, otherwise skip
                setupBoardAndSfKanban += delegate { loadPreExistingBoardData?.Invoke(); };
            }

            /// 
            /// 
            ///     Allows for safe public invoking of setupBoardAndSfKanban event
            /// 
            /// 
            public void InvokeInitEvent()
            {
                setupBoardAndSfKanban?.Invoke();
            }

            /// 
            /// 
            ///     Getting a ref to the SfKanban from the inflated layout from XML, therefore getting it from the Resource.Id class
            /// 
            /// 
            private void GetRefToSfKanbanFromResource()
            {
                UseBoardActivity.thisKanban = useBoardActivity.FindViewById<SfKanban>(Resource.Id.sfKanban);
            }

            ///
            /// 
            ///     Getting a ref to the Board that was created or was clicked on that started the UseBoardActivity
            /// 
            /// 
            private void GetRefToBoardFromAssetsManager()
            {
                // If our board has extra data needing to be loaded, attach a method to event for that
                if (!useBoardActivity.Intent.HasExtra("IsNew")) loadPreExistingBoardData += LoadDataFromBoardIntoSfKanban;

                // Getting a ref to the board we "working" with using an Intent.Extra
                useBoardActivity.thisBoard = AssetManager.boards.Single(board => board.Id == useBoardActivity.Intent.GetLongExtra("BoardId", -1));
            }

            /// 
            /// 
            ///     Initializes the workflow for the SfKanban
            /// 
            /// 
            private void InitSfKanbanWorkflow()
            {
                UseBoardActivity.thisKanban.Workflows = new List<KanbanWorkflow>();
            }

            /// 
            ///
            ///     Initializes the color key used for detecting and displaying whether a kanbanModel is "finished" 
            /// 
            /// 
            private void InitSfKanbanColorKey()
            {
                UseBoardActivity.thisKanban.IndicatorColorPalette = new List<KanbanColorMapping>
                {
                    new KanbanColorMapping(UNFINISHED_CARD_COLOR, Color.Green),
                    new KanbanColorMapping(FINISHED_CARD_COLOR, Color.Red)
                };
            }

            /// 
            /// 
            ///     Initializing the single click and double click gestures
            /// 
            /// 
            private void InitSfKanbanGestures()
            {
                // Single click 
                UseBoardActivity.thisKanban.ItemTapped += useBoardActivity.KanbanModelClicked;

                // Double click 
                useBoardActivity.thisDoubleClickGestureListener = new DoubleClickGesture();                
                useBoardActivity.thisDoubleClickGestureListener.InitDoubleClickGesture(useBoardActivity);
            }

            /// 
            /// 
            ///     Loads data from board ref into our Sfkanban (optional method), look GetRefToBoard()
            /// 
            ///     Steps:
            ///         1. Init KanbanColumn
            ///             A. Add this KanbanColumn instance's Category property to the allSupportedCategories list
            ///             B. Add new workflow using this KanbanColumn instance's Category and the allSupportedCategories list
            ///             C. Add this KanbanColumn instance to the SfKanban.Columns list
            ///             
            ///         2. Init KanbanModel
            ///             A. Determine whether the card is finished or unfinished when creating the kanbanModel
            ///             B. Add this kanbanModel to the main KanbanModel list
            ///             
            ///         3. Assign the fully setup list of KanbanModels to the SfKanban.ItemsSource
            ///
            private void LoadDataFromBoardIntoSfKanban()
            {

                // Initializing KanbanColumns
                foreach (Deck KanbanColumn in useBoardActivity.thisBoard.Decks)
                {
                    var newColumn = new KanbanColumn(useBoardActivity)
                    {
                        Title = KanbanColumn.Name,
                        ContentDescription = KanbanColumn.Description,
                        Categories = new List<object>() { KanbanColumn.Name }
                    };
                    newColumn.ErrorBarSettings.Color = Color.Green;
                    newColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
                    newColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
                    newColumn.ErrorBarSettings.Height = 4;

                    UseBoardActivity.thisKanban.Columns.Add(newColumn);
                    useBoardActivity.allSupportedCategories.Add(newColumn.Title);

                    // Initializing the workflows
                    UseBoardActivity.thisKanban.Workflows.Add(new KanbanWorkflow()
                    {
                        Category = KanbanColumn.Name,
                        AllowedTransitions = useBoardActivity.allSupportedCategories
                    });
                }


                // Creating KanbanModels from Card data and assigning all new KanbanModel instance into kanbans list
                UseBoardActivity.kanbanModels = useBoardActivity.thisBoard.Cards.Select(card =>
                    new KanbanModelWrapper()
                    {
                        ID = card.Id,
                        Title = card.Name,
                        Category = card.ParentDeck,
                        Description = card.Description,
                        ColorKey = card.IsFinished ? FINISHED_CARD_COLOR : UNFINISHED_CARD_COLOR,
                        Tags = card.ContributorEmails == null ? null : card.ContributorEmails.ToArray()
                    }).ToList();

                // Assigning the kanbans into the ItemsSource so they can be displayed
                // IMPORTANT: Finished cards are hidden by default
                UseBoardActivity.thisKanban.ItemsSource = UseBoardActivity.kanbanModels
                    .Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR);
            }
        }
    }
}