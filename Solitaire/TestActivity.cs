using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Syncfusion.SfKanban.Android;

namespace Solitaire
{
    ////
    //// Summary:
    ////     Represents fields for the card.
    //[Preserve(AllMembers = true)]
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {
        KanbanColumn menu;
        KanbanColumn order;
        KanbanColumn ready;
        KanbanColumn delivery;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //ActionBar actionBar = (this).ActionBar;
            //actionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#D53130")));

            var kanban = new SfKanban(this);
            kanban.SetBackgroundColor(Color.ParseColor("#F2F2F2"));
            kanban.PlaceholderStyle.SelectedBackgroundColor = Color.ParseColor("#FBC7AB");

            menu = new KanbanColumn(this) { Categories = new List<object>() { "Menu" } };

            // Sets the title of the entire column
            menu.Title = "Menu";
            // Enable / Disable allowing cards to be dropped into this column
            menu.AllowDrop = true;
            // Changes the color of the horizontal bar present inside inside the column
            menu.ErrorBarSettings.Color = Color.ParseColor("#130923");
            // Adds the column to the menu
            kanban.Columns.Add(menu);

            order = new KanbanColumn(this) { Categories = new List<object>() { "Dining", "Delivery" } };
            order.Title = "Order";
            order.ErrorBarSettings.Color = Color.ParseColor("#D53130");
            kanban.Columns.Add(order);

            ready = new KanbanColumn(this) { Categories = new List<object>() { "Ready" } };
            ready.Title = "Ready to Serve";
            ready.AllowDrag = true;
            ready.ErrorBarSettings.Color = Color.ParseColor("#D53130");
            kanban.Columns.Add(ready);

            delivery = new KanbanColumn(this) { Categories = new List<object>() { "Door Delivery" } };
            delivery.Title = "Delivery";
            delivery.AllowDrag = false;
            delivery.ErrorBarSettings.Color = Color.ParseColor("#D53130");
            kanban.Columns.Add(delivery);

            kanban.ItemsSource = new KanbanData().Data;

            kanban.Workflows.Add(new KanbanWorkflow("Menu", new List<object> { "Dining", "Delivery" }));

            kanban.Workflows.Add(new KanbanWorkflow("Dining", new List<object> { "Ready" }));

            kanban.Workflows.Add(new KanbanWorkflow("Delivery", new List<object> { "Door Delivery" }));

            //kanban.DragStart += Kanban_DragStart;

            //kanban.DragEnd += Kanban_DragEnd;

            //kanban.DragOver += Kanban_DragOver;

            kanban.Adapter = new CustomizationAdapter(kanban);


            SetContentView(kanban);
        }


        private void Kanban_DragOver(object sender, KanbanDragOverEventArgs e)
        {
            if (e.SourceColumn == menu)
                e.Cancel = true;
        }

        private void Kanban_DragEnd(object sender, KanbanDragEndEventArgs e)
        {
            if (e.TargetColumn == order)
            {
                e.Cancel = true;
                if (e.TargetColumn.IsExpanded)
                {
                    e.TargetColumn.InsertItem(CloneModel(e.Data as CustomKanbanModel, e.TargetCategory), 0);
                }
            }
        }

        CustomKanbanModel CloneModel(CustomKanbanModel model, object category)
        {
            CustomKanbanModel newModel = new CustomKanbanModel();

            newModel.Category = category;
            newModel.ColorKey = model.ColorKey;
            newModel.Description = model.Description;
            newModel.ID = model.ID;
            newModel.Tags = model.Tags;
            newModel.Title = model.Title;
            newModel.Name = model.Name;
            newModel.ImageURL = model.ImageURL;

            return newModel;
        }

