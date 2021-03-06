﻿using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.IO.FileHandlers;
using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

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

            // VM's
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<Staffs>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();

            // Domain
            SimpleIoc.Default.Register<MusicManager>();
            SimpleIoc.Default.Register<NoteFactory>();
            SimpleIoc.Default.Register<MidiMessageHandlerFactory>();
            SimpleIoc.Default.Register<MusicPlayer>();

            // Converters
            SimpleIoc.Default.Register<MidiConverter>();
            SimpleIoc.Default.Register<LilyPondConverter>();

            // File handler chain
            SimpleIoc.Default.Register<MusicFileHandler>(() =>
            {
                MusicFileHandler chain = new LilypondFileHandler(SimpleIoc.Default.GetInstance<LilyPondConverter>());
                chain = new MidiFileHandler(SimpleIoc.Default.GetInstance<MidiConverter>(), chain);
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