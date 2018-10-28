using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using Sanford.Multimedia.Midi;
using System;

namespace DPA_Musicsheets.IO
{
    public class MusicPlayer
    {
        private OutputDevice _outputDevice;
        private MidiConverter _converter;
        private Sequencer _sequencer;
        private Staff _staff;
        private bool _running;

        public event EventHandler StatusChanged;

        private Sequence MidiSequence
        {
            get { return _sequencer.Sequence; }
            set
            {
                Stop();
                if (value == null) value = new Sequence();
                _sequencer.Sequence = value;
            }
        }

        public bool Running
        {
            get { return _running; }
            private set
            {
                _running = value;
                OnStatusChanged(new MusicPlayerStatusChangedEventArgs(value));
            }
        }
        public Staff Staff
        {
            get { return _staff; }
            set
            {
                Stop();
                SetSequence(value);
                _staff = value;
            }
        }

        public MusicPlayer(MusicManager fileManager, MidiConverter midiConverter)
        {
            // The OutputDevice is a midi device on the midi channel of your computer.
            // The audio will be streamed towards this output.
            // DeviceID 0 is your computer's audio channel.
            _outputDevice = new OutputDevice(0);
            _sequencer = new Sequencer();
            _converter = midiConverter;

            _sequencer.ChannelMessagePlayed += ChannelMessagePlayed;

            // Wanneer de sequence klaar is moeten we alles closen en stoppen.
            _sequencer.PlayingCompleted += (playingSender, playingEvent) =>
            {
                Stop();
            };
        }

        public void Play()
        {
            Running = true;
            _sequencer.Continue();
        }

        public void Stop()
        {
            Running = false;
            _sequencer.Stop();
            _sequencer.Position = 0;
        }

        public void Pause()
        {
            Running = false;
            _sequencer.Stop();
        }

        private void SetSequence(Staff staff)
        {
            MidiSequence = _converter.Convert(staff);
        }

        protected virtual void OnStatusChanged(EventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        // Wanneer een channelmessage langskomt sturen we deze direct door naar onze audio.
        // Channelmessages zijn tonen met commands als NoteOn en NoteOff
        // In midi wordt elke noot gespeeld totdat NoteOff is benoemd. Wanneer dus nooit een NoteOff komt nadat die een NoteOn heeft gehad
        // zal deze note dus oneindig lang blijven spelen.
        private void ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            try
            {
                _outputDevice.Send(e.Message);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is OutputDeviceException)
            {
                // Don't crash when we can't play
                // We have to do it this way because IsDisposed on
                // _outDevice may be false when it is being disposed
                // so this is the only safe way to prevent race conditions
            }
        }

        public void Cleanup()
        {
            _sequencer.Stop();
            _sequencer.Dispose();
            _outputDevice.Dispose();
        }
    }
}
