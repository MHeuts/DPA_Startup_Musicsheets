using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;

namespace DPA_Musicsheets.ViewModels
{
    /// <summary>
    /// This is the MVVM-Light implementation of dependency injection.
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // TODO: BANISH
            SimpleIoc.Default.Register<MusicLoader>();

            // VM's
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<Staffs>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();

            // Domain

            SimpleIoc.Default.Register<MusicManager>();
            SimpleIoc.Default.Register<NoteFactory>();
            SimpleIoc.Default.Register<MidiMessageHandlerFactory>();
            SimpleIoc.Default.Register<MidiConverter>();
            SimpleIoc.Default.Register<MusicPlayer>();

            // File handler chain
            SimpleIoc.Default.Register(() =>
            {
                var chain = new LinkedList<MusicFileHandler>();
                chain.AddFirst(new MidiFileHandler(SimpleIoc.Default.GetInstance<MidiConverter>()));
                chain.AddFirst(new LilypondFileHandler(SimpleIoc.Default.GetInstance<LilyPondConverter>()));
                return chain;
            });
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LilypondViewModel LilypondViewModel => ServiceLocator.Current.GetInstance<LilypondViewModel>();
        public Staffs StaffsViewModel => ServiceLocator.Current.GetInstance<Staffs>();
        public MidiPlayerViewModel MidiPlayerViewModel => ServiceLocator.Current.GetInstance<MidiPlayerViewModel>();

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MusicPlayer>().Cleanup();
        }
    }
}