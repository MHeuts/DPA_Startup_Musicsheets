namespace DPA_Musicsheets.Models
{
    public interface IStaffElementVisitor
    {
        void Visit(Staff staff);
        void Visit(Bar bar);

    }
}
