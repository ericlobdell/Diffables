namespace Diffables
{
    public interface IDiffable
    {
        void RecordState();
        void RollBack();
        void RollForward();
        void LoadVersion(int position);
       // void RemoveVersion(int position);
        int GetPosition();
        int GetChangeCount();
        bool HasNext();
        bool HasPrevious();
        void Flush();

    }
}
