namespace StateAssessment.Models.ViewModels
{
    public class InventoryAssessment
    {
        public InventoryAssessment(Inventory inventory, Assessment assessment) {
            this.Inventory = inventory;
            this.Assessment = assessment;
        }  
        public Assessment Assessment { get; set; }
        public Inventory Inventory { get; set; }
    }
}
