namespace FastFood.Web.ViewModels.Items
{
    public class CreateItemViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryIdName => CategoryId + " " + CategoryName;
    }
}
