namespace DPA_Musicsheets.Models
{
    public class MusicNote
    {
        public Tone Tone { get; set; }
        public int Octave { get; set; }
        public Modifier Modifier { get; set; }
        public double Duration { get; set; }
        public bool Dot { get; set; }
        public double TotalDuration {
            get
            {
                if (Dot)
                {
                    return Duration + (Duration/2);
                }
                return Duration;
            }
        }
    }
}