        private void Kanban_DragStart(object sender, KanbanDragStartEventArgs e)
        {
            if (e.SourceColumn == menu)
            {
                e.KeepItem = true;
            }
        }

    }

    internal class CustomizationAdapter : KanbanAdapter
    {
        internal CustomizationAdapter(SfKanban kanban) : base(kanban) { }

        protected override void BindItemView(KanbanColumn column, KanbanItemViewHolder viewHolder, object data, int position)
        {
            LinearLayout layout = new LinearLayout(column.Context);
            layout.SetPadding(0, 0, 0, 10);
            layout.Orientation = Orientation.Vertical;
            TextView tex = new TextView(column.Context);
            tex.Text = "Title :" + (data as CustomKanbanModel).Title;
            tex.SetTextColor(Color.Black);
            tex.SetPadding(5, 5, 5, 5);

            TextView Description = new TextView(column.Context);
            Description.Text = "Description :" + (data as CustomKanbanModel).Description;
            Description.SetTextColor(Color.Black);
            Description.SetPadding(5, 5, 5, 5);

            layout.AddView(tex);
            layout.AddView(Description);

            viewHolder.ItemView = layout;
            base.BindItemView(column, viewHolder, data, position);

        }
    }

    public class CustomKanbanModel : KanbanModel
    {
        public string Name { get; set; }
    }

    class KanbanData
    {
        public ObservableCollection<CustomKanbanModel> Data { get; set; }

        public KanbanData() { Data = ItemsSourceCards(); }

        ObservableCollection<CustomKanbanModel> ItemsSourceCards()
        {
            ObservableCollection<CustomKanbanModel> cards = new ObservableCollection<CustomKanbanModel>();

            cards.Add(
                 new CustomKanbanModel()
                 {
                     ID = 1,
                     Title = "Margherita",
                     ImageURL = "margherita.png",
                     Category = "Menu",
                     Description = "The classic. Fresh tomatoes, garlic, olive oil, and basil. For pizza purists and minimalists only",
                     Tags = new string[] { "Cheese" }
                 }
             );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 2,
                    Title = "Double Cheese Margherita",
                    ImageURL = "doublecheesemargherita.png",
                    Category = "Menu",
                    Description = "The minimalist classic with a double helping of cheese",
                    Tags = new string[] { "Cheese" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 3,
                    Title = "Bucolic Pie",
                    ImageURL = "bucolicpie.png",
                    Category = "Menu",
                    Description = "The pizza you daydream about to escape city life. Onions, peppers, and tomatoes.",
                    Tags = new string[] { "Oninons", "Capsicum" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 4,
                    Title = "Bumper Crop",
                    ImageURL = "bumpercrop.png",
                    Category = "Menu",
                    Description = "Can�t get enough veggies? Eat this. Carrots, mushrooms, potatoes, and way more.",
                    Tags = new string[] { "Tomato", "Mushroom" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 5,
                    Title = "Spice of Life",
                    ImageURL = "spiceoflife.png",
                    Category = "Menu",
                    Description = "Thrill-seeking, heat-seeking pizza people only.  It�s hot. Trust us",
                    Tags = new string[] { "Corn", "Gherkins" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 6,
                    Title = "Very Nicoise",
                    ImageURL = "verynicoise.png",
                    Category = "Menu",
                    Description = "Anchovies, Dijon vinaigrette, shallots, red peppers, and potatoes.",
                    Tags = new string[] { "Red pepper", "Capsicum" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 8,
                    Title = "Salad Daze",
                    ImageURL = "saladdaze.png",
                    Category = "Menu",
                    Description = "Pretty much salad on a pizza. Broccoli, olives, cherry tomatoes, red onion.",
                    Tags = new string[] { "Capsicum", "Mushroom" }
                }
            );


            cards.Add(
             new CustomKanbanModel()
             {
                 ID = 9,                 
                 Title = "Margherita",
                 ImageURL = "margherita.png",
                 Category = "Delivery",
                 Description = "The classic. Fresh tomatoes, garlic, olive oil, and basil. For pizza purists and minimalists only",
                 Tags = new string[] { "Cheese" }
             }
         );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 10,
                    Title = "Double Cheese Margherita",
                    ImageURL = "doublecheesemargherita.png",
                    Category = "Ready",
                    Description = "The minimalist classic with a double helping of cheese",
                    Tags = new string[] { "Cheese" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 11,
                    Title = "Bucolic Pie",
                    ImageURL = "bucolicpie.png",
                    Category = "Dining",
                    Description = "The pizza you daydream about to escape city life. Onions, peppers, and tomatoes.",
                    Tags = new string[] { "Oninons", "Capsicum" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 12,
                    Title = "Bumper Crop",
                    ImageURL = "bumpercrop.png",
                    Category = "Ready",
                    Description = "Can�t get enough veggies? Eat this. Carrots, mushrooms, potatoes, and way more.",
                    Tags = new string[] { "Tomato", "Mushroom" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 13,
                    Title = "Spice of Life",
                    ImageURL = "spiceoflife.png",
                    Category = "Ready",
                    Description = "Thrill-seeking, heat-seeking pizza people only.  It�s hot. Trust us",
                    Tags = new string[] { "Corn", "Gherkins" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 14,
                    Title = "Very Nicoise",
                    ImageURL = "verynicoise.png",
                    Category = "Dining",
                    Description = "Anchovies, Dijon vinaigrette, shallots, red peppers, and potatoes.",
                    Tags = new string[] { "Red pepper", "Capsicum" }
                }
            );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 16,
                    Name ="DYFA8DF6A807FGSD807G0DF87GT08DS7G0DSGTS0DF87GTDSF08GTDG0DF",
                    Title = "D6FA86F890SF6S0D8AF6SF",
                    ImageURL = "saladdaze.png",
                    Category = "Delivery",
                    Description = "Pretty much salad on a pizza. Broccoli, olives, cherry tomatoes, red onion.",
                    Tags = new string[] { "TAG 1", "TAG 2" }
                }
            );



            cards.Add(
            new CustomKanbanModel()
            {
                ID = 17,
                Title = "Bumper Crop",
                ImageURL = "bumpercrop.png",
                Category = "Door Delivery",
                Description = "Can�t get enough veggies? Eat this. Carrots, mushrooms, potatoes, and way more.",
                Tags = new string[] { "Tomato", "Mushroom" }
            }
        );

            cards.Add(
                new CustomKanbanModel()
                {
                    ID = 18,
                    Title = "Spice of Life",
                    ImageURL = "spiceoflife.png",
                    Category = "Door Delivery",
                    Description = "Thrill-seeking, heat-seeking pizza people only.  It�s hot. Trust us",
                    Tags = new string[] { "Corn", "Gherkins" }
                }
            );
            return cards;
        }
    }
}