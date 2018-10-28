namespace DPA_Musicsheets.Models
{
    public abstract class StaffElement
    {
        public abstract void Accept(IStaffElementVisitor visitor);
    }
}
